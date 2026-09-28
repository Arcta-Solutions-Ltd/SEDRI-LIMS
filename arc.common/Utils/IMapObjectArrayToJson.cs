using System.Collections.Generic;

namespace arc.common.Utils
{
    public interface IMapObjectArrayToJson
    {
        string Map<T>(List<T> objectList, string startsWith = "");
    }
}
