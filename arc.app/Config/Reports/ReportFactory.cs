using arc.app.Common;

namespace arc.app.Config.Reports;

/// <summary>
/// Factory for creating report configurations based on report names.
/// </summary>
public class ReportFactory : IReportFactory
{
    /// <summary>
    /// Creates a report definition based on the specified report name.
    /// </summary>
    /// <param name="reportName">The name of the report to create. Case-insensitive.</param>
    /// <returns>An IDefinition implementation. Returns SpecimenRecordReportConfig as default if no match is found.</returns>
    public IDefinition GetReport(string reportName)
    {
        return reportName.ToLower() switch
        {
            "defaultspecimenreport" => new DefaultSpecimenReportConfig(),
            "specimenrecord" => new SpecimenRecordReportConfig(),
            _ => new SpecimenRecordReportConfig(),
        };
    }
}
