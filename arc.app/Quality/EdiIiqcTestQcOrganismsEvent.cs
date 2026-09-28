using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Quality
{
    internal class EdiIiqcTestQcOrganismsEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EdiIiqcTestQcOrganismsEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            return await _serviceProvider.GetService<IQualityRepository>().EditIqcTestQcOrganismsAsync(dataToSave);
        }
    }
}
