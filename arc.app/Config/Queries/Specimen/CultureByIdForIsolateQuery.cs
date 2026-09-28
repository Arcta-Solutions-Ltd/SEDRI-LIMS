using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents a specialized query definition used to retrieve culture data by ID for isolate processing.
/// Implements <see cref="IDefinition"/> to support query resolution in the system.
/// </summary>
internal class CultureByIdForIsolateQuery : IDefinition
{
    /// <summary>
    /// Returns the query definition as a JSON-formatted string.
    /// </summary>
    /// <returns>
    /// A JSON string containing metadata for the <c>CultureByIdForIsolateQuery</c>, 
    /// including table name, type, and tags.
    /// </returns>
    public string Get()
    {
        return @"{
            'Query': 'CultureByIdForIsolateQuery',
                'TableName': 'Culture',
                'Type': 'Special',
                'Tags': 'SP'
                }
        ";
    }
}

