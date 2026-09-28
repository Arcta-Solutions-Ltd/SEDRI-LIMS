using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Quality
{
    public interface IRunIqcTestHandler
    {
        Task<JustCraftedPages> GetInitialDataAsync(QueryFilterConfig queryFilters);
    }
}
