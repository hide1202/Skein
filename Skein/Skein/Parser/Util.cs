using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skein.Parser
{
    public static class Util
    {
        public static bool IsWhiteSpace(char token) { return token == ' '; }
        public static bool IsCarriageReturn(char token) { return token == '\r'; }
        public static bool IsLineFeed(char token) { return token == '\n'; }
    }
}
