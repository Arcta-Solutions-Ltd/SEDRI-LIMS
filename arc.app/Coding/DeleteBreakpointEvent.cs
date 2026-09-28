using arc.app.Common;
using arc.common;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.domain.Configuration.EventsConfig;

namespace arc.app.Coding
{
    internal class DeleteBreakpointEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteBreakpointEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var breakpointRepository = _serviceProvider.GetService<IBreakpointRepository>();
            await breakpointRepository.DeleteBreakpointAsync(id);

            return int.Parse(id);
        }
    }
}
