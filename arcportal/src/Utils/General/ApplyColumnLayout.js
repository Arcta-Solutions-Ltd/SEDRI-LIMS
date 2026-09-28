/**
 * Applies user column layout preferences (visibility, order, widths) to base columns.
 * @param {Array} baseColumns - Column configs from view (Key, Name, FieldName, MinWidth, MaxWidth, etc.)
 * @param {Object} [columnLayout] - User layout: { VisibleKeys, Order, Widths }
 * @returns {Array} Columns with layout applied
 */
export const applyColumnLayout = (baseColumns, columnLayout) => {
    if (!Array.isArray(baseColumns) || baseColumns.length === 0) return baseColumns;

    let result = [...baseColumns];

    // When no layout: exclude columns with defaultHidden so they are hidden by default
    if (!columnLayout) {
        result = result.filter((c) => !(c.defaultHidden || c.DefaultHidden));
        return result;
    }

    // Filter by VisibleKeys if present and non-empty
    const visibleKeys = columnLayout.VisibleKeys || columnLayout.visibleKeys;
    if (Array.isArray(visibleKeys) && visibleKeys.length > 0) {
        const keySet = new Set(visibleKeys.map((k) => String(k).toLowerCase()));
        result = result.filter((c) => keySet.has(String(c.Key || c.key || '').toLowerCase()));
    }

    // Reorder by Order if present and non-empty
    const order = columnLayout.Order || columnLayout.order;
    if (Array.isArray(order) && order.length > 0) {
        const orderMap = new Map(order.map((k, i) => [String(k).toLowerCase(), i]));
        result.sort((a, b) => {
            const keyA = String(a.Key || a.key || '').toLowerCase();
            const keyB = String(b.Key || b.key || '').toLowerCase();
            const idxA = orderMap.has(keyA) ? orderMap.get(keyA) : 9999;
            const idxB = orderMap.has(keyB) ? orderMap.get(keyB) : 9999;
            return idxA - idxB;
        });
    }

    // Apply widths: set calculatedWidth only; keep original MinWidth so columns can be shrunk again
    const widths = columnLayout.Widths || columnLayout.widths;
    if (widths && typeof widths === 'object') {
        result = result.map((col) => {
            const key = col.Key || col.key;
            const w = widths[key];
            if (typeof w === 'number' && w > 0) {
                return { ...col, calculatedWidth: w };
            }
            return col;
        });
    }

    return result;
};
