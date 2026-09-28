using arc.app.Common;

namespace arc.app.Config.Queries.Specimen;

/// <summary>
/// Defines a query specification for retrieving culture records by specimen ID.
/// </summary>
internal class CultureListBySpecimenIdQuery : IDefinition
{
    /// <summary>
    /// Returns the query definition as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string describing the query parameters.</returns>
    public string Get()
    {
        return @"{
                    'Query': 'CultureListBySpecimenId',
                    'TableName': 'Culture',
                    'Translate': true,
                    'Type': 'special',
                    'Tags': 'SP'
                }";
    }
}


