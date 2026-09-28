using arc.app.Common;
using arc.app.Configuration;
using arc.common;
using arc.common.Models;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Handles edit specimen saves with page TableName routing to Patient, Admission, Request and Specimen.
/// </summary>
public class EditSpecimenEvent : IRun
{
    private readonly ISpecimenRepository _specimenRepository;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSpecimenEvent"/> class.
    /// </summary>
    public EditSpecimenEvent(ISpecimenRepository specimenRepository, IServiceProvider serviceProvider, ILogWriter logWriter)
    {
        _specimenRepository = specimenRepository;
        _serviceProvider = serviceProvider;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Updates the specimen and any related entity MoreData fields routed by page configuration.
    /// </summary>
    /// <param name="dataToSave">Serialised edit payload including FormName.</param>
    /// <param name="Id">Specimen id.</param>
    /// <param name="command">Event command with workflow state.</param>
    /// <param name="eventData">Event configuration.</param>
    /// <returns>The specimen id.</returns>
    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        var routingService = _serviceProvider.GetRequiredService<IFormPageTableRoutingService>();
        var tableExceptions = await SpecimenFormSaveRouting.BuildTableExceptionsAsync(
            routingService, dataToSave, eventData);

        _logWriter.LogInfo(
            $"Edit specimen save routing: {tableExceptions.Count} table exception(s)",
            nameof(EditSpecimenEvent),
            nameof(RunAsync));

        await _specimenRepository.EditSpecimenWithRelatedEntitiesAsync(
            dataToSave,
            tableExceptions,
            command?.NewStateId ?? command?.StateId ?? "0");

        return int.Parse(Id);
    }
}
