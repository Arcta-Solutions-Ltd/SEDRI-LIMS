using arc.app.Configuration;
using arc.domain.Configuration.UIEventsConfig;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Adapter for retrieving and managing UI event configurations.
/// </summary>
public class UIEventConfigAdapter : IUIEventConfigAdapter
{
    /// <summary>
    /// Factory for creating UI event configurations.
    /// </summary>
    private readonly IUIEventConfigFactory _uieventFactory;

    /// <summary>
    /// Cache for storing and retrieving configuration data.
    /// </summary>
    private readonly IConfigCache _configCache;

    private readonly ILogger<UIEventConfigAdapter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UIEventConfigAdapter"/> class.
    /// </summary>
    /// <param name="uieventFactory">The UI event configuration factory to use.</param>
    /// <param name="configCache">The configuration cache to use.</param>
    /// <param name="logger">Logger for diagnostic messages when uievent resolution fails.</param>
    public UIEventConfigAdapter(IUIEventConfigFactory uieventFactory, IConfigCache configCache, ILogger<UIEventConfigAdapter> logger)
    {
        _uieventFactory = uieventFactory;
        _configCache = configCache;
        _logger = logger;
    }

    /// <summary>
    /// Asynchronously retrieves a UI event configuration based on the specified event name.
    /// </summary>
    /// <param name="eventName">The name of the event to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the UI event configuration,
    /// or null if no valid configuration record is found.
    /// </returns>
    public async Task<UIEventConfig> GetEventAsync(string eventName)
    {
        try
        {
            var configRecord = await _configCache.GetConfigRecordAsync(eventName).ConfigureAwait(false);

            if (configRecord == null || configRecord.Contents == null || configRecord.Contents == "{}")
            {
                return _uieventFactory.GetEvent(eventName);
            }

            return JsonConvert.DeserializeObject<UIEventConfig>(configRecord.Contents);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Config load: exception resolving uievent '{UiEventName}'", eventName);
            return null;
        }
    }
}
