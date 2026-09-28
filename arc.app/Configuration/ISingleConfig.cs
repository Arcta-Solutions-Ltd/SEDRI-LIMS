using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface ISingleConfig
    {
        Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData);
    }
}
