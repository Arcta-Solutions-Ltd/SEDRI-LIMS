using arc.common.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace arc.common.Utils
{
    public interface IJsonFieldsCollector
    {
        List<JsonFieldModel> GetAllFields(JToken token);
        string GetNamedObject(JToken jToken, string name);
    }
}
