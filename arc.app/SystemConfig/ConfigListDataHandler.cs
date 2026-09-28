using arc.common.Models.SystemConfig;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.SystemConfig
{
    public  class ConfigListDataHandler : IConfigListDataHandler
    {
        private readonly IConfigRepository _configRepository;
        private readonly IConfigFactoryProcessor _configFactoryProcessor;

        public ConfigListDataHandler (IConfigRepository configRepository, IConfigFactoryProcessor configFactoryProcessor)
        {
            _configRepository = configRepository;
            _configFactoryProcessor = configFactoryProcessor;
        }

        public async Task<List<OptionsConfig>> GetTestListAsync()
        {
            return await GetList("1,2");
        }

        public async Task<List<OptionsConfig>> GetDirectTestListAsync()
        {
            return await GetList("1");
        }

        public async Task<List<OptionsConfig>> GetCultureTestListAsync()
        {
            return await GetList("2");
        }

        public async Task<List<OptionsConfig>> GetReportListAsync()
        {
            return await GetList("13");
        }

        public async Task<List<OptionsConfig>> GetDataSectionListAsync()
        {
            return await GetList("16");
        }

        public async Task<List<OptionsConfig>> GetMappingListAsync()
        {
            return await GetListWithConfigNameAsync("22");
        }

        public async Task<List<OptionsConfig>> GetList(string configValue)
        {
            var parameters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "ConfigTypeId", Value = configValue } } };
            var listFromDatabase = await _configRepository.GetConfigListAsync(parameters);

            var itemlist = new List<OptionsConfig>();
            foreach (var item in listFromDatabase)
            {
                var contents = item.Contents == null || item.Contents == "{}" ? _configFactoryProcessor.GetInternalConfig(item) : item.Contents;
                var newEntry = JsonConvert.DeserializeObject<NameConfigModel>(contents);
                itemlist.Add(new OptionsConfig { Key = newEntry.Name, Text = newEntry.Title });
            }

            return itemlist.OrderBy(s => s.Key).ToList();
        }

        public async Task<List<OptionsConfig>> GetListWithConfigNameAsync(string configValue)
        {
            var parameters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "ConfigTypeId", Value = configValue } } };
            var listFromDatabase = await _configRepository.GetConfigListAsync(parameters);

            var itemlist = new List<OptionsConfig>();
            foreach (var item in listFromDatabase)
            {
                var contents = item.Contents == null || item.Contents == "{}" ? _configFactoryProcessor.GetInternalConfig(item) : item.Contents;
                var newEntry = JsonConvert.DeserializeObject<NameConfigModel>(contents);
                itemlist.Add(new OptionsConfig { Key = item.ConfigName, Text = newEntry.Name});
            }

            return itemlist.OrderBy(s => s.Key).ToList();
        }
    }
}
