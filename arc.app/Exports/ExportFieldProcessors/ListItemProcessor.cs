using arc.app.Common;
using arc.data.model.Lists;
using arc.domain.Configuration.PagesConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Exports.ExportFieldProcessors
{
    internal class ListItemProcessor
    {

        private List<ListItemDataModel> _listItems;

        public async Task InitialiseAsync(IListRepository listRepository)
        {
            var res = await listRepository.GetAllAsync<ListItemDataModel>("listitem");
            _listItems = res.ToList();
        }

        public string GetListItem(string id)
        {
            if (string.Equals(id, OtherOptionConstants.Key, StringComparison.Ordinal))
            {
                return "Other";
            }

            return Int32.TryParse(id, out var idAsInt) ? _listItems.FirstOrDefault(p => p.Id.ToString() == id).Value : id;
        }
    }
}
