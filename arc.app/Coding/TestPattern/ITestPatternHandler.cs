using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    public interface ITestPatternHandler
    {
        Task<string> GetTestPatternAsync(QueryFilterConfig queryFilters);
    }
}
