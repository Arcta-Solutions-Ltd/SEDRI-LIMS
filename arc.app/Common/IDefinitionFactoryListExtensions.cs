using Newtonsoft.Json;
using System.Collections.Generic;

namespace arc.app.Common
{
    static class IDefinitionFactoryListExtensions
    {
        public static T GetConfigByName<T>(this List<IDefinitionFactory> definitionFactories, string name)
        {
            foreach (var factory in definitionFactories)
            {
                var result = factory.Create(name);
                if (result != null)
                {
                    return JsonConvert.DeserializeObject<T>(result.Get());
                }
            }
            return default;
        }
    }
}
