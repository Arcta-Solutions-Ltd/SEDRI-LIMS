using arc.app.Common;
using arc.app.SystemConfig;
using arc.app.Tests;
using arc.common.Models;
using arc.common.Models.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Instruments;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Instruments;

/// <summary>
/// Populates the "request instrument test" form with navigation context and all instrument profile rows for the laboratory (client filters by context).
/// </summary>
internal class RequestInstrumentTestFormInitialQuery : IQueryRun
{
    private readonly IConfigRepository _configRepository;
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly ITestRepository _testRepository;

    public RequestInstrumentTestFormInitialQuery(
        IConfigRepository configRepository,
        IInstrumentRepository instrumentRepository,
        ITestRepository testRepository)
    {
        _configRepository = configRepository;
        _instrumentRepository = instrumentRepository;
        _testRepository = testRepository;
    }

    /// <inheritdoc />
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token = null)
    {
        var parameters = queryFilter?.Parameters ?? [];
        static string Get(IEnumerable<QueryValuesConfig> ps, string key) =>
            ps.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase))?.Value;

        var idStr = Get(parameters, "id");
        var recordView = Get(parameters, "recordView") ?? string.Empty;
        if (string.IsNullOrWhiteSpace(idStr) || !int.TryParse(idStr, out var id))
            return JsonConvert.SerializeObject(new { Error = "Missing or invalid id" });

        var ctx = new InstrumentManualRequestContextModel { RecordView = recordView };
        SingleInstrumentConfig scopeRow;

        if (recordView.Equals("specimenrecordview", StringComparison.OrdinalIgnoreCase))
        {
            ctx.ListKind = "specimen";
            ctx.SpecimenId = id;
            scopeRow = await _instrumentRepository.GetInstrumentProfileBySpecimenIdAsync(id);
        }
        else if (recordView.Equals("cultures", StringComparison.OrdinalIgnoreCase))
        {
            ctx.ListKind = "culture";
            ctx.CultureId = id;
            ctx.SpecimenId = await _instrumentRepository.GetSpecimenIdForCultureAsync(id);
            scopeRow = await _instrumentRepository.GetInstrumentProfileByCultureIdAsync(id);
        }
        else if (recordView.Equals("testrecordview", StringComparison.OrdinalIgnoreCase))
        {
            ctx.ListKind = "test";
            var source = (Get(parameters, "source") ?? "direct").ToLowerInvariant();
            ctx.Source = source;
            var test = source == "culture"
                ? await _testRepository.GetCultureTestAsync(id)
                : await _testRepository.GetTestAsync(id);
            if (test == null)
                return JsonConvert.SerializeObject(new { Error = "Test not found" });

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
            return JsonConvert.SerializeObject(new { Error = "Unknown or missing recordView" });

        if (scopeRow == null || string.IsNullOrWhiteSpace(scopeRow.LaboratoryId))
            return JsonConvert.SerializeObject(new { Error = "Could not resolve laboratory for instrument request" });

        ctx.LaboratoryId = scopeRow.LaboratoryId;
        ctx.SpecimenTypeId = scopeRow.SpecimenTypeId;
        ctx.CultureTypeId = scopeRow.CultureTypeId;
        ctx.OrgGroupCodingId = scopeRow.CultureOrgGroupCodingId;

        var metaSource = Get(parameters, "source") ?? string.Empty;

        var fullConfig = await LoadInstrumentConfigAsync();
        var labProfiles = (fullConfig.Instruments ?? [])
            .Where(i => IsProfileEnabled(i)
                && string.Equals(i.LaboratoryId, ctx.LaboratoryId, StringComparison.Ordinal))
            .ToList();

        var options = labProfiles.Select(i => new
        {
            key = string.IsNullOrWhiteSpace(i.Id) ? i.InstrumentName : i.Id,
            text = string.IsNullOrWhiteSpace(i.InstrumentName) ? i.Id : i.InstrumentName
        }).ToList();

        object instrumentProfileDetails;
        if (string.Equals(ctx.ListKind, "test", StringComparison.OrdinalIgnoreCase))
        {
            var sourceLower = (ctx.Source ?? "direct").ToLowerInvariant();
            var enriched = new List<JObject>();
            foreach (var i in labProfiles)
            {
                var jo = JObject.FromObject(i);
                if (sourceLower == "direct")
                {
                    jo["MatchesDirectTestContext"] = await MatchesDirectTestContextForProfileAsync(i, ctx.DirectTestName);
                    jo["MatchesCultureTestContext"] = false;
                }
                else
                {
                    jo["MatchesCultureTestContext"] = await MatchesCultureTestContextForProfileAsync(i, ctx.CultureTestName);
                    jo["MatchesDirectTestContext"] = false;
                }

                enriched.Add(jo);
            }

            instrumentProfileDetails = enriched;
        }
        else
        {
            instrumentProfileDetails = labProfiles;
        }

        var result = new
        {
            RequestContext = ctx,
            InstrumentProfileOptions = options,
            InstrumentProfileDetails = instrumentProfileDetails,
            InstrumentProfileId = (string)null,
            MetaParentId = id,
            MetaRecordView = recordView,
            MetaSource = metaSource
        };

        return JsonConvert.SerializeObject(result);
    }

    private static bool IsProfileEnabled(SingleInstrumentConfig i)
    {
        if (string.IsNullOrWhiteSpace(i.IsEnabled))
            return true;
        return !string.Equals(i.IsEnabled, "No", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(i.IsEnabled, "false", StringComparison.OrdinalIgnoreCase);
    }

    private async Task<InstrumentConfig> LoadInstrumentConfigAsync()
    {
        var queryFilter = new QueryFilterConfig();
        queryFilter.AddString("configname", "instrumentinfo");
        var instrumentConfig = await _configRepository.SingleConfigByNameAsync(queryFilter);
        return JsonConvert.DeserializeObject<InstrumentConfig>(instrumentConfig.Contents) ?? new InstrumentConfig();
    }

    /// <summary>
    /// Mirrors <see cref="InstrumentProfileMatcher.DirectTestMatchesAsync"/>: empty <c>DirectTestId</c> on the profile means no direct-test name constraint (wildcard).
    /// When set, the profile matches only if the config id list resolves to the current direct test name.
    /// </summary>
    private async Task<bool> MatchesDirectTestContextForProfileAsync(SingleInstrumentConfig profile, string directTestName)
    {
        if (string.IsNullOrWhiteSpace(profile.DirectTestId))
            return true;
        if (string.IsNullOrWhiteSpace(directTestName))
            return false;
        return await _instrumentRepository.ProfileConfigIdsMatchTestNameAsync(profile.DirectTestId, directTestName);
    }

    /// <summary>
    /// Mirrors <see cref="InstrumentProfileMatcher.CultureTestMatchesAsync"/>: empty <c>CultureTestId</c> matches only when no isolate test name is in context;
    /// when the form is opened from a specific isolate test row, the profile must list that test (non-empty <c>CultureTestId</c> resolving to the name).
    /// </summary>
    private async Task<bool> MatchesCultureTestContextForProfileAsync(SingleInstrumentConfig profile, string cultureTestName)
    {
        if (string.IsNullOrWhiteSpace(profile.CultureTestId))
        {
            if (!string.IsNullOrWhiteSpace(cultureTestName))
                return false;
            return true;
        }

        if (string.IsNullOrWhiteSpace(cultureTestName))
            return false;
        return await _instrumentRepository.ProfileConfigIdsMatchTestNameAsync(profile.CultureTestId, cultureTestName);
    }
}
