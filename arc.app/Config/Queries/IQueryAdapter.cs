using arc.domain.Configuration.QueryConfig;
using System.Threading.Tasks;

namespace arc.app.Config.Queries;

/// <summary>
/// Interface for an adapter responsible for retrieving query configurations.
/// </summary>
public interface IQueryAdapter
{
    /// <summary>
    /// Asynchronously retrieves a query configuration based on the specified query name.
    /// </summary>
    /// <param name="queryName">The name of the query to retrieve the configuration for.</param>
    /// <returns>A task representing the asynchronous operation, containing the query configuration.</returns>
    Task<QueryConfig> GetQueryAsync(string queryName);
}

