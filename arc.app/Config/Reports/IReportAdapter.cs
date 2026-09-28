using arc.domain.Configuration.ReportsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config.Reports;

/// <summary>
/// Adapter class for handling report configurations.
/// </summary>
public interface IReportAdapter
{
    /// <summary>
    /// Asynchronously retrieves a report configuration by its name.
    /// </summary>
    /// <param name="reportName">The name of the report.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the report configuration.</returns>
    Task<ReportConfig> GetReportAsync(string reportName);

    /// <summary>
    /// Asynchronously retrieves all report configurations.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of report configurations.</returns>
    Task<List<ReportConfig>> GetAllReportsAsync();
}
