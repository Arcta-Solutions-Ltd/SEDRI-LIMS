using System;
using System.Threading.Tasks;
using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace arc.app.Configuration.Events
{
    internal class DeleteReportConfigEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteReportConfigEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var configExtractionUtils = _serviceProvider.GetService<IConfigExtractionUtils>();
            var reportConfigDefinition = _serviceProvider.GetService<IReportConfigDefinition>();
            var reportConfigRepository = _serviceProvider.GetService<IReportConfigRepository>();

            var reportToDelete = JsonConvert.DeserializeObject<UpdateReportConfigModel>(dataToSave);

            var idList = reportToDelete.Id.Split("|");

            var view = await configExtractionUtils.GetViewConfigUsingIdAsync(int.Parse(idList[0]));
            var configName = idList[1].ToLower();
            view.RemoveReport(configName);

            if (configName.ToLower() != "defaultspecimenreport")
            {
                var report = await reportConfigDefinition.LoadReportAsync(configName);

                //Delete report to the database
                await reportConfigRepository.DeleteReportAsync(report);

                //Save the updated view
                await configExtractionUtils.SaveViewAsync(view);
            }
            return 0;
        }
    }
}
