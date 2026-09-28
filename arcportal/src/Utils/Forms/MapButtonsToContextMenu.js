/**
 * Builds a stable DOM id for an inline context-menu item from view name and button key.
 * @param {string|undefined} viewName - List view name (e.g. "patients").
 * @param {string|undefined} buttonKey - Button config key (e.g. "workflow1forpatient").
 * @returns {string|undefined} Id string or undefined when inputs are missing.
 */
const buildInlineMenuItemId = (viewName, buttonKey) => {
    if (!viewName || buttonKey === undefined || buttonKey === null || buttonKey === '') {
        return undefined;
    }
    return `${viewName}-inline-menu-${buttonKey}`;
};

/**
 * Builds a stable data-testid for list action buttons (matches TopBarMenu command bar convention).
 * @param {string|undefined} buttonKey - Button config key (e.g. "addorganisation").
 * @returns {string|undefined}
 */
const buildCommandBarTestId = (buttonKey) => {
    if (buttonKey === undefined || buttonKey === null || buttonKey === '') {
        return undefined;
    }
    return `commandbar-${buttonKey}`;
};

/**
 * Maps a single button config (and optional nested children) to a Fluent UI context-menu item.
 * @param {Object} button - Button config row (PascalCase or camelCase).
 * @param {Function} buttonClickHandler - Invoked with the button config when a leaf is clicked.
 * @param {string|undefined} viewName - List view name used for stable DOM ids.
 * @returns {Object} Context menu item descriptor.
 */
const mapButtonToContextMenuItem = (button, buttonClickHandler, viewName) => {
    const key = button.Key ?? button.key;
    const nestedButtons = button.Buttons ?? button.buttons;

    const commandBarTestId = buildCommandBarTestId(key);

    const baseItem = {
        key,
        id: buildInlineMenuItemId(viewName, key),
        text: button.Text ?? button.text,
        iconProps: { iconName: button.Icon ?? button.icon },
        primaryAction: button.PrimaryAction ?? button.primaryAction,
        button,
        entryStates: button.WorkflowEntryStates ?? button.EntryStates ?? button.entryStates,
        workflow: button.Workflow ?? button.workflow,
        itemProps: commandBarTestId ? { 'data-testid': commandBarTestId } : undefined,
    };

    if (Array.isArray(nestedButtons) && nestedButtons.length > 0) {
        const subItems = nestedButtons.map((child) =>
            mapButtonToContextMenuItem(child, buttonClickHandler, viewName)
        );
        return {
            ...baseItem,
            subMenuProps: { items: subItems },
        };
    }

    return {
        ...baseItem,
        onClick: () => buttonClickHandler(button),
    };
};

/**
 * Maps list-view button configs to Fluent UI {@code ContextualMenu} items, including one level
 * of nested {@code ButtonConfig.Buttons} as {@code subMenuProps} flyouts (same contract as TopbarMenu).
 *
 * @param {Array<Object>|undefined} buttons - OnSelect buttons from list view config.
 * @param {Function} buttonClickHandler - Called with the leaf button config on item click.
 * @param {string|undefined} [viewName] - View name for Cypress/DOM ids on menu items.
 * @returns {Array<Object>} Context menu item descriptors.
 */
const MapButtonsToContextMenu = (buttons, buttonClickHandler, viewName) => {
    if (buttons === undefined) {
        return [];
    }

    return buttons.map((button) => mapButtonToContextMenuItem(button, buttonClickHandler, viewName));
};

export default MapButtonsToContextMenu;
export { buildInlineMenuItemId, buildCommandBarTestId, mapButtonToContextMenuItem };
