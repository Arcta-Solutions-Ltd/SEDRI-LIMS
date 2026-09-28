import GetEntryStatesForRecord from "./GetEntryStatesForRecord";

/**
 * Returns whether a menu item passes workflow state filtering for the current row.
 * @param {Object} item - Context menu item descriptor.
 * @param {string|number} state - Current workflow state id.
 * @param {string|number|undefined} specimenTypeId - Specimen type id for workflow lookup.
 * @param {string|number|undefined} laboratoryId - Laboratory id for workflow lookup.
 * @param {Object|undefined} laboratoryConfig - Laboratory configuration.
 * @returns {boolean} True when the item is permitted in the current state.
 */
const itemPassesWorkflow = (item, state, specimenTypeId, laboratoryId, laboratoryConfig) => {
    if (!item.workflow) {
        return true;
    }

    if (laboratoryId !== undefined) {
        const entryStatesToUse = GetEntryStatesForRecord(specimenTypeId, laboratoryId, laboratoryConfig, item) ?? "";
        return entryStatesToUse.split(",").map((s) => s.trim()).includes(state.toString());
    }

    const rawStates = item.entryStates;
    const workflowStates = rawStates?.[0]?.States !== undefined ? rawStates[0].States : rawStates;
    if (!workflowStates) {
        return false;
    }

    return workflowStates.split(",").map((s) => s.trim()).includes(state.toString());
};

/**
 * Returns true when the item is a leaf permitted for inline icon display or overflow.
 * @param {Object} item - Context menu item descriptor.
 * @returns {boolean}
 */
const itemHasPrimaryAction = (item) => item.primaryAction !== undefined && item.primaryAction !== 0;

/**
 * Returns true when the item is a grouping parent with nested flyout children.
 * @param {Object} item - Context menu item descriptor.
 * @returns {boolean}
 */
const itemHasSubMenu = (item) => (item.subMenuProps?.items?.length ?? 0) > 0;

/**
 * Recursively filters context menu items for workflow state and visibility.
 * Grouping parents (no primaryAction, with subMenuProps) are kept when at least one child remains.
 *
 * @param {Array<Object>} menus - Menu items from {@code MapButtonsToContextMenu}.
 * @param {string|number|undefined} state - Row workflow state id.
 * @param {string|number|undefined} specimenTypeId - Specimen type id.
 * @param {string|number|undefined} laboratoryId - Laboratory id.
 * @param {Object|undefined} laboratoryConfig - Laboratory config.
 * @returns {Array<Object>} Filtered menu tree.
 */
const filterContextMenuItems = (menus, state, specimenTypeId, laboratoryId, laboratoryConfig, isNested = false) => {
    if (!Array.isArray(menus)) {
        return [];
    }

    const result = [];

    for (const item of menus) {
        if (itemHasSubMenu(item)) {
            const filteredChildren = filterContextMenuItems(
                item.subMenuProps.items,
                state,
                specimenTypeId,
                laboratoryId,
                laboratoryConfig,
                true
            );

            if (filteredChildren.length === 0) {
                continue;
            }

            if (state !== undefined && item.workflow && !itemPassesWorkflow(item, state, specimenTypeId, laboratoryId, laboratoryConfig)) {
                continue;
            }

            result.push({
                ...item,
                subMenuProps: {
                    ...item.subMenuProps,
                    items: filteredChildren,
                },
            });
            continue;
        }

        if (state !== undefined) {
            if ((!isNested && !itemHasPrimaryAction(item)) || !itemPassesWorkflow(item, state, specimenTypeId, laboratoryId, laboratoryConfig)) {
                continue;
            }
        } else if (!isNested && !itemHasPrimaryAction(item)) {
            continue;
        }

        result.push(item);
    }

    return result;
};

/**
 * Filters row-level menu items based on the current specimen workflow state.
 *
 * Supports one level of nested {@code subMenuProps} from {@code MapButtonsToContextMenu}.
 * Leaf items at the top level require {@code primaryAction}; nested submenu leaves do not.
 *
 * @param {Array<Object>} menus - Menu item descriptors from {@code MapButtonsToContextMenu}.
 * @param {string|number|undefined} state - Current specimen {@code stateid}.
 * @param {string|number|undefined} specimenTypeId - Specimen type identifier.
 * @param {string|number|undefined} laboratoryId - Laboratory identifier.
 * @param {Object|undefined} laboratoryConfig - Laboratory configuration.
 * @returns {Array<Object>} Filtered menu tree.
 */
const FilterMenusOnState = (menus, state, specimenTypeId, laboratoryId, laboratoryConfig) => {
    return filterContextMenuItems(menus, state, specimenTypeId, laboratoryId, laboratoryConfig);
};

/**
 * Filters group-header menu items based on the current specimen workflow state.
 *
 * @param {Array<Object>} groupMenu - Group menu descriptors from {@code DataList}.
 * @param {string|number|undefined} state - Specimen {@code stateid} for the group.
 * @param {string|number|undefined} specimenTypeId - Specimen type identifier.
 * @param {string|number|undefined} laboratoryId - Laboratory identifier.
 * @param {Object|undefined} laboratoryConfig - Laboratory configuration.
 * @returns {Array<Object>} Filtered group menu items.
 */
const FilterGroupMenuOnState = (groupMenu, state, specimenTypeId, laboratoryId, laboratoryConfig) => {

    if (state === undefined || !Array.isArray(groupMenu)) {
        return groupMenu;
    }

    return groupMenu.filter(item => {
        const isWorkflowControlled = item.workflow ?? item.Workflow ?? false;

        if (!isWorkflowControlled) {
            return true;
        }

        if (laboratoryId !== undefined) {
            const entryStatesToUse = GetEntryStatesForRecord(specimenTypeId, laboratoryId, laboratoryConfig, item) ?? "";
            return entryStatesToUse.split(",").map(s => s.trim()).includes(state.toString());
        } else {
            const rawStates = item.entryStates ?? item.WorkflowEntryStates;
            if (!rawStates) return false;
            const statesString = Array.isArray(rawStates)
                ? (rawStates[0]?.States ?? "")
                : rawStates;
            return statesString.split(",").map(s => s.trim()).includes(state.toString());
        }
    });
};

export default FilterMenusOnState;

export { FilterGroupMenuOnState, filterContextMenuItems };
