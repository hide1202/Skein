using Skein.Exception;
using System.Diagnostics;
using System.Text;
using System;

namespace Skein.Parser
{
    internal class JsonObjectParser
    {
        private int _position = 0;
        private StringBuilder _nameBuilder;

        private bool IsValidPosition(string json)
        {
            return !(_position < 0 || _position >= json.Length);
        }

        private void PassToken(string json)
        {
            if (!IsValidPosition(json)) return;

            while (Util.IsPassToken(json[_position]))
            {
                if (!IsValidPosition(json)) break;
                _position++;
            }
        }

        internal JsonObjectParser() { Clear(); }

        internal JsonObject Parse(string json)
        {
            JsonObject result = ParseValue(json);
            Clear();
            return result;
        }

        private void Clear()
        {
            _nameBuilder = new StringBuilder();
            _position = 0;
        }

        private JsonObject ParseValue(string json)
        {
            PassToken(json);

            // Check the start character.
            JsonType jsonType = GetJsonType(json[_position]);

            switch (jsonType)
            {
                case JsonType.Object:
                    return ParseObject(json);
                case JsonType.String:
                    return ParseStringValue(json);
                default:
                    throw new NotSupportedException("This type is not supported!! [start:\{jsonType.ToString()}");
            }
        }

        private JsonObject ParseObject(string json)
        {
            if (json[_position++] != Token.ObjectStart)
                throw new JsonParseException("Object must start '{'!!!");

            JsonObject result = new JsonObject();
            result.DataType = JsonType.Object;

            do
            {
                PassToken(json);

                string name = ParseString(json);

                if (json[_position++] != Token.Delimiter)
                    throw new JsonParseException("Delimiter must start ':'!!!");

                result.Append(name, ParseValue(json));
                PassToken(json);

                if (json[_position] != ',') break;
                else _position++;
            } while (true);

            if(json[_position++] != Token.ObjectEnd)
                throw new JsonParseException("Object must end '}'!!!");

            return result;
        }

        private JsonObject ParseStringValue(string json)
        {
            PassToken(json);

            JsonObject result = new JsonObject();
            result.DataType = JsonType.String;

            result.SetValue(ParseString(json));
            PassToken(json);

            return result;
        }

        private string ParseString(string json)
        {
            PassToken(json);

            if (json[_position++] != Token.DoubleQuote)
                throw new JsonParseException("String must start '\"'!!!");

            PassToken(json);

            string stringValue = null;
            int nameStart = _position;

            while (json[_position] != Token.DoubleQuote)
                _position++;

            stringValue = json.Substring(nameStart, (_position++ - nameStart));
            PassToken(json);

            return stringValue;
        }

        private JsonType GetJsonType(char startToken)
        {
            if (char.IsNumber(startToken))
                return JsonType.Integer;

            switch (startToken)
            {
                case Token.ObjectStart:
                    return JsonType.Object;
                case Token.StringToken:
                    return JsonType.String;
                default:
                    throw new NotSupportedException("This type is not supported!! [start:\{startToken}]");
            }
        }
    }
}