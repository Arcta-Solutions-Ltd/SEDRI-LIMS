/**
 * Recursively clones context menu items so each leaf {@code onClick} invokes {@code onLeafClick}
 * with the row's button config. Nested {@code subMenuProps.items} are wrapped the same way.
 *
 * @param {Array<Object>|undefined} items - Menu items from {@code MapButtonsToContextMenu}.
 * @param {Function|undefined} onLeafClick - Called with the leaf {@code button} config when clicked.
 * @returns {Array<Object>|undefined} Cloned items with row-scoped handlers.
 */
const wrapContextMenuItemsWithRowData = (items, onLeafClick) => {
    if (!onLeafClick || !items || !Array.isArray(items)) {
        return items;
    }

    return items.map((mi) => {
        const item = { ...mi };

        if (item.subMenuProps?.items?.length) {
            item.subMenuProps = {
                ...item.subMenuProps,
                items: wrapContextMenuItemsWithRowData(item.subMenuProps.items, onLeafClick),
            };
        }

        if (typeof item.onClick === 'function' && item.button) {
            item.onClick = (ev) => {
                if (ev?.preventDefault) {
                    ev.preventDefault();
                }
                if (ev?.stopPropagation) {
                    ev.stopPropagation();
                }
                onLeafClick(item.button);
            };
        }

        return item;
    });
};

export default wrapContextMenuItemsWithRowData;
