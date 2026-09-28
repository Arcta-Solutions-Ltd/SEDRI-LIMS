using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    public class RunExportEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public RunExportEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            return Task.FromResult(0);
        }
    }
}
