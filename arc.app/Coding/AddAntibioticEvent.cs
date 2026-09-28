using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    internal class AddAntibioticEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public AddAntibioticEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var antibioticRepository = _serviceProvider.GetService<IAntibioticRepository>();
            return await antibioticRepository.AddAntibioticAsync(dataToSave);
        }
    }
}
