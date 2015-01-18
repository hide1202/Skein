using Skein.Exception;

namespace Skein
{
    public class JsonReader
    {
        public static JsonObject Parse(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new JsonParseException("Input is null!!");

            if (json.Trim() == string.Empty)
                throw new JsonParseException("Input is empty!!");

            Parser.JsonObjectParser objectParser = new Parser.JsonObjectParser(json);
            JsonObject result = objectParser.Parse();
            return result;
        }
    }
}