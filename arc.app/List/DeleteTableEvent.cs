using arc.app.Common;
using arc.common;
using arc.common.Models.Lists;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.List
{
    internal class DeleteTableEvent : IRun
    {
        private readonly IListRepository _listRepository;

        public DeleteTableEvent(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var data = JsonConvert.DeserializeObject<TableModel>(dataToSave);

            await _listRepository.DeleteTableAsync(data.ListId);
            return 0;
        }
    }
}
