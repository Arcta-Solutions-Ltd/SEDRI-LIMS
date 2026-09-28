using arc.app.Common;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace arc.app.Settings
{
    internal class AddPatientReferenceTextEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public AddPatientReferenceTextEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var configRepository = _serviceProvider.GetService<IConfigRepository>();

            var data = JsonConvert.DeserializeObject<SettingsModel>(dataToSave);

            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("configname", "patientreference");
            var config = await configRepository.SingleConfigByNameAsync(queryFilter);
            var settingConfig = JsonConvert.DeserializeObject<List<SettingConfig>>(config.Contents);

            var accessNumberTextfieldId = "patientreference|text";
            var nextTextFieldId = settingConfig.Where(x => x.Id.StartsWith(accessNumberTextfieldId)).Select(s => Regex.Match(s.Id, @"\d+$").Value).Select(s => int.Parse(s)).DefaultIfEmpty().Max() + 1;

            settingConfig.Add(new SettingConfig()
            {
                Id = accessNumberTextfieldId + nextTextFieldId,
                Enabled = true,
                Type = "text",
                Text = "@GenTex@",
                Value = data.TextValue,
                ErrorMessage = "@SetTexNulErr@"
            });

            config.Contents = JsonConvert.SerializeObject(settingConfig);
            await configRepository.EditCustomEntryAsync(config);

            return 0;
        }
    }
}
