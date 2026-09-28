using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.SystemConfig
{
    public class ConfigItemHandler : IConfigItemHandler
    {
        private readonly IConfigRepository _configRepository;
        private readonly IConfigFactoryProcessor _configFactoryProcessor;

        public ConfigItemHandler(IConfigRepository configRepository, IConfigFactoryProcessor configFactoryProcessor)
        {
            _configRepository = configRepository;
            _configFactoryProcessor = configFactoryProcessor;
        }

        public async Task<string> GetSingleItemAsync(string configName)
        {
            var parameters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "ConfigName", Value = configName } } };
            var configRecord = await _configRepository.SingleConfigByNameAsync(parameters);

            var contents = configRecord.Contents == null || configRecord.Contents == "{}" ? _configFactoryProcessor.GetInternalConfig(configRecord) : configRecord.Contents;

            return contents;
        }
    }
}
