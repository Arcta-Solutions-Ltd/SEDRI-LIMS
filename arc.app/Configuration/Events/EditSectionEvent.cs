using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.ReportsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events
{
    internal class EditSectionEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditSectionEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var sectionDetails = JsonConvert.DeserializeObject<SectionModel>(dataToSave);

            var mapper = _serviceProvider.GetService<IMapType<SectionModel, ReportSectionConfig>>();
            var newSection = mapper.Map(sectionDetails);

            var reportRepository = _serviceProvider.GetService<IReportConfigRepository>();

            await reportRepository.EditSectionAsync(newSection);

            return 0;
        }
    }
}
