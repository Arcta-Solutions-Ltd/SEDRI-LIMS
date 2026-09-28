using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Reports.InclusionSelectors
{
    public interface ICultureDetailsSelector
    {
        Task<CultureDetailsSelectorModel> GetContentsAsync(QueryFilterConfig queryFilters);
    }
}
