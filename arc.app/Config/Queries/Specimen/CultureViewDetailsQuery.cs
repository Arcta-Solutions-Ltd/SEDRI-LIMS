using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Defines a specialized query configuration for retrieving detailed culture view data.
/// </summary>
/// <remarks>
/// This query targets the <c>Culture</c> table and is marked as <c>Special</c>,
/// indicating custom handling or presentation logic. It supports translation
/// and is tagged with <c>SP</c> for categorization or filtering.
/// </remarks>
internal class CultureViewDetailsQuery : IDefinition
{
    /// <summary>
    /// Returns the query definition as a JSON-formatted string.
    /// </summary>
    /// <returns>
    /// A JSON string containing metadata for the <c>CultureViewDetailsQuery</c>,
    /// including table name, type, translation flag, and tags.
    /// </returns>
    public string Get()
    {
        return @"{
                'Query': 'CultureViewDetailsQuery',
                'TableName': 'Culture',
                'Type': 'Special',
                'Translate': true,
                'Tags': 'SP'
            }";
    }
}
