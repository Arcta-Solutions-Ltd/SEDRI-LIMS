using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents a query definition for retrieving a list of laboratories.
/// </summary>
internal class LaboratoryListQuery : IDefinition
{
    /// <summary>
    /// Retrieves the JSON configuration string for the laboratory list query.
    /// </summary>
    /// <returns>
    /// A JSON string defining the query with:
    /// - "Query": the identifier for the query ('laboratorylist'),
    /// - "TableName": the target table name ('Laboratory'),
    /// - "Type": the query type ('Special').
    /// </returns>
    public string Get()
    {
        return @"{
                'Query': 'laboratorylist', 'TableName': 'Laboratory', 'Type': 'Special'
            }";
    }
}

