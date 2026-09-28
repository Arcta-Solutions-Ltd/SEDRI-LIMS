using arc.app.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.ReportsConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Adapter class for handling data section configurations.
/// </summary>
public class DataSectionAdapter : IDataSectionAdapter
{
    private readonly IConfigRepository _configRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataSectionAdapter"/> class.
    /// </summary>
    /// <param name="configRepository">The configuration repository.</param>
    public DataSectionAdapter(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }

    /// <summary>
    /// Asynchronously retrieves the data section configuration for the specified section name.
    /// </summary>
    /// <param name="sectionName">The name of the section.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the data section configuration.</returns>
    public async Task<DataSectionConfig> GetSectionAsync(string sectionName)
    {
        var queryFilterConfig = new QueryFilterConfig("ConfigName", sectionName);
        var configRecord = await _configRepository.SingleConfigByNameAsync(queryFilterConfig);
        var sectionFactory = new DataSectionFactory();

        return configRecord.IsContentsNullOrEmptyJsonString()
            ? sectionFactory.GetSection(sectionName)
            : JsonConvert.DeserializeObject<DataSectionConfig>(configRecord.Contents);
    }
}
