import React, { useState } from 'react';
import './CardItem.css';
import { TooltipHost, IconButton } from '@fluentui/react';
import InlineMenu from '../../../InlineMenu/InLineMenu';
import FilterMenusOnState from '../../../../../Utils/State/FilterMenusOnState';
import { BespokeMenuRemoval } from '../../../../../Utils/Forms/GetVisibleButtons';
import TranslateTag from '../../../../../Utils/Local/TranslateTag';
import { getStandardTooltipProps } from '../../../../../Utils/General/StandardTooltipProps';
import wrapContextMenuItemsWithRowData from '../../../../../Utils/Forms/wrapContextMenuItemsWithRowData';
import { buildInlineMenuItemId, buildCommandBarTestId } from '../../../../../Utils/Forms/MapButtonsToContextMenu';

const CardItem = (props) => {
    const [showContextualMenu, updateShowContextualMenu] = useState({
        visible: false,
        target: '',
    });
    const [selected, setSelected] = useState(false);

    const calloutProps = { gapSpace: 10 };
    const tooltipProps = getStandardTooltipProps();
    let tooltipId = 1;

    let tatColour;
    if (props.turnaroundTimeFieldName && props.data) {
        const fn = props.turnaroundTimeFieldName;
        tatColour = props.data[fn];
        if (tatColour === undefined) {
            tatColour =
                props.data.TurnAroundTimeColour === undefined
                    ? props.data.turnaroundtimecolour
                    : props.data.TurnAroundTimeColour;
        }
    }

    const menuClickHandler = (event) => {
        updateShowContextualMenu({ visible: true, target: event.target });
    };

    const closeMenuHandler = () => {
        updateShowContextualMenu({ visible: false, target: '' });
    };

    const cardClicked = () => {
        props.itemSelected(props.cardId, props.data, !selected);
        setSelected(selected ? false : true);
    };

    const cardDoubleClicked = () => {
        if (typeof props.itemInvokedHandler === 'function') {
            props.itemInvokedHandler(props.data);
        }
    };

    if (selected && props.cardId !== props.selectedItem) {
        setSelected(false);
    }

    const laboratoryId = props.data?.laboratoryid ?? props.data?.LaboratoryId;
    let fullMenuItems = FilterMenusOnState(props.menuItems, props.data.stateid, props.data.specimentypeid, laboratoryId, props.laboratoryConfig);
    fullMenuItems = BespokeMenuRemoval(fullMenuItems, props.data, props.forms ? { forms: props.forms } : undefined);
    const iconMenuItems = fullMenuItems.filter((item) => {
        return item.primaryAction !== undefined && item.primaryAction !== 0;
    }).slice(0, 3);

    const menuItemsForContext = wrapContextMenuItemsWithRowData(
        fullMenuItems,
        (button) => {
            if (props.onMenuButtonClick) {
                props.onMenuButtonClick(button, props.data);
            }
        }
    );

    const recordId = props.data?.id ?? props.data?.Id;
    const recordIdAttr =
        recordId !== undefined && recordId !== null ? String(recordId) : undefined;
    const moreButtonId = props.viewName && recordId != null
        ? `${props.viewName}-row-${recordId}-more`
        : undefined;

    return (
        <React.Fragment>
            <div
                data-record-id={recordIdAttr}
                onClick={() => {
                    cardClicked();
                }}
                onDoubleClick={() => {
                    cardDoubleClicked();
                }}
                className={selected ? 'carditem-selected' : 'carditem-content'}
            >
                <div className="carditem-header-row">
                    <div className="carditem-header-left">
                        {props.turnaroundTimeFieldName && tatColour ? (
                            <TooltipHost
                                content={
                                    props.language
                                        ? TranslateTag('@GenTAT@', props.language)
                                        : 'Turn around time'
                                }
                                id={tooltipId++}
                                tooltipProps={tooltipProps}
                                calloutProps={calloutProps}
                            >
                                <IconButton
                                    iconProps={{ iconName: 'Clock' }}
                                    ariaLabel="Turn around time"
                                    onClick={(e) => e.stopPropagation()}
                                    styles={{
                                        root: {
                                            color: tatColour,
                                            height: '20px',
                                            width: '20px',
                                            verticalAlign: 'middle',
                                        },
                                    }}
                                />
                            </TooltipHost>
                        ) : null}
                    </div>
                    <div className="carditem-iconbar">
                        {iconMenuItems.map((item, index) => {
                            if (item.primaryAction !== undefined && item.primaryAction !== 0) {
                                return (
                                    <TooltipHost
                                        key={index}
                                        content={item.text}
                                        id={tooltipId++}
                                        calloutProps={calloutProps}
                                    >
                                        <IconButton
                                            id={buildInlineMenuItemId(props.viewName, item.key)}
                                            data-testid={buildCommandBarTestId(item.key)}
                                            iconProps={item.iconProps}
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                e.preventDefault();
                                                if (props.onMenuButtonClick) {
                                                    props.onMenuButtonClick(item.button, props.data);
                                                } else {
                                                    item.onClick();
                                                }
                                            }}
                                            styles={{
                                                root: {
                                                    height: '20px',
                                                    verticalAlign: 'middle',
                                                },
                                            }}
                                        />
                                    </TooltipHost>
                                );
                            } else {
                                return null;
                            }
                        })}
                        {fullMenuItems.length > 0 ? (
                        <TooltipHost
                            content="More"
                            id={tooltipId++}
                            calloutProps={calloutProps}
                        >
                            <IconButton
                                id={moreButtonId}
                                data-testid={moreButtonId}
                                iconProps={{ iconName: 'More' }}
                                onClick={(event) => {
                                    event.stopPropagation();
                                    menuClickHandler(event);
                                }}
                                styles={{
                                    root: {
                                        height: '20px',
                                        verticalAlign: 'middle',
                                    },
                                }}
                            />
                        </TooltipHost>
                        ) : null}
                    </div>
                </div>
                {props.keys.map((key, index) => {
                    let fieldName = null;
                    let fieldIndex = props.fieldNames.findIndex((fieldName) => {
                        return fieldName.key === key;
                    });
                    if (fieldIndex !== -1) {
                        fieldName = props.fieldNames[fieldIndex].name;
                        return (
                            <div key={index} className="carditem-field">
                                <div className="carditem-itemname">
                                    {fieldName}:
                                </div>
                                <TooltipHost
                                    content={String(props.data[key] ?? '')}
                                    id={`field-tooltip-${tooltipId++}`}
                                    calloutProps={calloutProps}
                                    styles={{ root: { flex: 1, minWidth: 0, overflow: 'hidden' } }}
                                >
                                    <div className="carditem-itemvalue">
                                        {props.data[key]}
                                    </div>
                                </TooltipHost>
                            </div>
                        );
                    } else {
                        return null;
                    }
                })}
            </div>
            <InlineMenu
                showMenu={showContextualMenu.visible}
                closeMenu={closeMenuHandler}
                target={showContextualMenu.target}
                items={menuItemsForContext}
            ></InlineMenu>
        </React.Fragment>
    );
};

export default CardItem;
