using arc.common;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.Common;
using arc.domain.Configuration.EventsConfig;

namespace arc.app.Location
{
    public class EditLocationEvent : IRun
    {
        public IServiceProvider _serviceProvider;

        public EditLocationEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var locationRepository = _serviceProvider.GetService<ILocationRepository>();
            return await locationRepository.EditAsync(dataToSave);
        }
    }
}
