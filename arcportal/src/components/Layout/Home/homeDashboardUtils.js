import {
    normalizeKpiSection,
    DEFAULT_KPI_FONT_SIZE_PX,
    KPI_FONT_SIZE_MAX,
    KPI_FONT_SIZE_MIN,
} from './homeKpiPalette';

import TranslateTag from '../../../Utils/Local/TranslateTag';

/**
 * Home dashboard persisted model (stored in users.moredata.HomeDashboard).
 * Filter values use list item / entity IDs only (never display strings).
 * Optional `sectionTitle` is user-visible plain text for the tile heading.
 * KPI sections (`visualization === 'kpi'`) store `kpiColorSwatchId`, `kpiFontSizePx`, `kpiBold` (see `homeKpiPalette.js`).
 * TAT Compliance (`visualization === 'tatCompliance'`) stores `tatFontSizePx`, `tatBold` (same px range as KPI).
 */

/** Bump when persisted section shape changes (migration in parseHomeDashboardPreference). */
export const DASHBOARD_VERSION = 8;

/** Internal codes for recently-used entity kinds (not user-facing; labels use EnglishLanguage + TranslateTag). */
export const RECENTLY_USED_KINDS = ['specimen', 'patient', 'test', 'culture'];

/** Persisted section `visualization` values (invalid or missing values normalize to `buttons`). */
export const HOME_SECTION_VISUALIZATIONS = ['buttons', 'graph', 'recentlyUsed', 'kpi', 'tatCompliance'];

/**
 * Filter keys not shown or applied for TAT Compliance (specimen received→finalised; not arbitrary state or test-level requested→completed).
 * @type {readonly string[]}
 */
export const TAT_COMPLIANCE_EXCLUDED_FILTER_KEYS = ['stateid', 'testid'];

/**
 * @param {Record<string, unknown>|undefined|null} filters
 * @returns {Record<string, unknown>}
 */
export function omitTatComplianceExcludedFilterKeys(filters) {
    if (!filters || typeof filters !== 'object') {
        return {};
    }
    const drop = new Set(TAT_COMPLIANCE_EXCLUDED_FILTER_KEYS.map((k) => k.toLowerCase()));
    const next = {};
    for (const [k, v] of Object.entries(filters)) {
        if (drop.has(String(k).toLowerCase())) {
            continue;
        }
        next[k] = v;
    }
    return next;
}

/** Default max rows for Recently Used sections. */
export const DEFAULT_RECENTLY_USED_LIMIT = 10;

/**
 * List name in DB (see yuniql 072-dashboard-timerange-unit-list.sql).
 * @type {string}
 */
export const DASHBOARD_TIMERANGE_UNIT_LIST_NAME = 'dashboardtimerangeunit';

/**
 * Default rolling window when creating sections or migrating legacy "all" presets (1 year).
 * Must match migration listitem id for Years.
 */
export const DEFAULT_TIMERANGE_AMOUNT = 1;
export const DEFAULT_TIMERANGE_UNIT_LIST_ITEM_ID = '1539';

/**
 * Fixed ListItem Ids -> API unit for GraphDashboardTimeRangeResolver (must match DB migration).
 * @type {Record<number, string>}
 */
export const TIMERANGE_UNIT_ID_TO_API = {
    1539: 'years',
    1540: 'months',
    1541: 'days',
    1542: 'hours',
};

/** @deprecated Legacy preset keys only; used for migration. */
const LEGACY_TIME_PRESET_MAP = {
    all: { mode: 'all' },
    lastYear: { mode: 'rolling', amount: 1, unit: 'years' },
    sixMonths: { mode: 'rolling', amount: 6, unit: 'months' },
    lastThreeMonths: { mode: 'rolling', amount: 3, unit: 'months' },
    lastMonth: { mode: 'rolling', amount: 1, unit: 'months' },
    lastTwoWeeks: { mode: 'rolling', amount: 2, unit: 'weeks' },
    lastWeek: { mode: 'rolling', amount: 1, unit: 'weeks' },
    last3Days: { mode: 'rolling', amount: 3, unit: 'days' },
    last24hrs: { mode: 'rolling', amount: 24, unit: 'hours' },
    last3hrs: { mode: 'rolling', amount: 3, unit: 'hours' },
    lastHour: { mode: 'rolling', amount: 1, unit: 'hours' },
};

const UNIT_TO_LIST_ITEM_ID = {
    years: '1539',
    months: '1540',
    days: '1541',
    hours: '1542',
};

/**
 * @param {string} unit
 * @returns {string|undefined}
 */
function listItemIdForApiUnit(unit) {
    const u = (unit || '').toLowerCase();
    if (u === 'weeks') {
        return UNIT_TO_LIST_ITEM_ID.days;
    }
    return UNIT_TO_LIST_ITEM_ID[u];
}

/**
 * @param {string} presetKey
 * @returns {{ timerangeAmount: number, timerangeUnitListItemId: string }}
 */
