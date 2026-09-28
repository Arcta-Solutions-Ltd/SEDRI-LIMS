import React from 'react';
import { connect } from 'react-redux';
import { CommandBar, CommandBarButton, TooltipHost } from '@fluentui/react';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { getStandardTooltipProps } from '../../../Utils/General/StandardTooltipProps';
import './TopBarMenu.css';

const commandBarStyles = {
    root: {
        margin: 0,
        border: 0,
        padding: 0,
        background: 'transparent',
        width: '100%',
    },
    primarySet: {
        flexWrap: 'nowrap',
    },
    menu: {
        link: {
            height: 36,
            lineHeight: 36,
        },
        linkContent: {
            height: 36,
            lineHeight: 36,
        },
    },
};

const compactCommandBarStyles = {
    root: {
        margin: 0,
        border: 0,
        padding: 0,
        background: 'transparent',
        width: 'auto',
        maxWidth: '100%',
    },
    primarySet: {
        flexWrap: 'nowrap',
        justifyContent: 'flex-start',
    },
    itemButton: {
        flex: '0 0 auto',
    },
    menu: {
        link: {
            height: 36,
            lineHeight: 36,
            flex: '0 0 auto',
        },
        linkContent: {
            height: 36,
            lineHeight: 36,
        },
    },
};

const embeddedButtonStyles = {
    root: { height: 40, lineHeight: 40, flex: '0 0 auto' },
    label: { lineHeight: 40 },
};

/**
 * Returns true when all configured buttons are leaf items with no nested submenus.
 * @param {Array<Object>} buttons - Mapped button config from the parent view.
 * @returns {boolean}
 */
const hasOnlyLeafButtons = (buttons) => {
    return buttons.every(
        (button) => !Array.isArray(button.buttons) || button.buttons.length === 0
    );
};

/**
 * Top command bar for list views, record views, and embedded record panels.
 * List views with inlineActions render leaf buttons inline for stable data-testid attributes.
 * Record views with nested submenus use Fluent CommandBar in compact mode so actions overflow
 * into a submenu instead of stretching to fill the bar width.
 *
 * @param {Object} props
 * @param {Array<Object>} props.buttons - Action buttons to render.
 * @param {boolean} [props.embeddedInForm=false] - When true and all items are leaves,
 *   renders inline CommandBarButtons without CommandBar overflow (used by ViewRecord in approval forms).
 * @param {boolean} [props.suppressFarItems=false] - Hides utility icons (filter, grid, fullscreen).
 *   When true with only leaf buttons, also uses inline CommandBarButtons so record-view actions
 *   expose stable data-testid attributes (Fluent UI CommandBar does not forward buttonProps).
 * @param {boolean} [props.inlineActions=false] - When true and all items are leaves, renders inline
 *   CommandBarButtons on manage lists so action buttons expose data-testid (Fluent UI CommandBar
 *   does not forward buttonProps to the DOM).
 * @param {Function} props.clickButton - Handler invoked when an action button is clicked.
 *
 * Icon-only utility buttons (filter, columns, grid toggle, fullscreen) are wrapped in TooltipHost
 * with getStandardTooltipProps() so hover tooltips match the standard list/grid action pattern.
 */
