using System.Collections.Generic;
using System.Text;

namespace Skein
{
    internal static class Extension
    {
#if DEBUG
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
#endif

        /// <summary>
        /// int value contains min value, except for max value.
        /// </summary>
        /// <param name="thisInt"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        internal static bool IsBetween(this int thisInt, int min, int max)
        {
            return thisInt >= min && thisInt < max;
        }
    }
}
