using arc.app.Common;

namespace arc.app.Config.Queries.Reports;

/// <summary>
/// Provides the configuration for the UnapprovedReportsList UI query.
/// </summary>
internal class UnapprovedReportsListQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the UnapprovedReportsList query.
    /// </summary>
    /// <returns>A JSON string defining the UnapprovedReportsList query configuration.</returns>
    public string Get()
    {
        return @"{
            'Query': 'unapprovedreportslistquery',
            'TableName': 'ReportHistory',
            'Type': 'Special'
        }";
    }
}
