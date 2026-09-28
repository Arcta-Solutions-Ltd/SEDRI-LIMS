using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface ITableEntryHandler
    {
        Task<string> AddTableEntryAsync(QueryFilterConfig queryFilters);
        Task<string> EditTableEntryAsync(QueryFilterConfig queryFilters);
    }
}
