using Skein.Exception;
using System.Diagnostics;

namespace Skein
{
    public class JsonReader
    {
        #region Token constant variable
        private const char OBJECT_START = '{';
        private const char OBJECT_END = '}';
        private const char DELIMITER = ':';
        private const char QUOTE = '"';
        private const char COMMA = ',';
        #endregion

        protected enum State
        {
            Start,
            NameStart, Naming, NameEnd,
            ValueStart, Value, ValueEnd,
            End
        }

        public static JsonValue Parse(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new JsonParseException("Input is null!!");

            if (json.Trim() == string.Empty)
                throw new JsonParseException("Input is empty!!");

            // TODO Parsing

            Debug.WriteLine("Parse success!!!");

            return null;
        }

        private static bool IsPassCharacter(char ch)
        {
            return ch == ' ' || ch == '\n' || ch == '\r';
        }
    }
}