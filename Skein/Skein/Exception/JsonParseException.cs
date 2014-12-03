using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skein.Exception
{
    public class JsonParseException : JsonException
    {
        public JsonParseException() : base() { }
        public JsonParseException(string message) : base(message) { }
    }
}
