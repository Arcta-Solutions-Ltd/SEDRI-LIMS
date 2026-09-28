using arc.app.SystemConfig;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config.Queries;

/// <summary>
/// Adapter for retrieving and managing query configurations.
/// </summary>
public class QueryAdapter : IQueryAdapter
{
    /// <summary>
    /// Factory for creating query configurations.
    /// </summary>
    private readonly IQueryFactory _queryFactory;

    /// <summary>
    /// Repository for accessing configuration records from the data source.
    /// </summary>
    private readonly IConfigRepository _configRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryAdapter"/> class.
    /// </summary>
    /// <param name="queryFactory">The query configuration factory to use.</param>
    /// <param name="configRepository">The configuration repository to use.</param>
    public QueryAdapter(IQueryFactory queryFactory, IConfigRepository configRepository)
    {
        _queryFactory = queryFactory;
        _configRepository = configRepository;
    }

    /// <summary>
    /// Asynchronously retrieves a query configuration based on the specified query name.
    /// </summary>
    /// <param name="queryName">The name of the query to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the query configuration,
    /// or null if no valid configuration record is found.
    /// </returns>
    public async Task<QueryConfig> GetQueryAsync(string queryName)
    {
        var parameters = new QueryFilterConfig
        {
            Parameters = new List<QueryValuesConfig>
            {
                new QueryValuesConfig { Key = "ConfigName", Value = queryName }
            }
        };

        var configRecord = await _configRepository.SingleConfigByNameAsync(parameters);

        var queryDef = configRecord.Contents == null || configRecord.Contents == "{}"
            ? _queryFactory.GetQuery(queryName)
            : JsonConvert.DeserializeObject<QueryConfig>(configRecord.Contents);

        return queryDef;
    }
}

