using arc.common.Models;
using arc.common.Models.Lists;
using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IHandleList
    {
        Task<List<ListConfig>> HandleAsync(List<DynamicListModel> listModel, TokenInfoModel token);
    }
}
