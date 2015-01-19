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
        private StringBuilder _nameBuilder;

        private bool IsValidPosition()
        {
            return _position >= 0 && _position < _json.Length;
        }

        private char? NextToken()
        {
            while (true)
            {
                if (!IsValidPosition()) break;

                if (!Util.IsPassableToken(_json[_position]))
                    return _json[_position++];

                _position++;
            }

            return null;
        }

        internal JsonObjectParser(string json) { _json = json; Clear(); }

        internal JsonObject Parse()
        {
            JsonObject result = ParseValue();
            Clear();
            return result;
        }

        private void Clear()
        {
            _nameBuilder = new StringBuilder();
            _position = 0;
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
                    _position--;
                    return ParseNumberValue();
                case JsonType.Array:
                    return ParseArray();
                default:
                    throw new NotSupportedException("This type is not supported!! [start:\{jsonType.ToString()}");
            }
        }

        private JsonObject ParseObject()
        {
            //if (_json[_position++] != Token.ObjectStart)
            //    throw new JsonParseException("Object must start '{'!!!");

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
            JsonObject result = new JsonObject();
            result.DataType = JsonType.String;

            result.SetValue(ParseString());

            return result;
        }

        private JsonObject ParseNumberValue()
        {
            JsonObject result = new JsonObject();            

            int startPos = _position;

            while (true)
            {
                _position++;

                if (_json[_position] == ','
                || _json[_position] == ']'
                || _json[_position] == '}')
                    break;
            }

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
            JsonObject result = new JsonObject();
            result.DataType = JsonType.Array;
            int index = 0;
            do
            {                
                result.Append(index++, ParseValue());

                if (_json[_position] != Token.Comma || _json[_position] == Token.ArrayEnd)
                {
                    _position++;
                    break;
                }

                _position++;

            } while (true);

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