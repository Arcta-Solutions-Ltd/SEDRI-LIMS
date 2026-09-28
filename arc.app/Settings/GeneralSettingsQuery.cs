using arc.app.Configuration;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using arc.app.SystemConfig;
using Newtonsoft.Json;
using arc.domain.Settings;
using System.Linq;

namespace arc.app.Settings
{
    internal class GeneralSettingsQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GeneralSettingsQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var configRepository = _serviceProvider.GetService<IConfigRepository>();

            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("configname", "generalsettings");
            var config = await configRepository.SingleConfigByNameAsync(queryFilter);
            var settingConfig = JsonConvert.DeserializeObject<List<SettingConfig>>(config.Contents);
            var returnList = settingConfig.Select(s => new { s.Id , Name = s.Text, Enabled = s.Enabled ? "@GenYesA@" : "@GenNo@" });
            return JsonConvert.SerializeObject(returnList);
        }
    }
}
