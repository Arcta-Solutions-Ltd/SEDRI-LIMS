using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    internal class AddCustomEntryEvent : IRun
    {
        private readonly ICodingRepository _codingRepository;

        public AddCustomEntryEvent(ICodingRepository codingRepository)
        {
            _codingRepository = codingRepository;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            return await _codingRepository.AddCustomEntryAsync(dataToSave);
        }
    }
}