function migrateLegacyTimePreset(presetKey) {
    const def = LEGACY_TIME_PRESET_MAP[presetKey] || LEGACY_TIME_PRESET_MAP.all;
    if (def.mode === 'all') {
        return {
            timerangeAmount: DEFAULT_TIMERANGE_AMOUNT,
            timerangeUnitListItemId: DEFAULT_TIMERANGE_UNIT_LIST_ITEM_ID,
        };
    }
    let amount = def.amount ?? 1;
    let unit = def.unit;
    if (unit === 'weeks') {
        amount *= 7;
        unit = 'days';
    }
    const listId = listItemIdForApiUnit(unit) || DEFAULT_TIMERANGE_UNIT_LIST_ITEM_ID;
    return {
        timerangeAmount: Math.max(1, amount),
        timerangeUnitListItemId: listId,
    };
}

/**
 * @param {object} section
 * @returns {object}
 */
export function normalizeHomeSection(section) {
    if (!section || typeof section !== 'object') {
        return section;
    }
    let next;
    if (section.timerangeAmount != null && section.timerangeUnitListItemId != null) {
        const amount = Math.max(1, parseInt(String(section.timerangeAmount), 10) || 1);
        const unitId = String(section.timerangeUnitListItemId);
        next = { ...section, timerangeAmount: amount, timerangeUnitListItemId: unitId };
        delete next.timePreset;
    } else if (section.timePreset != null && section.timePreset !== '') {
        const migrated = migrateLegacyTimePreset(String(section.timePreset));
        const { timePreset, ...rest } = section;
        next = { ...rest, ...migrated };
    } else {
        next = {
            ...section,
            timerangeAmount: DEFAULT_TIMERANGE_AMOUNT,
            timerangeUnitListItemId: DEFAULT_TIMERANGE_UNIT_LIST_ITEM_ID,
        };
        delete next.timePreset;
    }
    const viz = next.visualization;
    if (!HOME_SECTION_VISUALIZATIONS.includes(viz)) {
        next = { ...next, visualization: 'buttons' };
    }

    const sectionTitle = typeof next.sectionTitle === 'string' ? next.sectionTitle : '';
    next = { ...next, sectionTitle };

    if (next.visualization === 'recentlyUsed') {
        return normalizeRecentlyUsedSection(next);
    }
    if (next.visualization === 'kpi') {
        return normalizeKpiSection(next);
    }
    if (next.visualization === 'tatCompliance') {
        return normalizeTatComplianceSection(next);
    }
    return next;
}

/**
 * Defaults for TAT Compliance visualization (per-user section; numeric thresholds only in JSON).
 * @param {object} section
 * @returns {object}
 */
function normalizeTatComplianceSection(section) {
    if (!section || section.visualization !== 'tatCompliance') {
        return section;
    }
    const t = parseFloat(section.tatTargetHours);
    const tatTargetHours = Number.isFinite(t) && t > 0 ? t : 48;
    const l = parseFloat(section.tatLateThresholdHours);
    const tatLateThresholdHours = Number.isFinite(l) && l > 0 ? l : 48;
    const d = parseInt(String(section.tatRollingAverageDays != null ? section.tatRollingAverageDays : 0), 10);
    const tatRollingAverageDays = Number.isFinite(d) && d >= 0 ? d : 0;
    let rg = parseFloat(section.ragGreenMin);
    rg = Number.isFinite(rg) ? Math.min(100, Math.max(0, rg)) : 90;
    let ra = parseFloat(section.ragAmberMin);
    ra = Number.isFinite(ra) ? Math.min(100, Math.max(0, ra)) : 75;
    if (ra > rg) {
        ra = rg;
    }
    let fontPx = parseInt(String(section.tatFontSizePx != null ? section.tatFontSizePx : DEFAULT_KPI_FONT_SIZE_PX), 10);
    if (!Number.isFinite(fontPx)) {
        fontPx = DEFAULT_KPI_FONT_SIZE_PX;
    }
    fontPx = Math.min(KPI_FONT_SIZE_MAX, Math.max(KPI_FONT_SIZE_MIN, fontPx));
    const tatBold = section.tatBold === undefined ? true : Boolean(section.tatBold);
    const rawFilters = section.filters && typeof section.filters === 'object' ? section.filters : {};
    return {
        ...section,
        tatTargetHours,
        tatLateThresholdHours,
        tatRollingAverageDays,
        ragGreenMin: rg,
        ragAmberMin: ra,
        tatFontSizePx: fontPx,
        tatBold,
        filters: omitTatComplianceExcludedFilterKeys(rawFilters),
    };
}

/**
 * Defaults for Recently Used visualization (IDs only; no display strings in persisted state).
 * Includes `recentlyUsedDirectTestIds` (direct test config list keys) and `recentlyUsedCultureTypeIds` (culturetype list item ids).
 * @param {object} section
 * @returns {object}
 */
