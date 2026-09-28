using arc.app.Common;
using arc.app.Laboratory;
using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Handles the execution of the "Add Isolate" event, integrating configuration and repository logic.
/// </summary>
/// <remarks>
/// This class retrieves necessary services via <see cref="IServiceProvider"/> and uses the current <see cref="TokenInfoModel"/>
/// to identify the user initiating the event. It loads specimen-specific configuration and delegates culture addition
/// to the <see cref="ICultureRepository"/> using the provided event context.
/// </remarks>
internal class AddIsolateEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Contains user token information, including the username used for audit or tracking.
    /// </summary>
    public TokenInfoModel _token;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddIsolateEvent"/> class with required services and user context.
    /// </summary>
    /// <param name="serviceProvider">Service provider for resolving dependencies.</param>
    /// <param name="token">User token containing identity and session data.</param>
    public AddIsolateEvent(IServiceProvider serviceProvider, TokenInfoModel token)
    {
        _serviceProvider = serviceProvider;
        _token = token;
    }

    /// <summary>
    /// Executes the isolate addition logic, loading configuration and persisting culture data.
    /// </summary>
    /// <param name="dataToSave">Serialized event data, expected to contain a <c>SpecimenId</c>.</param>
    /// <param name="Id">Unused identifier parameter (reserved for future use).</param>
    /// <param name="command">Event metadata describing the action.</param>
    /// <param name="eventData">Optional configuration for the event.</param>
    /// <returns>The result of the culture addition operation, typically a status code or record ID.</returns>
    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        var laboratoryConfigurationHandler = _serviceProvider.GetService<ILaboratoryConfigurationHandler>();
        var cultureRepository = _serviceProvider.GetService<ICultureRepository>();
        string username = _token.Username;

        var specimenId = dataToSave.GetStringFromJson("SpecimenId") ?? "0";
        await laboratoryConfigurationHandler.LoadConfigurationForSpecimenAsync(int.Parse(specimenId));

        return await cultureRepository.AddCultureAsync(
            dataToSave,
            command,
            eventData,
            laboratoryConfigurationHandler.LaboratoryConfigurationList,
            username
        );
    }
}

