using arc.app.Instruments;
using arc.domain.Instruments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Shared filtering of <see cref="SingleInstrumentConfig"/> rows against an <see cref="InstrumentProfileMatchContext"/>
/// (laboratory, specimen/culture type, tests, organism group). Used by automatic triggers and manual instrument requests.
/// </summary>
public static class InstrumentProfileMatcher
{
    /// <summary>
    /// Returns profiles in <paramref name="instruments"/> that match <paramref name="ctx"/> using AND semantics for each non-empty profile constraint.
    /// </summary>
    public static async Task<List<SingleInstrumentConfig>> GetMatchingProfilesAsync(
        IInstrumentRepository instrumentRepository,
        IEnumerable<SingleInstrumentConfig> instruments,
        InstrumentProfileMatchContext ctx)
    {
        var list = new List<SingleInstrumentConfig>();
        foreach (var i in instruments)
        {
            if (!IsProfileEnabled(i))
                continue;
            if (!string.Equals(i.LaboratoryId, ctx.LaboratoryId, StringComparison.Ordinal))
                continue;
            if (!SpecimenTypeMatches(i, ctx))
                continue;
            if (!CultureTypeMatches(i, ctx))
                continue;
            if (!await DirectTestMatchesAsync(instrumentRepository, i, ctx))
                continue;
            if (!await CultureTestMatchesAsync(instrumentRepository, i, ctx))
                continue;
            if (!OrganismGroupMatches(i, ctx))
                continue;
            list.Add(i);
        }

        return list;
    }

    /// <summary>True when the profile is considered enabled (empty means enabled).</summary>
    public static bool IsProfileEnabled(SingleInstrumentConfig i)
    {
        if (string.IsNullOrWhiteSpace(i.IsEnabled))
            return true;
        return !string.Equals(i.IsEnabled, "No", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(i.IsEnabled, "false", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Specimen type on the profile must match context when set.</summary>
    public static bool SpecimenTypeMatches(SingleInstrumentConfig profile, InstrumentProfileMatchContext ctx)
    {
        if (string.IsNullOrWhiteSpace(profile.SpecimenTypeId))
            return true;
        return string.Equals(profile.SpecimenTypeId, ctx.SpecimenTypeId, StringComparison.Ordinal);
    }

    /// <summary>Culture type on the profile must match context when set.</summary>
    public static bool CultureTypeMatches(SingleInstrumentConfig profile, InstrumentProfileMatchContext ctx)
    {
        if (string.IsNullOrWhiteSpace(profile.CultureTypeId))
            return true;
        return !string.IsNullOrWhiteSpace(ctx.CultureTypeId)
            && string.Equals(profile.CultureTypeId, ctx.CultureTypeId, StringComparison.Ordinal);
    }

    /// <summary>
    /// Parses <see cref="SingleInstrumentConfig.CultureTypeId"/> as a single list-item id (trimmed).
    /// Instrument profile UI stores one culture type per profile.
    /// </summary>
    /// <param name="cultureTypeId">Raw value from instrument profile JSON.</param>
    /// <returns>The parsed id, or <c>null</c> if empty or not a valid integer.</returns>
    public static int? TryParseProfileCultureTypeId(string cultureTypeId)
    {
        if (string.IsNullOrWhiteSpace(cultureTypeId))
            return null;
        return int.TryParse(cultureTypeId.Trim(), out var id) ? id : null;
    }

    /// <summary>Direct test ids on the profile must include the context test name when set.</summary>
    public static async Task<bool> DirectTestMatchesAsync(
        IInstrumentRepository instrumentRepository,
        SingleInstrumentConfig profile,
        InstrumentProfileMatchContext ctx)
    {
        if (string.IsNullOrWhiteSpace(profile.DirectTestId))
            return true;
        if (string.IsNullOrWhiteSpace(ctx.DirectTestName))
            return false;
        return await instrumentRepository.ProfileConfigIdsMatchTestNameAsync(profile.DirectTestId, ctx.DirectTestName);
    }

    /// <summary>
    /// Culture/isolate test ids on the profile must include the context test name when <see cref="SingleInstrumentConfig.CultureTestId"/> is set.
    /// When <c>CultureTestId</c> is empty: if the context names a specific isolate test, the profile does not match—profiles without an isolate-test list
    /// must not be triggered by arbitrary isolate-test saves (e.g. betalactamase). When the context has no isolate test name (culture-level triggers,
    /// manual request from specimen/culture, etc.), empty <c>CultureTestId</c> imposes no isolate-test constraint.
    /// </summary>
    public static async Task<bool> CultureTestMatchesAsync(
        IInstrumentRepository instrumentRepository,
        SingleInstrumentConfig profile,
        InstrumentProfileMatchContext ctx)
    {
        if (string.IsNullOrWhiteSpace(profile.CultureTestId))
        {
            if (!string.IsNullOrWhiteSpace(ctx.CultureTestName))
                return false;
            return true;
        }

        if (string.IsNullOrWhiteSpace(ctx.CultureTestName))
            return false;
        return await instrumentRepository.ProfileConfigIdsMatchTestNameAsync(profile.CultureTestId, ctx.CultureTestName);
    }

    /// <summary>Organism group on the profile must match isolate organism group when set (wildcard if no organism yet).</summary>
    public static bool OrganismGroupMatches(SingleInstrumentConfig profile, InstrumentProfileMatchContext ctx)
    {
        if (string.IsNullOrWhiteSpace(profile.OrganismGroupId))
            return true;
        if (string.IsNullOrWhiteSpace(ctx.OrgGroupCodingId))
            return true;
        return string.Equals(profile.OrganismGroupId, ctx.OrgGroupCodingId, StringComparison.Ordinal);
    }
}
