using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// This class defines the query to retrieve items contents.
/// Implements the IDefinition interface.
/// </summary>
internal class ItemsContentsQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query details as a JSON string.
    /// </summary>
    /// <returns>A JSON string containing the query details.</returns>
    public string Get()
    {
        return @"{ 'Query': 'ItemsContentsQuery', 'TableName': 'Queue', 'Type': 'Special', Translate: true, 'Tags': 'PA,SP,MO' }";
    }
}
