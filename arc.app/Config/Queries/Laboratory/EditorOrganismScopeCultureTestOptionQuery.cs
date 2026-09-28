using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query for retrieving an organism scope culture test option by ID for editing.
/// </summary>
internal class EditorOrganismScopeCultureTestOptionQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query configuration for fetching a single organism scope culture test option.
    /// </summary>
    /// <returns>A JSON string defining the query name and type.</returns>
    public string Get()
    {
        return @"{ 'Query': 'editorganismscopeculturetestoptionquery', 'Type': 'Config', 'Translate': true}";
    }
}
