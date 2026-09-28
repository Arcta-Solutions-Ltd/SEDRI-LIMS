using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    internal class AddExportProfileEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public AddExportProfileEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var exportProfile = _serviceProvider.GetService<IExportProfileRepository>();
            return await exportProfile.AddExportProfileAsync(dataToSave);
        }
    }
}
