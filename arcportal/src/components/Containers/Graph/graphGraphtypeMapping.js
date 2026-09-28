/**
 * Maps analytics `graphtype` list item keys (same as CombinedFilter / Graph page)
 * to Chart.js props used by GraphFactory.
 *
 * Known GraphType list ids: 477–478 bar (stacked / grouped), 482 line, 483 pie,
 * 484 polar area, 485 donut, 487 radar (486 also maps to radar for older databases).
 */

import { getFirstGraphtypeId } from '../../Layout/Home/homeGraphFilterUi';

const DEFAULT = { type: 'bar', stack: false };
const DEFAULT_BAR_GRAPHTYPE_ID = '478';

/**
 * @param {string|number|undefined|null} listKey - List option key from configuration (e.g. "478", "482")
 * @returns {{ type: string, stack: boolean }}
 */
export function chartPropsFromGraphtypeKey(listKey) {
    const k = listKey != null ? String(listKey) : '';
    if (k === '477' || k === '478') {
        return { type: 'bar', stack: k === '477' };
    }
    if (k === '482') {
        return { type: 'line', stack: false };
    }
    if (k === '485') {
        return { type: 'donut', stack: false };
    }
    if (k === '483') {
        return { type: 'pie', stack: false };
    }
    if (k === '484') {
        return { type: 'polar', stack: false };
    }
    if (k === '486' || k === '487') {
        return { type: 'radar', stack: false };
    }
    return { ...DEFAULT };
}

/**
 * Resolves Chart.js type/stack from Analytics CombinedFilter state.
 * Uses graphtype list item id (list 14), never translated labels.
 *
 * @param {{ filters?: Array<{ Key?: string, values?: string[] }> }|undefined|null} filterState - Analytics filter state
 * @param {{ Type?: string, Stack?: boolean, Filters?: Array<{ Key?: string }> }|undefined|null} fallbackGraphConfig - Graph config when no graphtype filter exists
 * @returns {{ type: string, stack: boolean }}
 */
export function chartPropsFromAnalyticsFilters(filterState, fallbackGraphConfig) {
    const hasGraphtypeFilter = filterState?.filters?.some((f) => f.Key === 'graphtype')
        || fallbackGraphConfig?.Filters?.some((f) => f.Key === 'graphtype');

    if (hasGraphtypeFilter) {
        const graphtypeFilter = filterState?.filters?.find((f) => f.Key === 'graphtype');
        const graphtypeId = getFirstGraphtypeId(graphtypeFilter?.values?.[0]) || DEFAULT_BAR_GRAPHTYPE_ID;
        return chartPropsFromGraphtypeKey(graphtypeId);
    }

    return {
        type: fallbackGraphConfig?.Type ?? DEFAULT.type,
        stack: fallbackGraphConfig?.Stack ?? DEFAULT.stack,
    };
}
