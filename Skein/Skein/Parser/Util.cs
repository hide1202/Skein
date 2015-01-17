namespace Skein.Parser
{
    public static class Util
    {
        public static bool IsWhiteSpace(char token) { return token == ' '; }
        public static bool IsCarriageReturn(char token) { return token == '\r'; }
        public static bool IsLineFeed(char token) { return token == '\n'; }

        public static bool IsPassToken(char token)
        {
            return IsWhiteSpace(token) || IsCarriageReturn(token) || IsLineFeed(token);
        }
    }

    public static class Token
    {
        public const char ObjectStart = '{';
        public const char ObjectEnd = '}';

        public const char ArrayStart = '[';
        public const char ArrayEnd = ']';

        public const char DoubleQuote = '"';
        public const char StringToken = DoubleQuote;

        public const char Comma = ',';

        public const char Delimiter = ':';
    }
}