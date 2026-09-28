using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace arc.app.Configuration.Events
{
    internal class AddDirectTestEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public AddDirectTestEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var copyFormConfiguration = _serviceProvider.GetService<ICopyFormConfiguration>();

            await copyFormConfiguration.Copy(dataToSave, "directtest", 1);

            return 0;
        }
    }
}
