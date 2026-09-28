using arc.app.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.ReportsConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config.Reports;

/// <summary>
/// Adapter class for handling report configurations.
/// </summary>
public class ReportAdapter(IReportFactory reportFactory, IConfigRepository configRepository) : IReportAdapter
{
    /// <summary>
    /// Asynchronously retrieves a report configuration by its name.
    /// </summary>
    /// <param name="reportName">The name of the report.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the report configuration.</returns>
    public async Task<ReportConfig> GetReportAsync(string reportName)
    {
        var parameters = new QueryFilterConfig().AddString("ConfigName", reportName.ToLower());

        var configRecord = await configRepository.SingleConfigByNameAsync(parameters);

        var contents = configRecord.IsContentsNullOrEmptyJsonString()
            ? reportFactory.GetReport(reportName).Get()
            : configRecord.Contents;

        return JsonConvert.DeserializeObject<ReportConfig>(contents);
    }

    /// <summary>
    /// Asynchronously retrieves all report configurations.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of report configurations.</returns>
    public async Task<List<ReportConfig>> GetAllReportsAsync()
    {
        var parameters = new QueryFilterConfig().AddString("ConfigTypeId", "13");

        var configList = await configRepository.GetConfigListAsync(parameters);
        var reportList = new List<ReportConfig>();

        foreach (var configRecord in configList)
        {
            var contents = configRecord.IsContentsNullOrEmptyJsonString()
                ? reportFactory.GetReport(configRecord.ConfigName).Get()
                : configRecord.Contents;

            reportList.Add(JsonConvert.DeserializeObject<ReportConfig>(contents));
        }

        return reportList;
    }
}
