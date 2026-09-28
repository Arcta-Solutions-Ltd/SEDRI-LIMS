using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.Security
{
    public class AddOrganisationEvent : IRun
    {
        public IOrganisationRepository _organisationRepository;

        public AddOrganisationEvent(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            return await _organisationRepository.AddAsync(dataToSave);
        }

    }
}
