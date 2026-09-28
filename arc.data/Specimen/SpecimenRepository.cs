using arc.app.Common;
using arc.app.Specimen;
using arc.common;
using arc.common.Data;
using arc.common.Models;
using arc.common.Models.Config;
using arc.common.Models.Home;
using arc.common.Models.Laboratory;
using arc.common.Models.Patient;
using arc.common.Models.Specimen;
using arc.common.Utils;
using arc.data.Configuration;
using arc.data.Instruments;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace arc.data.Specimen
{
    public class SpecimenRepository : ISpecimenRepository
    {
        private readonly IOptionsMonitor<DataOptions> _options;
        private readonly ILogger _logger;
        private readonly ILogWriter _logWriter;
        private readonly IGenerateMoreData _moreDataGenerator;
        private readonly ISqlQuery _sqlQuery;
        private readonly ISqlCommand _sqlCommand;
        private readonly IMoreDataRepository _moreDataRepository;
        private readonly IJsonUtils _jsonUtils;
        private readonly IJsonReplacer _jsonReplacer;
        private readonly IInstrumentInterfaceHandler _instrumentInterfaceHandler;
        private readonly IJsonElementRemover _jsonRemover;

        public SpecimenRepository(IOptionsMonitor<DataOptions> options, ILogger logger, IGenerateMoreData moreDataGenerator, ISqlQuery sqlQuery, ILogWriter logWriter, ISqlCommand sqlCommand, IMoreDataRepository moreDataRepository, IJsonUtils jsonUtils
            , IJsonReplacer jsonReplacer, IInstrumentInterfaceHandler instrumentInterfaceHandler, IJsonElementRemover jsonRemover)
        {
            _options = options;
            _logger = logger;
            _moreDataGenerator = moreDataGenerator;
            _sqlQuery = sqlQuery;
            _logWriter = logWriter;
            _sqlCommand = sqlCommand;
            _moreDataRepository = moreDataRepository;
            _jsonUtils = jsonUtils;
            _jsonReplacer = jsonReplacer;
            _instrumentInterfaceHandler = instrumentInterfaceHandler;
            _jsonRemover = jsonRemover;
        }

        /// <summary>
        /// Creates a new specimen record together with the patient, admission and request it hangs from when
        /// those are being created on the same form, and with its initial tests and cultures.
        /// Admission and request are optional, so forms that create a specimen straight against a patient
        /// leave both foreign keys null.
        /// After inserting the initial culture rows the <c>Specimen.NextCultureNumber</c> watermark
        /// is set to the highest culture number assigned, so that any subsequent isolate additions
        /// continue from the correct next value and never reuse a number.
        /// </summary>
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
        /// <exception cref="Exception">Propagates any database or transaction failure after logging contextual details.</exception>
        public async Task<int> CreateSpecimenAsync(CreateSpecimenEventModel data, bool specimenAlreadyReceived, EventModel command, TokenInfoModel token, List<TableExceptionModel> tableExceptions, string json, string accessionNumber, string patientref, LaboratoryConfigurationListModel laboratoryConfiguration)
        {
            var creatingPatient = data.Surname != null && data.PatientId == 0;
            var creatingAdmission = data.AdmissionId == 0;
            var creatingRequest = data.RequestId == 0;
            var formName = ReadFormName(json);

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {
                    var createCommand = new CreateSpecimenCommand(_options, _logger, _logWriter, _moreDataGenerator, _instrumentInterfaceHandler);
                    await createCommand.ExecuteAsync(connect, data, specimenAlreadyReceived, command, token, tableExceptions, json, accessionNumber, patientref, laboratoryConfiguration);
                }

                scope.Complete();

                return data.Id;
            }
            catch (TransactionAbortedException ex)
            {
                LogCreateSpecimenFailure(ex, command?.Event, data?.PatientId ?? 0, patientref, formName, creatingPatient, creatingAdmission, creatingRequest);
                throw;
            }
            catch (Exception ex)
            {
                LogCreateSpecimenFailure(ex, command?.Event, data?.PatientId ?? 0, patientref, formName, creatingPatient, creatingAdmission, creatingRequest);
                throw;
            }
        }

        /// <summary>
        /// Logs contextual details when a specimen create transaction fails.
        /// </summary>
        private void LogCreateSpecimenFailure(Exception ex, string eventName, int patientId, string patientRef, string formName, bool creatingPatient, bool creatingAdmission, bool creatingRequest)
        {
            _logWriter.LogError(
                $"Create specimen failed: event={eventName}, form={formName}, patientId={patientId}, patientRef={patientRef}, " +
                $"creatingPatient={creatingPatient}, creatingAdmission={creatingAdmission}, creatingRequest={creatingRequest}: {ex}",
                nameof(SpecimenRepository),
                nameof(CreateSpecimenAsync));
        }

        /// <summary>
        /// Reads <c>FormName</c> from the raw save payload when present.
        /// </summary>
        private static string ReadFormName(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return string.Empty;
            }

            try
            {
                var token = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);
                return token?["FormName"]?.ToString() ?? string.Empty;
            }
            catch (JsonException)
            {
                return string.Empty;
            }
        }

        public async Task<SpecimenPatientModel> GetSingleAsync(int id)
        {
            var queryFilters = new QueryFilterConfig().AddInteger("id", id);
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenPatientByIdQuery(), "Get single specimen", queryFilters);
        }

        public async Task<SpecimenPatientModel> GetCultureSingleAsync(int id)
        {
            var queryFilters = new QueryFilterConfig().AddInteger("id", id);
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenPatientByCultureIdQuery(), "Get single culture", queryFilters);
        }

        public async Task<SpecimenPatientModel> GetSingleSpecimenTagAsync(int id)
        {
            var queryFilters = new QueryFilterConfig().AddInteger("id", id);
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenPatientBySpecimenTagIdQuery(), "Get single specimen tag", queryFilters);
        }

        public async Task<int> SetSpecimenTagsAsync(int specimenId, IEnumerable<int> listItemIds)
        {
            var model = new SetSpecimenTagsModel { SpecimenId = specimenId, ListItemIds = listItemIds };
            return await _sqlCommand.CommandWithTypeQueryAsync(new SetSpecimenTagsCommand(), "Set Specimen Tags", model, _logWriter);
        }

        public async Task<int> AddSpecimenTagsAsync(int specimenId, IEnumerable<int> listItemIds)
        {
            var model = new SetSpecimenTagsModel { SpecimenId = specimenId, ListItemIds = listItemIds };
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddSpecimenTagsCommand(), "Add Specimen Tags", model, _logWriter);
        }

        /// <summary>
        /// Replaces all specimen file attachments with the given list of file attachment IDs.
        /// Deletes existing links for the specimen, then inserts the new set.
        /// </summary>
        public async Task<int> SetSpecimenFileAttachmentsAsync(int specimenId, IEnumerable<int> fileAttachmentIds)
        {
            var model = new SetSpecimenFileAttachmentsModel
            {
                SpecimenId = specimenId,
                FileAttachmentIds = fileAttachmentIds
            };
            return await _sqlCommand.CommandWithTypeQueryAsync(
                new SetSpecimenFileAttachmentsCommand(),
                "Set Specimen File Attachments",
                model,
                _logWriter);
        }

        /// <summary>
        /// Returns specimen and patient context for an existing specimen comment row.
        /// </summary>
        /// <param name="id">The <c>specimencomment.id</c> value, not a culture or specimen id.</param>
        /// <returns>Specimen and patient context, or an empty model when the comment is not found.</returns>
        public async Task<SpecimenPatientModel> GetSingleCommentAsync(int id)
        {
            var queryFilters = new QueryFilterConfig().AddInteger("id", id);
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenPatientByCommentIdQuery(), "Get single specimen comment", queryFilters);
        }

        public async Task<SpecimenPatientModel> GetSingleFromDirectTestsAsync(int id)
        {
            var queryFilters = new QueryFilterConfig().AddInteger("id", id);
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenPatientByDirectTestIdQuery(), "Get single specimen from direct tests", queryFilters);
        }

        public async Task<SpecimenPatientModel> GetSingleFromCultureTestsAsync(int id)
        {
            var queryFilters = new QueryFilterConfig().AddInteger("id", id);
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenPatientByCultureTestIdQuery(), "Get single specimen from culture tests", queryFilters);
        }

        public async Task<int> GetSpecimenTypeAsync(int id)
        {
            var queryFilters = new QueryFilterConfig().AddInteger("id", id);
            return await _sqlQuery.QueryReturningIntegerAsync(new SpecimenTypeBySpecimenIdQuery(), "Get specimen type", queryFilters);
        }

        public async Task<int> GetSpecimenTypeFromCultureAsync(int id)
        {
            var queryFilters = new QueryFilterConfig().AddInteger("id", id);
            return await _sqlQuery.QueryReturningIntegerAsync(new SpecimenTypeByCultureIdQuery(), "Get specimen type from culture", queryFilters);
        }

        public async Task<DateTime> GetCollectionDateAsync(int id)
        {
            var queryFilters = new QueryFilterConfig().AddInteger("id", id);
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenCollectionDateQuery(), "Get specimen collection date", queryFilters);
        }

        public async Task<IEnumerable<SpecimenBatchListModel>> SpecimenBatchListAsync(QueryFilterConfig queryFilters, TokenInfoModel token)
        {
            queryFilters.Parameters.Add(new QueryValuesConfig { Key = "LaboratoryId", Value = token.LaboratoryId });
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenBatchListQuery(), "Specimen Batch List", queryFilters);
        }

        public async Task<List<SpecimenCountModel>> SpecimenTypeCountAsync(QueryFilterConfig queryFilters)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenTypeCountQuery(), "Specimen Type Count", queryFilters);
        }

        public async Task<List<SpecimenCountModel>> SpecimenStateCountAsync(QueryFilterConfig queryFilters)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenStateCountQuery(), "Specimen State Count", queryFilters);
        }

        public async Task<List<SpecimenCountModel>> SpecimenTagCountAsync(QueryFilterConfig queryFilters)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenTagCountQuery(), "Specimen Tag Count", queryFilters);
        }

        /// <inheritdoc />
        public async Task<List<HomeDashboardRecentItemModel>> HomeDashboardRecentlyUsedAsync(QueryFilterConfig queryFilters)
        {
            var rows = await _sqlQuery.QueryReturningTypeAsync(new HomeDashboardRecentlyUsedQuery(), "Home dashboard recently used", queryFilters);
            _logWriter.LogInfo(
                $"Home dashboard recently used returned {rows?.Count ?? 0} rows",
                nameof(SpecimenRepository),
                nameof(HomeDashboardRecentlyUsedAsync));
            return rows;
        }

        /// <inheritdoc />
        public async Task<TatComplianceKpiModel> HomeDashboardTatComplianceAsync(QueryFilterConfig queryFilters)
        {
            var result = await _sqlQuery.QueryReturningTypeAsync(new HomeDashboardTatComplianceQuery(), "Home dashboard TAT compliance", queryFilters);
            _logWriter.LogInfo(
                $"Home dashboard TAT compliance: total={result?.TotalCount ?? 0} onTime={result?.OnTimeCount ?? 0} late={result?.LateCount ?? 0} rag={result?.Rag}",
                nameof(SpecimenRepository),
                nameof(HomeDashboardTatComplianceAsync));
            return result;
        }

        public async Task<SpecimenApprovalModel> SpecimenApprovalAsync(QueryFilterConfig queryFilters)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenApprovalQuery(), "Specimen Approval", queryFilters);
        }

        public async Task<SpecimenModel> SpecimenByIdAsync(QueryFilterConfig queryFilters)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenByIdQuery(), "Specimen By Id Query", queryFilters);
        }

        public async Task AcknowledgeReceiptCommandAsync(ACKReceiptEventModel dataToSave, List<CultureTestConfigModel> cultureTestConfig, string json)
        {
            _logWriter.LogInfo("Acknowledge receipt command", "SpecimenRepository", "AcknowledgeReceiptCommandAsync");

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
            {
                var command = new AcknowledgeSpecimenCommand(_instrumentInterfaceHandler, _moreDataGenerator, _moreDataRepository, _logWriter, _jsonReplacer, _jsonRemover);
                await command.ExecuteAsync(connect, dataToSave, cultureTestConfig, json);
            }
            scope.Complete();
        }

        public async Task EditSpecimenAsync(string dataToSave, string configName, string newStateId)
        {
            _logWriter.LogInfo("Run edit specimen command", "SpecimenRepository", "SpecimenAlertAsync");
            await _sqlCommand.CarryOutCommandAsync(new EditSpecimenCommand(_moreDataGenerator, _logWriter, _jsonUtils, _moreDataRepository, _jsonReplacer, _jsonRemover), "Edit Specimen", dataToSave, configName, newStateId);
        }

        /// <inheritdoc />
        public async Task EditSpecimenWithRelatedEntitiesAsync(string dataToSave, List<TableExceptionModel> tableExceptions, string newStateId)
        {
            _logWriter.LogInfo(
                $"Edit specimen transaction starting with {tableExceptions?.Count ?? 0} table exception(s)",
                nameof(SpecimenRepository),
                nameof(EditSpecimenWithRelatedEntitiesAsync));
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
            await connect.OpenAsync();

            var editCommand = new EditSpecimenCommand(_moreDataGenerator, _logWriter, _jsonUtils, _moreDataRepository, _jsonReplacer, _jsonRemover);
            var multiTableCommand = new EditSpecimenMultiTableCommand(_moreDataGenerator, _moreDataRepository, _jsonUtils, _logWriter, editCommand);
            await multiTableCommand.ExecuteAsync(connect, dataToSave, tableExceptions, newStateId);
            scope.Complete();
            _logWriter.LogInfo(
                "Edit specimen transaction completed",
                nameof(SpecimenRepository),
                nameof(EditSpecimenWithRelatedEntitiesAsync));
        }

        public async Task<string> GetSpecimenIdByAccessionNumberAsync(string accessionNumber)
        {
            var queryFilters = new QueryFilterConfig().AddString("AccessionNumber", accessionNumber);
            return await _sqlQuery.QueryReturningStringAsync(new IdByAccessionNumberQuery(), "Get specimen id by accession number query", queryFilters);
        }

        public async Task<int> MoveSpecimenAsync(string dataToSave)
        {
            var tableExceptions = new List<TableExceptionModel> {
                new TableExceptionModel { Table = "patient", Field = "gender" },
                new TableExceptionModel { Table = "patient", Field = "patientid" }
            };
            var specimenInfo = JsonConvert.DeserializeObject<MovePatientModel>(dataToSave);
            specimenInfo.MoreData = _moreDataGenerator.GetMoreDataJsonString("patient", dataToSave, tableExceptions);
            _logWriter.LogInfo("Move specimen command", "SpecimenRepository", "MoveSpecimenAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new MoveSpecimenCommand(), "Move Specimen", specimenInfo);
        }

        public async Task<Dictionary<int, DateTime?>> GetSpecimenFinalisedDatesAsync(IEnumerable<int> specimenIds)
        {
            var queryFilters = new QueryFilterConfig().AddString("specimenids", string.Join(",", specimenIds ?? []));
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenFinalisedDatesQuery(), "Get specimen finalised dates", queryFilters);
        }

        /// <inheritdoc />
        public async Task<Dictionary<int, DateTime?>> GetSpecimenTerminalStateEndTimesAsync(IEnumerable<int> specimenIds)
        {
            var queryFilters = new QueryFilterConfig().AddString("specimenids", string.Join(",", specimenIds ?? []));
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenTerminalStateEndTimesQuery(), "Get specimen terminal state end times", queryFilters);
        }
    }
}
