using arc.app.Common;
using arc.app.Configuration;
using arc.app.SystemConfig;
using arc.common.Models.Settings;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Settings;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Settings
{
    internal class EditSettingQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        public EditSettingQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            var listRepository = _serviceProvider.GetService<IListRepository>();

            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;
            var idList = id.Split("|");
            var category = idList[0];
            var settingId = idList[1];

            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("configname", category);
            var config = await configRepository.SingleConfigByNameAsync(queryFilter);
            var settingConfig = JsonConvert.DeserializeObject<List<SettingConfig>>(config.Contents);

            var currentSetting = settingConfig.FirstOrDefault(c => c.Id.ToLower() == id.ToLower());

            var enabled = currentSetting.Enabled ? "Yes" : "No";
            var enabled2 = currentSetting.Enabled2 ? "Yes" : "No";

            var mapping = new List<SettingsMappingModel>();
            if (currentSetting.Type == "mapping")
            {
                var specimenTypeList = await listRepository.GetListValuesByIdAsync(4);
                mapping = specimenTypeList.Select(v => new SettingsMappingModel { Key = v.Key, MapType = v.Text, MapValue = "" }).ToList();
                foreach (var item in mapping)
                {
                    var thisSetting = currentSetting.MappingValues.FirstOrDefault(v => v.Type == int.Parse(item.Key));
                    item.MapValue = thisSetting == null ? "" : thisSetting.Value;
                }
            }
            if ((category == "accessionnumber" || category == "patientreference") && settingId.StartsWith("text"))
            {
                currentSetting.TextValue = currentSetting.Value;
            }

            var returnValue = new
            {
                Setting = currentSetting.Text,
                Category = category,
                SettingId = settingId,
                currentSetting.Type,
                value = currentSetting.Value,
                Enabled = enabled,
                Enabled2 = enabled2,
                YearSetting = currentSetting.Value,
                MappingGrid = mapping,
                currentSetting.TextValue
            };

            return JsonConvert.SerializeObject(returnValue);
        }
    }
}
