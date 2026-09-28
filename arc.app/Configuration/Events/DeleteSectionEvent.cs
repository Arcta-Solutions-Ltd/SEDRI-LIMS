using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.SystemConfig;

namespace arc.app.Configuration.Events
{
    internal class DeleteSectionEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteSectionEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var reportRepository = _serviceProvider.GetService<IReportConfigRepository>();

            await reportRepository.DeleteSectionAsync(id);
            return 0;
        }
    }
}
