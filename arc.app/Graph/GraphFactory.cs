using arc.app.Common;
using arc.common.Models;
using arc.common.Models.Graph;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Graph;

/// <summary>
/// Factory responsible for orchestrating graph data retrieval and JSON serialization
/// based on the provided query filter.
/// </summary>
public class GraphFactory : IGraphFactory
{
    /// <summary>
    /// Repository used to fetch raw graph data for various categories.
    /// </summary>
    private readonly IGraphRepository _graphRepository;
    /// <summary>
    /// Service used to interpret or transform test names in the graph data.
    /// </summary>
    private readonly ITestNameInterpreter _testNameInterpreter;

    /// <summary>
    /// Logger for diagnostic messages when tracing graph data requests in production.
    /// </summary>
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="GraphFactory"/> class.
    /// </summary>
    /// <param name="graphRepository">Repository used to fetch graph data.</param>
    /// <param name="testNameInterpreter">Interpreter applied to test graph data.</param>
    /// <param name="logWriter">Logger for graph request diagnostics.</param>
    public GraphFactory(IGraphRepository graphRepository, ITestNameInterpreter testNameInterpreter, ILogWriter logWriter)
    {
        _graphRepository = graphRepository;
        _testNameInterpreter = testNameInterpreter;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Retrieves graph data according to the <paramref name="queryFilter"/>'s Name,
    /// invokes the appropriate repository method, applies any necessary interpretation,
    /// and returns the result as a JSON string.
    /// </summary>
    /// <param name="queryFilter">Configuration defining which graph to fetch and filters to apply.</param>
    /// <param name="token">Token information for the current request context.</param>
    /// <returns>JSON-formatted string representing the list of <see cref="GraphModel"/> entries.</returns>
    public async Task<string> GetGraphDataAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var paramSummary = queryFilter.Parameters == null || queryFilter.Parameters.Count == 0
            ? "(none)"
            : string.Join(", ", queryFilter.Parameters.Select(p => $"{p.Key}={p.Value}"));
        _logWriter.LogInfo($"GetGraphDataAsync graph={queryFilter.Name}; filters=[{paramSummary}]", nameof(GraphFactory), nameof(GetGraphDataAsync));

        var result = new List<GraphModel>();

        switch (queryFilter.Name.ToLower())
        {
            case "gendersummarygraph":
                result = await _graphRepository.GetGenderGraphDataAsync(queryFilter);
                break;
            case "locationgraph":
                result = await _graphRepository.GetLocationGraphDataAsync(queryFilter);
                break;
            case "organisationgraph":
                result = await _graphRepository.GetOrganisationGraphDataAsync(queryFilter);
                break;
            case "organismgraph":
                result = await _graphRepository.GetOrganismGraphDataAsync(queryFilter);
                break;
            case "organismsusceptibilitygraph":
                result = await _graphRepository.GetSusceptibilityGraphDataAsync(queryFilter);
                break;
            case "specimentypesummarygraph":
                result = await _graphRepository.GetSpecimenTypeGraphDataAsync(queryFilter);
                break;
            case "taggraph":
                result = await _graphRepository.GetTagGraphDataAsync(queryFilter);
                break;
            case "testgraph":
                result = await _graphRepository.GetTestGraphDataAsync(queryFilter);
                result = await _testNameInterpreter.InterpretAsync(result);
                break;
            case "specimenstategraph":
                result = await _graphRepository.GetSpecimenStateGraphDataAsync(queryFilter);
                break;
            default:
                result = await _graphRepository.GetSpecimenTypeGraphDataAsync(queryFilter);
                break;
        }

        return JsonConvert.SerializeObject(result);
    }
}
