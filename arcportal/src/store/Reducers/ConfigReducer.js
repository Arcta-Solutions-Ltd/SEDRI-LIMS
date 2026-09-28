const initialState = {
    sidebar: [
    ],
    rightSidebar: [
        {
          links: [
            { name: 'Preferences', key: 'preferences' },
            { name: 'Log Out', key: 'logout' }
          ]
        },
    ],
    views: [
      {Name: "login", Type: "login"}
    ]
}

/**
 * Updates FilterPresets for a view/graph in the given array.
 * @param {Array} arr - Config array (views, recordviews, reportinggrids, or graphs)
 * @param {string} viewName - Name of the view/graph to update
 * @param {Array} filterPresets - New FilterPresets array
 * @returns {Array} New array with updated item, or original if no match
 */
const updateFilterPresetsInArray = (arr, viewName, filterPresets) => {
    if (!Array.isArray(arr)) return arr;
    const idx = arr.findIndex((v) => v && v.Name === viewName);
    if (idx < 0) return arr;
    return [
        ...arr.slice(0, idx),
        { ...arr[idx], FilterPresets: filterPresets },
        ...arr.slice(idx + 1),
    ];
};

/**
 * Updates ColumnLayout for a view in the given array.
 * @param {Array} arr - Config array (views, recordviews, or reportinggrids)
 * @param {string} viewName - Name of the view to update
 * @param {Object} columnLayout - New ColumnLayout object (VisibleKeys, Order, Widths)
 * @returns {Array} New array with updated item, or original if no match
 */
const updateColumnLayoutInArray = (arr, viewName, columnLayout) => {
    if (!Array.isArray(arr)) return arr;
    const idx = arr.findIndex((v) => v && v.Name === viewName);
    if (idx < 0) return arr;
    return [
        ...arr.slice(0, idx),
        { ...arr[idx], ColumnLayout: columnLayout },
        ...arr.slice(idx + 1),
    ];
};

const configReducer = (state = initialState, action) => {
    let newState = state;

    if (action.type === 'UPDATECONFIG') {
        newState = action.value;
    } else if (action.type === 'UPDATE_VIEW_FILTER_PRESETS') {
        const { viewName, filterPresets } = action;
        if (viewName && filterPresets) {
            newState = { ...state };
            if (Array.isArray(state.views)) {
                newState.views = updateFilterPresetsInArray(state.views, viewName, filterPresets);
            }
            if (Array.isArray(state.recordviews)) {
                newState.recordviews = updateFilterPresetsInArray(state.recordviews, viewName, filterPresets);
            }
            if (Array.isArray(state.reportinggrids)) {
                newState.reportinggrids = updateFilterPresetsInArray(state.reportinggrids, viewName, filterPresets);
            }
            if (Array.isArray(state.graphs)) {
                newState.graphs = updateFilterPresetsInArray(state.graphs, viewName, filterPresets);
            }
        }
    } else if (action.type === 'UPDATE_VIEW_COLUMN_LAYOUT') {
        const { viewName, columnLayout } = action;
        if (viewName) {
            newState = { ...state };
            if (Array.isArray(state.views)) {
                newState.views = updateColumnLayoutInArray(state.views, viewName, columnLayout);
            }
            if (Array.isArray(state.recordviews)) {
                newState.recordviews = updateColumnLayoutInArray(state.recordviews, viewName, columnLayout);
            }
            if (Array.isArray(state.reportinggrids)) {
                newState.reportinggrids = updateColumnLayoutInArray(state.reportinggrids, viewName, columnLayout);
            }
        }
    } else if (action.type === 'UPDATE_PREFERENCES_HOME_DASHBOARD') {
        const { homeDashboard } = action;
        if (homeDashboard !== undefined) {
            newState = { ...state };
            const prefs = state.preferences || {};
            newState.preferences = { ...prefs, HomeDashboard: homeDashboard };
        }
    } else if (action.type === 'UPDATE_PREFERENCES_COLUMN_LAYOUT') {
        const { viewKey, columnLayout } = action;
        if (viewKey) {
            newState = { ...state };
            const prefs = state.preferences || {};
            let layouts = prefs.ColumnLayouts;
            if (typeof layouts === 'string') {
                try { layouts = JSON.parse(layouts); } catch { layouts = {}; }
            }
            layouts = typeof layouts === 'object' && layouts !== null ? { ...layouts } : {};
            layouts[viewKey] = columnLayout;
            newState.preferences = { ...prefs, ColumnLayouts: layouts };
        }
    }
    return newState;
}

export default configReducer;