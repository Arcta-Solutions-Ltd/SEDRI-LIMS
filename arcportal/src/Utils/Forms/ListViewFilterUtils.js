/**
 * @typedef {Object} ListViewFilterConfig
 * @property {Array<Object>} [Filters]
 * @property {Array<Object>} [filters]
 * @property {boolean} [FilterSearch]
 * @property {boolean} [filterSearch]
 * @property {string} [DateSearch]
 * @property {string} [dateSearch]
 * @property {Array<Object>} [NumberRanges]
 * @property {Array<Object>} [numberRanges]
 * @property {Array<Object>} [FilterPresets]
 * @property {Array<Object>} [filterPresets]
 */

/**
 * Returns the dropdown filter definitions from a list view config (supports PascalCase and camelCase).
 * @param {ListViewFilterConfig|undefined|null} viewConfig
 * @returns {Array<Object>}
 */
const getFilterDefinitions = (viewConfig) => {
    const filters = viewConfig?.Filters ?? viewConfig?.filters;
    return Array.isArray(filters) ? filters : [];
};

/**
 * Returns true when the view config defines at least one filter control
 * (dropdown filters, keyword search, date search, or number ranges).
 * Matches server ListViewConfig semantics: FilterSearch must be explicitly true;
 * DateSearch must be a non-empty string (e.g. 'range').
 * @param {ListViewFilterConfig|undefined|null} viewConfig
 * @returns {boolean}
 */
export function hasListViewFilterControls(viewConfig) {
    if (!viewConfig) {
        return false;
    }

    const hasDropdownFilters = getFilterDefinitions(viewConfig).length > 0;
    const filterSearch = viewConfig.FilterSearch ?? viewConfig.filterSearch;
    const hasKeywordSearch = filterSearch === true;
    const dateSearch = viewConfig.DateSearch ?? viewConfig.dateSearch;
    const hasDateSearch = dateSearch != null && String(dateSearch).trim() !== '';
    const numberRanges = viewConfig.NumberRanges ?? viewConfig.numberRanges;
    const hasNumberRanges = Array.isArray(numberRanges)
        ? numberRanges.length > 0
        : numberRanges != null && String(numberRanges).trim() !== '';

    return hasDropdownFilters || hasKeywordSearch || hasDateSearch || hasNumberRanges;
}

/**
 * Returns true when runtime or config filter presets exist (array length > 0).
 * @param {ListViewFilterConfig|undefined|null} viewConfig
 * @param {Array<Object>|undefined|null} runtimePresets - Presets from component state (may include user-saved presets)
 * @returns {boolean}
 */
export function hasListViewFilterPresets(viewConfig, runtimePresets) {
    if (Array.isArray(runtimePresets) && runtimePresets.length > 0) {
        return true;
    }
    const configPresets = viewConfig?.FilterPresets ?? viewConfig?.filterPresets;
    return Array.isArray(configPresets) && configPresets.length > 0;
}

/**
 * Returns true when the CombinedFilter wrapper should mount on a manage list.
 * Preset-only UI is not shown without filter controls, so this matches hasListViewFilterControls.
 * @param {ListViewFilterConfig|undefined|null} viewConfig
 * @param {Array<Object>|undefined|null} runtimePresets
 * @returns {boolean}
 */
export function shouldShowListViewFilterUi(viewConfig, runtimePresets) {
    return hasListViewFilterControls(viewConfig);
}
