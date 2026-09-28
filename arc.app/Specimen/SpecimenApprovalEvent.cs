using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.common.Utils;

namespace arc.app.Specimen
{
    /// <summary>
    /// Handles the event for specimen approval.
    /// </summary>
    public class SpecimenApprovalEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecimenApprovalEvent"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
        public SpecimenApprovalEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Executes the specimen approval event asynchronously.
        /// </summary>
        /// <param name="dataToSave">The data that defines the event.</param>
        /// <param name="Id">The identifier for the specimen.</param>
        /// <param name="command">The event command model.</param>
        /// <param name="eventData">Optional event configuration data.</param>
        /// <returns>A task that represents the asynchronous operation. This alwaysreturns 0.</returns>
        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            // Retrieve the specimen repository service.
            var specimenRepository = _serviceProvider.GetService<ISpecimenRepository>();

            // Retrieve the JSON element remover service.
            var jsonRemover = _serviceProvider.GetService<IJsonElementRemover>();

            // Remove specific elements from the data to be saved.
            dataToSave = jsonRemover.RemoveElementsByValue(dataToSave, "Decision");

            // Edit the specimen asynchronously using the specimen repository service.
            await specimenRepository.EditSpecimenAsync(dataToSave, "specimen", command.NewStateId);

            return 0;
        }
    }

}
