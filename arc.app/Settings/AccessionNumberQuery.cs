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

namespace arc.app.Settings
{
    internal class AccessionNumberQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal AccessionNumberQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var configRepository = _serviceProvider.GetService<IConfigRepository>();

            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("configname", "accessionnumber");
            var config = await configRepository.SingleConfigByNameAsync(queryFilter);
            var settingConfig = JsonConvert.DeserializeObject<List<SettingConfig>>(config.Contents);
            var returnList = settingConfig.Select(s => new
            {
                s.Id,
                Name = s.Type == "text" ? s.Value : s.Text,
                Enabled = s.Enabled ? "@GenYesA@" : "@GenNo@",
                Enabled2 = s.Enabled2 ? "@GenYesA@" : "@GenNo@",
                stateid = s.Type == "text" ? "AllowDelete" : ""
            });
            return JsonConvert.SerializeObject(returnList);
        }
    }
}
