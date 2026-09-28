using arc.domain.Configuration.ReportsConfig;
using System.Threading.Tasks;

namespace arc.app.Config.Reports;

/// <summary>
/// Provides operations for managing report footer configurations.
/// </summary>
public interface IReportFooterAdapter
{
    /// <summary>
    /// Retrieves a report footer configuration by its name.
    /// </summary>
    /// <param name="footerName">The name of the report footer to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ReportHeaderFooterConfig.</returns>
    Task<ReportHeaderFooterConfig> GetFooterAsync(string footerName);
}
