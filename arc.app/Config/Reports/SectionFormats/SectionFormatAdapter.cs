using arc.app.Configuration;
using arc.domain.Configuration.ReportsConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Adapter for retrieving section format configurations.
/// Provides operations for managing section format configurations with cache support.
/// </summary>
public class SectionFormatAdapter : ISectionFormatAdapter
{
    /// <summary>
    /// Factory for creating section format configurations.
    /// </summary>
    private readonly ISectionFormatFactory _sectionFormatFactory;

    /// <summary>
    /// Cache for storing and retrieving configuration data.
    /// </summary>
    private readonly IConfigCache _configCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="SectionFormatAdapter"/> class.
    /// </summary>
    /// <param name="sectionFormatFactory">The section format configuration factory to use.</param>
    /// <param name="configCache">The configuration cache to use.</param>
    public SectionFormatAdapter(ISectionFormatFactory sectionFormatFactory, IConfigCache configCache)
    {
        _sectionFormatFactory = sectionFormatFactory;
        _configCache = configCache;
    }

    /// <summary>
    /// Asynchronously retrieves a section format configuration by its name.
    /// Will attempt to locate a config in the cache before falling back to code.
    /// </summary>
    /// <param name="name">The name of the section format to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ReportSectionFormatConfig.</returns>
    public async Task<ReportSectionFormatConfig> GetFormatAsync(string name)
    {
        var configRecord = await _configCache.GetConfigRecordAsync(name);

        var contents = configRecord == null || configRecord.Contents == null || configRecord.Contents == "{}"
            ? _sectionFormatFactory.Create(name).Get()
            : configRecord.Contents;

        return JsonConvert.DeserializeObject<ReportSectionFormatConfig>(contents);
    }
}
