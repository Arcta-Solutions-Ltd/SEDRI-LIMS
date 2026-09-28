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
    internal class DeleteSettingQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteSettingQuery(IServiceProvider serviceProvider)
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

            var mapping = new List<SettingsMappingModel>();

            var returnValue = new
            {
                Setting = currentSetting.Text,
                Category = category,
                SettingId = settingId,
                currentSetting.Type,
                value = currentSetting.Value,
                Enabled = enabled,
                YearSetting = currentSetting.Value,
                MappingGrid = mapping,
                currentSetting.TextValue
            };

            return JsonConvert.SerializeObject(returnValue);
        }
    }
}
