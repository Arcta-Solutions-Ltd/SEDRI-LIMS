/** @typedef {'specimen'|'culture'} TestListParentType */

/**
 * @typedef {Object} TestListSyncPayload
 * @property {number} generation - Monotonic id; grid applies each sync once.
 * @property {Array} rows - Authoritative test list from the completed fetch.
 * @property {TestListParentType} parentType
 */

let syncGenerationCounter = 0;

/**
 * Creates a sync payload for in-memory handoff from View/Update panel to the record-view grid.
 * This is not a persistent cache — each payload represents one completed list fetch.
 * @param {TestListParentType} parentType
 * @param {Array} rows
 * @returns {TestListSyncPayload}
 */
export function createTestListSyncPayload(parentType, rows) {
    syncGenerationCounter += 1;
    return {
        generation: syncGenerationCounter,
        rows: Array.isArray(rows) ? rows : [],
        parentType,
    };
}
