using arc.domain.Configuration.ReportsConfig;
using System.Threading.Tasks;

namespace arc.app.Config.Reports;

/// <summary>
/// Provides operations for managing report header configurations.
/// </summary>
public interface IReportHeaderAdapter
{
    /// <summary>
    /// Retrieves a report header configuration by its name.
    /// </summary>
    /// <param name="headerName">The name of the report header to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ReportHeaderFooterConfig.</returns>
    Task<ReportHeaderFooterConfig> GetHeaderAsync(string headerName);
}
