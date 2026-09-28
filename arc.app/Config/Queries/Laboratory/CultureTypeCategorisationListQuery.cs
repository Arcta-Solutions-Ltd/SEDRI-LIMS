using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query for retrieving the list of culture type categorizations.
/// </summary>
internal class CultureTypeCategorisationListQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query configuration for fetching culture type categorization data.
    /// </summary>
    /// <remarks>
    /// This configuration defines the query name, type, and translation settings required
    /// to retrieve culture type categorization data for configuration purposes.
    /// </remarks>
    /// <returns>
    /// A string representation of the query configuration, including metadata and query details.
    /// </returns>
    public string Get()
    {
        return @"{ 'Query': 'CultureTypeCategorisationListQuery', 'Type': 'Config', 'Translate': true}";
    }
}
