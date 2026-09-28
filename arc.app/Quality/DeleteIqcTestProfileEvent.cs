using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Quality
{
    internal class DeleteIqcTestProfileEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteIqcTestProfileEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string listItemId, EventModel command, EventConfig eventData = null)
        {
            var qualityRepository = _serviceProvider.GetService<IQualityRepository>();

            await qualityRepository.DeleteIqcTestProfileAsync(listItemId);

            return int.Parse(listItemId);
        }
    }
}
