using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Monitoring
{
    public interface IFormattedJsonQueryHandler
    {
        Task<string> GetDataAsync(QueryFilterConfig queryFilters);
    }
}
