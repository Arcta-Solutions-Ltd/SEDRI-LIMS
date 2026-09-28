using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents the definition for an antibiogram report query.
/// </summary>
internal class AntibiogramQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON-formatted query definition used to identify and configure the antibiogram report.
    /// </summary>
    /// <returns>
    /// A string containing the query metadata, including query name, target table, and report type.
    /// </returns>
    public string Get()
    {
        return @"{  
                    'Query': 'antibiogramquery', 
                    'TableName': 'Specimen', 
                    'Type': 'Report'
                }";
    }
}
