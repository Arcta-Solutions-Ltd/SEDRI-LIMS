using arc.app.SystemConfig;
using System.Collections.Generic;
using System.Threading.Tasks;
using arc.common.Models.SystemConfig;
using System.Linq;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.common.Utils;
using arc.common.Models.Config;

namespace arc.app.Exports.ExportFieldProcessors;
internal class ExportMappingProcessor
{
    private List<ConfigsModel> _mappings;

    public async Task InitialiseAsync(IConfigRepository configRepository)
    {
        var queryConfig = new QueryFilterConfig();
        var queryValueConfig = new QueryValuesConfig
        {
            Key = "configtypeid",
            Value = "22"
        };
        queryConfig.Parameters.Add(queryValueConfig);
        var res = await configRepository.GetConfigListAsync(queryConfig);
        _mappings = res.ToList();
    }

    public string GetLine(string mapping, string element)
    {
        var newElement = element;
        var map = _mappings.FirstOrDefault(p => p.ConfigName == mapping);
        if (map != null) {

            var items = ArcJson.Deserialize<MappingModel>(map.Contents);
            if (element.Contains('|'))
            {
                var elements = element.Split("|");

                var newE = "";
                var count = 0;

                foreach (var e in elements) {
                    newE += GetNewElement(items, e);

                    if (count < elements.Length - 1)
                    {
                        newE += "|";
                        count++;
                    }
                }
                newElement = newE;
            }
            else
            {
                newElement = GetNewElement(items, element);
            }
        }
        return newElement;
    }

    private string GetNewElement(MappingModel items, string element)
    {
        var newElement = element;
        if (items.Mapping.Any(p => p.BeforeMappingValue.Trim() == newElement.Trim()))
        {
            newElement = items.Mapping.FirstOrDefault(p => p.BeforeMappingValue.Trim() == newElement.Trim()).AfterMappingValue.Trim();
        }
        return newElement;
    }
}
