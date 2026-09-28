using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Reports;

public interface IReportInclusionSelector
{
    Task<string> GetFilterListsAsync(QueryFilterConfig queryFilters);
}
