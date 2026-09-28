using arc.app.Common;
using arc.app.Instruments;
using arc.app.Specimen;
using arc.app.SystemConfig;
using arc.app.Tests;
using arc.common;
using arc.common.Models.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Instruments;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Implements manual instrument test requests: prerequisite cultures/tests and a single pending <c>instrumentresults</c> row.
/// </summary>
public class InstrumentManualRequestService : IInstrumentManualRequestService
{
    private const int InstrumentResultStatusPending = 882;

    private readonly IConfigRepository _configRepository;
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly ITestRepository _testRepository;
    private readonly ICultureRepository _cultureRepository;
    private readonly ILogWriter _logWriter;

    public InstrumentManualRequestService(
        IConfigRepository configRepository,
        IInstrumentRepository instrumentRepository,
        ITestRepository testRepository,
        ICultureRepository cultureRepository,
        ILogWriter logWriter)
    {
        _configRepository = configRepository;
        _instrumentRepository = instrumentRepository;
        _testRepository = testRepository;
        _cultureRepository = cultureRepository;
        _logWriter = logWriter;
    }

    /// <inheritdoc />
    public async Task<int> RequestAsync(string dataToSave, string username)
    {
        var vm = JsonConvert.DeserializeObject<RequestInstrumentTestFormViewModel>(dataToSave);
        if (vm == null || string.IsNullOrWhiteSpace(vm.InstrumentProfileId))
            throw new ArgumentException("Invalid request instrument test payload.", nameof(dataToSave));

        var fullConfig = await LoadInstrumentConfigAsync();
        var profile = FindProfile(fullConfig, vm.InstrumentProfileId);
        if (profile == null)
            throw new InvalidOperationException("Instrument profile not found or disabled.");

        var (ctxModel, scopeRow) = await ResolveScopeAsync(vm);
        if (string.IsNullOrWhiteSpace(scopeRow?.LaboratoryId))
            throw new InvalidOperationException("Could not resolve laboratory for this request.");

        if (!string.Equals(profile.LaboratoryId, scopeRow.LaboratoryId, StringComparison.Ordinal))
            throw new InvalidOperationException("Instrument profile does not belong to this specimen's laboratory.");

        if (vm.MetaRecordView.Equals("specimenrecordview", StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(profile.CultureTestId))
            throw new InvalidOperationException("This profile is scoped to isolate tests; request it from an isolate or isolate test list.");

        int specimenId = ctxModel.SpecimenId;
        int cultureId = ctxModel.CultureId;

        if (vm.MetaRecordView.Equals("specimenrecordview", StringComparison.OrdinalIgnoreCase))
        {
            cultureId = await CreateCultureForProfileTypeWhenNeededAsync(vm, profile, specimenId, cultureId);

            if (!string.IsNullOrWhiteSpace(profile.DirectTestId))
            {
                var testName = await _instrumentRepository.GetFirstConfigNameFromConfigIdListAsync(profile.DirectTestId);
                if (string.IsNullOrWhiteSpace(testName))
                {
                    _logWriter.LogError(
                        $"Manual instrument request: could not resolve direct test name from DirectTestId. ProfileId={profile.Id}, InstrumentName={profile.InstrumentName}, DirectTestId='{profile.DirectTestId}', specimenId={specimenId}.",
                        nameof(InstrumentManualRequestService),
                        nameof(RequestAsync));
                    throw new InvalidOperationException(
                        $"Could not resolve direct test name for profile '{profile.InstrumentName}' (DirectTestId='{profile.DirectTestId}'). Ensure the first id or name in the list exists in configs.");
                }

                await _testRepository.EnsureDirectTestExistsAsync(specimenId, testName);
                ctxModel.DirectTestName = testName;
            }
        }
        else if (vm.MetaRecordView.Equals("cultures", StringComparison.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrWhiteSpace(profile.CultureTestId))
            {
                var ctName = await _instrumentRepository.GetFirstConfigNameFromConfigIdListAsync(profile.CultureTestId);
                if (string.IsNullOrWhiteSpace(ctName))
                {
                    _logWriter.LogError(
                        $"Manual instrument request: could not resolve isolate test name from CultureTestId. ProfileId={profile.Id}, InstrumentName={profile.InstrumentName}, CultureTestId='{profile.CultureTestId}', cultureId={cultureId}.",
                        nameof(InstrumentManualRequestService),
                        nameof(RequestAsync));
                    throw new InvalidOperationException(
                        $"Could not resolve isolate test name for profile '{profile.InstrumentName}' (CultureTestId='{profile.CultureTestId}'). Ensure the first id or name in the list exists in configs.");
                }

                await _testRepository.EnsureCultureTestExistsAsync(cultureId, ctName);
                ctxModel.CultureTestName = ctName;
            }
        }
        else if (vm.MetaRecordView.Equals("testrecordview", StringComparison.OrdinalIgnoreCase))
        {
            cultureId = await CreateCultureForProfileTypeWhenNeededAsync(vm, profile, specimenId, cultureId);
        }

        var matchCtx = await BuildMatchContextAsync(ctxModel, scopeRow, specimenId, cultureId);
        var matches = await InstrumentProfileMatcher.GetMatchingProfilesAsync(_instrumentRepository, fullConfig.Instruments ?? [], matchCtx);
        if (!matches.Any(p => ProfileEquals(p, profile)))
        {
            _logWriter.LogError($"Manual instrument request rejected: profile '{profile.InstrumentName}' does not match context (specimenId={specimenId}, cultureId={cultureId}).", nameof(InstrumentManualRequestService), nameof(RequestAsync));
            throw new InvalidOperationException("The selected instrument profile is not valid for the current specimen/isolate/test context.");
        }

        var moreData = BuildManualMoreData(matchCtx);
        var barcode = matchCtx.Barcode;
        if (!string.IsNullOrEmpty(barcode))
            barcode = barcode.Replace("'", "").Replace("\"", "");

        var machineId = TryParseProfileInstrumentMachineId(profile);
        if (!string.IsNullOrWhiteSpace(profile.InstrumentMachineId) && !machineId.HasValue)
        {
            _logWriter.LogInfo(
                $"Manual instrument request: InstrumentMachineId on profile is not a valid integer ('{profile.InstrumentMachineId}'), profile={profile.InstrumentName}",
                nameof(InstrumentManualRequestService),
                nameof(RequestAsync));
        }

        var accessionForRow = await _instrumentRepository.GetAccessionNumberForSpecimenIdAsync(specimenId);
        var cultureNumForRow = cultureId > 0 ? await _instrumentRepository.GetCultureNumberTextForCultureIdAsync(cultureId) : null;
        var row = new InstrumentResult
        {
            InstrumentName = profile.InstrumentName,
            SpecimenId = specimenId,
            CultureId = cultureId,
            Barcode = barcode,
            Enabled = true,
            StatusId = InstrumentResultStatusPending,
            MoreData = moreData,
            InstrumentMachineId = machineId,
            AccessionNumber = accessionForRow,
            CultureNumber = cultureNumForRow
        };

        var newId = await _instrumentRepository.AddAsync(row);
        _logWriter.LogInfo($"Manual instrument request: inserted pending instrumentresults id={newId}, profile={profile.InstrumentName}, instrumentMachineId={(machineId.HasValue ? machineId.Value.ToString() : "null")}, user={username}, specimenId={specimenId}, cultureId={cultureId}", nameof(InstrumentManualRequestService), nameof(RequestAsync));
        return newId;
    }

    /// <summary>
    /// When the profile specifies a culture type (single list id), insert a **new** culture on the specimen for this request.
    /// From the specimen embedded list this runs when the id parses; from the direct-test embedded list it runs when the test has no <c>CultureId</c> yet.
    /// </summary>
    private async Task<int> CreateCultureForProfileTypeWhenNeededAsync(
        RequestInstrumentTestFormViewModel vm,
        SingleInstrumentConfig profile,
        int specimenId,
        int cultureId)
    {
        var typeId = InstrumentProfileMatcher.TryParseProfileCultureTypeId(profile.CultureTypeId);
        if (!typeId.HasValue)
        {
            if (!string.IsNullOrWhiteSpace(profile.CultureTypeId))
            {
                _logWriter.LogInfo(
                    $"Manual instrument request: CultureTypeId '{profile.CultureTypeId}' is not a parseable integer; skipping culture creation.",
                    nameof(InstrumentManualRequestService),
                    nameof(CreateCultureForProfileTypeWhenNeededAsync));
            }

            return cultureId;
        }

        var specimenRv = vm.MetaRecordView.Equals("specimenrecordview", StringComparison.OrdinalIgnoreCase);
        var directTestRv = vm.MetaRecordView.Equals("testrecordview", StringComparison.OrdinalIgnoreCase)
            && (vm.MetaSource ?? "direct").Equals("direct", StringComparison.OrdinalIgnoreCase);

        if (!specimenRv && !(directTestRv && cultureId == 0))
            return cultureId;

        var id = await _cultureRepository.CreateCultureForSpecimenAndTypeAsync(specimenId, typeId.Value);
        if (specimenRv)
        {
            _logWriter.LogInfo(
                $"Manual instrument request: created culture id={id} for specimen {specimenId}, typeId={typeId}",
                nameof(InstrumentManualRequestService),
                nameof(RequestAsync));
        }
        else
        {
            _logWriter.LogInfo(
                $"Manual instrument request: created culture id={id} from direct-test instrument list (specimen {specimenId}, typeId={typeId.Value})",
                nameof(InstrumentManualRequestService),
                nameof(RequestAsync));
        }

        return id;
    }

    private static bool ProfileEquals(SingleInstrumentConfig a, SingleInstrumentConfig b)
    {
        if (!string.IsNullOrWhiteSpace(a.Id) && !string.IsNullOrWhiteSpace(b.Id))
            return string.Equals(a.Id, b.Id, StringComparison.Ordinal);
        return string.Equals(a.InstrumentName, b.InstrumentName, StringComparison.Ordinal);
    }

    private static string BuildManualMoreData(InstrumentProfileMatchContext ctx)
    {
        var payload = new JObject { ["triggerType"] = "manual" };
        if (!string.IsNullOrWhiteSpace(ctx.DirectTestName))
            payload["directTestName"] = ctx.DirectTestName;
        if (!string.IsNullOrWhiteSpace(ctx.CultureTestName))
            payload["cultureTestName"] = ctx.CultureTestName;
        return payload.ToString(Formatting.None);
    }

    private async Task<InstrumentProfileMatchContext> BuildMatchContextAsync(
        InstrumentManualRequestContextModel ctxModel,
        SingleInstrumentConfig scopeRow,
        int specimenId,
        int cultureId)
    {
        var cultureRow = cultureId > 0
            ? await _instrumentRepository.GetInstrumentProfileByCultureIdAsync(cultureId)
            : null;

        return new InstrumentProfileMatchContext
        {
            LaboratoryId = scopeRow.LaboratoryId,
            SpecimenTypeId = scopeRow.SpecimenTypeId,
            CultureTypeId = cultureRow?.CultureTypeId ?? scopeRow.CultureTypeId,
            DirectTestName = ctxModel.DirectTestName,
            CultureTestName = ctxModel.CultureTestName,
            OrgGroupCodingId = cultureRow?.CultureOrgGroupCodingId ?? scopeRow.CultureOrgGroupCodingId,
            Barcode = cultureRow?.Barcode ?? scopeRow.Barcode,
            SpecimenId = specimenId,
            CultureId = cultureId
        };
    }

    private async Task<(InstrumentManualRequestContextModel ctx, SingleInstrumentConfig scopeRow)> ResolveScopeAsync(RequestInstrumentTestFormViewModel vm)
    {
        var ctx = new InstrumentManualRequestContextModel { RecordView = vm.MetaRecordView };
        var id = vm.MetaParentId;
        SingleInstrumentConfig scopeRow;

        if (vm.MetaRecordView.Equals("specimenrecordview", StringComparison.OrdinalIgnoreCase))
        {
            ctx.ListKind = "specimen";
            ctx.SpecimenId = id;
            scopeRow = await _instrumentRepository.GetInstrumentProfileBySpecimenIdAsync(id);
        }
        else if (vm.MetaRecordView.Equals("cultures", StringComparison.OrdinalIgnoreCase))
        {
            ctx.ListKind = "culture";
            ctx.CultureId = id;
            ctx.SpecimenId = await _instrumentRepository.GetSpecimenIdForCultureAsync(id);
            scopeRow = await _instrumentRepository.GetInstrumentProfileByCultureIdAsync(id);
        }
        else if (vm.MetaRecordView.Equals("testrecordview", StringComparison.OrdinalIgnoreCase))
        {
            ctx.ListKind = "test";
            var source = (vm.MetaSource ?? "direct").ToLowerInvariant();
            ctx.Source = source;
            var test = source == "culture"
                ? await _testRepository.GetCultureTestAsync(id)
                : await _testRepository.GetTestAsync(id);
            if (test == null)
                throw new InvalidOperationException("Test record not found.");

            ctx.SpecimenId = test.SpecimenId;
            ctx.CultureId = test.CultureId;
            if (source == "direct")
            {
                ctx.DirectTestName = test.TestName;
                scopeRow = await _instrumentRepository.GetInstrumentProfileByDirectTestNameAsync(test.SpecimenId, test.TestName);
            }
            else
            {
                ctx.CultureTestName = test.TestName;
                scopeRow = await _instrumentRepository.GetInstrumentProfileByCultureTestNameAsync(test.CultureId, test.TestName);
            }

            if (string.IsNullOrWhiteSpace(scopeRow?.LaboratoryId))
                scopeRow = await _instrumentRepository.GetInstrumentProfileBySpecimenIdAsync(test.SpecimenId);
        }
        else
            throw new InvalidOperationException("Unsupported record view for manual instrument request.");

        if (scopeRow == null || string.IsNullOrWhiteSpace(scopeRow.LaboratoryId))
            throw new InvalidOperationException("Could not resolve laboratory or scope for this request.");

        ctx.LaboratoryId = scopeRow.LaboratoryId;
        ctx.SpecimenTypeId = scopeRow.SpecimenTypeId;
        ctx.CultureTypeId = scopeRow.CultureTypeId;
        ctx.OrgGroupCodingId = scopeRow.CultureOrgGroupCodingId;

        return (ctx, scopeRow);
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

    private static SingleInstrumentConfig FindProfile(InstrumentConfig cfg, string idOrKey)
    {
        foreach (var i in cfg.Instruments ?? [])
        {
            if (!InstrumentProfileMatcher.IsProfileEnabled(i))
                continue;
            if (!string.IsNullOrWhiteSpace(i.Id) && string.Equals(i.Id, idOrKey, StringComparison.Ordinal))
                return i;
            if (string.Equals(i.InstrumentName, idOrKey, StringComparison.Ordinal))
                return i;
        }

        return null;
    }

    private async Task<InstrumentConfig> LoadInstrumentConfigAsync()
    {
        var queryFilter = new QueryFilterConfig();
        queryFilter.AddString("configname", "instrumentinfo");
        var instrumentConfig = await _configRepository.SingleConfigByNameAsync(queryFilter);
        return JsonConvert.DeserializeObject<InstrumentConfig>(instrumentConfig.Contents) ?? new InstrumentConfig();
    }
}
