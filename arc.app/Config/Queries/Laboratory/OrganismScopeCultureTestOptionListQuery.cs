using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query for retrieving the list of organism scope culture test options.
/// </summary>
internal class OrganismScopeCultureTestOptionListQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query configuration for fetching organism scope culture test option data.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the query's name, type, and translation settings.
    /// It is designed to retrieve organism scope culture test option data for configuration purposes.
    /// </remarks>
    /// <returns>
    /// A string representation of the query configuration, including metadata such as
    /// the query name, type, and translation settings.
    /// </returns>
    public string Get()
    {
        return @"{ 'Query': 'organismscopeculturetestoptionlistquery', 'Type': 'Config', 'Translate': true}";
    }
}
