/**
 * Applies preset field values to the current filter list.
 * @param {Object} preset - Preset with Fields array (each { Key, Values })
 * @param {Array} currentFilters - Current filter definitions
 * @returns {Array} New filters with values from preset applied
 */
const SetFilterFieldValues = (preset, currentFilters) => {
    let newFilters = [];
    for (const filter of currentFilters) {
        let newFilter = {...filter};
        const filterKey = filter.Key || filter.key;
        const presetValue = preset.Fields.filter((flt) => (flt.Key || flt.key) === filterKey);
        if (presetValue.length > 0) {
            newFilter.values = presetValue[0] !== undefined ? (presetValue[0].Values || presetValue[0].values || []) : [];
        }
        newFilters.push(newFilter);
    }

    return newFilters;
}

export default SetFilterFieldValues;