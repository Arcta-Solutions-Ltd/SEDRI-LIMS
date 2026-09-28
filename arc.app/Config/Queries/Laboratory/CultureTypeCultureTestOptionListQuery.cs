using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query for retrieving the list of culture type culture test options.
/// </summary>
internal class CultureTypeCultureTestOptionListQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query configuration for fetching culture type culture test option data.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the query's name, type, and translation settings.
    /// It is designed to retrieve culture type culture test option data for configuration purposes.
    /// </remarks>
    /// <returns>
    /// A string representation of the query configuration, including metadata such as 
    /// the query name, type, and translation settings.
    /// </returns>
    public string Get()
    {
        return @"{ 'Query': 'culturetypeculturetestoptionlistquery', 'Type': 'Config', 'Translate': true}";
    }
}
