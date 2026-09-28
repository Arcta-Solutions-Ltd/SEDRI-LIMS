using arc.app.Common;
using arc.common;
using arc.common.Models.Config;
using arc.common.Models.Lists;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.List
{
    internal class OrderTableEvent : IRun
    {
        private readonly IListRepository _listRepository;

        public OrderTableEvent(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var dataModel = JsonConvert.DeserializeObject<OrderTableModel>(dataToSave);
            await _listRepository.OrderListCommandAsync(dataModel.FieldList);
            return 0;
        }
    }
}
