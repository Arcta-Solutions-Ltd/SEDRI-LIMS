using arc.app.Common;
using arc.app.Graph;
using arc.common.Models.Graph;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Graph;

/// <summary>
/// Provides graph data retrieval implementations for various graph types via SQL queries and logging.
/// </summary>
public class GraphRepository : IGraphRepository
{
    private readonly ISqlQuery _sqlQuery;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of <see cref="GraphRepository"/>.
    /// </summary>
    /// <param name="sqlQuery">The SQL query executor for graph data.</param>
    /// <param name="logWriter">The logger for informational messages.</param>
    public GraphRepository(ISqlQuery sqlQuery, ILogWriter logWriter)
    {
        _sqlQuery = sqlQuery;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Retrieves specimen type graph data based on specified filters.
    /// </summary>
    /// <param name="queryFilters">Filter configuration for the specimen type graph query.</param>
    /// <returns>A list of <see cref="GraphModel"/> representing specimen type graph data.</returns>
    public async Task<List<GraphModel>> GetSpecimenTypeGraphDataAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get specimen type graph data query", "GraphRepository", "GetSpecimenTypeGraphDataAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new SpecimenTypeGraph(), "Get Specimen Type graph data", queryFilters);
    }

    /// <summary>
    /// Retrieves location graph data based on specified filters.
    /// </summary>
    /// <param name="queryFilters">Filter configuration for the location graph query.</param>
    /// <returns>A list of <see cref="GraphModel"/> representing location graph data.</returns>
    public async Task<List<GraphModel>> GetLocationGraphDataAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get location graph data query", "GraphRepository", "GetLocationGraphDataAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new LocationGraph(), "Get location graph data", queryFilters);
    }

    /// <summary>
    /// Retrieves organisation graph data based on specified filters.
    /// </summary>
    /// <param name="queryFilters">Filter configuration for the organisation graph query.</param>
    /// <returns>A list of <see cref="GraphModel"/> representing organisation graph data.</returns>
    public async Task<List<GraphModel>> GetOrganisationGraphDataAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get organisation graph data query", "GraphRepository", "GetOrganisationGraphDataAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new OrganisationGraph(), "Get organisation graph data", queryFilters);
    }

    /// <summary>
    /// Retrieves tag graph data based on specified filters.
    /// </summary>
    /// <param name="queryFilters">Filter configuration for the tag graph query.</param>
    /// <returns>A list of <see cref="GraphModel"/> representing tag graph data.</returns>
    public async Task<List<GraphModel>> GetTagGraphDataAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get tag graph data query", "GraphRepository", "GetTagGraphDataAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new TagGraph(), "Get tag graph data", queryFilters);
    }

    /// <summary>
    /// Retrieves organism graph data based on specified filters.
    /// </summary>
    /// <param name="queryFilters">Filter configuration for the organism graph query.</param>
    /// <returns>A list of <see cref="GraphModel"/> representing organism graph data.</returns>
    public async Task<List<GraphModel>> GetOrganismGraphDataAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get organism graph data query", "GraphRepository", "GetOrganismGraphDataAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new OrganismGraph(), "Get organism graph data", queryFilters);
    }

    /// <summary>
    /// Retrieves susceptibility graph data based on specified filters.
    /// </summary>
    /// <param name="queryFilters">Filter configuration for the susceptibility graph query.</param>
    /// <returns>A list of <see cref="GraphModel"/> representing susceptibility graph data.</returns>
    public async Task<List<GraphModel>> GetSusceptibilityGraphDataAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get susceptibility graph data query", "GraphRepository", "GetSusceptibilityGraphDataAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new SusceptibilityGraph(), "Get susceptibility graph data", queryFilters);
    }

    /// <summary>
    /// Retrieves test graph data based on specified filters.
    /// </summary>
    /// <param name="queryFilters">Filter configuration for the test graph query.</param>
    /// <returns>A list of <see cref="GraphModel"/> representing test graph data.</returns>
    public async Task<List<GraphModel>> GetTestGraphDataAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get test graph data query", "GraphRepository", "GetTestGraphDataAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new TestGraph(), "Get test graph data", queryFilters);
    }

    /// <summary>
    /// Retrieves gender graph data based on specified filters.
    /// </summary>
    /// <param name="queryFilters">Filter configuration for the gender graph query.</param>
    /// <returns>A list of <see cref="GraphModel"/> representing gender graph data.</returns>
    public async Task<List<GraphModel>> GetGenderGraphDataAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get gender graph data query", "GraphRepository", "GetGenderGraphDataAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new GenderGraph(), "Get gender graph data", queryFilters);
    }

    /// <inheritdoc />
    public async Task<List<GraphModel>> GetSpecimenStateGraphDataAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get specimen state graph data query", "GraphRepository", "GetSpecimenStateGraphDataAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new SpecimenStateGraph(), "Get specimen state graph data", queryFilters);
    }
}