const TopbarMenu = (props) => {

    const tooltipProps = getStandardTooltipProps();
    const calloutProps = { gapSpace: 10 };

    /**
     * Wraps an icon-only utility button in a TooltipHost with standard styling.
     * @param {string} tooltipText - Translated tooltip content.
     * @param {React.ReactElement} button - The CommandBarButton to wrap.
     * @returns {React.ReactElement}
     */
    const wrapUtilityTooltip = (tooltipText, button) => (
        <TooltipHost content={tooltipText} tooltipProps={tooltipProps} calloutProps={calloutProps}>
            {button}
        </TooltipHost>
    );

    const selectedRecord = props.selectedRecord;
    const embeddedInForm = props.embeddedInForm === true;
    const suppressFarItems = props.suppressFarItems === true;
    const useEmbeddedLayout =
        hasOnlyLeafButtons(props.buttons) &&
        (embeddedInForm || suppressFarItems || props.inlineActions === true);
    const useCompactCommandBar = suppressFarItems && !useEmbeddedLayout;
    const noUtilities = suppressFarItems;

    const items = props.buttons.map((button) => {
        const hasSubMenu = Array.isArray(button.buttons) && button.buttons.length > 0;
        const testId = `commandbar-${button.key}`;

        if (hasSubMenu) {
            const subMenus = button.buttons.map((subMenuButton) => ({
                key: subMenuButton.key,
                id: subMenuButton.key,
                text: subMenuButton.text,
                iconProps: { iconName: subMenuButton.icon },
                onClick: () => props.clickButton(subMenuButton, selectedRecord),
                itemProps: {
                    id: subMenuButton.key,
                    'data-testid': `commandbar-${subMenuButton.key}`,
                },
            }));

            return {
                id: button.key,
                key: button.key,
                text: button.text,
                iconProps: { iconName: button.icon },
                subMenuProps: { items: subMenus },
                buttonProps: { 'data-testid': testId },
            };
        }

        return {
            id: button.key,
            key: button.key,
            text: button.text,
            iconProps: { iconName: button.icon },
            onClick: () => props.clickButton(button, selectedRecord),
            buttonProps: { 'data-testid': testId },
        };
    });

    const utilityButtons = [];

    if (!suppressFarItems) {
        if (props.showFilterIcon) {
            const filterAria = TranslateTag('@GenFil@', props.language);
            utilityButtons.push(
                wrapUtilityTooltip(filterAria,
                <CommandBarButton
                    key="filter"
                    data-testid="commandbar-filter"
                    iconOnly
                    iconProps={{ iconName: 'Filter' }}
                    ariaLabel={filterAria}
                    onClick={() => props.onFilterClick()}
                />)
            );
        }

        if (props.showColumnIcon && props.onColumnPickerClick) {
            const columnsAria = TranslateTag('@GenCol@', props.language);
            utilityButtons.push(
                wrapUtilityTooltip(columnsAria,
                <CommandBarButton
                    key="columns"
                    data-testid="commandbar-columns"
                    iconOnly
                    iconProps={{ iconName: 'ColumnOptions' }}
                    ariaLabel={columnsAria}
                    onClick={() => props.onColumnPickerClick()}
                />)
            );
        }

        if (props.showCardIcon && props.displayGridView) {
            const isCard = props.listType === 'card';
            const iconName = isCard ? 'Table' : 'Tiles';
            const gridAria = isCard ? TranslateTag('@GenTra@', props.language) : TranslateTag('@GenGri@', props.language);
            utilityButtons.push(
                wrapUtilityTooltip(gridAria,
                <CommandBarButton
                    key="tile"
                    id="commandbar-grid"
                    data-testid="commandbar-grid"
                    iconOnly
                    iconProps={{ iconName }}
                    ariaLabel={gridAria}
                    onClick={() => props.onListTypeClick()}
                />)
            );
        }

        const fullScreenAria = props.showFullScreen
            ? TranslateTag('@GenExiA@', props.language)
            : TranslateTag('@GenEnt@', props.language);
        const fullScreenIcon = props.showFullScreen ? 'BackToWindow' : 'FullScreen';
        utilityButtons.push(
            wrapUtilityTooltip(fullScreenAria,
            <CommandBarButton
                key="fullscreen"
                data-testid="commandbar-fullscreen"
                iconOnly
                iconProps={{ iconName: fullScreenIcon }}
                ariaLabel={fullScreenAria}
                onClick={() => props.onFullScreenClick()}
            />)
        );
    }

    let actionsClassName = 'topbar-menu-actions';
    if (useEmbeddedLayout) {
        actionsClassName = 'topbar-menu-actions topbar-menu-actions--embedded';
    } else if (useCompactCommandBar) {
        actionsClassName = 'topbar-menu-actions topbar-menu-actions--compact';
    }

    const rootClassName = noUtilities
        ? 'topbar-menu topbar-menu--no-utilities'
        : 'topbar-menu';

    const actionContent = useEmbeddedLayout ? (
        items.map((item) => (
            <CommandBarButton
                key={item.key}
                id={item.id}
                data-testid={`commandbar-${item.key}`}
                text={item.text}
                iconProps={item.iconProps}
                onClick={item.onClick}
                menuProps={item.subMenuProps}
                styles={embeddedButtonStyles}
            />
        ))
    ) : (
        <CommandBar
            items={items}
            styles={useCompactCommandBar ? compactCommandBarStyles : commandBarStyles}
            overflowButtonProps={{ 'data-testid': 'commandbar-overflow' }}
        />
    );

    return (
        <div className={rootClassName} data-testid="topbar-menu">
            <div className={actionsClassName} data-testid="topbar-menu-actions">
                {actionContent}
            </div>
            {utilityButtons.length > 0 && (
                <div className="topbar-menu-utilities" data-testid="topbar-menu-utilities">
                    {utilityButtons}
                </div>
            )}
        </div>
    );
};

const mapStateToProps = state => {
    return {
        showFullScreen: state.display.showFullScreen
    };
};

export default connect(mapStateToProps)(TopbarMenu);
