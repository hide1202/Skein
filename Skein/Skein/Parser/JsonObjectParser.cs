using Skein.Exception;
using System.Diagnostics;
using System.Text;
using System;

namespace Skein.Parser
{
    internal class JsonObjectParser
    {
        private string _json = string.Empty;
        private int _position = 0;

        private bool IsValidPosition() { return _position.IsBetween(0, _json.Length); }

        private char? CurrentToken() { if (IsValidPosition()) return _json[_position]; return null; }

        private char? NextToken()
        {
            while (IsValidPosition())
            {
                if (!Util.IsPassableToken(_json[_position]))
                    return _json[_position++];

                _position++;
            }
            return null;
        }

        internal JsonObjectParser(string json) { _json = json; Clear(); }

        private void Clear() { _position = 0; }

        internal JsonObject Parse()
        {
            JsonObject result = ParseValue();
            Clear();
            return result;
        }

        private JsonObject ParseValue()
        {
            // Check the start character.
            JsonType jsonType = GetJsonType(NextToken());

            switch (jsonType)
            {
                case JsonType.Object:
                    return ParseObject();
                case JsonType.String:
                    return ParseStringValue();
                case JsonType.Integer:
                    return ParseNumberValue();
                case JsonType.Array:
                    return ParseArray();
                default:
                    throw new NotSupportedException("This type is not supported!! [start:\{jsonType.ToString()}");
            }
        }

        private JsonObject ParseObject()
        {
            JsonObject result = new JsonObject();
            result.DataType = JsonType.Object;

            do
            {
                NextToken();
                string name = ParseString();

                if (NextToken() != Token.Delimiter)
                    throw new JsonParseException("Delimiter must ':'!!!");

                result.Append(name, ParseValue());

                if (_json[_position] != ',') break;
                else _position++;
            } while (true);

            if(NextToken() != Token.ObjectEnd)
                throw new JsonParseException("Object must end '}'!!!");

            return result;
        }

        private JsonObject ParseStringValue()
        {
            JsonObject result = new JsonObject(JsonType.String);
            result.SetValue(ParseString());
            return result;
        }

        private JsonObject ParseNumberValue()
        {
            JsonObject result = new JsonObject();

            int startPos = (--_position);

            do      _position++;
            while   (!Token.IsNumberEnd(_json[_position]));

            var numberStr = _json.Substring(startPos, _position - startPos);
            float resultNum;
            if (!float.TryParse(numberStr, out resultNum))
                throw new JsonParseException("Failed to parse the number");

            if (resultNum % 1 != 0)
                result.SetValue(JsonType.Float, resultNum);
            else
                result.SetValue(JsonType.Integer, (int)resultNum);

            return result;
        }

        private string ParseString()
        {            
            int nameStart = _position;

            try
            {
                char? token = null;
                do { token = NextToken(); }
                while (token.HasValue && token != Token.DoubleQuote);
            }
            catch (IndexOutOfRangeException) { throw new JsonParseException("String must end \""); }

            return _json.Substring(nameStart, (_position - nameStart - 1));
        }

        private JsonObject ParseArray()
        {
            JsonObject result = new JsonObject(JsonType.Array);
            int index = 0;
            char? token = null;

            do
            {                
                result.Append(index++, ParseValue());
                token = CurrentToken();
                if (token == Token.Comma)   token = NextToken();
            } while (token != null && token != Token.ArrayEnd);

            if(token == Token.ArrayEnd)
                NextToken();
            else
                throw new JsonParseException("Array must end ']'!!!");

            return result;
        }

        private JsonType GetJsonType(char? startToken)
        {
            if (startToken == null)
                throw new JsonParseException("Token is invalid!!");

            if (char.IsNumber(startToken.Value) || startToken == Token.Minus)
                return JsonType.Integer;

            switch (startToken)
            {
                case Token.ObjectStart:
                    return JsonType.Object;
                case Token.StringToken:
                    return JsonType.String;
                case Token.ArrayStart:
                    return JsonType.Array;
                default:
                    throw new NotSupportedException("This type is not supported!! [start:\{startToken}]");
            }
        }
    }
}