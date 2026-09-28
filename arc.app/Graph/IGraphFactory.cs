using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Graph
{
    public interface IGraphFactory
    {
        Task<string> GetGraphDataAsync(QueryFilterConfig queryFilter, TokenInfoModel token);
    }
}
