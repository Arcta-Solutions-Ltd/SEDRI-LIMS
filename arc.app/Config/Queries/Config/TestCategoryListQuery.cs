using arc.app.Common;

namespace arc.app.Config.Queries.Config;

/// <summary>
/// Query for retrieving the list of test categories.
/// </summary>
internal class TestCategoryListQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query configuration for fetching test category data.
    /// </summary>
    /// <remarks>
    /// This configuration defines the query name, type, and translation settings
    /// required to retrieve test categories for configuration purposes.
    /// </remarks>
    /// <returns>
    /// A string representation of the query configuration, including metadata and query details.
    /// </returns>
    public string Get()
    {
        return @"{ 'Query': 'TestCategoryListQuery', 'Type': 'Config', 'Translate': true}";
    }
}
