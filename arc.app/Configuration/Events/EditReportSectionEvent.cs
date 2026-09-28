using arc.app.Common;
using arc.app.Config.Reports;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using arc.common.Models.Config;
using System.Linq;
using arc.common.Models.SystemConfig;
using arc.app.SystemConfig;
using arc.data.model.Configuration;

namespace arc.app.Configuration.Events
{
    internal class EditReportSectionEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditReportSectionEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var reportAdapter = _serviceProvider.GetService<IReportAdapter>();
            var configRepository = _serviceProvider.GetService<IConfigRepository>();

            var dataModel = JsonConvert.DeserializeObject<EditReportSectionModel>(dataToSave);
            var idList = dataModel.Id.Split("|");
            var reportName = idList[0];
            var reportSection = idList[1];
            var report = await reportAdapter.GetReportAsync(reportName);

            var sections = dataModel.SectionList.Select(s => s.Value.Split("|")[1]).ToList();

            switch (reportSection)
            {
                case "top":
                    report.MainSections = sections;
                    break;
                case "bottom":
                    report.FinalSections = sections;
                    break;
                default:
                    report.OrganismSections = sections;
                    break;
            }

            var newData = new ConfigsDataModel { ConfigName = report.Name.ToLower(), Contents = JsonConvert.SerializeObject(report), ConfigTypeId = 13 };
            await configRepository.UpdateCustomEntryAsync(newData);
            return 0;
        }
    }
}
