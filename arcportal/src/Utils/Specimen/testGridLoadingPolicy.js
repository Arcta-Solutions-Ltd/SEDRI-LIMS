/**
 * @typedef {Object} TestGridLoadingState
 * @property {boolean} isDataLoaded - Passed to ListView; false only when no rows to show yet.
 * @property {boolean} showBlockingOverlay - Full spinner overlay on the grid.
 * @property {boolean} blockInteraction - Dim grid and disable pointer events during refresh.
 */

/**
 * Derives loading UI flags for embedded direct/isolate test grids.
 * @param {Array|null|undefined} listData
 * @param {boolean} listRefreshInFlight
 * @returns {TestGridLoadingState}
 */
export function getTestGridLoadingState(listData, listRefreshInFlight) {
    const hasExistingRows = Array.isArray(listData) && listData.length > 0;

    return {
        isDataLoaded: !listRefreshInFlight || hasExistingRows,
        showBlockingOverlay: listRefreshInFlight && !hasExistingRows,
        blockInteraction: listRefreshInFlight,
    };
}
