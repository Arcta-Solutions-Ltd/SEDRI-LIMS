using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.List
{
    internal class DeleteTableEntryEvent : IRun
    {
        private readonly IListRepository _listRepository;

        public DeleteTableEntryEvent(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            await _listRepository.DeleteTableEntryAsync(id);
            return 0;
        }
    }
}
