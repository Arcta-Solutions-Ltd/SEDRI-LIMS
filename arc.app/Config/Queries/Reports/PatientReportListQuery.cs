using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Provides a definition for a query that retrieves patient report data.
/// Implements the IDefinition interface to expose query metadata.
/// </summary>
internal class PatientReportListQuery : IDefinition
{
    /// <summary>
    /// Returns a static JSON-formatted string defining the query for patient reports.
    /// </summary>
    /// <returns>A JSON string specifying the query name, source table, and query type.</returns>
    public string Get()
    {
        return @"{  
                        'Query': 'patientreportlist', 
                        'TableName': 'ReportHistory', 
                        'Type': 'Report'
                    }";
    }
}
