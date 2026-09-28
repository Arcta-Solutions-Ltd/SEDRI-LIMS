import SetFilterFieldValues from './SetFilterFieldValues';

/**
 * Initializes sorted column and direction from config.
 * @param {Object} config - View config with GridColumns
 * @param {Object} state - Current filter state
 * @returns {Object} State with sortedColumn and descending set
 */
const SetInitialSortedColumn = (config, state) => {
    let initialSortedColumnDescending = false;
    let initialSortedColumn = config.GridColumns.filter((col) => col.IsSorted === true)[0];
    if (initialSortedColumn !== undefined) {
        initialSortedColumnDescending = initialSortedColumn.IsSortedDescending;
    } else {
        initialSortedColumn = { FieldName: " "}
    }
    return {...state, sortedColumn: initialSortedColumn.FieldName, descending: initialSortedColumnDescending }
}

/**
 * Toggles or sets the sort column.
 * @param {string} fieldName - Column to sort by
 * @param {Object} state - Current filter state
 * @returns {Object} Updated state
 */
const UpdateSortedColumn = (fieldName, state) => {
    const trimmed = (fieldName || '').trim();
    if (!trimmed) return { ...state };
    const newState = { ...state };
    if (trimmed === state.sortedColumn) {
        newState.descending = !state.descending;
    } else {
        newState.sortedColumn = trimmed;
        newState.descending = false;
    }
    return newState;
}

/**
 * Applies the default preset from config if one is marked Default.
 * @param {Object} config - View config with FilterPresets
 * @param {Object} state - Current filter state
 * @returns {Object} State with default preset applied, textSearch cleared
 */
const SetDefaultPreset = (config, state) => {
    let defaultPreset;
    let filters = state.filters;
    if (config.FilterPresets !== "") {
        defaultPreset = config.FilterPresets.filter((f) =>  f.Default === true)[0];
        if (defaultPreset !== undefined) {
            filters = SetFilterFieldValues(defaultPreset,state.filters)
            state.oldFilters = filters;
            state.selectedPreset = defaultPreset;
        } else {
            state.selectedPreset = null;
        }
    }

    return {...state, preset: defaultPreset, filters: filters, textSearch: ""}
}

/**
 * Applies initial filter values from passed-in data (e.g. navigation context).
 * @param {Object} data - Data with id and type
 * @param {Object} state - Current filter state
 * @returns {Object} Updated state
 */
const SetDefaultFiltersPassedIn = (data, state) => {
    const option = {key: data.id.toString(), selected: true};
    state = ChangeFilterCondition(option, data.type, state)

    // if (data.type !== "state") {
    //     const states = ["525", "526", "527", "528", "529", "530", "531", "532", "533", "535", "537"];
    //     for (const item of states) {
    //         state = ChangeFilterCondition({key: item, selected: true}, "state", state);
    //     }
    // }

    return state;
}

/**
 * Parses a date value from a preset (may be Date, ISO string, or other).
 * @param {Date|string} value - Date or ISO string from preset
 * @returns {Date|undefined} Parsed Date or undefined
 */
const parsePresetDate = (value) => {
    if (value == null) return undefined;
    if (value instanceof Date) return value;
    if (typeof value === 'string') {
        const d = new Date(value);
        return isNaN(d.getTime()) ? undefined : d;
    }
    return undefined;
};

/**
 * Applies a filter preset to the current state.
 * Restores dropdown filter values, keyword search text, and date range from the preset.
 * @param {Object} preset - Preset with Fields, optionally TextSearch, StartDate, EndDate
 * @param {Object} state - Current filter state
 * @returns {Object} Updated state with preset values applied
 */
const SelectPreset = (preset, state) => {
    let textSearch = '';
    let startDate = undefined;
    let endDate = undefined;

    if (state.selectedPreset === null || state.selectedPreset === undefined) {
        state.oldFilters = state.filters;
        state.filters = SetFilterFieldValues(preset, state.filters);
        state.selectedPreset = preset;
        textSearch = preset.TextSearch != null ? preset.TextSearch : '';
        startDate = parsePresetDate(preset.StartDate);
        endDate = parsePresetDate(preset.EndDate);
    } else {
        if (state.selectedPreset.Key !== preset.Key) {
            state.filters = SetFilterFieldValues(preset, state.filters);
            state.selectedPreset = preset;
            textSearch = preset.TextSearch != null ? preset.TextSearch : '';
            startDate = parsePresetDate(preset.StartDate);
            endDate = parsePresetDate(preset.EndDate);
        } else {
            state.filters = state.oldFilters;
            state.oldFilters = [];
            state.selectedPreset = null;
            textSearch = '';
        }
    }

    return { ...state, textSearch, startDate, endDate };
};

/**
 * Updates a single filter's selected values.
 * @param {Object|Array} option - Selected option(s) or { key, selected }
 * @param {string} key - Filter key to update
 * @param {Object} state - Current filter state
 * @returns {Object} Updated state with selectedPreset cleared
 */
const ChangeFilterCondition = (option, key, state) => {
    const newFilters = state.filters;
    let changedFilter = newFilters.filter((f) => (f.Key || f.key) === key)[0];

    if (changedFilter !== undefined) {
        if (option.selected === undefined) {
            if (Array.isArray(option)) {
                changedFilter.values = option;
            } else {
                changedFilter.values = [option.key];
            }
        } else {
            if (option.selected) {
                changedFilter.values = changedFilter.values === undefined ? [option.key] : [...changedFilter.values,option.key];
            } else {
                changedFilter.values = changedFilter.values.filter(key => key !== option.key);
            }
        }
    }

    state.filters = newFilters;
    state.selectedPreset = null;

    return {...state }
}

export {SetInitialSortedColumn, SetDefaultPreset, UpdateSortedColumn, SelectPreset, ChangeFilterCondition, SetDefaultFiltersPassedIn};