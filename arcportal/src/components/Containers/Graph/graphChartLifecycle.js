import { CHART_ANIMATION } from './chartDefaultOptions';

/**
 * @param {import('chart.js').ChartData|undefined} data
 * @param {import('chart.js').ChartOptions|undefined} options
 * @returns {string}
 */
export function buildChartSignature(data, options) {
    return JSON.stringify({ data, options });
}

/**
 * @param {number} [now=Date.now()]
 * @returns {number}
 */
export function createEnterGuardUntil(now = Date.now()) {
    return now + CHART_ANIMATION.duration;
}

/**
 * @param {number} enterGuardUntil
 * @param {number} [now=Date.now()]
 * @returns {number} Milliseconds to wait before applying an in-place chart update (0 = apply now).
 */
export function getDeferredUpdateDelayMs(enterGuardUntil, now = Date.now()) {
    return Math.max(0, enterGuardUntil - now);
}

/**
 * Section filters that affect graph/getdata, excluding client-only graphtype.
 * @param {Record<string, unknown>|undefined|null} sectionFilters
 * @returns {Record<string, unknown>|undefined}
 */
export function sectionFiltersForDataFetch(sectionFilters) {
    if (!sectionFilters) {
        return sectionFilters;
    }
    const { graphtype, ...rest } = sectionFilters;
    return rest;
}
