using arc.common.Models.Coding;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    public interface IBreakpointRepository
    {
        Task<int> AddBreakpointAsync(string dataToSave);
        Task<int> AddBreakpointApprovalAsync(BreakpointApproval data);
        Task<BreakpointListModel> BreakpointViewByIdQueryAsync(QueryFilterConfig queryFilters);
        Task<Breakpoint> EditBreakpointQueryAsync(QueryFilterConfig queryFilters);
        Task<int> EditBreakpointAsync(string dataToSave);
        Task DeleteBreakpointAsync(string id);
        Task<List<BreakpointLineListModel>> BreakpointLineListByBreakpointIdQueryAsync(QueryFilterConfig queryFilters);
        Task<List<BreakpointListModel>> BreakpointListQueryAsync(QueryFilterConfig queryFilters);
        Task<List<Breakpoint>> GetBreakpointsForOrganismAsync(QueryFilterConfig parameters);
        Task<Breakpoint> BreakpointWithOrganismQueryAsync(QueryFilterConfig queryFilters);
    }
}
