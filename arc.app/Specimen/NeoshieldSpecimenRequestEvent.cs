using arc.app.Barcodes;
using arc.app.Common;
using arc.app.Config;
using arc.app.Configuration;
using arc.app.Laboratory;
using arc.app.Settings;
using arc.common;
using arc.common.Models;
using arc.common.Models.Config;
using arc.common.Models.Specimen;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Creates a specimen from the Neoshield neonatal request form. The form differs from the received specimen
/// form in that the specimen may hang off an admission and a request as well as the patient, and in that the
/// request header carries values the ward does not type: they are stamped here from the clock and from the
/// identity of the user raising the request.
/// </summary>
public class NeoshieldSpecimenRequestEvent : IRun, ICreateParentRecords
{
    private readonly ISpecimenRepository _specimenRepository;
    private readonly IBarcodesHandler _barcodesHandler;
    private readonly ILogWriter _logWriter;
    private readonly IAccessionNumberCalculator _accessionNumberCalculator;
    private readonly IListViewConfigFactory _listViewConfigFactory;
    private readonly TokenInfoModel _token;
    private readonly IServiceProvider _serviceProvider;

    /// <inheritdoc />
    public int CreatedPatientId { get; private set; }

    /// <inheritdoc />
    public int? CreatedAdmissionId { get; private set; }

    /// <inheritdoc />
    public int? CreatedRequestId { get; private set; }

    /// <summary>
    /// Initialises a new instance of the <see cref="NeoshieldSpecimenRequestEvent"/> class.
    /// </summary>
    public NeoshieldSpecimenRequestEvent(
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
    /// Stamps the system-supplied request header values, then creates the patient, admission, request and
    /// specimen in the single transaction owned by the repository.
    /// </summary>
    /// <param name="dataToSave">Serialised event payload from the Neoshield form.</param>
    /// <param name="Id">Identifier associated with the operation, unused for a create.</param>
    /// <param name="command">The event model representing the trigger action.</param>
    /// <param name="eventData">Event configuration supplying the table exception routing and default view.</param>
    /// <returns>The identifier of the created specimen.</returns>
    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        dataToSave = StampSystemSuppliedRequestValues(dataToSave);

        var data = ArcJson.Deserialize<CreateSpecimenEventModel>(dataToSave);

        RemoveCategoryMarker(data, "testselectionpage");
        RemoveCategoryMarker(data, "culturetypeselectionpage");

        if (data.Surname != null && data.PatientId == 0)
        {
            _logWriter.LogInfo("Getting the patient barcode", "NeoshieldSpecimenRequestEvent", "Run");
            data.PatientBarcode = await _barcodesHandler.GetBarcodeAsync();
        }

        var routingService = _serviceProvider.GetRequiredService<IFormPageTableRoutingService>();
        var tableExceptions = await SpecimenFormSaveRouting.BuildTableExceptionsAsync(
            routingService, dataToSave, eventData);

        _logWriter.LogInfo(
            $"Neoshield specimen save routing: {tableExceptions.Count} table exception(s)",
            "NeoshieldSpecimenRequestEvent",
            "RunAsync");

        _ = eventData.DefaultView != null
            ? await _listViewConfigFactory.GetViewAsync(eventData.DefaultView)
            : await _listViewConfigFactory.GetViewAsync(data.View);

        var accessionNumber = string.IsNullOrWhiteSpace(data.AccessionNumber)
            ? await _accessionNumberCalculator.GetAccessionNumberAsync("accessionnumber", data)
            : data.AccessionNumber;

        var patientRef = string.IsNullOrWhiteSpace(data.PatientRef)
            ? await _accessionNumberCalculator.GetAccessionNumberAsync("patientreference", data)
            : data.PatientRef;

        _logWriter.LogInfo(
            $"Neoshield specimen save starting: PatientId={data.PatientId}, newPatient={data.Surname != null && data.PatientId == 0}, AdmissionId={data.AdmissionId}, RequestId={data.RequestId}",
            nameof(NeoshieldSpecimenRequestEvent),
            nameof(RunAsync));

        var laboratoryConfigurationHandler = _serviceProvider.GetService<ILaboratoryConfigurationHandler>();
        await laboratoryConfigurationHandler.LoadSingleLaboratoryConfigurationAsync(int.Parse(_token.LaboratoryId));

        var specimenId = await _specimenRepository.CreateSpecimenAsync(
            data,
            false,
            command,
            _token,
            tableExceptions,
            dataToSave,
            accessionNumber,
            patientRef,
            laboratoryConfigurationHandler.LaboratoryConfigurationList);

        // The repository fills these in as it creates each record, so they now hold the ids the next
        // specimen on this request should attach to rather than the zeros the form sent.
        CreatedPatientId = data.PatientId;
        CreatedAdmissionId = data.AdmissionId;
        CreatedRequestId = data.RequestId;

        _logWriter.LogInfo(
            $"Neoshield specimen save completed: specimenId={specimenId}, patientId={CreatedPatientId}, admissionId={CreatedAdmissionId}, requestId={CreatedRequestId}",
            nameof(NeoshieldSpecimenRequestEvent),
            nameof(RunAsync));

        return specimenId;
    }

    /// <summary>
    /// Adds the request header values the ward never types to the payload before it is split between tables,
    /// so they reach Request.MoreData through the same routing as the typed fields. Anything the payload
    /// already carries is left alone, which keeps a replayed queue message stamped with its original values.
    /// </summary>
    private string StampSystemSuppliedRequestValues(string dataToSave)
    {
        var payload = JObject.Parse(dataToSave);

        if (!IsNewRequest(payload))
        {
            return dataToSave;
        }

        var now = DateTime.Now;
        var stamped = new Dictionary<string, string>
        {
            { "RequestDate", now.ToString("dd/MM/yyyy") },
            { "RequestTime", now.ToString("HH:mm") },
            { "RequestingClinician", $"{_token.FirstName} {_token.LastName}".Trim() },
            { "RequestingClinicianUsername", _token.Username }
        };

        foreach (var value in stamped.Where(v => !string.IsNullOrWhiteSpace(v.Value)))
        {
            if (payload.Property(value.Key) == null)
            {
                payload.Add(value.Key, value.Value);
            }
        }

        return payload.ToString();
    }

    /// <summary>
    /// A request id of zero is the form asking for a new request; anything else attaches the specimen to a
    /// request that already carries its own header values.
    /// </summary>
    private static bool IsNewRequest(JObject payload)
    {
        var requestId = payload.Property("RequestId")?.Value;
        return requestId != null && requestId.Type != JTokenType.Null && requestId.ToString() == "0";
    }

    /// <summary>
    /// Strips the category marker the crafted selection pages carry so it is not treated as a chosen test.
    /// </summary>
    private static void RemoveCategoryMarker(CreateSpecimenEventModel data, string pageName)
    {
        var page = data.Crafted?.FirstOrDefault(t => t.Name == pageName);
        if (page?.Contents == null)
        {
            return;
        }

        var marker = page.Contents.FirstOrDefault(item => item.Key == "XCategX");
        if (marker != null)
        {
            page.Contents.Remove(marker);
        }
    }
}
