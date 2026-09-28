using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query definition for report history rows scoped to specimens on an admission record view.
/// </summary>
internal class AdmissionReportListQuery : IDefinition
{
    /// <summary>
    /// Returns the query metadata consumed by the report handler.
    /// </summary>
    /// <returns>A JSON string specifying the query name, source table, and query type.</returns>
    public string Get()
    {
        return @"{  
                        'Query': 'admissionreportlist', 
                        'TableName': 'ReportHistory', 
                        'Type': 'Report'
                    }";
    }
}
