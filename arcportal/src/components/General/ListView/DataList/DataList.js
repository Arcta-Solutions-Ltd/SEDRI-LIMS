import React, { useState, useMemo, useCallback, useLayoutEffect, useRef } from 'react';
import { TooltipHost, TooltipOverflowMode, Selection, IconButton, ShimmeredDetailsList, ActionButton, DetailsListLayoutMode, ColumnActionsMode } from '@fluentui/react';
import './DataList.css';
import InlineMenu from '../../InlineMenu/InLineMenu';
import FilterMenusOnState, { FilterGroupMenuOnState } from '../../../../Utils/State/FilterMenusOnState';
import ArcCallout from '../../ArcCallout/ArcCallout';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import {BespokeMenuRemoval} from '../../../../Utils/Forms/GetVisibleButtons';
import { getStandardTooltipProps } from '../../../../Utils/General/StandardTooltipProps';
import wrapContextMenuItemsWithRowData from '../../../../Utils/Forms/wrapContextMenuItemsWithRowData';
import { buildInlineMenuItemId, buildCommandBarTestId } from '../../../../Utils/Forms/MapButtonsToContextMenu';
import buildTestGridRowMenuTestId from '../../../../Utils/Specimen/testGridRowMenuTestId';

/**
 * DataList renders a Fluent UI ShimmeredDetailsList with configurable columns.
 * Supports column resize, reorder, and customisation when onColumnLayoutChange is provided.
 *
 * @param {Object} props - Component props
 * @param {Array} props.columns - Column configs (Key, Name, FieldName, MinWidth, MaxWidth, calculatedWidth, ...)
 * @param {function} [props.onColumnLayoutChange] - Called when user resizes or reorders columns; receives { VisibleKeys, Order, Widths }
 * @param {Object} [props.columnLayout] - Current user layout for merging width/order updates
 * @param {string} [props.viewName] - View identity; included in column sync key and as React key on ShimmeredDetailsList to avoid Fluent DetailsList retaining widths by column key.
 * @param {string} [props.addId] - DOM id for the add icon on the menu column header. Only applied on
 *   the two icon header, where each icon needs to be addressable in its own right.
 * @param {function} [props.onSecondaryAdd] - Supplying this turns the menu column header into two
 *   icons, the standard add icon plus a second action, and moves click handling onto the icons
 *   themselves. Left undefined, the header keeps its single icon exactly as every other grid renders it.
 * @param {string} [props.secondaryAddId] - DOM id for the second header icon.
 * @param {string} [props.secondaryAddIcon] - Fluent icon name for the second header icon. Must not be
 *   "Add", so the two icons stay distinguishable by icon name.
 * @param {string} [props.secondaryAddTooltip] - Translated tooltip for the second header icon.
 * @param {string} [props.addButtonTooltip] - Language tag for the add (+) column-header icon tooltip.
 */
