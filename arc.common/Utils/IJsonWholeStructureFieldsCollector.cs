using arc.common.Models.Monitoring;
using System.Collections.Generic;

namespace arc.common.Utils
{
    public interface IJsonWholeStructureFieldsCollector
    {
        Dictionary<string, string> GetFields();
        List<JsonItemModel> GetStructure(string message);
        List<JsonArrayModel> GetJsonArray(string message);
    }
}
