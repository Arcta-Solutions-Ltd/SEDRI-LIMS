using arc.app.Common;

namespace arc.app.Config.Queries.Reports;
/// <summary>
/// Defines the query metadata for retrieving the list of approved reports.
/// </summary>
internal class ApprovedReportsListQuery : IDefinition
{
    /// <summary>
    /// Returns a JSON string that specifies:
    ///   • Query – the identifier for the approved reports list query  
    ///   • TableName – the database table to query (ReportHistory)  
    ///   • Type – the category of this definition ("Special")  
    /// </summary>
    /// <returns>
    /// A JSON-formatted string containing the query definition.
    /// </returns>
    public string Get()
    {
        return @"{   
                    'Query': 'approvedreportslistquery', 'TableName': 'ReportHistory', 'Type': 'Special'
                }";
    }
}
