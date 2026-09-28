import { sectionTimerangeToParameters, omitTatComplianceExcludedFilterKeys } from './homeDashboardUtils';

/**
 * Parameters for {@link HOMEDASHBOARD_RECENTLY_USED_QUERY} (kinds and filters use list/config ids only).
 * @param {object} section - Home section with `recentlyUsedKinds`, `recentlyUsedDirectTestIds`, `recentlyUsedCultureTypeIds`, `recentlyUsedLimit`.
 * @returns {Array<{Key: string, Value: string}>}
 */
export function buildRecentlyUsedParameters(section) {
    const param = [];
    /** Rolling timerange is always included (same fields as other home tiles); server resolves bounds via GraphDashboardTimeRangeResolver. */
    for (const p of sectionTimerangeToParameters(section)) {
        param.push(p);
    }
    const kinds = section?.recentlyUsedKinds;
    if (Array.isArray(kinds) && kinds.length > 0) {
        param.push({ Key: 'recentlyusedkinds', Value: kinds.map(String).join(',') });
    }
    const directIds = section?.recentlyUsedDirectTestIds;
    if (Array.isArray(directIds) && directIds.length > 0) {
        param.push({ Key: 'recentlyuseddirecttestids', Value: directIds.map(String).join(',') });
    }
    const cultureTypeIds = section?.recentlyUsedCultureTypeIds;
    if (Array.isArray(cultureTypeIds) && cultureTypeIds.length > 0) {
        param.push({ Key: 'recentlyusedculturetypeids', Value: cultureTypeIds.map(String).join(',') });
    }
    const limit = section?.recentlyUsedLimit != null ? parseInt(String(section.recentlyUsedLimit), 10) : 10;
    param.push({ Key: 'recentlyusedlimit', Value: String(Number.isFinite(limit) && limit >= 1 ? limit : 10) });
    return param;
}

/**
 * Builds {@link QueryFilterConfig} parameters for specimen count queries and graph queries from a section model.
 * @param {object} section - Home section with optional `filters` (key -> id or comma-separated ids) and timerange fields.
 * @returns {Array<{Key: string, Value: string}>}
 */
export function buildDashboardQueryParameters(section) {
    const param = [];
    const f = section.filters || {};
    Object.entries(f).forEach(([key, val]) => {
        if (val === undefined || val === null || val === '') return;
        const v = Array.isArray(val) ? val.join(',') : String(val);
        param.push({ Key: key, Value: v });
    });
    for (const p of sectionTimerangeToParameters(section)) {
        param.push(p);
    }
    return param;
}

/**
 * Parameters for {@link HOMEDASHBOARD_TAT_COMPLIANCE_QUERY}: same filters/timerange as graph tiles, plus TAT/RAG thresholds.
 * @param {object} section
 * @returns {Array<{Key: string, Value: string}>}
 */
export function buildTatComplianceParameters(section) {
    const sectionForTat = {
        ...section,
        filters: omitTatComplianceExcludedFilterKeys(section?.filters || {}),
    };
    const param = buildDashboardQueryParameters(sectionForTat);
    const t = section?.tatTargetHours != null ? parseFloat(String(section.tatTargetHours)) : 48;
    const l = section?.tatLateThresholdHours != null ? parseFloat(String(section.tatLateThresholdHours)) : 48;
    const days = section?.tatRollingAverageDays != null ? parseInt(String(section.tatRollingAverageDays), 10) : 0;
    const rg = section?.ragGreenMin != null ? parseFloat(String(section.ragGreenMin)) : 90;
    const ra = section?.ragAmberMin != null ? parseFloat(String(section.ragAmberMin)) : 75;
    param.push({ Key: 'tattargethours', Value: String(Number.isFinite(t) && t > 0 ? t : 48) });
    param.push({ Key: 'tatlatethresholdhours', Value: String(Number.isFinite(l) && l > 0 ? l : 48) });
    param.push({ Key: 'tataveragedays', Value: String(Number.isFinite(days) && days >= 0 ? days : 0) });
    param.push({ Key: 'raggreenmin', Value: String(Number.isFinite(rg) ? rg : 90) });
    param.push({ Key: 'ragambermin', Value: String(Number.isFinite(ra) ? ra : 75) });
    return param;
}
