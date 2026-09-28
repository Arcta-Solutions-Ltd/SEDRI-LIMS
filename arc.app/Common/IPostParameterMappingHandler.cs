using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Common
{
    /// <summary>
    /// Defines a handler that runs after query parameter mapping, allowing query-specific side effects (e.g. logging).
    /// </summary>
    public interface IPostParameterMappingHandler
    {
        /// <summary>
        /// Handles post-mapping logic for the specified query.
        /// </summary>
        /// <param name="queryName">The name of the query.</param>
        /// <param name="queryFilters">The mapped query filters.</param>
        Task HandleAsync(string queryName, QueryFilterConfig queryFilters);
    }
}
