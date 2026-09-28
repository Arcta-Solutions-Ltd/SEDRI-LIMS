using arc.domain.Instruments;
using System.Threading.Tasks;

namespace arc.app.Instruments;

/// <summary>
/// Provides methods to manage and retrieve single instrument profiles.
/// </summary>
public interface ISingleInstrumentProfile
{
    /// <summary>
    /// Asynchronously gets a single instrument configuration by profile name.
    /// </summary>
    /// <param name="profileName">The name of the profile.</param>
    /// <returns>The single instrument configuration.</returns>
    Task<SingleInstrumentConfig> GetAsync(string profileName);
}

