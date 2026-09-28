using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.Config.Reports;
using Newtonsoft.Json;
using arc.common.Models.Config;
using arc.app.SystemConfig;
using arc.common.Models.SystemConfig;
using arc.data.model.Configuration;

namespace arc.app.Configuration.Events
{
    internal class ReportSectionMoveEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public ReportSectionMoveEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var reportAdapter = _serviceProvider.GetService<IReportAdapter>();
            var configRepository = _serviceProvider.GetService<IConfigRepository>();

            var typeModel = JsonConvert.DeserializeObject<TypeModel>(dataToSave);

            //Get the config for the report

            var idList = id.Split("|");
            var reportDef = await reportAdapter.GetReportAsync(idList[0]);

            if (typeModel.Type == "up")
            {
                reportDef.MoveSectionUp(idList[1]);
            } else
            {
                reportDef.MoveSectionDown(idList[1]);
            }

            //Save the config for the report

            var configModel = new ConfigsDataModel
            {
                ConfigName = idList[0],
                ConfigTypeId = 13,
                Contents = JsonConvert.SerializeObject(reportDef)
            };
            await configRepository.UpdateCustomEntryAsync(configModel);

            return 0;
        }
    }
}
