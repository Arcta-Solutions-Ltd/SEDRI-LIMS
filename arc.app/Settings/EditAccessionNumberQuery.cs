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
    internal class EditAccessionNumberQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal EditAccessionNumberQuery(IServiceProvider serviceProvider)
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

            var selectorValues = settingConfig.Where(p => p.Id != "accessionnumber|sequence").Select(p => new { id = p.Id, label = p.Type == "text" ? p.Value : p.Text, value = p.Id });

            var result = new { Id = "editaccessionnumberform", FieldList = selectorValues };

            return JsonConvert.SerializeObject(result);
        }
    }
}
