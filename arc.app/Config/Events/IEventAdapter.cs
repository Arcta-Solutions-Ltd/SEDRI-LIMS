using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.Config.Events;

/// <summary>
/// Interface for an adapter responsible for retrieving event configurations.
/// </summary>
public interface IEventAdapter
{
    /// <summary>
    /// Asynchronously retrieves an event configuration based on the specified event name.
    /// </summary>
    /// <param name="eventName">The name of the event to retrieve the configuration for.</param>
    /// <returns>A task representing the asynchronous operation, containing the event configuration.</returns>
    Task<EventConfig> GetEventAsync(string eventName);
}

