using arc.app.Common;
using arc.common;
using Microsoft.Extensions.DependencyInjection;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;
using System;

namespace arc.app.Configuration.Events
{
    internal class AddCultureTestEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public AddCultureTestEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var copyFormConfiguration  = _serviceProvider.GetService<ICopyFormConfiguration>();

            await copyFormConfiguration.Copy(dataToSave, "culturetest", 2);

            return 0;
        }
    }
}
