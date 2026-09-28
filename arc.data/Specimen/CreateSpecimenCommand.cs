using arc.app.Common;
using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Data;
using arc.common.Models;
using arc.common.Models.Config;
using arc.common.Models.Laboratory;
using arc.common.Models.Role;
using arc.common.Models.Specimen;
using arc.data.Configuration;
using arc.data.Instruments;
using arc.domain.Configuration.ListsConfig;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Creates a new specimen record together with the patient, admission and request it hangs from when
/// those are being created on the same form, and with its initial tests and cultures.
/// Admission and request are optional, so forms that create a specimen straight against a patient
/// leave both foreign keys null.
/// After inserting the initial culture rows the <c>Specimen.NextCultureNumber</c> watermark
/// is set to the highest culture number assigned, so that any subsequent isolate additions
/// continue from the correct next value and never reuse a number.
/// </summary>
internal class CreateSpecimenCommand
{
    private readonly IOptionsMonitor<DataOptions> _options;
    private readonly ILogger _logger;
    private readonly ILogWriter _logWriter;
    private readonly IGenerateMoreData _moreDataGenerator;
    private readonly IInstrumentInterfaceHandler _instrumentInterfaceHandler;

    /// <summary>
    /// Initialises a new instance of the <see cref="CreateSpecimenCommand"/> class.
    /// </summary>
    public CreateSpecimenCommand(
        IOptionsMonitor<DataOptions> options,
        ILogger logger,
        ILogWriter logWriter,
        IGenerateMoreData moreDataGenerator,
        IInstrumentInterfaceHandler instrumentInterfaceHandler)
    {
        _options = options;
        _logger = logger;
        _logWriter = logWriter;
        _moreDataGenerator = moreDataGenerator;
        _instrumentInterfaceHandler = instrumentInterfaceHandler;
    }

