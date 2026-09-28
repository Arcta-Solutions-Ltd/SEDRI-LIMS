using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Asset;

/// <summary>
/// Represents an event for adding a new storage record.
/// </summary>
public class AddStorageEvent : IRun
{
    public IServiceProvider _serviceProvider;
    private readonly TokenInfoModel _token;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddStorageEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    public AddStorageEvent(IServiceProvider serviceProvider, TokenInfoModel token)
    {
        _serviceProvider = serviceProvider;
        _token = token;
    }

    /// <summary>
    /// Executes the event to add a new storage record asynchronously.
    /// </summary>
    /// <param name="dataToSave">The data to be saved as a new storage record.</param>
    /// <param name="Id">The identifier associated with the event (not used in this implementation).</param>
    /// <param name="command">The event command containing additional details.</param>
    /// <param name="eventData">Optional configuration data for the event.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the ID of the newly added storage record.
    /// </returns>
    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        var storageRepository = _serviceProvider.GetService<IStorageRepository>();
        return await storageRepository.AddAsync(dataToSave,_token);
    }
}

