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

        private void SkipPassableToken()
        {
            if (!IsValidPosition()) return;

            while (Util.IsPassableToken(_json[_position]))
            {
                if (!IsValidPosition()) break;
                _position++;
            }
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
            SkipPassableToken();

            // Check the start character.
            JsonType jsonType = GetJsonType(_json[_position]);

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
            if (_json[_position++] != Token.ObjectStart)
                throw new JsonParseException("Object must start '{'!!!");

            JsonObject result = new JsonObject();
            result.DataType = JsonType.Object;

            do
            {
                string name = ParseString();
                SkipPassableToken();

                if (_json[_position++] != Token.Delimiter)
                    throw new JsonParseException("Delimiter must ':'!!!");

                result.Append(name, ParseValue());
                SkipPassableToken();

                if (_json[_position] != ',') break;
                else _position++;
            } while (true);

            if(_json[_position++] != Token.ObjectEnd)
                throw new JsonParseException("Object must end '}'!!!");

            return result;
        }

        private JsonObject ParseStringValue()
        {
            SkipPassableToken();

            JsonObject result = new JsonObject();
            result.DataType = JsonType.String;

            result.SetValue(ParseString());

            return result;
        }

        private JsonObject ParseNumberValue()
        {
            SkipPassableToken();

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
            SkipPassableToken();

            if (_json[_position++] != Token.DoubleQuote)
                throw new JsonParseException("String must start '\"'!!!");
            
            int nameStart = _position;

            try { while (_json[_position++] != Token.DoubleQuote) ; }
            catch (IndexOutOfRangeException) { throw new JsonParseException("String must end \""); }

            return _json.Substring(nameStart, (_position - nameStart - 1));
        }

        private JsonObject ParseArray()
        {
            SkipPassableToken();

            if(_json[_position++] != Token.ArrayStart)
                throw new JsonParseException("Array must start '['!!!");

            JsonObject result = new JsonObject();
            result.DataType = JsonType.Array;
            int index = 0;
            do
            {                
                result.Append(index++, ParseValue());
                SkipPassableToken();

                if (_json[_position] != Token.Comma || _json[_position] == Token.ArrayEnd)
                {
                    _position++;
                    break;
                }

                _position++;

            } while (true);

            SkipPassableToken();

            return result;
        }

        private JsonType GetJsonType(char startToken)
        {
            if (char.IsNumber(startToken) || startToken == Token.Minus)
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