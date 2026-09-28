using arc.app.Common;
using arc.app.Instruments;
using arc.app.SystemConfig;
using arc.common;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Instruments;
using arc.data.SystemConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Instruments
{
    /// <summary>
    /// Handles the interface for instrument-related operations.
    /// </summary>
    public class InstrumentInterfaceHandler : IInstrumentInterfaceHandler
    {
        private const int InstrumentResultStatusPending = 882;

        private readonly IConfigRepository _configRepository;
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstrumentInterfaceHandler"/> class.
        /// </summary>
        /// <param name="instrumentRepository">The instrument repository.</param>
        /// <param name="configRepository">The configuration repository.</param>
        /// <param name="logWriter">The log writer for diagnostic messages.</param>
        public InstrumentInterfaceHandler(IInstrumentRepository instrumentRepository, IConfigRepository configRepository, ILogWriter logWriter)
        {
            _configRepository = configRepository;
            _instrumentRepository = instrumentRepository;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Adds instrument results for tests based on the specified topic.
        /// </summary>
        /// <param name="topic">The topic for the tests.</param>
        public async Task AddInstrumentResultsForTests(string topic)
        {
            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task CreateInstrumentPendingResultsForCultureId(int specimenId, int cultureId)
        {
            var cultureInfo = await _instrumentRepository.GetInstrumentProfileByCultureIdAsync(cultureId);
            var ctx = BuildContextFromCultureInfo(cultureInfo, specimenId, cultureId);
            var matchingResults = await MatchingInstrumentsAsync(ctx);
            if (matchingResults.Count == 0)
            {
                _logWriter.LogInfo($"Instrument match: no profiles for culture trigger (specimenId={specimenId}, cultureId={cultureId})", nameof(InstrumentInterfaceHandler), nameof(CreateInstrumentPendingResultsForCultureId));
                return;
            }

            foreach (var result in matchingResults)
            {
                var moreData = BuildTriggerMoreData("cultureType", cultureTestName: null);
                await InsertPendingResultAsync(result, ctx, moreData, nameof(CreateInstrumentPendingResultsForCultureId));
            }
        }

        /// <inheritdoc />
        public async Task CreateInstrumentPendingResultsWhenEditingACulture(string dataBeingSaved, NpgsqlConnection connect)
        {
            var idModel = JsonConvert.DeserializeObject<IdModel>(dataBeingSaved);
            if (string.IsNullOrEmpty(idModel?.Id))
            {
                _logWriter.LogInfo("CreateInstrumentPendingResultsWhenEditingACulture: Id is null or empty, skipping", nameof(InstrumentInterfaceHandler), nameof(CreateInstrumentPendingResultsWhenEditingACulture));
                return;
            }

            if (connect == null)
            {
                _logWriter.LogInfo("CreateInstrumentPendingResultsWhenEditingACulture: no shared connection supplied, skipping to avoid opening additional connections inside TransactionScope", nameof(InstrumentInterfaceHandler), nameof(CreateInstrumentPendingResultsWhenEditingACulture));
                return;
            }

            var cultureId = int.Parse(idModel.Id);
            _logWriter.LogInfo($"CreateInstrumentPendingResultsWhenEditingACulture: using caller shared connection for cultureId={cultureId}", nameof(InstrumentInterfaceHandler), nameof(CreateInstrumentPendingResultsWhenEditingACulture));

            var specimenId = await GetSpecimenIdForCultureOnConnectionAsync(connect, cultureId);
            var cultureInfo = await GetInstrumentProfileByCultureIdOnConnectionAsync(connect, cultureId);
            var ctx = BuildContextFromCultureInfo(cultureInfo, specimenId, cultureId);
            var matchingResults = await MatchingInstrumentsOnConnectionAsync(connect, ctx);
            _logWriter.LogInfo($"CreateInstrumentPendingResultsWhenEditingACulture: matched {matchingResults.Count} profile(s) for cultureId={cultureId}, specimenId={specimenId}", nameof(InstrumentInterfaceHandler), nameof(CreateInstrumentPendingResultsWhenEditingACulture));

            if (matchingResults.Count == 0)
            {
                _logWriter.LogInfo($"Instrument match: no profiles for culture edit (specimenId={specimenId}, cultureId={cultureId})", nameof(InstrumentInterfaceHandler), nameof(CreateInstrumentPendingResultsWhenEditingACulture));
                return;
            }

            foreach (var result in matchingResults)
            {
                var moreData = BuildTriggerMoreData("cultureEdit", cultureTestName: null);
                await InsertPendingResultOnConnectionAsync(result, ctx, moreData, nameof(CreateInstrumentPendingResultsWhenEditingACulture), connect);
            }
        }

        /// <inheritdoc />
        public async Task CreateInstrumentPendingResultsForSpecimenId(int specimenId)
        {
            var info = await _instrumentRepository.GetInstrumentProfileBySpecimenIdAsync(specimenId);
            var ctx = new InstrumentProfileMatchContext
            {
                LaboratoryId = info.LaboratoryId,
                SpecimenTypeId = info.SpecimenTypeId,
                SpecimenId = specimenId
            };
            var matchingResults = await MatchingInstrumentsAsync(ctx);
            if (matchingResults.Count == 0)
            {
                _logWriter.LogInfo($"Instrument match: no profiles for specimen-only trigger (specimenId={specimenId})", nameof(InstrumentInterfaceHandler), nameof(CreateInstrumentPendingResultsForSpecimenId));
                return;
            }

            foreach (var result in matchingResults)
            {
                var moreData = BuildTriggerMoreData("specimen");
                await InsertPendingResultAsync(result, ctx, moreData, nameof(CreateInstrumentPendingResultsForSpecimenId));
            }
        }

        /// <inheritdoc />
        public async Task CreateInstrumentPendingResultsForDirectTestId(int specimenId, string testName)
        {
            var info = await _instrumentRepository.GetInstrumentProfileByDirectTestNameAsync(specimenId, testName);
            var ctx = new InstrumentProfileMatchContext
            {
                LaboratoryId = info.LaboratoryId,
                SpecimenTypeId = info.SpecimenTypeId,
                DirectTestName = testName,
                SpecimenId = specimenId
            };
            var matchingResults = await MatchingInstrumentsAsync(ctx);
            if (matchingResults.Count == 0)
            {
                _logWriter.LogInfo($"Instrument match: no profiles for direct test (specimenId={specimenId}, testName={testName})", nameof(InstrumentInterfaceHandler), nameof(CreateInstrumentPendingResultsForDirectTestId));
                return;
            }

            foreach (var result in matchingResults)
            {
                var moreData = BuildTriggerMoreData("directTest", directTestName: testName);
                await InsertPendingResultAsync(result, ctx, moreData, nameof(CreateInstrumentPendingResultsForDirectTestId));
            }
        }

        /// <inheritdoc />
        public async Task CreateInstrumentPendingResultsForCultureTestId(int specimenId, int cultureId, string testName)
        {
            if (specimenId <= 0 && cultureId > 0)
                specimenId = await _instrumentRepository.GetSpecimenIdForCultureAsync(cultureId);

            var info = await _instrumentRepository.GetInstrumentProfileByCultureTestNameAsync(cultureId, testName);
            var ctx = new InstrumentProfileMatchContext
            {
                LaboratoryId = info.LaboratoryId,
                SpecimenTypeId = info.SpecimenTypeId,
                CultureTypeId = info.CultureTypeId,
                CultureTestName = testName,
                SpecimenId = specimenId,
                CultureId = cultureId
            };
            var matchingResults = await MatchingInstrumentsAsync(ctx);
            if (matchingResults.Count == 0)
            {
                _logWriter.LogInfo($"Instrument match: no profiles for culture test (specimenId={specimenId}, cultureId={cultureId}, testName={testName})", nameof(InstrumentInterfaceHandler), nameof(CreateInstrumentPendingResultsForCultureTestId));
                return;
            }

            foreach (var result in matchingResults)
            {
                var moreData = BuildTriggerMoreData("cultureTest", cultureTestName: testName);
                await InsertPendingResultAsync(result, ctx, moreData, nameof(CreateInstrumentPendingResultsForCultureTestId));
            }
        }

        /// <summary>
        /// Builds a match context from a culture-scoped profile row (laboratory, specimen/culture type, barcode, organism group on isolate).
        /// </summary>
        private static InstrumentProfileMatchContext BuildContextFromCultureInfo(SingleInstrumentConfig cultureInfo, int specimenId, int cultureId)
        {
            return new InstrumentProfileMatchContext
            {
                LaboratoryId = cultureInfo.LaboratoryId,
                SpecimenTypeId = cultureInfo.SpecimenTypeId,
                CultureTypeId = cultureInfo.CultureTypeId,
                OrgGroupCodingId = cultureInfo.CultureOrgGroupCodingId,
                Barcode = cultureInfo.Barcode,
                SpecimenId = specimenId,
                CultureId = cultureId
            };
        }

        /// <summary>
        /// Serializes trigger metadata for <c>instrumentresults.moredata</c> (direct/isolate test names, trigger kind).
        /// </summary>
        private static string BuildTriggerMoreData(string triggerType, string directTestName = null, string cultureTestName = null)
        {
            var payload = new JObject
            {
                ["triggerType"] = triggerType
            };
            if (!string.IsNullOrWhiteSpace(directTestName))
                payload["directTestName"] = directTestName;
            if (!string.IsNullOrWhiteSpace(cultureTestName))
                payload["cultureTestName"] = cultureTestName;
            return payload.ToString(Formatting.None);
        }

        /// <summary>
        /// Inserts a pending instrument result row for outbound integration using the caller's shared connection.
        /// </summary>
        private async Task InsertPendingResultOnConnectionAsync(SingleInstrumentConfig profile, InstrumentProfileMatchContext ctx, string moreDataJson, string callerName, NpgsqlConnection connect)
        {
            var barcode = ctx.Barcode;
            if (!string.IsNullOrEmpty(barcode))
                barcode = barcode.Replace("'", "").Replace("\"", "");

            var machineId = TryParseProfileInstrumentMachineId(profile);
            if (!string.IsNullOrWhiteSpace(profile.InstrumentMachineId) && !machineId.HasValue)
            {
                _logWriter.LogInfo(
                    $"Instrument pending result: InstrumentMachineId on profile is not a valid integer ('{profile.InstrumentMachineId}'), profile={profile.InstrumentName}, caller={callerName}",
                    nameof(InstrumentInterfaceHandler),
                    callerName);
            }

            var queryFilters = new QueryFilterConfig();
            queryFilters.AddInteger("specimenid", ctx.SpecimenId);
            var accessionNumber = await new SpecimenAccessionNumberBySpecimenIdQuery().ExecuteAsync(connect, queryFilters);
            string cultureNumberText = null;
            if (ctx.CultureId > 0)
            {
                var cultureFilters = new QueryFilterConfig();
                cultureFilters.AddInteger("cultureid", ctx.CultureId);
                cultureNumberText = await new CultureNumberTextByCultureIdQuery().ExecuteAsync(connect, cultureFilters);
            }

            var newResultRecord = new InstrumentResult
            {
                InstrumentName = profile.InstrumentName,
                SpecimenId = ctx.SpecimenId,
                CultureId = ctx.CultureId,
                Barcode = barcode,
                Enabled = true,
                StatusId = InstrumentResultStatusPending,
                MoreData = string.IsNullOrWhiteSpace(moreDataJson) ? "{}" : moreDataJson,
                InstrumentMachineId = machineId,
                AccessionNumber = accessionNumber,
                CultureNumber = cultureNumberText
            };

            await new AddInstrumentCommand().ExecuteAsync(connect, newResultRecord, _logWriter);
            _logWriter.LogInfo($"Instrument pending result inserted on shared connection: profile={profile.InstrumentName}, specimenId={ctx.SpecimenId}, cultureId={ctx.CultureId}, instrumentMachineId={(machineId.HasValue ? machineId.Value.ToString() : "null")}, caller={callerName}", nameof(InstrumentInterfaceHandler), callerName);
        }

        /// <summary>
        /// Inserts a pending instrument result row for outbound integration.
        /// </summary>
        private async Task InsertPendingResultAsync(SingleInstrumentConfig profile, InstrumentProfileMatchContext ctx, string moreDataJson, string callerName)
        {
            var barcode = ctx.Barcode;
            if (!string.IsNullOrEmpty(barcode))
                barcode = barcode.Replace("'", "").Replace("\"", "");

            var machineId = TryParseProfileInstrumentMachineId(profile);
            if (!string.IsNullOrWhiteSpace(profile.InstrumentMachineId) && !machineId.HasValue)
            {
                _logWriter.LogInfo(
                    $"Instrument pending result: InstrumentMachineId on profile is not a valid integer ('{profile.InstrumentMachineId}'), profile={profile.InstrumentName}, caller={callerName}",
                    nameof(InstrumentInterfaceHandler),
                    callerName);
            }

            var accessionNumber = await _instrumentRepository.GetAccessionNumberForSpecimenIdAsync(ctx.SpecimenId);
            string cultureNumberText = null;
            if (ctx.CultureId > 0)
                cultureNumberText = await _instrumentRepository.GetCultureNumberTextForCultureIdAsync(ctx.CultureId);

            var newResultRecord = new InstrumentResult
            {
                InstrumentName = profile.InstrumentName,
                SpecimenId = ctx.SpecimenId,
                CultureId = ctx.CultureId,
                Barcode = barcode,
                Enabled = true,
                StatusId = InstrumentResultStatusPending,
                MoreData = string.IsNullOrWhiteSpace(moreDataJson) ? "{}" : moreDataJson,
                InstrumentMachineId = machineId,
                AccessionNumber = accessionNumber,
                CultureNumber = cultureNumberText
            };

            await _instrumentRepository.AddAsync(newResultRecord);
            _logWriter.LogInfo($"Instrument pending result inserted: profile={profile.InstrumentName}, specimenId={ctx.SpecimenId}, cultureId={ctx.CultureId}, instrumentMachineId={(machineId.HasValue ? machineId.Value.ToString() : "null")}, caller={callerName}", nameof(InstrumentInterfaceHandler), callerName);
        }

        /// <summary>
        /// Parses <see cref="SingleInstrumentConfig.InstrumentMachineId"/> to a positive integer, or null if unset or invalid.
        /// </summary>
        private static int? TryParseProfileInstrumentMachineId(SingleInstrumentConfig profile)
        {
            if (profile == null || string.IsNullOrWhiteSpace(profile.InstrumentMachineId))
                return null;
            return int.TryParse(profile.InstrumentMachineId.Trim(), out var id) && id > 0 ? id : (int?)null;
        }

        /// <summary>
        /// Returns matching instrument profiles using the caller's shared connection (config read and test-name checks).
        /// </summary>
        private async Task<List<SingleInstrumentConfig>> MatchingInstrumentsOnConnectionAsync(NpgsqlConnection connect, InstrumentProfileMatchContext ctx)
        {
            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("configname", "instrumentinfo");
            var instrumentConfig = await new SingleConfigByNameQuery().ExecuteAsync(connect, queryFilter);
            var fullConfig = JsonConvert.DeserializeObject<InstrumentConfig>(instrumentConfig.Contents);
            if (fullConfig?.Instruments == null)
                return [];

            return await GetMatchingProfilesOnConnectionAsync(connect, fullConfig.Instruments, ctx);
        }

        /// <summary>
        /// Filters instrument profiles on a shared connection without opening additional database connections.
        /// </summary>
        private async Task<List<SingleInstrumentConfig>> GetMatchingProfilesOnConnectionAsync(
            NpgsqlConnection connect,
            IEnumerable<SingleInstrumentConfig> instruments,
            InstrumentProfileMatchContext ctx)
        {
            var list = new List<SingleInstrumentConfig>();
            foreach (var profile in instruments)
            {
                if (!InstrumentProfileMatcher.IsProfileEnabled(profile))
                    continue;
                if (!string.Equals(profile.LaboratoryId, ctx.LaboratoryId, StringComparison.Ordinal))
                    continue;
                if (!InstrumentProfileMatcher.SpecimenTypeMatches(profile, ctx))
                    continue;
                if (!InstrumentProfileMatcher.CultureTypeMatches(profile, ctx))
                    continue;
                if (!await DirectTestMatchesOnConnectionAsync(connect, profile, ctx))
                    continue;
                if (!await CultureTestMatchesOnConnectionAsync(connect, profile, ctx))
                    continue;
                if (!InstrumentProfileMatcher.OrganismGroupMatches(profile, ctx))
                    continue;
                list.Add(profile);
            }

            return list;
        }

        /// <summary>
        /// Returns whether a profile's direct-test constraint matches the context, using the shared connection when a DB lookup is required.
        /// </summary>
        private async Task<bool> DirectTestMatchesOnConnectionAsync(NpgsqlConnection connect, SingleInstrumentConfig profile, InstrumentProfileMatchContext ctx)
        {
            if (string.IsNullOrWhiteSpace(profile.DirectTestId))
                return true;
            if (string.IsNullOrWhiteSpace(ctx.DirectTestName))
                return false;
            return await ProfileConfigIdsMatchTestNameOnConnectionAsync(connect, profile.DirectTestId, ctx.DirectTestName);
        }

        /// <summary>
        /// Returns whether a profile's culture-test constraint matches the context, using the shared connection when a DB lookup is required.
        /// </summary>
        private async Task<bool> CultureTestMatchesOnConnectionAsync(NpgsqlConnection connect, SingleInstrumentConfig profile, InstrumentProfileMatchContext ctx)
        {
            if (string.IsNullOrWhiteSpace(profile.CultureTestId))
            {
                if (!string.IsNullOrWhiteSpace(ctx.CultureTestName))
                    return false;
                return true;
            }

            if (string.IsNullOrWhiteSpace(ctx.CultureTestName))
                return false;
            return await ProfileConfigIdsMatchTestNameOnConnectionAsync(connect, profile.CultureTestId, ctx.CultureTestName);
        }

        /// <summary>
        /// Returns whether config ids match a test name on the shared connection.
        /// </summary>
        private static async Task<bool> ProfileConfigIdsMatchTestNameOnConnectionAsync(NpgsqlConnection connect, string commaSeparatedProfileIds, string testName)
        {
            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("profileids", commaSeparatedProfileIds ?? "");
            queryFilter.AddString("testname", testName ?? "");
            return await new InstrumentProfileConfigIdsMatchQuery().ExecuteAsync(connect, queryFilter);
        }

        /// <summary>
        /// Returns the specimen id for a culture using the shared connection.
        /// </summary>
        private static async Task<int> GetSpecimenIdForCultureOnConnectionAsync(NpgsqlConnection connect, int cultureId)
        {
            var queryFilter = new QueryFilterConfig();
            queryFilter.AddInteger("cultureid", cultureId);
            return await new SpecimenIdByCultureIdQuery().ExecuteAsync(connect, queryFilter);
        }

        /// <summary>
        /// Returns instrument profile match fields for a culture using the shared connection.
        /// </summary>
        private static async Task<SingleInstrumentConfig> GetInstrumentProfileByCultureIdOnConnectionAsync(NpgsqlConnection connect, int cultureId)
        {
            var queryFilter = new QueryFilterConfig { Parameters = [new QueryValuesConfig { Key = "id", Value = cultureId.ToString() }] };
            return await new InstrumentProfileByCultureIdQuery().ExecuteAsync(connect, queryFilter);
        }

        /// <summary>
        /// Filters enabled <c>instrumentinfo</c> profile rows: laboratory must match; optional specimen type, culture type,
        /// direct test, culture test, and organism group must each match when set on the profile (AND).
        /// </summary>
        private async Task<List<SingleInstrumentConfig>> MatchingInstrumentsAsync(InstrumentProfileMatchContext ctx)
        {
            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("configname", "instrumentinfo");
            var instrumentConfig = await _configRepository.SingleConfigByNameAsync(queryFilter);
            var fullConfig = JsonConvert.DeserializeObject<InstrumentConfig>(instrumentConfig.Contents);
            if (fullConfig?.Instruments == null)
                return [];

            return await InstrumentProfileMatcher.GetMatchingProfilesAsync(_instrumentRepository, fullConfig.Instruments, ctx);
        }
    }
}
