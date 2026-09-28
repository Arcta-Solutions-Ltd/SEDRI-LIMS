using arc.domain.Configuration.ViewConfig.ListViewConfig;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.app.SystemConfig;
using arc.app.Config;
using arc.common.Models.SystemConfig;
using Newtonsoft.Json;
using arc.data.model.Configuration;

namespace arc.app.Configuration
{
    public class ConfigExtractionUtils : IConfigExtractionUtils
    {
        private readonly IServiceProvider _serviceProvider;

        public ConfigsDataModel Record { get; private set; }

        public ConfigExtractionUtils(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ListViewConfig> GetViewConfigUsingIdAsync(int id)
        {
            var queryFilter = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "id", Value = id.ToString() } } };
            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            Record = await configRepository.SingleConfigByIdAsync(queryFilter);

            var listViewFactory = _serviceProvider.GetService<IListViewConfigFactory>();
            return await listViewFactory.GetViewAsync(Record.ConfigName);
        }

        public async Task SaveViewAsync(ListViewConfig view) 
        {
            var record = Record;
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            record.Contents = JsonConvert.SerializeObject(view, settings);

            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            await configRepository.UpdateCustomEntryAsync(record);

            var configCache = _serviceProvider.GetService<IConfigCache>();
            if (configCache.isLoaded())
            {
                configCache.UpsertConfig(record);
            }
        }


    }
}
