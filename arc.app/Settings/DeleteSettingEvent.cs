using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.SystemConfig;
using Newtonsoft.Json;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Settings;
using arc.common.Models.Settings;

namespace arc.app.Settings
{
    internal class DeleteSettingEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteSettingEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var configRepository = _serviceProvider.GetService<IConfigRepository>();

            var data = JsonConvert.DeserializeObject<SettingsModel>(dataToSave);

            var queryFilter = new QueryFilterConfig();
            var idList = id.Split("|");
            queryFilter.AddString("configname", idList[0]);
            var config = await configRepository.SingleConfigByNameAsync(queryFilter);
            var settingConfig = JsonConvert.DeserializeObject<List<SettingConfig>>(config.Contents);

            settingConfig.RemoveAll(x => x.Id == data.Id);

            config.Contents = JsonConvert.SerializeObject(settingConfig);
            await configRepository.EditCustomEntryAsync(config);

            return 0;
        }
    }
}