function normalizeRecentlyUsedSection(section) {
    if (!section || section.visualization !== 'recentlyUsed') {
        return section;
    }
    let kinds = section.recentlyUsedKinds;
    if (!Array.isArray(kinds) || kinds.length === 0) {
        kinds = [...RECENTLY_USED_KINDS];
    } else {
        kinds = kinds.filter((k) => RECENTLY_USED_KINDS.includes(String(k)));
    }
    let directIds = section.recentlyUsedDirectTestIds;
    if (!Array.isArray(directIds)) {
        directIds = [];
    } else {
        directIds = directIds.map((id) => String(id));
    }
    let cultureTypeIds = section.recentlyUsedCultureTypeIds;
    if (!Array.isArray(cultureTypeIds)) {
        cultureTypeIds = [];
    } else {
        cultureTypeIds = cultureTypeIds.map((id) => String(id));
    }
    const limitRaw = section.recentlyUsedLimit;
    const limit = Math.max(1, parseInt(String(limitRaw != null ? limitRaw : DEFAULT_RECENTLY_USED_LIMIT), 10) || DEFAULT_RECENTLY_USED_LIMIT);
    return {
        ...section,
        recentlyUsedKinds: kinds,
        recentlyUsedDirectTestIds: directIds,
        recentlyUsedCultureTypeIds: cultureTypeIds,
        recentlyUsedLimit: limit,
    };
}

/**
 * Maps section timerange to API parameters for GraphDashboardTimeRangeResolver / filteredget.
 * Rolling only; uses ListItem Id -> API unit map (not ListItem.Value).
 * @param {object} section
 * @returns {Array<{Key: string, Value: string}>}
 */
export function sectionTimerangeToParameters(section) {
    const idStr =
        section?.timerangeUnitListItemId != null
            ? String(section.timerangeUnitListItemId)
            : DEFAULT_TIMERANGE_UNIT_LIST_ITEM_ID;
    const numId = parseInt(idStr, 10);
    const unit = TIMERANGE_UNIT_ID_TO_API[numId] || 'days';
    const amount = Math.max(1, parseInt(String(section?.timerangeAmount), 10) || 1);
    return [
        { Key: 'timerangemode', Value: 'rolling' },
        { Key: 'timerangeamount', Value: String(amount) },
        { Key: 'timerangeunit', Value: unit },
    ];
}

/** @returns {object} Default dashboard model (three button sections). */
export function createDefaultHomeDashboard(language) {
    const baseSection = {
        visualization: 'buttons',
        graphName: '',
        sectionTitle: '',
        timerangeAmount: DEFAULT_TIMERANGE_AMOUNT,
        timerangeUnitListItemId: DEFAULT_TIMERANGE_UNIT_LIST_ITEM_ID,
        filters: {},
    };
    const stateSection = {...baseSection}; stateSection.sectionTitle = TranslateTag("@SpeStaA@", language);
    const typeSection = {...baseSection}; typeSection.sectionTitle = TranslateTag("@SpeTyp@", language);
    const tagSection = {...baseSection}; tagSection.sectionTitle = TranslateTag("@SpeTag@", language);
    return {
        version: DASHBOARD_VERSION,
        layout: [
            { i: 'sec_state', x: 0, y: 0, w: 4, h: 10, minW: 2, minH: 4 },
            { i: 'sec_type', x: 4, y: 0, w: 4, h: 10, minW: 2, minH: 4 },
            { i: 'sec_tag', x: 8, y: 0, w: 4, h: 10, minW: 2, minH: 4 },
        ],
        sections: {
            sec_state: { ...stateSection, buttonKind: 'state' },
            sec_type: { ...typeSection, buttonKind: 'specimentype' },
            sec_tag: { ...tagSection, buttonKind: 'tag' },
        },
    };
}

/**
 * @param {string|object|null|undefined} raw
 * @returns {object}
 */
export function parseHomeDashboardPreference(raw, language) {
    if (raw == null || raw === '') {
        return createDefaultHomeDashboard(language);
    }
    try {
        const parsed = typeof raw === 'string' ? JSON.parse(raw) : raw;
        if (parsed && parsed.sections && parsed.layout) {
            const defaults = createDefaultHomeDashboard(language);
            const sections = {};
            for (const [key, sec] of Object.entries(parsed.sections)) {
                sections[key] = normalizeHomeSection(sec);
            }
            return {
                ...defaults,
                ...parsed,
                sections,
                version: parsed.version >= DASHBOARD_VERSION ? parsed.version : DASHBOARD_VERSION,
            };
        }
    } catch {
        /* fall through */
    }
    return createDefaultHomeDashboard(language);
}

/**
 * @param {string} buttonKind
 * @returns {string}
 */
export function buttonKindToQueryName(buttonKind) {
    switch (buttonKind) {
        case 'state':
            return 'specimenstatecount';
        case 'specimentype':
            return 'specimentypecount';
        case 'tag':
            return 'specimentagcount';
        default:
            return 'specimenstatecount';
    }
}

/** Query name for home dashboard Recently Used (server SpecialFactory). */
export const HOMEDASHBOARD_RECENTLY_USED_QUERY = 'homedashboardrecentlyused';

/** Query name for home dashboard TAT Compliance KPI (server SpecialFactory). */
export const HOMEDASHBOARD_TAT_COMPLIANCE_QUERY = 'homedashboardtatcompliance';
