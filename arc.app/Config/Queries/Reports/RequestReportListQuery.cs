using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query definition for report history rows scoped to specimens on a request record view.
/// </summary>
internal class RequestReportListQuery : IDefinition
{
    /// <summary>
    /// Returns the query metadata consumed by the report handler.
    /// </summary>
    /// <returns>A JSON string specifying the query name, source table, and query type.</returns>
    public string Get()
    {
        return @"{  
                        'Query': 'requestreportlist', 
                        'TableName': 'ReportHistory', 
                        'Type': 'Report'
                    }";
    }
}
