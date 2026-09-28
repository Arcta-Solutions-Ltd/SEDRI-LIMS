using arc.app.Common;
using arc.app.Settings.SettingProviders;
using arc.app.SystemConfig;
using arc.common.Models.Settings;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Settings;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Settings
{
    internal class EditSettingValidator : ISpecialValidatorAsync
    {
        private readonly string _message;
        private readonly IConfigRepository _configRepository;
        private readonly ISettingProviderFactory _settingProviderFactory;

        public EditSettingValidator(IConfigRepository configRepository, ISettingProviderFactory settingProviderFactory, string message)
        {
            _message = message;
            _configRepository = configRepository;
            _settingProviderFactory = settingProviderFactory;
        }

        public async Task<string> ValidateMessageAsync()
        {
            var data = JsonConvert.DeserializeObject<SettingsModel>(_message);

            var idList = data.Id.Split("|");

            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("configname", idList[0]);
            var config = await _configRepository.SingleConfigByNameAsync(queryFilter);
            var settingConfig = JsonConvert.DeserializeObject<List<SettingConfig>>(config.Contents);

            var currentSetting = settingConfig.FirstOrDefault(c => c.Id.ToLower() == data.Id.ToLower());

            var provider = _settingProviderFactory.GetProvider(currentSetting.Type);

            return provider == null ? "" : provider.Validate(currentSetting, data);
        }
    }
}
