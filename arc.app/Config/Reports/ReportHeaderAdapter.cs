using arc.app.Config.Reports.Headers;
using arc.app.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.ReportsConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Config.Reports;

/// <summary>
/// Provides operations for managing report header configurations.
/// </summary>
public class ReportHeaderAdapter(IConfigRepository configRepository) : IReportHeaderAdapter
{
    /// <summary>
    /// Retrieves a report header configuration by its name.
    /// Will attempt to locate a config in the database before falling back to code.
    /// </summary>
    /// <param name="headerName">The name of the report header to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ReportHeaderFooterConfig.</returns>
    public async Task<ReportHeaderFooterConfig> GetHeaderAsync(string headerName)
    {
        var parameters = new QueryFilterConfig().AddString("ConfigName", headerName);
        var configRecord = await configRepository.SingleConfigByNameAsync(parameters);

        string contents = configRecord.IsContentsNullOrEmptyJsonString()
            ? new ReportHeaderFactory().Create(headerName).Get()
            : configRecord.Contents;

        return JsonConvert.DeserializeObject<ReportHeaderFooterConfig>(contents);
    }
}
