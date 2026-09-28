/** Maps test-grid context menu button keys to short automation suffixes. */
const TEST_GRID_MENU_KEY_SUFFIX = {
    viewtest: 'view',
    edittest: 'edit',
    deletetest: 'delete',
};

/**
 * Builds a stable data-testid for a test-grid row inline menu icon.
 * @param {string|undefined} prefix - Row menu prefix (e.g. culturetest, directtest).
 * @param {string|number|undefined} recordId - Test row id (CultureTests.Id / Tests.Id).
 * @param {string|undefined} menuKey - Context menu button key (viewtest, edittest, deletetest).
 * @returns {string|undefined} data-testid value, or undefined when inputs are incomplete.
 */
export function buildTestGridRowMenuTestId(prefix, recordId, menuKey) {
    if (!prefix || recordId == null || recordId === '' || !menuKey) {
        return undefined;
    }

    const suffix = TEST_GRID_MENU_KEY_SUFFIX[menuKey] ?? menuKey;
    return `${prefix}-row-${recordId}-${suffix}`;
}

export default buildTestGridRowMenuTestId;
