using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    internal class DeleteExportProfileEvent: IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteExportProfileEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var exportProfile = _serviceProvider.GetService<IExportProfileRepository>();
            await exportProfile.DeleteExportProfileAsync(Id);
            return int.Parse(Id);
        }
    }
}
