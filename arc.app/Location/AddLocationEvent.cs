using arc.app.Common;
using arc.common;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.domain.Configuration.EventsConfig;

namespace arc.app.Location;

/// <summary>
/// Represents an event for adding a new location.
/// </summary>
public class AddLocationEvent : IRun
{
    /// <summary>
    /// The service provider instance used to resolve dependencies.
    /// </summary>
    public IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddLocationEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    public AddLocationEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the event to add a new location asynchronously.
    /// </summary>
    /// <param name="dataToSave">The data to be saved as a new location.</param>
    /// <param name="Id">The identifier associated with the event (not used).</param>
    /// <param name="command">The event command containing additional details.</param>
    /// <param name="eventData">Optional configuration data for the event.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the ID of the newly added location.
    /// </returns>
    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        var locationRepository = _serviceProvider.GetService<ILocationRepository>();
        return await locationRepository.AddAsync(dataToSave);
    }
}

