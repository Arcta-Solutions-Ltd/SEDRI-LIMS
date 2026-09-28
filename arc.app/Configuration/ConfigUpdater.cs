using arc.app.SystemConfig;
using arc.common.Models.SystemConfig;
using arc.data.model.Configuration;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public class ConfigUpdater : IConfigUpdater
    {
        private IConfigRepository _configRepository;

        public ConfigUpdater(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public async Task Edit(string configName, int configTypeId, string contents)
        {
            var newConfig = new ConfigsDataModel
            {
                ConfigTypeId = configTypeId,
                ConfigName = configName,
                Contents = contents
            };

            //Save report to the database
            await _configRepository.UpdateCustomEntryAsync(newConfig);
        }

        public async Task Delete(string configName)
        {
            await _configRepository.DeleteCustomEntryAsync(configName);
        }
    }
}
