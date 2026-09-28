/**
 * Client-side filter for instrument profile rows returned by RequestInstrumentTestFormInitialQuery.
 * Mirrors list-kind rules and InstrumentProfileMatcher; test-name ↔ config id resolution is done on the server
 * (MatchesDirectTestContext / MatchesCultureTestContext on each row when ListKind is test).
 * @param {object} ctx RequestContext from InitialQuery
 * @param {object[]} profiles InstrumentProfileDetails (SingleInstrumentConfig JSON, optionally with match flags)
 * @returns {object[]} profiles visible in the selector
 */
export function filterInstrumentProfilesForManualRequest(ctx, profiles) {
    if (!ctx || !profiles || !profiles.length) return [];
    const kind = String(ctx.ListKind || ctx.listKind || '').toLowerCase();
    const source = String(ctx.Source || ctx.source || 'direct').toLowerCase();

    return profiles.filter((p) => {
        if (!isProfileEnabled(p)) return false;
        if (!specimenTypeMatches(p, ctx)) return false;
        if (!cultureTypeMatchesForContext(p, ctx, kind)) return false;
        if (!organismGroupMatches(p, ctx)) return false;

        if (kind === 'specimen') {
            if (hasNonEmptyCultureTestId(p)) return false;
            return true;
        }

        if (kind === 'culture') {
            if (isDirectOnlyProfile(p)) return false;
            return true;
        }

        if (kind === 'test') {
            if (source === 'direct') {
                const md = p.MatchesDirectTestContext ?? p.matchesDirectTestContext;
                return md === true;
            }
            if (source === 'culture') {
                const mc = p.MatchesCultureTestContext ?? p.matchesCultureTestContext;
                return mc === true;
            }
        }

        return true;
    });
}

function isProfileEnabled(p) {
    const v = p.IsEnabled ?? p.isEnabled;
    if (v === undefined || v === null || String(v).trim() === '') return true;
    const s = String(v).toLowerCase();
    return s !== 'no' && s !== 'false';
}

function specimenTypeMatches(p, ctx) {
    const pid = p.SpecimenTypeId ?? p.specimenTypeId;
    if (!pid || String(pid).trim() === '') return true;
    const c = ctx.SpecimenTypeId ?? ctx.specimenTypeId;
    return String(pid) === String(c);
}

/**
 * Specimen list: do not hide profiles that require a culture type (user may create/link culture later).
 * Culture / test: profile culture type must match context when set on the profile.
 */
function cultureTypeMatchesForContext(p, ctx, kind) {
    if (kind === 'specimen') return true;

    const pid = p.CultureTypeId ?? p.cultureTypeId;
    if (!pid || String(pid).trim() === '') return true;
    const c = ctx.CultureTypeId ?? ctx.cultureTypeId;
    return c != null && String(c) !== '' && String(pid) === String(c);
}

/**
 * Mirrors InstrumentProfileMatcher.OrganismGroupMatches: empty profile OrganismGroupId passes;
 * if the isolate has no organism id yet, pass (wildcard); when both set, ids must match.
 */
function organismGroupMatches(p, ctx) {
    const pid = p.OrganismGroupId ?? p.organismGroupId;
    if (!pid || String(pid).trim() === '') return true;
    const c = ctx.OrgGroupCodingId ?? ctx.orgGroupCodingId;
    if (c == null || String(c).trim() === '') return true;
    return String(pid) === String(c);
}

function hasNonEmptyCultureTestId(p) {
    const v = p.CultureTestId ?? p.cultureTestId;
    return v != null && String(v).trim() !== '';
}

function hasNonEmptyDirectTestId(p) {
    const v = p.DirectTestId ?? p.directTestId;
    return v != null && String(v).trim() !== '';
}

/** Direct specimen test only (no isolate test path). Excluded from isolate embedded list (rule 2). */
function isDirectOnlyProfile(p) {
    return hasNonEmptyDirectTestId(p) && !hasNonEmptyCultureTestId(p);
}

/**
 * Maps filtered profiles to Fluent combo options (key = Id or InstrumentName).
 */
export function toComboOptions(profiles) {
    return profiles.map((p) => {
        const id = p.Id ?? p.id;
        const name = p.InstrumentName ?? p.instrumentName;
        const key = id != null && String(id).trim() !== '' ? String(id) : String(name || '');
        const text = name != null && String(name).trim() !== '' ? String(name) : key;
        return { key, text };
    });
}
