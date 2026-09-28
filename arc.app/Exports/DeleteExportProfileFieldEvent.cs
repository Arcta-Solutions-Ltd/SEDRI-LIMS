using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    internal class DeleteExportProfileFieldEvent:IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteExportProfileFieldEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var exportProfile = _serviceProvider.GetService<IExportProfileFieldRepository>();
            await exportProfile.DeleteExportProfileFieldAsync(Id);
            return int.Parse(Id);
        }
    }
}
