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

        #region Constructor
        public JsonObject() { _data = new Dictionary<object, JsonObject>(); DataType = JsonType.Null; }
        public JsonObject(JsonType dataType) : this() { DataType = dataType; }
        #endregion

        #region Properties
        public JsonType DataType { get; set; }
        #endregion

        #region Indexer
        public JsonObject this[string key] { get { return GetValue(key); } }
        public JsonObject this[int index] { get { return GetValue(index); } }
        #endregion

        private JsonObject GetValue(object key)
        {
            if (!_data.ContainsKey(key))
                throw new JsonException("This object doesn't contain '{0}'", key);
            return _data[key];
        }

        public void SetValue(JsonType type, object data)
        {
            DataType = type;
            _value = data;
        }

        public bool Append(object name, JsonObject data)
        {
            if (_data.ContainsKey(name)) return false;
            _data.Add(name, data); return true;
        }

        #region Casting operator
        public static implicit operator int (JsonObject data) { return Convert<int>(data); }
        public static implicit operator long (JsonObject data) { return Convert<long>(data); }
        public static implicit operator double (JsonObject data) { return Convert<double>(data); }
        public static implicit operator bool (JsonObject data) { return Convert<bool>(data); }
        public static implicit operator string (JsonObject data) { return Convert<string>(data); }
        private static T Convert<T>(JsonObject data)
        {
            if ((typeof(T) == typeof(long) && data._value is int) ||
                (typeof(T) == typeof(int) && data._value is long))
                    return (T)data._value;

            if (!(data._value is T))
                throw new InvalidCastException(string.Format("This json's value is not {0}!!", typeof(T).Name));
            return (T)data._value;
        }
        #endregion

#if DEBUG
        public string ToLogString()
        {

            System.Text.StringBuilder log = new System.Text.StringBuilder();
            if (_value != null)
            {
                log.Append("(v:");
                log.Append(_value != null ? _value.ToString() : "null");
                log.Append(")");
            }

            if (_data != null && _data.Count > 0)
            {
                log.Append("(d:");
                log.Append(_data.ToLog());
                log.Append(")");
            }

            return log.ToString();
        }
#endif
    }
}