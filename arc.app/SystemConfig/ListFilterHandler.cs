using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using arc.app.Common;
using arc.domain.Configuration.ListsConfig;

namespace arc.app.SystemConfig
{
    public class ListFilterHandler : IListFilterHandler
    {
        private readonly IListRepository _listRepository;

        public ListFilterHandler(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }

        public async Task<List<OptionsConfig>> GetGridFieldListAsync()
        {
            var fieldList = await _listRepository.GetListValuesAsync("fieldtypelist", true);

            var fieldListItemIds = new List<string>()
            {
                "450",
                "453",
                "454",
                "457",
                "458"
            };

            return fieldList
                .Where(f => fieldListItemIds.Any(id => id == f.Key))
                .ToList();
        }
    }
}
