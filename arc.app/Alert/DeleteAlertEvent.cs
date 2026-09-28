using arc.app.Common;
using arc.common;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.domain.Configuration.EventsConfig;

namespace arc.app.Alert
{
    internal class DeleteAlertEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteAlertEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var alertRepository = _serviceProvider.GetService<IAlertRepository>();
            await alertRepository.DeleteAlertAsync(id);

            return int.Parse(id);
        }
    }
}
