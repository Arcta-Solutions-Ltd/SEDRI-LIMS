using arc.app.Common;
using arc.app.Specification;
using arc.common;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using arc.domain.Configuration.EventsConfig;

namespace arc.app.Coding
{
    internal class EditSpecificationEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditSpecificationEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Executes the edit specification event, persisting the specification data to the database.
        /// </summary>
        /// <param name="dataToSave">JSON-serialized specification data.</param>
        /// <param name="id">The specification ID (returned as the result).</param>
        /// <param name="command">Event command model.</param>
        /// <param name="eventData">Optional event configuration.</param>
        /// <returns>The specification ID as an integer.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var specificationRepository = _serviceProvider.GetService<ISpecificationRepository>();
            await specificationRepository.EditSpecificationEventAsync(dataToSave);

            return int.Parse(id);
        }
    }
}