using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    internal class AddBreakpointEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public AddBreakpointEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var breakpointRepository = _serviceProvider.GetService<IBreakpointRepository>();
            return await breakpointRepository.AddBreakpointAsync(dataToSave);
        }
    }
}
