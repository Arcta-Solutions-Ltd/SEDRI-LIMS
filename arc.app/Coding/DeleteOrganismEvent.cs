using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    public class DeleteOrganismEvent : IRun
    {
        private readonly ICodingRepository _codingRepository;

        public DeleteOrganismEvent(ICodingRepository codingRepository)
        {
            _codingRepository = codingRepository;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            await _codingRepository.DeleteOrganismAsync(Id);

            return int.Parse(Id);
        }
    }
}
