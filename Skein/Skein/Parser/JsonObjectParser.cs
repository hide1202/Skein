using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skein.Exception;
using System.Diagnostics;

namespace Skein.Parser
{
    public class JsonObjectParser
    {
        enum State
        {
            Initial,
            ObjectStart,
            ObjectEnd,
            Naming,
            NameEnd,
            ValueStart
        }

        State _state = State.Initial;

        private const char START_TOKEN = '{';
        private const char QUOTE = '"';
        private const char DELIMITER = ':';
        private const char END_TOKEN = '}';

        private StringBuilder _nameBuilder;

        public JsonObjectParser()
        {
            _nameBuilder = new StringBuilder();
        }

        public JsonObject Parse(string json)
        {
            JsonObject result = new JsonObject();
            string name = string.Empty;

            foreach (char token in json)
            {
                if (Util.IsWhiteSpace(token) ||
                    Util.IsCarriageReturn(token) ||
                    Util.IsLineFeed(token))
                    continue;

                if (token == START_TOKEN)
                {
                    if (_state == State.Initial) _state = State.ObjectStart;
                    else throw new JsonException("Object must start \"");
                }
                else if (token == QUOTE)
                {
                    if (_state == State.ObjectStart)
                        _state = State.Naming;
                    else if (_state == State.Naming)
                    {
                        name = _nameBuilder.ToString();
                        Debug.WriteLine(string.Format("This object name is \{name}"));
                        _state = State.NameEnd;
                    }
                }
                else if (token == DELIMITER) {
                    if (_state == State.NameEnd)
                    {
                        _state = State.ValueStart;
                    }
                }
                else if (token == END_TOKEN) { }
                else
                {
                    if (_state == State.Naming)
                        _nameBuilder.Append(token);
                }
            }
            return null;
        }
    }
}
