using Newtonsoft.Json;
using System.Collections.Generic;

namespace arc.common.Utils
{
    public class MapObjectArrayToJson : IMapObjectArrayToJson
    {
        public string Map<T>(List<T> objectList, string startsWith = "") 
        {
            var json = "[ " + startsWith;
            foreach (var view in objectList)
            {
                var newView = JsonConvert.SerializeObject(view).Replace("null", "\"\"");
                json += json == "[ " ? newView : ", " + newView;
            }
            return json += "]";
        }
    }
}
