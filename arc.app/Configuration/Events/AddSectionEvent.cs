using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using arc.common.Models.Config;
using arc.domain.Configuration.ReportsConfig;
using arc.app.SystemConfig;

namespace arc.app.Configuration.Events
{
    internal class AddSectionEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public AddSectionEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var sectionDetails = JsonConvert.DeserializeObject<SectionModel>(dataToSave);

            var mapper = _serviceProvider.GetService<IMapType<SectionModel, ReportSectionConfig>>();
            var newSection = mapper.Map(sectionDetails);

            var reportRepository = _serviceProvider.GetService<IReportConfigRepository>();

            await reportRepository.AddNewSectionAsync(newSection);

            return 0;
        }
    }
}

