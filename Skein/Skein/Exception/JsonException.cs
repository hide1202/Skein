﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skein.Exception
{
    public class JsonException : ApplicationException
    {
        public JsonException() : base() { }
        public JsonException(string message) : base(message) { }
        public JsonException(string message, params object[] args) : this(string.Format(message, args)) { }
    }
}
