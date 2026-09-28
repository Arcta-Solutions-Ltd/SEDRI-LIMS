/**
 * Case-insensitive match for view Name vs route/type (config casing can differ from navigation).
 * @param {string|null|undefined} viewName
 * @param {string|null|undefined} currentView
 * @returns {boolean}
 */
export function viewNameMatchesCurrent(viewName, currentView) {
    if (viewName == null || currentView == null || currentView === '') {
        return false;
    }
    return String(viewName).toLowerCase() === String(currentView).toLowerCase();
}

/**
 * Resolves a record view definition from `recordviews` only (list-view config in `views` is not used).
 * @param {string|null|undefined} type - Record view name (e.g. from UI event Action or route).
 * @param {Array<{Name?: string}>|undefined} recordviews
 * @returns {object|undefined}
 */
export function findRecordViewDefinition(type, recordviews) {
    if (type == null || type === '') {
        return undefined;
    }
    return recordviews?.find((v) => viewNameMatchesCurrent(v.Name, type));
}
