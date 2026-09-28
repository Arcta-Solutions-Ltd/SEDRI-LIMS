using arc.app.SystemConfig;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config.Events;

/// <summary>
/// Adapter for retrieving and managing event configurations.
/// </summary>
public class EventAdapter : IEventAdapter
{
    /// <summary>
    /// Factory for creating event configurations.
    /// </summary>
    private readonly IEventFactory _eventFactory;

    /// <summary>
    /// Repository for accessing configuration records from the data source.
    /// </summary>
    private readonly IConfigRepository _configRepository;

    /// <summary>
    /// Logger used to record which source an event configuration came from.
    /// </summary>
    private readonly ILogger<EventAdapter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventAdapter"/> class.
    /// </summary>
    /// <param name="eventFactory">The event configuration factory to use.</param>
    /// <param name="configRepository">The configuration repository to use.</param>
    /// <param name="logger">Logger used to record whether the stored override or the built-in definition was used.</param>
    public EventAdapter(IEventFactory eventFactory, IConfigRepository configRepository, ILogger<EventAdapter> logger)
    {
        _eventFactory = eventFactory;
        _configRepository = configRepository;
        _logger = logger;
    }

    /// <summary>
    /// Asynchronously retrieves an event configuration based on the specified event name. A non-empty
    /// <c>configs</c> record overrides the built-in definition, which is the usual reason an installed site
    /// behaves differently from a fresh one, so the source is logged.
    /// </summary>
    /// <param name="eventName">The name of the event to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the event configuration,
    /// or null if no valid configuration record is found.
    /// </returns>
    public async Task<EventConfig> GetEventAsync(string eventName)
    {
        var parameters = new QueryFilterConfig
        {
            Parameters =
            [
                new() { Key = "ConfigName", Value = eventName }
            ]
        };

        var configRecord = await _configRepository.SingleConfigByNameAsync(parameters);

        var useStoredOverride = configRecord.Contents != null && configRecord.Contents != "{}";

        var eventDef = useStoredOverride
            ? JsonConvert.DeserializeObject<EventConfig>(configRecord.Contents)
            : _eventFactory.GetEvent(eventName);

        _logger.LogDebug(
            "Event configuration for {EventName} came from {ConfigSource} and has {DisplayCount} display rows",
            eventName,
            useStoredOverride ? "the stored configs override" : "the built-in event factory",
            eventDef?.Display?.Count ?? 0);

        return eventDef;
    }
}

