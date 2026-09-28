import { useRef, useCallback, useEffect } from 'react';
import PostEvent from '../../Data/PostEvents';

/**
 * Hook that encapsulates column layout change logic: merge, save to backend, debounce.
 * @param {Object} params
 * @param {string} params.viewKey - View name for the layout
 * @param {Object|null} params.columnLayout - Current layout { VisibleKeys, Order, Widths }
 * @param {Function} params.onUpdate - (viewKey, newLayout) => void, e.g. dispatch Redux action
 * @param {Function} [params.onSaveError] - (response) => void, called when PostEvent fails
 * @returns {Function} onColumnLayoutChange(update) for ListView
 */
export const useColumnLayoutChange = ({ viewKey, columnLayout, onUpdate, onSaveError }) => {
    const saveTimeoutRef = useRef(null);

    useEffect(() => {
        return () => {
            if (saveTimeoutRef.current) clearTimeout(saveTimeoutRef.current);
        };
    }, []);

    const saveColumnLayout = useCallback((newLayout) => {
        const payload = { Event: 'savecolumnlayoutsevent', Id: '0', ViewName: viewKey, ColumnLayout: newLayout };
        PostEvent(payload, () => onUpdate(viewKey, newLayout), onSaveError ?? (() => {}));
    }, [viewKey, onUpdate, onSaveError]);

    const debouncedSaveColumnLayout = useCallback((newLayout) => {
        if (saveTimeoutRef.current) clearTimeout(saveTimeoutRef.current);
        saveTimeoutRef.current = setTimeout(() => saveColumnLayout(newLayout), 300);
    }, [saveColumnLayout]);

    const onColumnLayoutChange = useCallback((update) => {
        const current = columnLayout || {};
        const newLayout = {
            VisibleKeys: update.VisibleKeys !== undefined ? update.VisibleKeys : (current.VisibleKeys || current.visibleKeys),
            Order: update.Order !== undefined ? update.Order : (current.Order || current.order),
            Widths: update.Widths !== undefined ? update.Widths : (current.Widths || current.widths),
        };
        onUpdate(viewKey, newLayout);
        if (update.Widths !== undefined || update.Order !== undefined) {
            debouncedSaveColumnLayout(newLayout);
        } else {
            saveColumnLayout(newLayout);
        }
    }, [columnLayout, debouncedSaveColumnLayout, saveColumnLayout, viewKey, onUpdate]);

    return onColumnLayoutChange;
};
