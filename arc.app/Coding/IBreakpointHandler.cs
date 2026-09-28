using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    public interface IBreakpointHandler
    {
        Task<string> GetBreakpoint(QueryFilterConfig queryFilters);
    }
}
