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
        public JsonObject(JsonType dataType) : this() { DataType = dataType; }

        public JsonType DataType { get; set; } = JsonType.Null;

        #region Indexer
        public JsonObject this[string key] { get { return _data[key]; } }
        public JsonObject this[int index] { get { return _data[index]; } }
        #endregion

        public void SetValue(object data)
        {
            _value = data;
        }

        public void SetValue(JsonType type, object data)
        {
            DataType = type;
            _value = data;
        }

        public void Append(object name, JsonObject data)
        {
            if (!TryAppend(name, data))
                throw new JsonException(string.Format("Already this object contains {0}", name));
        }

        public bool TryAppend(object name, JsonObject data)
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
                throw new InvalidCastException(string.Format("This json's value is not {0}!!", typeof(T).Name));
            return (T)data._value;
        }
        #endregion

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
    }
}