using arc.app.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Instruments;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Instruments;

/// <summary>
/// Provides methods to manage and retrieve single instrument profiles.
/// </summary>
public class SingleInstrumentProfile : ISingleInstrumentProfile
{
    private readonly IConfigRepository _configRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleInstrumentProfile"/> class.
    /// </summary>
    /// <param name="configRepository">The configuration repository.</param>
    public SingleInstrumentProfile(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }

    /// <summary>
    /// Asynchronously gets a single instrument configuration by profile name from the <c>instrumentinfo</c> config JSON
    /// (including <see cref="SingleInstrumentConfig.InstrumentMachineId"/> when present).
    /// </summary>
    /// <param name="profileName">The name of the profile.</param>
    /// <returns>The single instrument configuration.</returns>
    public async Task<SingleInstrumentConfig> GetAsync(string profileName)
    {
        var filter = new QueryFilterConfig();
        filter.AddString("configname", "Instrumentinfo");
        var instrumentConfig = await _configRepository.SingleConfigByNameAsync(filter);

        var fullConfig = JsonConvert.DeserializeObject<InstrumentConfig>(instrumentConfig.Contents);

        var currentConfig = fullConfig.Instruments.FirstOrDefault(c => c.InstrumentName.ToLower() == profileName.ToLower());

        return currentConfig;
    }
}

