using arc.app.Common;

namespace arc.app.Config.Queries.Config;
/// <summary>
/// Represents the MappingListQuery class which defines a query for mapping lists.
/// </summary>
internal class MappingListQuery : IDefinition
{
    /// <summary>
    /// Gets the query definition as a JSON string.
    /// </summary>
    /// <returns>A JSON string representing the query definition.</returns>
    public string Get()
    {
        return @"{ 'Query': 'mappinglistquery', 'Type': 'Special', 'Translate': true}";
    }
}
