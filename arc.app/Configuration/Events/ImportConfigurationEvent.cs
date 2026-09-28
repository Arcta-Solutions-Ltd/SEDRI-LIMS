using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.SystemConfig;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Runs the event which imports configuration information.
    /// </summary>
    internal class ImportConfigurationEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public ImportConfigurationEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Runs the event which imports configuration information.
        /// </summary>
        /// <param name="dataToSave">Json data which contains the information which arrived with the event request</param>
        /// <param name="id">The id of the record the event corresponds to if required</param>
        /// <param name="command">General information about the event</param>
        /// <param name="eventData">Meta data the system holds describing the event</param>
        /// <returns>String with each attribute in the model divided by the delimiter specified</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var configRepository = _serviceProvider.GetService<IConfigRepository>();

            await configRepository.ImportConfigurationAsync(dataToSave);

            return 0;
        }
    }
}
