using System.Collections.Generic;

namespace arc.common.Utils
{
    public class JsonUtils : IJsonUtils
    {
        public Dictionary<string, string> ExtractFields(string jsonMessage, bool raw = false)
        {
            var returnList = new Dictionary<string, string>();

            jsonMessage = RemoveFirstAndLast(jsonMessage);
            var fields = SplitStringIntoFields(jsonMessage, ',');

            foreach (var field in fields)
            {
                var splitField = SplitStringIntoFields(field, ':');

                if (splitField[1] != "null") {
                    var fieldValue = raw ? splitField[1].Trim() : splitField[1].Trim().Replace("\"", "").Replace("'", "");
                    returnList.Add(splitField[0].Trim().Replace("\"", "").Replace("'", ""), fieldValue);
                }
            }

            return returnList;
        }

        public string GetSingleFieldValue(string jsonMessage, string fieldValue, bool raw = false)
        {
            var fields = ExtractFields(jsonMessage, raw);
            if (fields.ContainsKey(fieldValue))
            {
                return fields[fieldValue];
            }
            return "";
        }

        public List<string> SplitStringIntoFields(string contents, char separator)
        {
            var insideJsonCount = 0;
            var newFieldList = new List<string>();
            var count = 0;
            var startPos = 0;

            foreach (var character in contents)
            {
                if (character == separator && insideJsonCount == 0)
                {
                    var newField = contents.Substring(startPos, count - startPos).Trim();
                    startPos = count + 1;
                    newFieldList.Add(newField);
                }
                if (character == '{' || character == '[') { insideJsonCount += 1; } 
                if (character == '}' || character == ']') { insideJsonCount -= 1; }

                count++;
            }

            var lastField = contents.Substring(startPos, count - startPos).Trim();
            newFieldList.Add(lastField);

            return newFieldList;
        }

        public string RemoveFirstAndLast(string contents)
        {
            var returnValue = contents.Trim().Remove(0, 1);
            var pos = returnValue.Length - 1;
            return returnValue.Remove(pos, 1).Trim();
        }
    }
}
