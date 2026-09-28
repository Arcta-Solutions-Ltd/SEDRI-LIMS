using arc.app.Configuration;
using arc.app.SystemConfig;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Settings;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Settings;
internal class EditPatientReferenceQuery : ISingleConfig
{
    private readonly IServiceProvider _serviceProvider;

    internal EditPatientReferenceQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
    {
        var configRepository = _serviceProvider.GetService<IConfigRepository>();

        var queryFilter = new QueryFilterConfig();
        queryFilter.AddString("configname", "patientreference");
        var config = await configRepository.SingleConfigByNameAsync(queryFilter);
        var settingConfig = JsonConvert.DeserializeObject<List<SettingConfig>>(config.Contents);

        var selectorValues = settingConfig.Where(p => p.Id != "patientreference|sequence").Select(p => new { id = p.Id, label = p.Type == "text" ? p.Value : p.Text, value = p.Id });

        var result = new { Id = "editpatientreferenceform", FieldList = selectorValues };

        return JsonConvert.SerializeObject(result);
    }
}
