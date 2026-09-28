using arc.app.Common;
using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.SystemConfig;
using arc.common.Models.SystemConfig;
using arc.app.Config.Reports;
using arc.data.model.Configuration;

namespace arc.app.Configuration.Events
{
    internal class EditReportConfigEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditReportConfigEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            var reportAdapter = _serviceProvider.GetService<IReportAdapter>();
            var configExtractionUtils = _serviceProvider.GetService<IConfigExtractionUtils>();

            var reportToEdit = JsonConvert.DeserializeObject<UpdateReportConfigModel>(dataToSave);

            var idList = reportToEdit.Id.Split("|");

            var view = await configExtractionUtils.GetViewConfigUsingIdAsync(int.Parse(idList[0]));
            var configName = idList[1].ToLower();
            if (reportToEdit.Enabled == "Yes") {
                view.AddReport(configName);
            } else
            {
                view.RemoveReport(configName);
            }

            var reportDef = await reportAdapter.GetReportAsync(configName);
            reportDef.Title = reportToEdit.Title;
            var newConfig = new ConfigsDataModel
            {
                ConfigTypeId = 13,
                ConfigName = configName,
                Contents = JsonConvert.SerializeObject(reportDef)
            };

            //Save report to the database
            await configRepository.UpdateCustomEntryAsync(newConfig);

            //Save the updated view
            await configExtractionUtils.SaveViewAsync(view);

            return 0;
        }
    }
}
