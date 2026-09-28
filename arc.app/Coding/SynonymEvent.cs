using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    internal class SynonymEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public SynonymEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var organismRepository = _serviceProvider.GetService<IOrganismRepository>();
            await organismRepository.EditSynonymsAsync(dataToSave);
            return 0;
        }
    }
}
