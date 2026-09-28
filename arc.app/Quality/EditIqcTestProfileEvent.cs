using arc.app.Common;
using arc.common;
using arc.common.Models.QualityAssurance;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Quality
{
    public class EditIqcTestProfileEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditIqcTestProfileEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var craftedData = JsonConvert.DeserializeObject<QualityCraftedModel>(dataToSave);
            return await _serviceProvider.GetService<IQualityRepository>().EditIqcTestProfileAsync(craftedData);
        }
    }
}
