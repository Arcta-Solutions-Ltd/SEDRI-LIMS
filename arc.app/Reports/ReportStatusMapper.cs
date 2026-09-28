using arc.app.Common;
using arc.app.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Settings;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Reports;

/// <summary>
/// Default implementation of <see cref="IReportStatusMapper"/> that reads the
/// <c>reportstatus</c> system configuration to resolve display text and colour
/// for a specimen state before it is embedded in a printed report.
/// </summary>
public class ReportStatusMapper : IReportStatusMapper
{
    private readonly IConfigRepository _configRepository;
    private readonly IListRepository _listRepository;

    /// <summary>
    /// Initialises a new instance of <see cref="ReportStatusMapper"/>.
    /// </summary>
    /// <param name="configRepository">
    /// Repository used to retrieve the <c>reportstatus</c> system configuration
    /// that contains the colour and text mapping for each specimen state.
    /// </param>
    /// <param name="listRepository">
    /// Repository used to convert a specimen state string to its numeric list ID
    /// (list 60) so it can be matched against the configuration mapping.
    /// </param>
    public ReportStatusMapper(IConfigRepository configRepository, IListRepository listRepository)
    {
        _configRepository = configRepository;
        _listRepository = listRepository;
    }

    /// <inheritdoc/>
    public async Task<string> AddMappingAsync(string reportStatus)
    {
        var reportStatusCode = await _listRepository.GetListIdFromValueAsync(reportStatus, 60);

        var queryFilter = new QueryFilterConfig().AddString("configname", "reportstatus");
        var config = await _configRepository.SingleConfigByNameAsync(queryFilter);

        if (config.Contents != null)
        {
            var settingConfig = JsonConvert.DeserializeObject<List<ColourSettingConfig>>(config.Contents);

            var matchedValue = settingConfig.First().MappingValues.Where(m => m.Type == reportStatusCode).FirstOrDefault();

            if (matchedValue != null)
            {
                return $"<:C:{matchedValue.Colour}:>{matchedValue.Text}";
            }
        }

        return reportStatus;
    }
}
