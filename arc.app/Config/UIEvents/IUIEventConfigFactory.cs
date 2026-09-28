using arc.domain.Configuration.UIEventsConfig;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Interface for a factory responsible for creating and retrieving UI event configurations.
/// </summary>
public interface IUIEventConfigFactory
{
    /// <summary>
    /// Retrieves a UI event configuration based on the specified event name.
    /// </summary>
    /// <param name="eventName">The name of the UI event to retrieve the configuration for.</param>
    /// <returns>The corresponding UI event configuration.</returns>
    UIEventConfig GetEvent(string eventName);
}

