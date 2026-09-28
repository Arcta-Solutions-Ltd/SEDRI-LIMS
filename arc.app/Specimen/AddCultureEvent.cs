using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.common.Models;
using arc.app.Laboratory;
using arc.common.ExtensionMethods;

namespace arc.app.Specimen;

/// <summary>
/// Represents an event for adding culture data.
/// Implements the IRun interface to execute the operation.
/// </summary>
internal class AddCultureEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    public TokenInfoModel _token;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddCultureEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency resolution.</param>
    /// <param name="token">The token containing user authentication details.</param>
    public AddCultureEvent(IServiceProvider serviceProvider, TokenInfoModel token)
    {
        _serviceProvider = serviceProvider;
        _token = token;
    }

    /// <summary>
    /// Executes the event asynchronously to add culture data.
    /// </summary>
    /// <param name="dataToSave">The JSON string containing data to be saved.</param>
    /// <param name="Id">The unique identifier for the operation.</param>
    /// <param name="command">The event model containing command details.</param>
    /// <param name="eventData">Optional configuration data for the event.</param>
    /// <returns>The result of the operation as an integer.</returns>
    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        var laboratoryConfigurationHandler = _serviceProvider.GetService<ILaboratoryConfigurationHandler>();
        var cultureRepository = _serviceProvider.GetService<ICultureRepository>();
        string username = _token.Username;

        var specimenId = dataToSave.GetStringFromJson("SpecimenId") ?? "0";
        await laboratoryConfigurationHandler.LoadConfigurationForSpecimenAsync(int.Parse(specimenId));

        return await cultureRepository.AddCultureAsync(dataToSave, command, eventData, laboratoryConfigurationHandler.LaboratoryConfigurationList, username);
    }
}
