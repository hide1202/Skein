using Skein.Exception;
using System;
using System.Collections.Generic;

namespace Skein
{
    public class JsonObject
    {
        #region Private variables
        private Dictionary<object, JsonObject> _data;
        private object _value = null;
        #endregion

        public JsonObject() { _data = new Dictionary<object, JsonObject>(); }

        public JsonType DataType { get; set; } = JsonType.Null;

        #region Indexer
        public JsonObject this[string key] { get { return _data[key]; } }
        public JsonObject this[int index] { get { return _data[index]; } }
        #endregion

        public void SetValue(object data)
        {
            _value = data;
        }

        public void Append(string name, JsonObject data)
        {
            if (!TryAppend(name, data))
                throw new JsonException(string.Format("Already this object contains \{name}"));
        }

        public bool TryAppend(string name, JsonObject data)
        {
            if (_data.ContainsKey(name))    return false;
            _data.Add(name, data);          return true;
        }

        #region Casting operator
        public static explicit operator int (JsonObject data) { return Convert<int>(data); }
        public static explicit operator double (JsonObject data) { return Convert<double>(data); }
        public static explicit operator bool (JsonObject data) { return Convert<bool>(data); }
        private static T Convert<T>(JsonObject data)
        {
            if (!(data._value is T))
                throw new InvalidCastException(string.Format("This json's value is not \{typeof(T).Name}!!"));
            return (T)data._value;
        }
        #endregion

        public override string ToString()
        {
            string @value = _value is string ? (string)_value : "null";
            return string.Format("value:{0}, dictionary:{1}", @value, _data);
        }
    }
}