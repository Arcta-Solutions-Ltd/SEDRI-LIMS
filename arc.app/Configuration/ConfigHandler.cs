using arc.common.Models;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Configuration;

/// <summary>
/// Handles configuration requests by delegating them to the appropriate configuration handler.
/// </summary>
public class ConfigHandler : IConfigHandler
{
    private readonly IConfigFactory _configFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigHandler"/> class.
    /// </summary>
    /// <param name="configFactory">The factory used to create configuration handlers.</param>
    public ConfigHandler(IConfigFactory configFactory)
    {
        _configFactory = configFactory;
    }

    /// <summary>
    /// Processes the configuration request asynchronously.
    /// </summary>
    /// <param name="contents">The configuration contents.</param>
    /// <param name="queryFilters">The query filter configuration.</param>
    /// <param name="queryData">The query configuration data.</param>
    /// <param name="token">The token information used for authentication or authorization.</param>
    /// <param name="excludeTokenFilters">A flag indicating whether token-based filters should be excluded.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the configuration response as a string.
    /// </returns>
    public async Task<string> HandleAsync(string contents, QueryFilterConfig queryFilters, QueryConfig queryData, TokenInfoModel token, bool excludeTokenFilters = false)
    {
        var configHandler = _configFactory.Create(queryFilters.Name);
        return await configHandler.GetAsync(queryFilters, queryData);
    }
}
