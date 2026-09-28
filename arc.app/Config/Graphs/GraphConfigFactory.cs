using arc.domain.Configuration.ReportsConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Graphs;

/// <summary>
/// Factory class responsible for creating <see cref="GraphConfig"/> instances based on graph name identifiers.
/// </summary>
public class GraphConfigFactory : IGraphConfigFactory
{
    /// <summary>
    /// Retrieves a strongly typed <see cref="GraphConfig"/> object for the specified graph name.
    /// </summary>
    /// <param name="name">The name identifier of the graph configuration to retrieve.</param>
    /// <returns>
    /// A <see cref="GraphConfig"/> instance deserialized from the corresponding graph definition string,
    /// or <c>null</c> if the name does not match any known configuration.
    /// </returns>
    public GraphConfig GetGraph(string name)
    {
        var definition = name.ToLower() switch
        {
            "gendersummarygraph" => new GenderGraphConfig().Get(),
            "locationgraph" => new LocationGraphConfig().Get(),
            "organisationgraph" => new OrganisationGraphConfig().Get(),
            "organismgraph" => new OrganismGraphConfig().Get(),
            "organismsusceptibilitygraph" => new OrganismSusceptibilityGraphConfig().Get(),
            "taggraph" => new TagGraphConfig().Get(),
            "testgraph" => new TestGraphConfig().Get(),
            "specimentypesummarygraph" => new SpecimenTypeSummaryGraphConfig().Get(),
            "specimenstategraph" => new SpecimenStateGraphConfig().Get(),
            _ => null
        };

        return JsonConvert.DeserializeObject<GraphConfig>(definition);
    }
}
