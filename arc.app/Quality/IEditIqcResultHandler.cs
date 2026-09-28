using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Quality
{
    public interface IEditIqcResultHandler
    {
        Task<JustCraftedPages> GetInitialDataAsync(QueryFilterConfig queryFilters);
    }
}
