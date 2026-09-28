using arc.app.Common;
using arc.app.Settings.SettingProviders;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Models.Settings;
using arc.domain.Configuration.EventsConfig;
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
    internal class EditSettingEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditSettingEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            var accessionNumberProviderFactory = _serviceProvider.GetService<ISettingProviderFactory>();

            var data = JsonConvert.DeserializeObject<SettingsModel>(dataToSave);
            if (data.Id.StartsWith("accessionnumber|text") || data.Id.StartsWith("patientreference|text"))
            {
                data.Value = data.TextValue;
            }
            var idList = id.Split("|");

            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("configname", idList[0]);
            var config = await configRepository.SingleConfigByNameAsync(queryFilter);
            var settingConfig = JsonConvert.DeserializeObject<List<SettingConfig>>(config.Contents);

            var currentSetting = settingConfig.FirstOrDefault(c => c.Id.ToLower() == id.ToLower());

            var provider = accessionNumberProviderFactory.GetProvider(currentSetting.Type);

            if (provider != null)
            {
                currentSetting = provider.SetValue(currentSetting, data);
            }
            currentSetting.Enabled = data.Enabled == "Yes" || data.Id ==  $"{idList[0]}|sequence" ? true : false;
            currentSetting.Enabled2 = data.Enabled2 == "Yes" && data.Id != $"{idList[0]}|sequence" ? true : false;

            config.Contents = JsonConvert.SerializeObject(settingConfig);
            await configRepository.EditCustomEntryAsync(config);

            return 0;
        }
    }
}
