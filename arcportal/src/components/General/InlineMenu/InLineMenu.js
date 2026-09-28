import React from 'react';
import { ContextualMenu } from '@fluentui/react';

/**
 * Renders a Fluent UI {@code ContextualMenu} anchored to a list-row "More" button.
 * Menu item {@code id} values are supplied by {@code MapButtonsToContextMenu}.
 *
 * @param {Object} props
 * @param {boolean} props.showMenu - When true the menu is visible.
 * @param {Function} props.closeMenu - Dismiss handler.
 * @param {Array<Object>} props.items - Context menu items (may include nested {@code subMenuProps}).
 * @param {Object} props.target - DOM target for menu positioning.
 */
const InlineMenu = (props) => {
    return (
        <div>
            {props.showMenu && (
                <ContextualMenu
                    setInitialFocus
                    onDismiss={props.closeMenu}
                    items={props.items}
                    shouldFocusOnMount={true}
                    target={props.target}
                ></ContextualMenu>
            )}
        </div>
    );
};

export default InlineMenu;
