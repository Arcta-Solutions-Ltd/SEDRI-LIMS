using Newtonsoft.Json.Linq;

namespace arc.common.Utils
{
    public interface IJsonReplacer
    {
        string ChangeValueInJsonString(string json, string key, string newValue);
        string GetValueInJsonString(string json, string key);
        string AddNewJsonValue(string json, string key, JObject value);
        string AddNewStringValue(string json, string key, string value);
        string ChangeKey(string json, string oldKey, string newKey);
        string DeleteStringValue(string json, string key);
    }
}
