import FilterButtonsOnState from '../State/FilterButtonsOnState';
import ApplyButtonRule from '../Rules/ApplyButtonRule';
import MapButtons from './MapButtons';

const GetVisibleButtons = (buttons, selection, state, filterValues, batchStatus, specimenTypeId, laboratoryId, laboratoryList) => {

    let allButtons = [];
    let newButtons = [];

    for (const button of buttons) {
            let tempButton = {...button};
            let childButtons = false
            if (button.Buttons !== undefined && Array.isArray(button.Buttons)) {
                childButtons = true
                tempButton.Buttons = [];
                for (const btn of button.Buttons) {
                    if ( ApplyButtonRule(btn, filterValues) ) {
                        tempButton.Buttons.push(btn);
                        childButtons = false;
                    }
                }
            }

            if ( ! childButtons && ApplyButtonRule(button, filterValues) ) {
                allButtons.push(tempButton);
            }
    }    

    for (const button of allButtons) {
        const useButton = ((selection && button.OnSelect) || (batchStatus && button.OnBatch)) || (! button.OnSelect && ! button.OnBatch)
        if (useButton) {
            let tempButton = {...button};
            if (button.Buttons !== undefined && Array.isArray(button.Buttons)) {
                tempButton.Buttons = [];
                for (const btn of button.Buttons) {
                    const useSubButton = ((selection && btn.OnSelect) || (batchStatus && btn.OnBatch)) || (! btn.OnSelect && ! btn.OnBatch)
                    if (useSubButton) {
                        tempButton.Buttons.push(btn);
                    }
                }
            }
            newButtons.push(tempButton);
        }
    }

    newButtons = MapButtons(FilterButtonsOnState(newButtons, state, specimenTypeId, laboratoryId, laboratoryList));

    return newButtons;

}

const PutButtonsIntoPassiveMode = (buttons) => {
    buttons = buttons.filter(button => button.Key.toLowerCase() == "printpreview");
    const newButtons = MapButtons(buttons);
    return newButtons;
}

/** Growth parent list item id (427). Cultures under this parent may show Add Isolate and AST. */
const GROWTH_PARENT_ID = '427';

/** No Growth parent list item id (428). When growth has this parent, Add Isolate and AST are hidden. */
const NO_GROWTH_PARENT_ID = '428';

/** SpecimenGrowth list 133 leaf ids under No Growth parent 428 — match by id, not label. */
const NO_GROWTH_LEAF_IDS = new Set(['177', '1050', '1086', '125']);

/**
 * Returns true when the culture/isolate has no growth (Add Isolate and AST menu items are hidden).
 * Uses {@code growthTypeParentId} when present; falls back to {@code growthid} leaf ids when the
 * parent lookup is missing from the culture list query.
 *
 * @param {Object} item - Culture list item (GrowthTypeParentId and/or growthid from culture list query)
 * @returns {boolean} True when the culture should be treated as no-growth for menu filtering.
 */
const isNoGrowth = (item) => {
    if (!item) return false;

    const parentIdRaw = item.GrowthTypeParentId ?? item.growthTypeParentId;
    const parentId = parentIdRaw != null && parentIdRaw !== '' ? String(parentIdRaw).trim() : '';

    if (parentId === GROWTH_PARENT_ID) {
        return false;
    }
    if (parentId === NO_GROWTH_PARENT_ID) {
        return true;
    }
    if (parentId !== '') {
        return false;
    }

    const growthIdRaw = item.growthid ?? item.GrowthId;
    const growthId = growthIdRaw != null && growthIdRaw !== '' ? String(growthIdRaw).trim() : '';
    if (!growthId) {
        return true;
    }
    if (NO_GROWTH_LEAF_IDS.has(growthId)) {
        return true;
    }

    return false;
};

/**
 * Recursively filters nested context menu items using the same bespoke rules as top-level items.
 * @param {Array<Object>} menuItems - Menu items (may include subMenuProps).
 * @param {Object} item - Current list row.
 * @param {Object|undefined} options - Optional context (e.g. forms).
 * @returns {Array<Object>} Filtered menu tree.
 */
const filterBespokeMenuItemsRecursive = (menuItems, item, options) => {
    if (!Array.isArray(menuItems)) {
        return menuItems;
    }

    let filtered = menuItems;

    if (isNoGrowth(item)) {
        filtered = filtered.filter((m) => {
            const k = (m.key || m.Key || '').toLowerCase();
            return k !== 'addisolate' && k !== 'ast';
        });
    }

    const specimenCount = Number(item?.specimencount ?? item?.SpecimenCount ?? 0);
    if (specimenCount > 0) {
        filtered = filtered.filter((m) => {
            const k = (m.key || m.Key || '').toLowerCase();
            return k !== 'delete';
        });
    }

    const requestCount = Number(item?.requestcount ?? item?.RequestCount ?? 0);
    if (requestCount > 0) {
        filtered = filtered.filter((m) => {
            const k = (m.key || m.Key || '').toLowerCase();
            return k !== 'delete';
        });
    }

    if (options?.forms && item?.TestName && filtered.some((m) => m.key === 'viewtest')) {
        const form = options.forms.filter((f) => f.Name === item.TestName);
        if (form.length === 0 || !form[0].InitialQuery) {
            filtered = filtered.filter((m) => m.key !== 'viewtest');
        }
    }

    return filtered.map((menuItem) => {
        if (menuItem.subMenuProps?.items?.length) {
            const nested = filterBespokeMenuItemsRecursive(menuItem.subMenuProps.items, item, options);
            return {
                ...menuItem,
                subMenuProps: {
                    ...menuItem.subMenuProps,
                    items: nested,
                },
            };
        }
        return menuItem;
    }).filter((menuItem) => {
        if (menuItem.subMenuProps?.items?.length === 0) {
            return false;
        }
        return true;
    });
};

/**
 * Filters menu items based on item-specific rules (e.g. hide View when form has no InitialQuery).
 * @param {Array} menuItems - Menu items to filter
 * @param {Object} item - The current row/item
 * @param {Object} [options] - Optional context (e.g. { forms } for test grids)
 * @returns {Array} Filtered menu items
 */
const BespokeMenuRemoval = (menuItems, item, options) => {
    return filterBespokeMenuItemsRecursive(menuItems, item, options);
}

export default GetVisibleButtons;
export {PutButtonsIntoPassiveMode, BespokeMenuRemoval}

