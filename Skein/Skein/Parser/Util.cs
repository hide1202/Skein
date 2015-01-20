
namespace Skein.Parser
{
    public static class Util
    {
        public static bool IsWhiteSpace(char token) { return token == ' '; }
        public static bool IsCarriageReturn(char token) { return token == '\r'; }
        public static bool IsLineFeed(char token) { return token == '\n'; }

        public static bool IsPassableToken(char token)
        {
            return IsWhiteSpace(token) || IsCarriageReturn(token) || IsLineFeed(token);
        }
    }

    internal static class Token
    {
        internal const char ObjectStart = '{';
        internal const char ObjectEnd = '}';

        internal const char ArrayStart = '[';
        internal const char ArrayEnd = ']';

        internal const char DoubleQuote = '"';
        internal const char StringToken = DoubleQuote;

        internal const char Comma = ',';

        internal const char Delimiter = ':';
        internal const char Minus = '-';

        private const string NumberEnd = " ,]}";
        internal static bool IsNumberEnd(char token) { return NumberEnd.Contains(token.ToString()); }
    }    
}