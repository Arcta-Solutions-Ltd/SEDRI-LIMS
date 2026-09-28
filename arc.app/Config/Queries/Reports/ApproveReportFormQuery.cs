using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents the definition for an approve report form query.
/// </summary>
internal class ApproveReportFormQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON-formatted query definition for approving a report form.
    /// </summary>
    /// <returns>
    /// A JSON string specifying the query name, target table, and 'special' type.
    /// </returns>
    public string Get()
    {
        return @"{  
                    'Query': 'approvereportformquery', 
                    'TableName': 'reporthistory', 
                    'Type': 'special'
                }";
    }
}