    /// <summary>
    /// Creates the patient, admission, request and specimen rows, updates the initial state,
    /// and inserts any crafted test and culture selections.
    /// </summary>
    /// <param name="connect">An open connection participating in the caller's transaction.</param>
    /// <param name="data">Parsed specimen payload including optional crafted test/culture pages.</param>
    /// <param name="specimenAlreadyReceived">When <see langword="true"/> the received date is taken from <paramref name="data"/> rather than being left null.</param>
    /// <param name="command">Event command metadata used for specimen state transitions.</param>
    /// <param name="token">Authenticated user token supplying laboratory and organisation context.</param>
    /// <param name="tableExceptions">Fields to exclude when generating the <c>MoreData</c> JSON blob.</param>
    /// <param name="json">Raw JSON payload, used to build the <c>MoreData</c> blob.</param>
    /// <param name="accessionNumber">Pre-computed accession number to stamp on the new specimen row.</param>
    /// <param name="patientref">Patient reference string used when a new patient record must be created inline.</param>
    /// <param name="laboratoryConfiguration">Laboratory configuration providing culture-test defaults.</param>
    /// <returns>The primary key (<c>Id</c>) of the newly created specimen.</returns>
    public async Task<int> ExecuteAsync(
        NpgsqlConnection connect,
        CreateSpecimenEventModel data,
        bool specimenAlreadyReceived,
        EventModel command,
        TokenInfoModel token,
        List<TableExceptionModel> tableExceptions,
        string json,
        string accessionNumber,
        string patientref,
        LaboratoryConfigurationListModel laboratoryConfiguration)
    {
        await ResolveLaboratoryAndOrganisationAsync(data, token);

        AssignParsedDates(data, specimenAlreadyReceived);

        if (data.Gender != null)
        {
            data.GenderId = int.Parse(data.Gender);
        }

        if (data.Surname != null && data.PatientId == 0)
        {
            data.MoreData = _moreDataGenerator.GetExceptionListString("patient", json, tableExceptions);
            LogMoreDataCount("patient", data.MoreData);

            data.FirstName = data.FirstName == "" ? " " : data.FirstName;
            data.Surname = data.Surname == "" ? " " : data.Surname;
            data.PatientRef = patientref;

            _logWriter.LogInfo(
                $"Create specimen stage patient: inserting new patient ref={patientref}",
                nameof(CreateSpecimenCommand),
                nameof(ExecuteAsync));
            try
            {
                data.PatientId = await connect.QueryFirstAsync<int>(
                    """
                    insert into Patient(patientref, firstname, surname, age, dateofbirth, telephonenumber,
                                        genderid, addressline1, addressline2, locationid, zipcode, barcode, lastmodifieddate, moredata)
                    values (@patientref, @firstname, @surname, @age, @dateofbirthasdate, @telephonenumber, @genderid,
                            @addressline1, @addressline2, @locationid, @zipcode, @patientbarcode, now(), cast(@MoreData as json))
                    returning id
                    """,
                    data);
            }
            catch (Exception ex)
            {
                LogStageFailure("patient", patientref, data.PatientId, ex);
                throw;
            }
        }

        if (data.AdmissionId == 0)
        {
            data.AdmissionMoreData = _moreDataGenerator.GetExceptionListString("admission", json, tableExceptions);
            LogMoreDataCount("admission", data.AdmissionMoreData);

            _logWriter.LogInfo(
                $"Create specimen stage admission: inserting for patientId={data.PatientId}",
                nameof(CreateSpecimenCommand),
                nameof(ExecuteAsync));
            try
            {
                data.AdmissionId = await connect.QueryFirstAsync<int>(
                    """
                    insert into Admission(patientid, moredata, lastmodifieddate)
                    values (@PatientId, cast(@MoreData as json), now())
                    returning id
                    """,
                    new { PatientId = data.PatientId, MoreData = data.AdmissionMoreData });
            }
            catch (Exception ex)
            {
                LogStageFailure("admission", patientref, data.PatientId, ex);
                throw;
            }
        }

        if (data.RequestId == 0)
        {
            data.RequestMoreData = _moreDataGenerator.GetExceptionListString("request", json, tableExceptions);
            LogMoreDataCount("request", data.RequestMoreData);

            _logWriter.LogInfo(
                $"Create specimen stage request: inserting for patientId={data.PatientId}, admissionId={data.AdmissionId}",
                nameof(CreateSpecimenCommand),
                nameof(ExecuteAsync));
            try
            {
                var request = await connect.QueryFirstAsync<(int Id, string RequestId)>(
                    """
                    insert into Request(patientid, admissionid, requestid, moredata, lastmodifieddate)
                    values (@PatientId,
                            @AdmissionId,
                            coalesce(@RequestReference, 'REQ' || to_char(now(), 'YY') || lpad(nextval('request_reference_seq')::text, 6, '0')),
                            cast(@MoreData as json),
                            now())
                    returning id, requestid
                    """,
                    new
                    {
                        PatientId = data.PatientId,
                        AdmissionId = data.AdmissionId,
                        RequestReference = string.IsNullOrWhiteSpace(data.RequestReference) ? null : data.RequestReference,
                        MoreData = data.RequestMoreData
                    });

                data.RequestId = request.Id;
                data.RequestReference = request.RequestId;
            }
            catch (Exception ex)
            {
                LogStageFailure("request", patientref, data.PatientId, ex);
                throw;
            }
        }

        data.AccessionNumber = accessionNumber;
        data.MoreData = _moreDataGenerator.GetMoreDataJsonString("specimen", json, tableExceptions);
        LogMoreDataCount("specimen", data.MoreData);

        _logWriter.LogInfo(
            $"Create specimen stage specimen: inserting accessionNumber={data.AccessionNumber}, patientId={data.PatientId}",
            nameof(CreateSpecimenCommand),
            nameof(ExecuteAsync));
        if (data.AgeYears.HasValue || data.AgeMonths.HasValue || data.AgeDays.HasValue || data.AgeHours.HasValue)
        {
            _logger?.LogDebug("Specimen age at creation: Years={Years}, Months={Months}, Days={Days}, Hours={Hours}", data.AgeYears, data.AgeMonths, data.AgeDays, data.AgeHours);
        }

        try
        {
            data.Id = await connect.QueryFirstAsync<int>(
                """
                insert into Specimen(patientid, admissionid, requestid, patientlocationid, diagnosisid, clinicalcontactno, barcode, existingbarcode,
                                     specimentypeid, specimensiteid, collectiondate, collectiontime, admissiondate, specimenweight,
                                     receivedconditionid, specimenappearanceid, receiveddate, receivedtime, lastmodifieddate, laboratoryid, organisationid, moredata,
                                     BottleOnlyWeight, BloodAndBottleWeight, AccessionNumber, RejectionReason, AgeYears, AgeMonths, AgeDays, AgeHours)
                values(@patientid, @admissionid, @requestid, @patientlocationid, @diagnosisid, @clinicalcontactno, @barcode, @existingbarcode,
                       @specimentypeid, @specimensiteid, @collectiondateasdate, @collectiontime,
                       @admissiondateasdate, @specimenweight, @receivedconditionid, @specimenappearanceid, @receiveddateasdate, @receivedtime, now(),
                       @laboratoryid, @organisationid, cast(@MoreData as json), @BottleOnlyWeight, @BloodAndBottleWeight, @AccessionNumber, @RejectionReason,
                       @AgeYears, @AgeMonths, @AgeDays, @AgeHours)
                returning id
                """,
                data);
        }
        catch (Exception ex)
        {
            LogStageFailure("specimen", patientref, data.PatientId, ex);
            throw;
        }

        await connect.ExecuteAsync(
            $"update {command.Table} set {command.Field} = @StateId, lastmodifieddate = now() where id = @Id",
            new { StateId = int.Parse(command.NewStateId), data.Id });

        await _instrumentInterfaceHandler.CreateInstrumentPendingResultsForSpecimenId(data.Id);

        if (data.Crafted != null)
        {
            for (var index = 0; index < data.Crafted.Count; index++)
            {
                if (data.Crafted[index].Name == "testselectionpage")
                {
                    await InsertDirectTestsAsync(connect, data.Id, data.Crafted[index].Contents);
                }

                if (data.Crafted[index].Name == "culturetypeselectionpage")
                {
                    data.ManufacturersBarcode ??= "";
                    await InsertCulturesAsync(connect, data.Id, data.Crafted[index].Contents, data.ManufacturersBarcode, laboratoryConfiguration);
                }
            }
        }

        return data.Id;
    }

