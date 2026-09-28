using arc.app.Config.Reports.Sections;
using arc.app.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.ReportsConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config.Reports;

/// <summary>
/// Adapter class for handling report sections configuration.
/// </summary>
public class SectionAdapter(IConfigRepository configRepository) : ISectionAdapter
{
    /// <summary>
    /// Asynchronously retrieves a report section configuration by its name.
    /// </summary>
    /// <param name="sectionName">The name of the section to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the report section configuration.</returns>
    public async Task<ReportSectionConfig> GetSectionAsync(string sectionName)
    {
        var parameters = new QueryFilterConfig().AddString("ConfigName", sectionName);
        var configRecord = await configRepository.SingleConfigByNameAsync(parameters);

        var contents = configRecord.IsContentsNullOrEmptyJsonString()
            ? new ReportSectionFactory().Create(sectionName).Get()
            : configRecord.Contents;

        return JsonConvert.DeserializeObject<ReportSectionConfig>(contents);
    }

    /// <summary>
    /// Asynchronously retrieves all report section configurations.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of report section configurations.</returns>
    public async Task<List<ReportSectionConfig>> GetAllSectionsAsync()
    {
        var parameters = new QueryFilterConfig().AddString("ConfigTypeId", "14,15");
        var configList = await configRepository.GetConfigListAsync(parameters);
        var sectionList = new List<ReportSectionConfig>();

        foreach (var configRecord in configList)
        {
            var contents = configRecord.IsContentsNullOrEmptyJsonString()
                ? new ReportSectionFactory().Create(configRecord.ConfigName).Get()
                : configRecord.Contents;

            var newSection = JsonConvert.DeserializeObject<ReportSectionConfig>(contents);
            sectionList.Add(newSection);
        }

        return sectionList;
    }
}
