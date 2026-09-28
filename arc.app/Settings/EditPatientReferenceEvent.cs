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
using arc.common.Models.Config;
using System.Linq;

namespace arc.app.Settings
{
    internal class EditPatientReferenceEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditPatientReferenceEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var configRepository = _serviceProvider.GetService<IConfigRepository>();

            var data = JsonConvert.DeserializeObject<EditPageModel>(dataToSave);

            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("configname", "patientreference");
            var config = await configRepository.SingleConfigByNameAsync(queryFilter);
            var settingConfig = JsonConvert.DeserializeObject<List<SettingConfig>>(config.Contents);

            var returnConfig = new List<SettingConfig>();
            foreach(var item in data.FieldList)
            {
                var nextConfig = settingConfig.FirstOrDefault(c => c.Id == item.Value);
                returnConfig.Add(nextConfig);
            }

            var sequenceConfig = settingConfig.FirstOrDefault(c => c.Id == "patientreference|sequence");
            returnConfig.Add(sequenceConfig);

            config.Contents = JsonConvert.SerializeObject(returnConfig);
            await configRepository.EditCustomEntryAsync(config);

            return 0;
        }
    }
}
