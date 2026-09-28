using arc.app.Common;
using arc.app.Instruments;
using arc.common.Data;
using arc.common.Models.Instruments;
using arc.data.Common;
using arc.data.Instruments;
using arc.data.model.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Instruments;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen
{
    /// <summary>
    /// Repository of all the methods which query or update instrument information.
    /// Author: Arcta Solutions Limited
    /// </summary>
    public class InstrumentRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter, IGenerateMoreData moreDataGenerator) : GeneralRepository(sqlQuery, logWriter, sqlCommand), IInstrumentRepository
    {
        private readonly IGenerateMoreData _moreDataGenerator = moreDataGenerator;

        /// <summary>
        /// Gets the list of instrument result based upon the criteria specified.
        /// </summary>
        /// <param name="queryFilter">Filter conditions for the query</param>
        /// <returns>List of instrument results</returns>
        public async Task<List<InstrumentResultsListModel>> GetInstrumentResultsListAsync(QueryFilterConfig queryFilter)
        {
            _logWriter.LogInfo("Get instrument result list query", nameof(InstrumentRepository), nameof(GetInstrumentResultsListAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new InstrumentResultListQuery(), "Get instrument result list Query", queryFilter);
        }

        /// <summary>
        /// Gets the list of instrument result for a culture
        /// </summary>
        /// <param name="queryFilter">Filter conditions for the query</param>
        /// <returns>List of instrument results</returns>
        public async Task<List<InstrumentResultsListModel>> GetCultureInstrumentResultsAsync(QueryFilterConfig queryFilter)
        {
            _logWriter.LogInfo("Get culture instruments results query", nameof(InstrumentRepository), nameof(GetCultureInstrumentResultsAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new CultureInstrumentResultsQuery(), "Get culture instrument result list Query", queryFilter);
        }

        /// <summary>
        /// Gets the list of instrument result for a specimen
        /// </summary>
        /// <param name="queryFilter">Filter conditions for the query</param>
        /// <returns>List of instrument results</returns>
        public async Task<List<InstrumentResultsListModel>> GetSpecimenInstrumentResultsAsync(QueryFilterConfig queryFilter)
        {
            _logWriter.LogInfo("Get specimen instruments results query", nameof(InstrumentRepository), nameof(GetSpecimenInstrumentResultsAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenInstrumentResultsQuery(), "Get specimen instrument result list Query", queryFilter);
        }

        /// <inheritdoc />
        public async Task<List<InstrumentResultsListModel>> GetTestInstrumentResultsAsync(QueryFilterConfig queryFilter)
        {
            var id = queryFilter.Parameters?.FirstOrDefault(p => p.Key.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value;
            var source = queryFilter.Parameters?.FirstOrDefault(p => p.Key.Equals("source", StringComparison.OrdinalIgnoreCase))?.Value;
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(source))
            {
                _logWriter.LogInfo("GetTestInstrumentResultsAsync: missing id or source in query filters", nameof(InstrumentRepository), nameof(GetTestInstrumentResultsAsync));
                return new List<InstrumentResultsListModel>();
            }

            _logWriter.LogInfo($"Get test instrument results query (test id={id}, source={source})", nameof(InstrumentRepository), nameof(GetTestInstrumentResultsAsync));
            var list = await _sqlQuery.QueryReturningTypeAsync(new TestInstrumentResultsQuery(), "Get test instrument result list Query", queryFilter);
            if (list.Count == 0)
                _logWriter.LogInfo($"GetTestInstrumentResultsAsync: no matching instrumentresults rows for test id={id}, source={source}", nameof(InstrumentRepository), nameof(GetTestInstrumentResultsAsync));
            return list;
        }

        /// <summary>
        /// Asynchronously retrieves the next set of instrument requests based on the specified query filter.
        /// </summary>
        /// <param name="queryFilter">The query filter configuration.
        /// Queryfilters are:
        /// 1. Profiles - the name of the profiles to get the next request for
        /// 2. Batch - The number ofinstrument requests to get
        /// 3. InstrumentMachineId (optional) - when set to a positive integer, only rows with that <c>instrumentresults.instrumentmachineid</c> are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of instrument request models.</returns>
        public async Task<List<InstrumentRequestModel>> GetNextRequestAsync(QueryFilterConfig queryFilter)
        {
            var machineParam = queryFilter.Parameters?.FirstOrDefault(p => p.Key != null && p.Key.ToLower() == "instrumentmachineid");
            var machineLog = machineParam != null && int.TryParse(machineParam.Value, out var mid) && mid > 0
                ? $" instrumentMachineId={mid}"
                : "";
            _logWriter.LogInfo($"Get instrument requests query{machineLog}", nameof(InstrumentRepository), nameof(GetNextRequestAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new InstrumentRequestQuery(), "Get Requests for instrument Query", queryFilter);
        }


        /// <summary>
        /// Asynchronously confirms the specified requests.
        /// </summary>
        /// <param name="requestModel">The request confirmation model containing the data to confirm.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
        public async Task<int> ConfirmRequestsAsync(RequestConfirmModel requestModel)
        {
            var attachmentCount = requestModel.SourceFileAttachmentIds?.Count ?? 0;
            _logWriter.LogInfo(
                $"Run request confirmation command instrumentResultId={requestModel.Id} sourceFileAttachmentIdsCount={attachmentCount}",
                nameof(InstrumentRepository),
                nameof(ConfirmRequestsAsync));
            return await _sqlCommand.CommandWithTypeQueryAsync(new ConfirmRequestCommand(), "Confirm requests", requestModel);
        }


        /// <summary>
        /// Asynchronously updates the instrument culture.
        /// </summary>
        /// <param name="responseModel">The response model containing the data to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
        public async Task<int> InstrumentCultureUpdateAsync(ResponseModel responseModel)
        {
            _logWriter.LogInfo("Run instrument culture command", nameof(InstrumentRepository), nameof(InstrumentCultureUpdateAsync));
            return await _sqlCommand.CommandWithTypeQueryAsync(new InstrumentCultureUpdateCommand(), "Instrument culture update", responseModel);
        }


        /// <summary>
        /// Asynchronously adds a new instrument result.
        /// </summary>
        /// <param name="dataToSave">The instrument result data to save.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
        public async Task<int> AddAsync(InstrumentResult dataToSave)
        {
            _logWriter.LogInfo("Run add instrument command", nameof(InstrumentRepository), nameof(AddAsync));
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddInstrumentCommand(), "Add Instrument result", dataToSave);
        }


        /// <summary>
        /// Asynchronously updates the instrument data. This will edit the data if it already exists or add it if it does not.
        /// </summary>
        /// <param name="dataToSave">The instrument result data to save.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
        public async Task<int> UpdateAsync(InstrumentResult dataToSave)
        {
            _logWriter.LogInfo("Run update instrument command", nameof(InstrumentRepository), nameof(UpdateAsync));
            return await _sqlCommand.CommandWithTypeQueryAsync(new UpdateInstrumentCommand(), "Update Instrument result", dataToSave);
        }


        /// <summary>
        /// Asynchronously edits the instrument data.
        /// </summary>
        /// <param name="dataToSave">The data to save as a JSON string.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
        public async Task<int> EditAsync(string dataToSave)
        {
            var instrument = JsonConvert.DeserializeObject<InstrumentResult>(dataToSave);
            instrument.MoreData = _moreDataGenerator.GetMoreDataJsonString("instrumentresults", dataToSave);
            _logWriter.LogInfo("Run edit instrument command", nameof(InstrumentRepository), nameof(EditAsync));
            return await _sqlCommand.CommandWithTypeQueryAsync(new EditInstrumentCommand(), "Add Organisation", instrument);
        }


        /// <inheritdoc />
        public async Task<InboundInstrumentMatchResult> ResolveInboundInstrumentResultAsync(
            int instrumentResultId,
            string profileName,
            string accessionNumber,
            string cultureNumber,
            int cultureIdFromPayload,
            int? instrumentMachineId,
            bool isDirectTestProfile,
            string resolvedDirectTestName)
        {
            _logWriter.LogInfo("Resolve inbound instrument result", nameof(InstrumentRepository), nameof(ResolveInboundInstrumentResultAsync));

            if (instrumentResultId > 0)
            {
                var qf = new QueryFilterConfig();
                qf.AddInteger("id", instrumentResultId);
                var row = await _sqlQuery.QueryReturningTypeAsync(new InstrumentResultByIdQuery(), "Instrument result by id", qf);
                return new InboundInstrumentMatchResult { Row = row, IsAmbiguous = false };
            }

            if (string.IsNullOrWhiteSpace(accessionNumber))
                return new InboundInstrumentMatchResult { Row = null, IsAmbiguous = false };

            if (isDirectTestProfile)
            {
                if (string.IsNullOrWhiteSpace(resolvedDirectTestName))
                    return new InboundInstrumentMatchResult { Row = null, IsAmbiguous = false };

                var qf = new QueryFilterConfig();
                qf.AddString("profilename", profileName ?? "");
                qf.AddString("accessionnumber", accessionNumber.Trim());
                qf.AddString("matchdirect", "true");
                qf.AddString("directtestname", resolvedDirectTestName.Trim());
                if (instrumentMachineId.HasValue)
                    qf.AddInteger("instrumentmachineid", instrumentMachineId.Value);

                var list = await _sqlQuery.QueryReturningTypeAsync(new InstrumentResultInboundCompositeMatchQuery(), "Inbound instrument composite match (direct)", qf);
                if (list.Count > 1)
                {
                    _logWriter.LogInfo($"Inbound match ambiguous: {list.Count} rows (direct)", nameof(InstrumentRepository), nameof(ResolveInboundInstrumentResultAsync));
                    return new InboundInstrumentMatchResult { Row = null, IsAmbiguous = true };
                }

                return new InboundInstrumentMatchResult { Row = list.Count == 1 ? list[0] : null, IsAmbiguous = false };
            }

            if (string.IsNullOrWhiteSpace(cultureNumber))
                return new InboundInstrumentMatchResult { Row = null, IsAmbiguous = false };

            var qfCulture = new QueryFilterConfig();
            qfCulture.AddString("profilename", profileName ?? "");
            qfCulture.AddString("accessionnumber", accessionNumber.Trim());
            qfCulture.AddString("culturenumber", cultureNumber.Trim());
            qfCulture.AddString("matchdirect", "false");
            qfCulture.AddInteger("cultureid", cultureIdFromPayload);
            if (instrumentMachineId.HasValue)
                qfCulture.AddInteger("instrumentmachineid", instrumentMachineId.Value);

            var listCulture = await _sqlQuery.QueryReturningTypeAsync(new InstrumentResultInboundCompositeMatchQuery(), "Inbound instrument composite match (culture)", qfCulture);
            if (listCulture.Count > 1)
            {
                _logWriter.LogInfo($"Inbound match ambiguous: {listCulture.Count} rows (culture)", nameof(InstrumentRepository), nameof(ResolveInboundInstrumentResultAsync));
                return new InboundInstrumentMatchResult { Row = null, IsAmbiguous = true };
            }

            return new InboundInstrumentMatchResult { Row = listCulture.Count == 1 ? listCulture[0] : null, IsAmbiguous = false };
        }

        /// <inheritdoc />
        public async Task<string> GetAccessionNumberForSpecimenIdAsync(int specimenId)
        {
            var qf = new QueryFilterConfig();
            qf.AddInteger("specimenid", specimenId);
            return await _sqlQuery.QueryReturningStringAsync(new SpecimenAccessionNumberBySpecimenIdQuery(), "Specimen accession by specimen id", qf);
        }

        /// <inheritdoc />
        public async Task<string> GetCultureNumberTextForCultureIdAsync(int cultureId)
        {
            var qf = new QueryFilterConfig();
            qf.AddInteger("cultureid", cultureId);
            return await _sqlQuery.QueryReturningStringAsync(new CultureNumberTextByCultureIdQuery(), "Culture number by culture id", qf);
        }

        /// <inheritdoc />
        public async Task<InstrumentResultRecordViewModel> GetInstrumentResultRecordViewByIdAsync(QueryFilterConfig queryFilter)
        {
            _logWriter.LogInfo("Instrument result record view by id", nameof(InstrumentRepository), nameof(GetInstrumentResultRecordViewByIdAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new InstrumentResultRecordViewByIdDataQuery(), "Instrument result record view by id", queryFilter);
        }

        /// <summary>
        /// Asynchronously retrieves the instrument profile by culture ID.
        /// </summary>
        /// <param name="id">The culture ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the single instrument configuration.</returns>
        public async Task<SingleInstrumentConfig> GetInstrumentProfileByCultureIdAsync(int id)
        {
            var queryFilter = new QueryFilterConfig { Parameters = [ new QueryValuesConfig { Key = "id", Value = id.ToString() } ] };
            _logWriter.LogInfo("Run instrumentprofile by culture id query", nameof(InstrumentRepository), nameof(GetInstrumentProfileByCultureIdAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new InstrumentProfileByCultureIdQuery(), "Get instrument profile by culture id query", queryFilter);
        }


        /// <summary>
        /// Asynchronously retrieves the instrument profile by specimen ID.
        /// </summary>
        /// <param name="id">The specimen ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the single instrument configuration.</returns>
        public async Task<SingleInstrumentConfig> GetInstrumentProfileBySpecimenIdAsync(int id)
        {
            var queryFilter = new QueryFilterConfig { Parameters = [new QueryValuesConfig { Key = "id", Value = id.ToString() }] };
            _logWriter.LogInfo("Run instrument profile by specimen ID query", nameof(InstrumentRepository), nameof(GetInstrumentProfileBySpecimenIdAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new InstrumentProfileBySpecimenIdQuery(), "Get instrument profile by specimen ID query", queryFilter);
        }

        /// <summary>
        /// Asynchronously retrieves the instrument profile by direct test name.
        /// </summary>
        /// <param name="id">The specimen ID.</param>
        /// <param name="name">The direct test name.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the single instrument configuration.</returns>
        public async Task<SingleInstrumentConfig> GetInstrumentProfileByDirectTestNameAsync(int id, string name)
        {
            var queryFilter = new QueryFilterConfig { Parameters = [ new QueryValuesConfig { Key = "specimenid", Value = id.ToString() }, new QueryValuesConfig { Key = "testname", Value = name }] };
            _logWriter.LogInfo("Run instrument profile by direct test name query", nameof(InstrumentRepository), nameof(GetInstrumentProfileByDirectTestNameAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new InstrumentProfileByDirectNameQuery(), "Get instrument profile by direct test name query", queryFilter);
        }

        /// <inheritdoc />
        public async Task<bool> ProfileConfigIdsMatchTestNameAsync(string commaSeparatedProfileIds, string testName)
        {
            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("profileids", commaSeparatedProfileIds ?? "");
            queryFilter.AddString("testname", testName ?? "");
            _logWriter.LogInfo("Instrument profile config ids match test name", nameof(InstrumentRepository), nameof(ProfileConfigIdsMatchTestNameAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new InstrumentProfileConfigIdsMatchQuery(), "Instrument profile config ids match test name", queryFilter);
        }

        /// <inheritdoc />
        public async Task<int> GetSpecimenIdForCultureAsync(int cultureId)
        {
            var queryFilter = new QueryFilterConfig();
            queryFilter.AddInteger("cultureid", cultureId);
            _logWriter.LogInfo("Get specimen id for culture", nameof(InstrumentRepository), nameof(GetSpecimenIdForCultureAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenIdByCultureIdQuery(), "Get specimen id for culture", queryFilter);
        }


        /// <summary>
        /// Asynchronously retrieves the instrument profile by culture test name.
        /// </summary>
        /// <param name="id">The culture ID.</param>
        /// <param name="name">The test name.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the single instrument configuration.</returns>
        public async Task<SingleInstrumentConfig> GetInstrumentProfileByCultureTestNameAsync(int id, string name)
        {
            var queryFilter = new QueryFilterConfig { Parameters = [new QueryValuesConfig { Key = "cultureid", Value = id.ToString() }, new QueryValuesConfig { Key = "testname", Value = name }] };
            _logWriter.LogInfo("Run instrument profile by culture test name query", nameof(InstrumentRepository), nameof(GetInstrumentProfileByCultureTestNameAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new InstrumentProfileByCultureTestNameQuery(), "Get instrument profile by culture test name query", queryFilter);
        }

        /// <inheritdoc />
        public async Task<string> GetFirstConfigNameFromConfigIdListAsync(string commaSeparatedConfigIds)
        {
            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("profileids", commaSeparatedConfigIds ?? "");
            _logWriter.LogInfo(
                $"Config name by first profile id (profileids='{commaSeparatedConfigIds ?? ""}')",
                nameof(InstrumentRepository),
                nameof(GetFirstConfigNameFromConfigIdListAsync));
            var row = await _sqlQuery.QueryReturningTypeAsync(new ConfigNameByFirstIdQuery(), "Get config name by first profile id", queryFilter);
            var name = row?.Name;
            if (string.IsNullOrWhiteSpace(name))
            {
                _logWriter.LogInfo(
                    $"No configs.configname resolved for profile id list (profileids='{commaSeparatedConfigIds ?? ""}'). Check first token is a valid configs id or configname.",
                    nameof(InstrumentRepository),
                    nameof(GetFirstConfigNameFromConfigIdListAsync));
            }

            return name;
        }

        /// <inheritdoc />
        public async Task<int> AddInstrumentResultFileAttachmentsAsync(int instrumentResultId, IEnumerable<int> fileAttachmentIds)
        {
            _logWriter.LogInfo(
                $"Add instrument result file attachments (instrumentResultId={instrumentResultId})",
                nameof(InstrumentRepository),
                nameof(AddInstrumentResultFileAttachmentsAsync));
            var model = new AddInstrumentResultFileAttachmentsModel
            {
                InstrumentResultId = instrumentResultId,
                FileAttachmentIds = fileAttachmentIds
            };
            return await _sqlCommand.CommandWithTypeQueryAsync(
                new AddInstrumentResultFileAttachmentsCommand(),
                "Add instrument result file attachments",
                model,
                _logWriter);
        }

    }
}
