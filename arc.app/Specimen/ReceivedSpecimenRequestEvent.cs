using arc.app.Barcodes;
using arc.app.Common;
using arc.app.Config;
using arc.app.Configuration;
using arc.app.Laboratory;
using arc.app.Settings;
using arc.common;
using arc.common.Models;
using arc.common.Models.Config;
using arc.common.Models.Laboratory;
using arc.common.Models.Specimen;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Handles the creation of a new specimen record from a received event,
/// applying patient and barcode logic, managing view configuration, and
/// coordinating with the repository for persistence.
/// </summary>
public class ReceivedSpecimenRequestEvent : IRun
{
    private readonly ISpecimenRepository _specimenRepository;
    private readonly IBarcodesHandler _barcodesHandler;
    private readonly ILogWriter _logWriter;
    private readonly IAccessionNumberCalculator _accessionNumberCalculator;
    private readonly IListViewConfigFactory _listViewConfigFactory;
    private readonly TokenInfoModel _token;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReceivedSpecimenRequestEvent"/> class with required dependencies.
    /// </summary>
    public ReceivedSpecimenRequestEvent(
        ISpecimenRepository specimenRepository,
        IBarcodesHandler barcodesHandler,
        TokenInfoModel token,
        ILogWriter logWriter,
        IAccessionNumberCalculator accessionNumberCalculator,
        IListViewConfigFactory listViewConfigFactory,
        IServiceProvider serviceProvider)
    {
        _specimenRepository = specimenRepository;
        _barcodesHandler = barcodesHandler;
        _token = token;
        _logWriter = logWriter;
        _accessionNumberCalculator = accessionNumberCalculator;
        _listViewConfigFactory = listViewConfigFactory;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the workflow to create a new specimen, including barcode generation,
    /// accession number and patient reference assignment, and validation against view configuration.
    /// </summary>
    /// <param name="dataToSave">Serialized event data containing specimen and patient information.</param>
    /// <param name="Id">Optional identifier associated with the operation (not used directly here).</param>
    /// <param name="command">The event model representing the trigger action.</param>
    /// <param name="eventData">Optional configuration data for event behavior and table exceptions.</param>
    /// <returns>The identifier of the created specimen.</returns>
    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        var data = ArcJson.Deserialize<CreateSpecimenEventModel>(dataToSave);

        data.Crafted.FirstOrDefault(t => t.Name == "testselectionpage")
            ?.Contents.Remove(data.Crafted.FirstOrDefault(t => t.Name == "testselectionpage")
            .Contents.FirstOrDefault(item => item.Key == "XCategX"));

        data.Crafted.FirstOrDefault(t => t.Name == "culturetypeselectionpage")
            ?.Contents.Remove(data.Crafted.FirstOrDefault(t => t.Name == "culturetypeselectionpage")
            .Contents.FirstOrDefault(item => item.Key == "XCategX"));

        _logWriter.LogInfo("Getting the specimen barcode", "ReceivedSpecimenRequestEvent", "Run");

        if (data.Surname != null && data.PatientId == 0)
        {
            _logWriter.LogInfo("Getting the patient barcode", "ReceivedSpecimenRequestEvent", "Run");
            data.PatientBarcode = await _barcodesHandler.GetBarcodeAsync();
        }

        _logWriter.LogInfo("Creating the specimen", "ReceivedSpecimenRequestEvent", "Run");

        var routingService = _serviceProvider.GetRequiredService<IFormPageTableRoutingService>();
        var tableExceptions = await SpecimenFormSaveRouting.BuildTableExceptionsAsync(
            routingService, dataToSave, eventData);

        _logWriter.LogInfo(
            $"Received specimen save routing: {tableExceptions.Count} table exception(s)",
            "ReceivedSpecimenRequestEvent",
            "RunAsync");

        var viewConfig = eventData.DefaultView != null
            ? await _listViewConfigFactory.GetViewAsync(eventData.DefaultView)
            : await _listViewConfigFactory.GetViewAsync(data.View);

        var accessionNumber = data.AccessionNumber;
        if (string.IsNullOrWhiteSpace(accessionNumber))
        {
            accessionNumber = await _accessionNumberCalculator.GetAccessionNumberAsync("accessionnumber", data);
        }

        var patientRef = data.PatientRef;
        if (string.IsNullOrWhiteSpace(patientRef))
        {
            patientRef = await _accessionNumberCalculator.GetAccessionNumberAsync("patientreference", data);
        }

        // Load laboratory configuration for culture test defaults
        var laboratoryConfigurationHandler = _serviceProvider.GetService<ILaboratoryConfigurationHandler>();
        await laboratoryConfigurationHandler.LoadSingleLaboratoryConfigurationAsync(int.Parse(_token.LaboratoryId));

        _logWriter.LogInfo(
            $"Received specimen save starting: PatientId={data.PatientId}, newPatient={data.Surname != null && data.PatientId == 0}, AdmissionId={data.AdmissionId}, RequestId={data.RequestId}",
            nameof(ReceivedSpecimenRequestEvent),
            nameof(RunAsync));

        var specimenId = await _specimenRepository.CreateSpecimenAsync(
            data,
            true,
            command,
            _token,
            tableExceptions,
            dataToSave,
            accessionNumber,
            patientRef,
            laboratoryConfigurationHandler.LaboratoryConfigurationList
        );

        _logWriter.LogInfo(
            $"Received specimen save completed: specimenId={specimenId}",
            nameof(ReceivedSpecimenRequestEvent),
            nameof(RunAsync));

        return specimenId;
    }
}