    /// <summary>
    /// Normalizes date strings from the save payload into typed properties on <paramref name="data"/>.
    /// Optional dates (admission, date of birth) become null when empty; required specimen dates throw when missing.
    /// </summary>
    /// <param name="data">Specimen create payload whose string date fields are converted in place.</param>
    /// <param name="specimenAlreadyReceived">When true, <see cref="CreateSpecimenEventModel.ReceivedDate"/> must parse.</param>
    private void AssignParsedDates(CreateSpecimenEventModel data, bool specimenAlreadyReceived)
    {
        data.CollectionDateAsDate = RequireParsedDate(data.CollectionDate, nameof(data.CollectionDate));
        data.ReceivedDateAsDate = specimenAlreadyReceived
            ? RequireParsedDate(data.ReceivedDate, nameof(data.ReceivedDate))
            : null;
        data.AdmissionDateAsDate = ParseOptionalDate(data.AdmissionDate, nameof(data.AdmissionDate));
        data.DateOfBirthAsDate = ParseOptionalDate(data.DateOfBirth, nameof(data.DateOfBirth));

        _logWriter.LogInfo(
            $"Create specimen date parse: CollectionDate={FormatDateForLog(data.CollectionDateAsDate)}, " +
            $"ReceivedDate={FormatDateForLog(data.ReceivedDateAsDate)}, " +
            $"AdmissionDate={FormatDateForLog(data.AdmissionDateAsDate)}, " +
            $"DateOfBirth={FormatDateForLog(data.DateOfBirthAsDate)}, patientId={data.PatientId}, patientRef={data.PatientRef}",
            nameof(CreateSpecimenCommand),
            nameof(AssignParsedDates));
    }