const DataList = (props) => {

    const resolveAddButtonTooltip = () => {
        const value = props.addButtonTooltip || '@ConAddJ@';
        if (value && !String(value).startsWith('@')) {
            return value;
        }
        const translated = TranslateTag(value, props.language);
        if (props.addButton && !translated) {
            // #region agent log
            console.log('DEBUG', {
                location: 'DataList.js',
                message: 'add button tooltip resolved to empty string',
                data: { tag: value, viewName: props.viewName, hasLanguage: Boolean(props.language) },
                timestamp: Date.now(),
            });
            // #endregion
        }
        return translated || value;
    };

    let selectionModel;
    const selectionChangeHandler = () => {
        props.selectionChanged(selectionModel);
    };

    const [showContextualMenu, updateShowContextualMenu] = useState({ visible: false, target: ""});
    const [renderCallout, setRenderCallout] = useState(null);
    const [renderEvent, setRenderEvent] = useState(null);
    const [triggerCallout, setTriggerCallout] = useState(false);
    
    if (props.multiSelect) {
        selectionModel = new Selection({selectionMode: 2, getKey: item => item.id ?? item.Id, onSelectionChanged: selectionChangeHandler});
    } else {
        selectionModel = new Selection({selectionMode: 1, getKey: item => item.id ?? item.Id, onSelectionChanged: selectionChangeHandler});
    }

    if (props.selectionModel !== undefined && props.selectionModel !== null) {
        selectionModel = props.selectionModel;
    }

    const calloutProps = { gapSpace: 10 };
    const tooltipProps = getStandardTooltipProps();
    let tooltipId = 1;

    const onColumnClick = (event, column) => {
        if (props.updateSortedColumn !== undefined) {
            props.updateSortedColumn(column.fieldName);
        }
    }

    const menuItemClicked = (onClick, button, index) => {
        const item = processedData ? processedData[index] : props.data?.[index];
        if (item && props.basicModeButtonHandler) {
            props.basicModeButtonHandler(item, button);
        } else if (item && button && props.onMenuButtonClick) {
            props.onMenuButtonClick(button, item);
        } else {
            onClick();
        }
    };

    const renderItemColumn = (item, index, column) => {
        const fieldContent = item[column.fieldName];
        switch (column.key) {
            case 'menu':
                const stateId = item?.stateid ?? item?.StateId;
                const specimenTypeId = item?.specimentypeid ?? item?.SpecimenTypeId;
                const laboratoryId = item?.laboratoryid ?? item?.LaboratoryId;
                let fullMenuItems = FilterMenusOnState(props.menuItems, stateId, specimenTypeId, laboratoryId, props.laboratoryConfig);
                fullMenuItems = BespokeMenuRemoval(fullMenuItems, item, props.forms ? { forms: props.forms } : undefined);
                const iconMenuItems = fullMenuItems.filter((menuItem) => {
                    return menuItem.primaryAction !== undefined && menuItem.primaryAction !== 0;
                }).slice(0, 3);
                const recordId = item?.id ?? item?.Id;
                const moreButtonId = props.viewName && recordId != null
                    ? `${props.viewName}-row-${recordId}-more`
                    : undefined;
                if (fullMenuItems.length > 0 ) {
                    return (
                        <div>
                            {iconMenuItems.map((menuItem) => {
                                if (menuItem.primaryAction !== undefined && menuItem.primaryAction !== 0) {
                                    return (
                                        <TooltipHost
                                            key={menuItem.key}
                                            content={menuItem.text}
                                            id={tooltipId++}
                                            tooltipProps={tooltipProps}
                                            calloutProps={calloutProps}>
                                            <IconButton
                                                id={props.viewName === 'exportschedules' && menuItem.key
                                                    ? `exportschedules-${menuItem.key}`
                                                    : (recordId != null && menuItem.key && props.viewName
                                                        ? `${props.viewName}-row-${recordId}-inline-menu-${menuItem.key}`
                                                        : buildInlineMenuItemId(props.viewName, menuItem.key))}
                                                data-testid={
                                                    buildTestGridRowMenuTestId(props.testRowMenuIdPrefix, recordId, menuItem.key)
                                                    ?? buildCommandBarTestId(menuItem.key)
                                                }
                                                iconProps={menuItem.iconProps}
                                                onClick={() => {menuItemClicked(menuItem.onClick, menuItem.button, index)}}
                                                styles={{
                                                    root: { height: '20px', verticalAlign: 'middle', textAlign: 'left' }
                                                }}
                                            />
                                        </TooltipHost>
                                    )
                                } else {
                                    return null;
                                }   
                            })}
                            <TooltipHost
                                 content={TranslateTag("@GenMor@", props.language)}
                                 id={tooltipId++}
                                 tooltipProps={tooltipProps}
                                 calloutProps={calloutProps}>
                                 <IconButton
                                      id={moreButtonId}
                                      data-testid={moreButtonId}
                                      iconProps={{ iconName: 'More' }}
                                      onClick={(event) => menuClickHandler(item, event, index)}
                                      styles={{
                                      root: { height: '20px', verticalAlign: 'middle' }
                                 }}
                            />
                            </TooltipHost>
                        </div>
                      );                    
                } else {
                    return (null);
                }
            case "turnaroundtime":
                const tatColour = item.TurnAroundTimeColour === undefined ? item.turnaroundtimecolour : item.TurnAroundTimeColour;
                if (tatColour) {
                    return (
                        <TooltipHost content="Turn Around Time" id={tooltipId++} tooltipProps={tooltipProps} calloutProps={calloutProps}>
                            <IconButton
                                iconProps={{ iconName: 'Clock' }}
                                ariaLabel="Turn Around Time"
                                styles={{
                                    root: { color: tatColour, height: '20px', width: '20px', verticalAlign: 'middle', textAlign: 'left' }
                                }}
                            />
                        </TooltipHost>
                    );
                }
                return null;
            case "alert":
                const alertTypeId = item.AlertCategoryId === undefined ? item.alertcategoryid : item.AlertCategoryId;
                const alertTitle = item.AlertTitle && item.AlertTitle !== "" ? item.AlertTitle : "Alert";
                const colour = item.Colour === undefined ? item.colour : item.Colour;
                if (alertTypeId !== 989 && alertTypeId !== 990 && alertTypeId !== 991) {
                    return (null)
                } else {
                    if (alertTypeId === 989) {
                        return (
                            <div>
                                <IconButton iconProps={{ iconName: 'AlertSolid'}} title={alertTitle} ariaLabel="Alert"
                                    styles={{
                                        root: { color: colour, height: '10px', width: '10px', verticalAlign: 'middle', textAlign: 'left' }
                                    }}>
                                </IconButton>
                            </div>
                        )                        
                    }
                    if (alertTypeId === 990) {
                        return (
                            <div>
                                <IconButton iconProps={{ iconName: 'WarningSolid' }} title={alertTitle} ariaLabel="Alert"
                                    styles={{
                                        root: { color: colour, height: '10px', width: '10px', verticalAlign: 'middle', textAlign: 'left' }
                                    }}>
                                </IconButton>
                            </div>
                        )                        
                    }
                    if (alertTypeId === 991) {
                        return (
                            <div>
                                <IconButton iconProps={{ iconName: 'InfoSolid' }} title={alertTitle} ariaLabel="Alert"
                                    styles={{
                                        root: { color: colour, height: '10px', width: '10px', verticalAlign: 'middle', textAlign: 'left' }
                                    }}>
                                </IconButton>
                            </div>
                        )                        
                    }
                }
                break;
            case 'validity':
                const isValid = item.isValid === true || item.IsValid === true;
                const validityIcon = isValid ? 'CheckMark' : 'Cancel';
                const validityColor = isValid ? '#107c10' : '#d13438';
                const validityTooltip = isValid ? 'Hash valid' : 'Hash invalid or not verified';
                return (
                    <TooltipHost content={validityTooltip} id={tooltipId++} tooltipProps={tooltipProps} calloutProps={calloutProps}>
                        <IconButton
                            iconProps={{ iconName: validityIcon }}
                            ariaLabel={validityTooltip}
                            styles={{
                                root: { color: validityColor, height: '16px', width: '16px', verticalAlign: 'middle', textAlign: 'left' }
                            }}
                        />
                    </TooltipHost>
                );
            default:
                return (
                    <TooltipHost id={item.key} content={fieldContent} overflowMode={TooltipOverflowMode.Parent}>
                        <span>{fieldContent}</span>
                    </TooltipHost>
                )
          }
    };

    const propsToFluentColumns = useCallback((cols) => {
        return cols.map((column) => {
            let minWidth = column.MinWidth;
            const maxWidth = column.MaxWidth;
            if (props.onColumnLayoutChange && column.IsResizable !== false) {
                minWidth = 50;
            }
            const fieldName = column.FieldName ?? column.fieldName;
            const isSortable = !!fieldName;
            const fluentCol = {
                     key: column.Key,
                     name: column.Name,
                     fieldName,
                     minWidth,
                     maxWidth,
                     isResizable: column.IsResizable !== false,
                     isCollapsible: column.IsCollapsible,
                     isSorted: isSortable ? column.IsSorted : false,
                     isSortedDescending: isSortable ? column.IsSortedDescending : false,
                     highlight: column.Highlight,
                     onColumnClick: isSortable ? onColumnClick : undefined
            };
            if (column.calculatedWidth !== undefined) {
                fluentCol.calculatedWidth = column.calculatedWidth;
            }
            return fluentCol;
        });
    }, [props.onColumnLayoutChange]);

    const [columns, setColumns] = useState(() => propsToFluentColumns(props.columns));
    const columnsKeySetRef = useRef(null);
    useLayoutEffect(() => {
        const keySet = props.columns.map((c) => (c.Key || c.key)).sort().join(',');
        const sortStateKey = props.columns.map((c) => `${c.FieldName ?? c.fieldName}:${c.IsSorted ?? c.isSorted}:${c.IsSortedDescending ?? c.isSortedDescending}`).join('|');
        const viewKey = props.viewName != null ? String(props.viewName) : '';
        const widthSignature = [...props.columns]
            .map((c) => {
                const k = String(c.Key || c.key || '');
                const w = c.calculatedWidth;
                return `${k}:${w === undefined ? '' : w}`;
            })
            .sort()
            .join('|');
        const fullKey = `${viewKey}|${keySet}|${sortStateKey}|${widthSignature}`;
        if (columnsKeySetRef.current !== fullKey) {
            columnsKeySetRef.current = fullKey;
            setColumns(propsToFluentColumns(props.columns));
        }
    });

    const handleColumnResize = useCallback((resizedColumn, newWidth) => {
        if (!props.onColumnLayoutChange || !resizedColumn?.key || typeof newWidth !== 'number') return;
        setColumns((prev) => prev.map((c) =>
            c.key === resizedColumn.key ? { ...c, calculatedWidth: newWidth } : c
        ));
        const currentWidths = (props.columnLayout?.Widths || props.columnLayout?.widths) || {};
        const newWidths = { ...currentWidths, [resizedColumn.key]: newWidth };
        props.onColumnLayoutChange({
            Widths: newWidths,
            VisibleKeys: props.columnLayout?.VisibleKeys || props.columnLayout?.visibleKeys,
            Order: props.columnLayout?.Order || props.columnLayout?.order,
        });
    }, [props.onColumnLayoutChange, props.columnLayout]);

    const handleColumnReorder = useCallback((draggedIndex, targetIndex) => {
        if (!props.onColumnLayoutChange || draggedIndex === targetIndex) return;
        setColumns((prev) => {
            const newCols = [...prev];
            const [dragged] = newCols.splice(draggedIndex, 1);
            newCols.splice(targetIndex, 0, dragged);
            const newOrder = newCols.map((c) => c.key);
            props.onColumnLayoutChange({
                Order: newOrder,
                VisibleKeys: props.columnLayout?.VisibleKeys || props.columnLayout?.visibleKeys,
                Widths: props.columnLayout?.Widths || props.columnLayout?.widths,
            });
            return newCols;
        });
    }, [props.onColumnLayoutChange, props.columnLayout]);

    const columnReorderOptions = props.onColumnLayoutChange ? {
        frozenColumnCountFromStart: 0,
        frozenColumnCountFromEnd: 0,
        handleColumnReorder: handleColumnReorder,
    } : undefined;

    const detailsListKey = props.viewName != null && String(props.viewName) !== ''
        ? String(props.viewName)
        : 'datalist-default';

    /**
     * Opens the row overflow context menu with workflow-filtered items bound to the selected row.
     */
    const menuClickHandler = (item, event, index) => {
        const stateId = item.stateid ?? item.StateId;
        const specimenTypeId = item.specimentypeid ?? item.SpecimenTypeId;
        const laboratoryId = item?.laboratoryid ?? item?.LaboratoryId;
        let menuItems = FilterMenusOnState(props.menuItems, stateId, specimenTypeId, laboratoryId, props.laboratoryConfig);
        menuItems = BespokeMenuRemoval(menuItems, item, props.forms ? { forms: props.forms } : undefined);
        const dataItem = processedData ? processedData[index] : props.data?.[index];
        if (dataItem && props.basicModeButtonHandler) {
            menuItems = wrapContextMenuItemsWithRowData(menuItems, (button) => {
                props.basicModeButtonHandler(dataItem, button);
            });
        }
        updateShowContextualMenu({ visible: true, target: event.target, menuItems: menuItems});
    };

    const closeMenuHandler = () => {
        updateShowContextualMenu({ visible: false, target: ""});
    };

    const itemInvokedHandler = (item, index, event) => {
        if (!props.basic) {
            props.itemInvokedHandler();
        }
    }

    const onRenderRow = (rowProps, defaultRender) => {
        const row = defaultRender(rowProps);
        const item = rowProps.item;
        const rid = item?.id ?? item?.Id;
        const inner = rid != null ? (
            <div data-list-row-id={String(rid)} style={{ display: 'contents' }}>
                {row}
            </div>
        ) : row;

        if (props.displaySummary) {
            return (
                <div
                    onMouseEnter={(event) => {
                        setRenderCallout(rowProps.item);
                        setRenderEvent(event.currentTarget);
                        setTriggerCallout(true);
                    }}
                    onMouseLeave={() => { setTriggerCallout(false); }}
                >
                    {inner}
                </div>
            );
        }
        return inner;
    };

        // Helper function to format group headers using props.groupText
    const formatGroupHeader = (groupValue, sampleItem) => {
        if (!props.groupText) {
            return groupValue;
        }

        // Replace @FieldName@ patterns with actual values from the sample item
        let result = props.groupText.replace(/@(\w+)@/g, (match, fieldName) => {
            return sampleItem[fieldName] || '';
        });

        // Clean up separators that appear on their own when followed by empty fields
        // Remove patterns like " - " or " : " when they appear alone
        result = result.replace(/\s*[-:]\s*$/g, ''); // Remove trailing separators
        result = result.replace(/^\s*[-:]\s*/g, ''); // Remove leading separators
        result = result.replace(/\s*[-:]\s*[-:]/g, ' - '); // Handle multiple consecutive separators
        
        // Remove separators that are followed only by spaces (orphaned separators)
        result = result.replace(/\s*[-:]\s+(?=\s*$)/g, ''); // Remove separator followed only by spaces at end
        result = result.replace(/\s*[-:]\s+(?=\s*[-:])/g, ' '); // Remove separator followed by spaces and another separator
        
        result = result.replace(/\s+\s+/g, ' '); // Clean up multiple spaces
        result = result.trim(); // Remove leading/trailing whitespace

        return result;
    };

    // Process data for grouping if groupBy is specified
    const { processedData, groups } = useMemo(() => {
        if (!props.groupBy || !props.data || props.data.length === 0) {
            return { processedData: props.data, groups: undefined };
        }

        // Create a map to group items by the specified field
        const groupMap = new Map();
        const processedData = [...props.data];
        
        // Sort data by the group field to ensure items are grouped together
        processedData.sort((a, b) => {
            const aValue = a[props.groupBy] || '';
            const bValue = b[props.groupBy] || '';
            return aValue.localeCompare(bValue);
        });

        // Group items and calculate start indices and counts
        let currentGroup = null;
        let startIndex = 0;
        let count = 0;
        const groups = [];

        // Handle the first item specially to ensure the first group is created
        if (processedData.length > 0) {
            currentGroup = processedData[0][props.groupBy] || '';
            startIndex = 0;
            count = 1;
        }

        // Process remaining items
        for (let index = 1; index < processedData.length; index++) {
            const item = processedData[index];
            const groupValue = item[props.groupBy] || '';
            
            if (currentGroup !== groupValue) {
                // Save current group
                groups.push({
                    key: `group${currentGroup}${startIndex}`,
                    name: formatGroupHeader(currentGroup, processedData[startIndex]),
                    startIndex: startIndex,
                    count: count,
                    level: 0
                });
                
                // Start new group
                currentGroup = groupValue;
                startIndex = index;
                count = 1;
            } else {
                count++;
            }
        }

        // Add the last group (this will also handle the case where there's only one group)
        if (currentGroup !== null) {
            groups.push({
                key: `group${currentGroup}${startIndex}`,
                name: formatGroupHeader(currentGroup, processedData[startIndex]),
                startIndex: startIndex,
                count: count,
                level: 0
            });
        }

        return { processedData, groups };
    }, [props.data, props.groupBy, props.groupText]);



    //Put in place menu on the menu column if required

    const onColumnHeaderClick = (event, column) => {
        // The two icon header wires each icon up individually, so a header wide handler here would
        // fire the add action again whichever icon was pressed.
        if (props.onSecondaryAdd) {
            return;
        }
        const key = column?.key != null ? String(column.key).toLowerCase() : '';
        const isAddHeaderColumn = key === 'menu' || column?.id === 'headermenu' || column?.id === 'exportschedules-add';
        if (isAddHeaderColumn && props.onClick && props.addButton) {
            props.onClick({ UIEvent: props.addButton, OnFinish: 'embeddedrefresh' }, { id: props.parentId });
        }
    };

    /**
     * Custom renderer for Fluent UI DetailsList group headers.
     *
     * Filters the {@code groupMenu} items through two stages before rendering:
     * 1. {@link BespokeMenuRemoval} — removes growth-type-specific actions (e.g. hides
     *    Add Isolate and AST buttons when the culture row represents no-growth).
     * 2. {@link FilterGroupMenuOnState} — hides any workflow-controlled action whose
     *    required entry states do not include the current specimen workflow state. This
     *    prevents Add Isolate, Edit Culture, and Delete Culture from appearing after a
     *    specimen has been submitted (state 530 / 531 / 534 etc.).
     *
     * The specimen state, specimen type, and laboratory are read from {@code sampleItem}
     * (the first data row belonging to the group). These values are injected into every
     * culture row by {@code ManageListEmbedded.dataReceivedHandler} so that the correct
     * parent-specimen context is available here without additional props.
     *
     * @param {Object} headerProps - The Fluent UI group header render props supplied by
     *   {@code ShimmeredDetailsList}.
     * @param {Function} defaultRender - The Fluent UI default group header render function.
     * @returns {React.ReactElement|null} The rendered group header, or {@code null} when
     *   {@code headerProps} or {@code defaultRender} are not available.
     */
    const onRenderGroupHeader = (headerProps, defaultRender) => {
        if (!headerProps || !defaultRender) return null;
        const { group } = headerProps;
        const sampleItem = processedData && group ? processedData[group.startIndex] : null;

        // Stage 1: bespoke growth-type filtering (e.g. no-growth hides isolate/AST actions)
        let filteredGroupMenu = BespokeMenuRemoval(
            props.groupMenu || [],
            sampleItem,
            props.forms ? { forms: props.forms } : undefined
        );

        // Stage 2: workflow state filtering — resolves specimen state from the injected
        // fields on sampleItem and removes any group action not permitted at this state.
        const stateId        = sampleItem?.stateid        ?? sampleItem?.StateId;
        const specimenTypeId = sampleItem?.specimentypeid ?? sampleItem?.SpecimenTypeId;
        const laboratoryId   = sampleItem?.laboratoryid   ?? sampleItem?.LaboratoryId;
        filteredGroupMenu = FilterGroupMenuOnState(
            filteredGroupMenu, stateId, specimenTypeId, laboratoryId, props.laboratoryConfig
        );

        const renderTitle = () => (
            <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', width: '100%', gap: 8 }}>
                <span>{group?.name}</span>
                {filteredGroupMenu && Array.isArray(filteredGroupMenu) && filteredGroupMenu.length > 0 && (
                    <div style={{ display: 'flex', gap: 8, flexWrap: 'wrap', justifyContent: 'flex-end' }} onClick={(e) => e.stopPropagation()}>
                        {filteredGroupMenu.map((menu, index) => {
                            const iconName = menu.icon || menu.Icon;
                            const text = menu.text || menu.Text;
                            const uiEvent = menu.uievent || menu.UIEvent;
                            const key = menu.key || menu.Key || uiEvent || text;
                            const tooltipId = `group-menu-tooltip-${group?.key || 'unknown'}-${index}`;
                            const tooltipText = text;

                            return (
                            <TooltipHost
                                key={key}
                                content={tooltipText}
                                id={tooltipId}
                                tooltipProps={tooltipProps}
                                calloutProps={calloutProps}
                                delay={0}>
                                <span title={tooltipText} style={{ display: 'inline-block' }}>
                                    <IconButton
                                        id={`groupmenu-${String(uiEvent || key || 'action').toLowerCase()}`}
                                        iconProps={{ iconName: iconName }}
                                        ariaLabel={tooltipText}
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            if (props.onClick) {
                                                props.onClick({ UIEvent: uiEvent, OnFinish: 'embeddedrefresh' }, sampleItem);
                                            }
                                        }}
                                        styles={{
                                            root: { 
                                                height: 28, 
                                                background: 'transparent', 
                                                border: 'none', 
                                                color: '#106ebe'
                                            },
                                            rootHovered: {
                                                background: 'transparent',
                                                color: '#005a9e'
                                            },
                                            icon: { color: '#106ebe' },
                                            iconHovered: { color: '#005a9e' }
                                        }}
                                    />
                                </span>
                            </TooltipHost>);
                        })}
                    </div>
                )}
            </div>
        );

        return defaultRender({ ...headerProps, onRenderTitle: renderTitle });
    };

    const addHeaderId = props.viewName === 'exportschedules' ? 'exportschedules-add' : 'headermenu';

    const raiseAddClick = () => {
        if (props.onClick && props.addButton) {
            props.onClick({ UIEvent: props.addButton, OnFinish: 'embeddedrefresh' }, { id: props.parentId });
        }
    };

    /**
     * Renders the menu column header as two icons, the standard add icon plus the caller's second
     * action. Used only when onSecondaryAdd is supplied.
     *
     * Both icons are drawn here rather than letting Fluent draw the first from column.iconName,
     * because each needs its own id, tooltip and handler. Clicks are stopped from bubbling to the
     * column header, which would otherwise raise the add action whichever icon was pressed.
     * @returns {JSX.Element} The header content.
     */
    const renderTwoIconHeader = () => (
        <div id={addHeaderId} className="datalist-addheader">
            <TooltipHost
                content={resolveAddButtonTooltip()}
                tooltipProps={tooltipProps}
                calloutProps={calloutProps}
            >
                <IconButton
                    id={props.addId}
                    iconProps={{ iconName: 'Add' }}
                    className="datalist-addheader-btn"
                    onClick={(event) => {
                        event.stopPropagation();
                        raiseAddClick();
                    }}
                />
            </TooltipHost>
            <TooltipHost
                content={props.secondaryAddTooltip}
                tooltipProps={tooltipProps}
                calloutProps={calloutProps}
            >
                <IconButton
                    id={props.secondaryAddId}
                    iconProps={{ iconName: props.secondaryAddIcon }}
                    className="datalist-addheader-btn"
                    onClick={(event) => {
                        event.stopPropagation();
                        props.onSecondaryAdd();
                    }}
                />
            </TooltipHost>
        </div>
    );

    const renderSingleAddHeader = () => {
        const addTooltip = resolveAddButtonTooltip();
        return (
        <div
            id={addHeaderId}
            className="datalist-addheader"
            data-testid={props.viewName ? `${props.viewName}-add-header` : undefined}
        >
            <TooltipHost
                content={addTooltip}
                tooltipProps={tooltipProps}
                calloutProps={calloutProps}
                delay={0}
            >
                <span title={addTooltip} style={{ display: 'inline-block' }}>
                    <IconButton
                        id={props.addId || (props.viewName ? `${props.viewName}-add-header-btn` : addHeaderId)}
                        data-testid={props.viewName ? `${props.viewName}-add-header-btn` : undefined}
                        iconProps={{ iconName: 'Add' }}
                        className="datalist-addheader-btn datalist-menuicon"
                        ariaLabel={addTooltip}
                        onClick={(event) => {
                            event.stopPropagation();
                            raiseAddClick();
                        }}
                    />
                </span>
            </TooltipHost>
        </div>
        );
    };

    const columnsToRender = props.addButton ? columns.map((col) =>
        col.key?.toLowerCase() === "menu"
            ? props.onSecondaryAdd
                ? {
                    ...col,
                    isIconOnly: true,
                    // The icons are the only interactive elements, so the header cell itself must not
                    // also present as a button.
                    columnActionsMode: ColumnActionsMode.disabled,
                    onRenderHeader: renderTwoIconHeader,
                }
                : {
                    ...col,
                    isIconOnly: true,
                    columnActionsMode: ColumnActionsMode.disabled,
                    onRenderHeader: renderSingleAddHeader,
                }
            : col
    ) : columns;

    // Column header tooltips: stable column key ids on headers; Add-column tooltip when addButton is set.
    const onRenderDetailsHeader = (headerProps, defaultRender) => {
        const customOnRenderColumnHeaderTooltip = (tooltipHostProps) => {
            if (props.addButton && (tooltipHostProps.column?.id === 'headermenu' || tooltipHostProps.column?.id === 'exportschedules-add')) {
                return (
                    <TooltipHost
                        content={resolveAddButtonTooltip()}
                        id={tooltipHostProps.id}
                        hostClassName={tooltipHostProps.hostClassName}
                        componentRef={tooltipHostProps.componentRef}
                        tooltipProps={tooltipProps}
                        calloutProps={calloutProps}>
                        {tooltipHostProps.children}
                    </TooltipHost>
                );
            }
            const columnKey = tooltipHostProps.column?.key;
            return (
                <span id={columnKey || undefined} className={tooltipHostProps.hostClassName}>
                    {tooltipHostProps.children}
                </span>
            );
        };
        return defaultRender({
            ...headerProps,
            onRenderColumnHeaderTooltip: customOnRenderColumnHeaderTooltip
        });
    };

    return (
        <div
            className="datalist-content"
            data-testid={props.viewName ? `manage-list-${props.viewName}` : 'manage-list'}
        >
            <ShimmeredDetailsList
                key={detailsListKey}
                items={processedData}
                columns={columnsToRender}
                groups={groups}
                groupProps={{ onRenderHeader: onRenderGroupHeader }}
                onColumnHeaderClick={onColumnHeaderClick}
                onRenderDetailsHeader={onRenderDetailsHeader}
                onRenderItemColumn={renderItemColumn}
                onRenderRow={onRenderRow}
                selectionMode={props.basic ? 0 : props.multiSelect === true ? 2 : 1}
                selection={selectionModel}
                selectionPreservedOnEmptyClick={true}
                enableShimmer={!props.isDataLoaded}
                compact={true}
                onItemInvoked={itemInvokedHandler}
                layoutMode={DetailsListLayoutMode.fixedColumns}
                onColumnResize={props.onColumnLayoutChange ? handleColumnResize : undefined}
                columnReorderOptions={columnReorderOptions}
            />
            <InlineMenu
                showMenu={showContextualMenu.visible}
                closeMenu={closeMenuHandler}
                target={showContextualMenu.target}
                items={showContextualMenu.menuItems}>
            </InlineMenu>

            {renderCallout !== null && (
                <ArcCallout
                    event={renderEvent}
                    item={renderCallout}
                    trigger={triggerCallout}
                    calloutLazyLoad={props.calloutLazyLoad}
                    calloutParentType={props.calloutParentType}
                />
            )}
        </div>
        );

};

export default DataList;
