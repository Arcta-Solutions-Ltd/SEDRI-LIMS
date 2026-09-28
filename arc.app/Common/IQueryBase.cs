using arc.common.Models;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IQueryBase
    {
        Task<string> HandleAsync(string queryName, QueryFilterConfig queryFilters, QueryConfig queryData, TokenInfoModel token = null, bool excludeTokenFilters = false);
    }
}