    /// <summary>
    /// Parses a required date field, throwing when the value is missing or not parseable.
    /// </summary>
    private DateTime RequireParsedDate(string value, string fieldName)
    {
        var parsed = value.ToNullableDateTime();
        if (!parsed.HasValue)
        {
            _logWriter.LogError(
                $"Create specimen required date missing or invalid: {fieldName}, rawValue='{value ?? "(null)"}'",
                nameof(CreateSpecimenCommand),
                nameof(RequireParsedDate));
            throw new ArgumentException($"{fieldName} must be a valid date.");
        }

        return parsed.Value;
    }

    /// <summary>
    /// Parses an optional date field, logging when a non-empty value could not be parsed.
    /// </summary>
    private DateTime? ParseOptionalDate(string value, string fieldName)
    {
        var parsed = value.ToNullableDateTime();
        if (!string.IsNullOrWhiteSpace(value) && !parsed.HasValue)
        {
            _logger?.LogWarning(
                "Create specimen optional date could not be parsed: {FieldName}, rawValue='{RawValue}'",
                fieldName,
                value);
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            _logger?.LogDebug(
                "Create specimen optional date empty: {FieldName} → null",
                fieldName);
        }

        return parsed;
    }

    /// <summary>
    /// Formats a nullable date for diagnostic log output.
    /// </summary>
    private static string FormatDateForLog(DateTime? value) => value?.ToString("yyyy-MM-dd") ?? "null";

    /// <summary>
    /// Fills in whichever of laboratory and organisation the form did not supply. A client user is tied to
    /// their organisation and falls back to the first laboratory; a laboratory user is the other way round.
    /// </summary>
    private async Task ResolveLaboratoryAndOrganisationAsync(CreateSpecimenEventModel data, TokenInfoModel token)
    {
        var clientUser = !string.IsNullOrEmpty(token.OrganisationId) && token.OrganisationId != "0";

        using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
        await connect.OpenAsync();

        if (clientUser)
        {
            if (data.LaboratoryId == 0)
            {
                var result = await connect.QueryAsync<OptionsConfig>("select id as key from laboratory order by id");
                var laboratory = result.FirstOrDefault();
                data.LaboratoryId = !string.IsNullOrEmpty(laboratory?.Key) ? int.Parse(laboratory.Key) : 0;
            }

            data.OrganisationId = int.Parse(token.OrganisationId);
            return;
        }

        data.LaboratoryId = int.Parse(token.LaboratoryId);

        if (data.OrganisationId == 0)
        {
            var result = await connect.QueryAsync<OptionsConfig>("select id as key from organisation where enabled = 'Yes' order by id");
            var organisation = result.FirstOrDefault();
            data.OrganisationId = !string.IsNullOrEmpty(organisation?.Key) ? int.Parse(organisation.Key) : 0;
        }
    }

    /// <summary>
    /// Inserts the direct tests chosen on the test selection page and raises pending instrument results.
    /// </summary>
    private async Task InsertDirectTestsAsync(NpgsqlConnection connect, int specimenId, List<CraftedSelectionsModel> testSelection)
    {
        testSelection?.RemoveAll(c => c == null || c.Key == "XCategX");

        var sql = """
            insert into Tests(specimenid, testname, status, lastmodifieddate, requested)
            values(@SpecimenId, @TestName, 'Requested', now(), now())
            """;

        foreach (var test in testSelection ?? [])
        {
            if (test == null || test.Allowed != "Yes")
            {
                continue;
            }

            await connect.ExecuteAsync(sql, new { SpecimenId = specimenId, TestName = test.Key });
            await _instrumentInterfaceHandler.CreateInstrumentPendingResultsForDirectTestId(specimenId, test.Key);
        }
    }

