using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Asset;

/// <summary>
/// Represents an event for editing an existing storage record.
/// </summary>
public class EditStorageEvent : IRun
{
    /// <summary>
    /// The service provider instance used to resolve dependencies.
    /// </summary>
    public IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditStorageEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    public EditStorageEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the event to edit an existing storage record asynchronously.
    /// </summary>
    /// <param name="dataToSave">The updated storage data in JSON format.</param>
    /// <param name="Id">The identifier associated with the event (not used in this implementation).</param>
    /// <param name="command">The event command containing additional details.</param>
    /// <param name="eventData">Optional configuration data for the event.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the ID of the edited storage record.
    /// </returns>
    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        var storageRepository = _serviceProvider.GetService<IStorageRepository>();
        return await storageRepository.EditAsync(dataToSave);
    }
}

