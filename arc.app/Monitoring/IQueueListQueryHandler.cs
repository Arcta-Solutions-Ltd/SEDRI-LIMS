using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Monitoring
{
    /// <summary>
    /// Handles the QueueList query with hash chain validation.
    /// Computes validity for each record and returns the list with an IsValid indicator.
    /// </summary>
    public interface IQueueListQueryHandler
    {
        /// <summary>
        /// Executes the QueueList query with hash validation and returns the result as JSON.
        /// </summary>
        /// <param name="queryFilters">The query filters (including token filters applied by HandleQuery).</param>
        /// <param name="token">The token information model.</param>
        /// <returns>A JSON string containing the queue list with isValid for each record.</returns>
        Task<string> GetQueueListAsync(QueryFilterConfig queryFilters, arc.common.Models.TokenInfoModel token);
    }
}