    /// <summary>
    /// Inserts the cultures chosen on the culture type selection page together with their default culture tests.
    /// </summary>
    private async Task InsertCulturesAsync(
        NpgsqlConnection connect,
        int specimenId,
        List<CraftedSelectionsModel> cultureSelection,
        string manufacturersBarcode,
        LaboratoryConfigurationListModel laboratoryConfiguration)
    {
        cultureSelection?.RemoveAll(c => c == null || c.Key == "XCategX");

        var cultureTestConfig = laboratoryConfiguration.GetConfigurationsForFirstEntryInList("culturetypeculturetestdefault");
        var cultureNumber = 1;

        foreach (var culture in cultureSelection ?? [])
        {
            if (culture == null || culture.Allowed != "Yes")
            {
                continue;
            }

            var typeId = int.Parse(culture.Key);
            var cultureId = await connect.QueryFirstAsync<int>(
                """
                insert into Culture(specimenid, typeid, culturenumber, displayonreport, moredata, lastmodifieddate)
                values(@SpecimenId, @TypeId, @CultureNumber, 'Yes', cast(@MoreData as json), now())
                returning id
                """,
                new
                {
                    SpecimenId = specimenId,
                    TypeId = typeId,
                    CultureNumber = cultureNumber,
                    MoreData = "{\"ManufacturersBarcode\":\"" + manufacturersBarcode + "\"}"
                });

            cultureNumber++;

            var cultureTestEntry = cultureTestConfig?.FirstOrDefault(c => int.Parse(c.GroupId) == typeId);
            if (cultureTestEntry != null)
            {
                var configIds = cultureTestEntry.AssociatedListId.Split(',');

                if (configIds.Length > 0)
                {
                    await connect.ExecuteAsync(
                        """
                        insert into CultureTests (cultureid, testname, status, lastmodifieddate, requested)
                        select @CultureId, configname, 'Requested', now(), now()
                        from configs
                        where configname = ANY(@ConfigIds)
                        """,
                        new { CultureId = cultureId, ConfigIds = configIds });

                    foreach (var configId in configIds)
                    {
                        await _instrumentInterfaceHandler.CreateInstrumentPendingResultsForCultureTestId(specimenId, cultureId, configId);
                    }
                }
            }

            await connect.ExecuteAsync(
                "update culture set ParentCultureId = @CultureId where Id = @CultureId and ParentCultureId is null",
                new { cultureId });

            await _instrumentInterfaceHandler.CreateInstrumentPendingResultsForCultureId(specimenId, cultureId);
        }

        await connect.ExecuteAsync(
            "update Specimen set NextCultureNumber = @MaxCultureNumber where Id = @SpecimenId",
            new { MaxCultureNumber = cultureNumber - 1, SpecimenId = specimenId });
    }

    /// <summary>
    /// Logs contextual details when a create-specimen insert stage fails.
    /// </summary>
    /// <param name="stage">The save stage that failed (patient, admission, request, or specimen).</param>
    /// <param name="patientRef">Patient reference from the payload.</param>
    /// <param name="patientId">Patient id when known.</param>
    /// <param name="ex">The exception raised by the database call.</param>
    private void LogStageFailure(string stage, string patientRef, int patientId, Exception ex)
    {
        _logWriter.LogError(
            $"CreateSpecimen failed at stage: {stage}, patientRef={patientRef}, patientId={patientId}: {ex}",
            nameof(CreateSpecimenCommand),
            nameof(ExecuteAsync));
    }

    /// <summary>
    /// Logs the number of keys stored in a MoreData JSON fragment when non-empty.
    /// </summary>
    private void LogMoreDataCount(string tableName, string moreDataJson)
    {
        if (string.IsNullOrWhiteSpace(moreDataJson) || moreDataJson == "{}")
        {
            return;
        }

        try
        {
            var keys = Newtonsoft.Json.Linq.JObject.Parse(moreDataJson).Properties().Count();
            _logWriter.LogInfo(
                $"Create specimen {tableName} MoreData contains {keys} field(s)",
                nameof(CreateSpecimenCommand),
                nameof(LogMoreDataCount));
        }
        catch
        {
            _logWriter.LogInfo(
                $"Create specimen {tableName} MoreData updated",
                nameof(CreateSpecimenCommand),
                nameof(LogMoreDataCount));
        }
    }
}
