using arc.app.Common;

namespace arc.app.Config.Reports;

/// <summary>
/// Factory for creating report configurations based on report names.
/// </summary>
public interface IReportFactory
{
    /// <summary>
    /// Creates a report definition based on the specified report name.
    /// </summary>
    /// <param name="reportName">The name of the report to create.</param>
    /// <returns>An IDefinition implementation.</returns>
    IDefinition GetReport(string reportName);
}
