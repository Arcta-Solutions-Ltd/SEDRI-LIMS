using arc.common.Models;
using arc.domain.Configuration.ListsConfig;
using System.Threading.Tasks;

namespace arc.app.Config
{
    public interface IDataListFactory
    {
        Task<ListConfig> GetListAsync(string listName, bool includeFixed, TokenInfoModel token, bool parentNodesOnly = false);
    }
}
