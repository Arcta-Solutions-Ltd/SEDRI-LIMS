using arc.app.Config.Reports.Footers;
using arc.app.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.ReportsConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Config.Reports;

/// <summary>
/// Provides operations for managing report footer configurations.
/// </summary>
public class ReportFooterAdapter(IConfigRepository configRepository) : IReportFooterAdapter
{
    /// <summary>
    /// Retrieves a report footer configuration by its name.
    /// Will attempt to locate a config in the database before falling back to code.
    /// </summary>
    /// <param name="footerName">The name of the report footer to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ReportHeaderFooterConfig.</returns>
    public async Task<ReportHeaderFooterConfig> GetFooterAsync(string footerName)
    {
        var parameters = new QueryFilterConfig().AddString("ConfigName", footerName);
        var configRecord = await configRepository.SingleConfigByNameAsync(parameters);

        string contents = configRecord.IsContentsNullOrEmptyJsonString()
            ? new ReportFooterFactory().Create(footerName).Get()
            : configRecord.Contents;

        return JsonConvert.DeserializeObject<ReportHeaderFooterConfig>(contents);
    }
}
