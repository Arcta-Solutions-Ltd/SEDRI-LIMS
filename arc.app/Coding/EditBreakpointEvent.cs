using arc.app.Common;
using arc.common;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using arc.domain.Configuration.EventsConfig;

namespace arc.app.Coding
{
    internal class EditBreakpointEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditBreakpointEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var breakpointRepository = _serviceProvider.GetService<IBreakpointRepository>();
            return await breakpointRepository.EditBreakpointAsync(dataToSave);
        }
    }
}
