using arc.app.Common;
using arc.app.Laboratory;
using arc.common.Models;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Handles the 'EditCulture' event, orchestrating the editing of a culture record
/// by delegating to the <see cref="ICultureRepository"/>.
/// </summary>
internal class EditCultureEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    public TokenInfoModel _token;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditCultureEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
    /// <param name="token">The token information model containing user context.</param>
    public EditCultureEvent(IServiceProvider serviceProvider, TokenInfoModel token)
    {
        _serviceProvider = serviceProvider;
        _token = token;
    }

    /// <summary>
    /// Executes the editing of a culture asynchronously.
    /// </summary>
    /// <param name="dataToSave">The serialized culture data to be saved.</param>
    /// <param name="Id">The ID of the culture to be edited.</param>
    /// <param name="command">The event model containing command details.</param>
    /// <param name="eventData">Optional event configuration data.</param>
    /// <returns>A task representing the asynchronous operation, returning the ID of the edited culture.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var cultureRepository = _serviceProvider.GetService<ICultureRepository>();

        return await cultureRepository.EditCultureAsync(dataToSave, command, eventData, id);
    }
}
