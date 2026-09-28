using arc.domain.Configuration.UIEventsConfig;
using System.Threading.Tasks;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Interface for an adapter responsible for retrieving UI event configurations.
/// </summary>
public interface IUIEventConfigAdapter
{
    /// <summary>
    /// Asynchronously retrieves a UI event configuration based on the specified event name.
    /// </summary>
    /// <param name="eventName">The name of the event to retrieve the configuration for.</param>
    /// <returns>A task representing the asynchronous operation, containing the UI event configuration.</returns>
    Task<UIEventConfig> GetEventAsync(string eventName);
}

