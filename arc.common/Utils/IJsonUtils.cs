using System.Collections.Generic;

namespace arc.common.Utils
{
    public interface IJsonUtils
    {
        Dictionary<string, string> ExtractFields(string jsonMessage, bool raw = false);
        string GetSingleFieldValue(string jsonMessage, string fieldValue, bool raw = false);
        List<string> SplitStringIntoFields(string contents, char separator);
        string RemoveFirstAndLast(string contents);
    }
}
