using arc.app.Common;
using arc.common;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using arc.common.Models.Alert;
using arc.domain.Configuration.EventsConfig;

namespace arc.app.Alert;

/// <summary>
/// Represents an event for adding a new alert.
/// </summary>
internal class AddAlertEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddAlertEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used for dependency injection.</param>
    public AddAlertEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the event to add a new alert asynchronously.
    /// </summary>
    /// <param name="dataToSave">The alert data to be saved in JSON format.</param>
    /// <param name="id">An identifier associated with the event (not used in this implementation).</param>
    /// <param name="command">The event command containing additional details.</param>
    /// <param name="eventData">Optional configuration data for the event.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the ID of the newly added alert.
    /// </returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var alertRepository = _serviceProvider.GetService<IAlertRepository>();

        var extractedModel = JsonConvert.DeserializeObject<AlertDetailsModel>(dataToSave);

        return await alertRepository.AddAlertAsync(extractedModel);
    }
}

