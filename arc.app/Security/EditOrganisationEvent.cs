using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.Security
{
    /// <summary>
    /// Handles the edit client organisation event by persisting updated organisation details.
    /// </summary>
    public class EditOrganisationEvent : IRun
    {
        private readonly IOrganisationRepository _organisationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditOrganisationEvent"/> class.
        /// </summary>
        /// <param name="organisationRepository">Repository used to persist organisation changes.</param>
        public EditOrganisationEvent(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        /// <summary>
        /// Persists the edited organisation, including enabled/disabled state changes.
        /// </summary>
        /// <param name="dataToSave">Serialised organisation payload from the edit form.</param>
        /// <param name="Id">The organisation id being edited.</param>
        /// <param name="command">The event command metadata.</param>
        /// <param name="eventData">Optional event configuration.</param>
        /// <returns>The number of rows affected by the update.</returns>
        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            return await _organisationRepository.EditAsync(dataToSave);
        }

    }
}
