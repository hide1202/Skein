using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skein
{
    internal static class Extension
    {
        internal static string ToLog(this Dictionary<object, JsonObject> thisDic)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("{");
            foreach (var key in thisDic.Keys)
            {
                builder.Append(key.ToString());
                builder.Append(":");
                builder.Append(thisDic[key].ToLogString());
            }
            builder.Append("}");
            return builder.ToString();
        }
    }
}
