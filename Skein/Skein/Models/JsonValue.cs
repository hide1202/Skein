using Skein.Exception;
using System;
using System.Collections.Generic;

namespace Skein
{
    public class JsonValue
    {
        #region Private variables
        private Dictionary<object, JsonValue> _data;
        private object _value = null;
        #endregion

        public JsonValue()
        {
            _data = new Dictionary<object, JsonValue>();
        }

        public JsonType DataType { get; set; } = JsonType.Null;

        #region Indexer
        public JsonValue this[string key] { get { return _data[key]; } }
        public JsonValue this[int index] { get { return _data[index]; } }
        #endregion

        public void Append(string name, JsonValue data)
        {
            if (!TryAppend(name, data))
                throw new JsonException(string.Format("Already this object contains \{name}"));
        }

        public bool TryAppend(string name, JsonValue data)
        {
            if (_data.ContainsKey(name))
                return false;

            _data.Add(name, data);
            return true;
        }

        #region Casting operator
        public static explicit operator int (JsonValue data) { return Convert<int>(data); }
        public static explicit operator double (JsonValue data) { return Convert<double>(data); }
        public static explicit operator bool (JsonValue data) { return Convert<bool>(data); }
        private static T Convert<T>(JsonValue data)
        {
            if (!(data._value is T))
                throw new InvalidCastException(string.Format("This json's value is not {0}!!", typeof(T).Name));
            return (T)data._value;
        }
        #endregion
    }
}