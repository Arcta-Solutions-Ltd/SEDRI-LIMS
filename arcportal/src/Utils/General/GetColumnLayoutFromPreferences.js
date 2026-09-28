/**
 * Extracts column layout for a view from user preferences.
 * @param {Object} preferences - User preferences (may have ColumnLayouts as object or JSON string)
 * @param {string} viewKey - View name (e.g. config.Name or config.QueryName)
 * @returns {Object|null} Column layout { VisibleKeys, Order, Widths } or null
 */
export const getColumnLayoutFromPreferences = (preferences, viewKey) => {
    if (!preferences?.ColumnLayouts || !viewKey) return null;
    const layouts = typeof preferences.ColumnLayouts === 'string'
        ? (() => { try { return JSON.parse(preferences.ColumnLayouts); } catch { return {}; } })()
        : preferences.ColumnLayouts;
    return layouts?.[viewKey] ?? null;
};
