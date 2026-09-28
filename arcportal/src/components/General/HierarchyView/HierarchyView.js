import React, { useState, useMemo } from 'react';
import { Icon, IconButton, ContextualMenu, Shimmer, getTheme, TooltipHost } from '@fluentui/react';
import buildTreeFromFlatData from '../../../Utils/General/buildTreeFromFlatData';
import FilterMenusOnState from '../../../Utils/State/FilterMenusOnState';
import { BespokeMenuRemoval } from '../../../Utils/Forms/GetVisibleButtons';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import InlineMenu from '../InlineMenu/InLineMenu';
import './HierarchyView.css';

/**
 * Renders a hierarchical tree of items with expand/collapse, selection, and context menu.
 * Columns are driven by list view config; columns with HideInHierarchy are excluded.
 * Uses a table layout so each value aligns under its header.
 *
 * @param {Object} props - Component props.
 * @param {Array} props.data - Flat array of items with id and parent id fields.
 * @param {string} props.idField - Field name for item id (e.g. 'id', 'Id').
 * @param {string} props.parentIdField - Field name for parent id (e.g. 'parentorganisationid').
 * @param {Array} props.columns - Column configs: FieldName, Name, MinWidth, MaxWidth, HideInHierarchy.
 * @param {Array} props.menuItems - Context menu items (from MapButtonsToContextMenu).
 * @param {function} props.routeAction - (button, item) => void - invoked when menu action is clicked.
 * @param {function} props.onItemSelected - (item, selected) => void - selection callback.
 * @param {Object} props.selectedItem - Currently selected item (for highlight).
 * @param {boolean} props.isDataLoaded - When false, shows shimmer.
 * @param {Object} props.laboratoryConfig - Lab config for FilterMenusOnState.
 * @param {Object} props.language - Language config for TranslateTag (column names use @Tag@ format).
 */
const HierarchyView = (props) => {
    const [expandedIds, setExpandedIds] = useState(new Set());
    const [contextMenu, setContextMenu] = useState({ visible: false, target: null, item: null });
    const [showInlineMenu, setShowInlineMenu] = useState({ visible: false, target: null, menuItems: [] });

    const treeData = useMemo(() => {
        const idField = props.idField || 'id';
        const parentIdField = props.parentIdField || 'parentorganisationid';
        return buildTreeFromFlatData(props.data || [], idField, parentIdField);
    }, [props.data, props.idField, props.parentIdField]);

    const getItemId = (item) => {
        return item?.id ?? item?.Id ?? item?.key ?? item?.Key;
    };

    const isSelected = (item) => {
        const selected = props.selectedItem;
        if (!selected) return false;
        return getItemId(selected) === getItemId(item);
    };

    const toggleExpand = (id) => {
        setExpandedIds(prev => {
            const next = new Set(prev);
            if (next.has(id)) next.delete(id);
            else next.add(id);
            return next;
        });
    };

    const getMenuItemsForItem = (item) => {
        const menuItems = props.menuItems || [];
        const stateId = item?.stateid ?? item?.StateId;
        const specimenTypeId = item?.specimentypeid ?? item?.SpecimenTypeId;
        const laboratoryId = item?.laboratoryid ?? item?.LaboratoryId;
        let filtered = FilterMenusOnState(menuItems, stateId, specimenTypeId, laboratoryId, props.laboratoryConfig);
        filtered = BespokeMenuRemoval(filtered, item);
        filtered = filtered.filter(m => m.primaryAction !== undefined && m.primaryAction !== 0);
        return filtered.slice(0, 3).map(mi => ({
            ...mi,
            onClick: () => {
                props.routeAction(mi.button, item);
                setContextMenu({ visible: false, target: null, item: null });
            }
        }));
    };

    const getFullMenuItemsForItem = (item) => {
        const menuItems = props.menuItems || [];
        const stateId = item?.stateid ?? item?.StateId;
        const specimenTypeId = item?.specimentypeid ?? item?.SpecimenTypeId;
        const laboratoryId = item?.laboratoryid ?? item?.LaboratoryId;
        let filtered = FilterMenusOnState(menuItems, stateId, specimenTypeId, laboratoryId, props.laboratoryConfig);
        filtered = BespokeMenuRemoval(filtered, item);
        return filtered.map(mi => ({
            ...mi,
            onClick: () => {
                props.routeAction(mi.button, item);
                setShowInlineMenu({ visible: false, target: null, menuItems: [] });
            }
        }));
    };

    const openInlineMenu = (e, item) => {
        e.stopPropagation();
        props.onItemSelected?.(item, true);
        const menuItems = getFullMenuItemsForItem(item);
        setShowInlineMenu({ visible: true, target: e.target, menuItems });
    };

    const closeInlineMenu = () => {
        setShowInlineMenu({ visible: false, target: null, menuItems: [] });
    };

    const handleContextMenu = (e, item) => {
        e.preventDefault();
        e.stopPropagation();
        props.onItemSelected?.(item, true);
        setContextMenu({ visible: true, target: e.target, item });
    };

    const handleNodeClick = (item) => {
        props.onItemSelected?.(item, true);
    };

    const getFieldValue = (item, fieldName) => {
        if (!item) return '';
        const key = Object.keys(item).find(k => k.toLowerCase() === (fieldName || '').toLowerCase());
        return key ? (item[key] ?? '') : '';
    };

    const displayColumns = useMemo(() => {
        return (props.columns || []).filter(
            c => !(c.HideInHierarchy === true) && ((c.Key && String(c.Key).toLowerCase() === 'menu') || (c.FieldName || c.fieldName))
        );
    }, [props.columns]);

    const theme = getTheme();
    const rowTextStyle = {
        fontFamily: theme.fonts.small.fontFamily,
        fontSize: theme.fonts.small.fontSize,
        fontWeight: theme.fonts.small.fontWeight,
        color: theme.palette.neutralSecondary
    };

    const renderTreeNode = (node, depth = 0) => {
        const item = node.item;
        const id = getItemId(item);
        const hasChildren = node.children && node.children.length > 0;
        const isExpanded = expandedIds.has(id);
        const selected = isSelected(item);

        return (
            <React.Fragment key={id}>
                <tr
                    className={`hierarchy-node-row hierarchy-depth-${Math.min(depth, 9)} ${hasChildren ? 'hierarchy-has-children' : 'hierarchy-leaf'} ${selected ? 'hierarchy-node-selected' : ''}`}
                    style={{ ...rowTextStyle, ...(hasChildren && { fontWeight: 500 }) }}
                    onClick={() => handleNodeClick(item)}
                    onContextMenu={(e) => handleContextMenu(e, item)}
                    data-depth={depth}
                    data-testid={`hierarchy-row-${id}`}
                >
                    <td className="hierarchy-node-expand-cell" style={{ paddingLeft: `${depth * 24 + 8}px` }}>
                        {hasChildren ? (
                            <IconButton
                                iconProps={{ iconName: isExpanded ? 'ChevronDown' : 'ChevronRight' }}
                                onClick={(e) => { e.stopPropagation(); toggleExpand(id); }}
                                styles={{ root: { width: 24, height: 24 } }}
                                ariaLabel={isExpanded ? 'Collapse' : 'Expand'}
                                data-testid={`hierarchy-expand-${id}`}
                            />
                        ) : (
                            <span className="hierarchy-node-spacer" style={{ width: 24, display: 'inline-block' }} />
                        )}
                    </td>
                    {displayColumns.map(col => {
                        const colKey = col.Key || col.key;
                        const isMenuColumn = colKey && String(colKey).toLowerCase() === 'menu';
                        return (
                            <td
                                key={col.Key}
                                className={`hierarchy-node-cell ${isMenuColumn ? 'hierarchy-node-menu-cell' : ''}`}
                                style={{
                                    minWidth: col.MinWidth || 80,
                                    maxWidth: col.MaxWidth || 400
                                }}
                            >
                                {isMenuColumn ? (() => {
                                    const primaryItems = getMenuItemsForItem(item);
                                    const fullItems = getFullMenuItemsForItem(item);
                                    if (fullItems.length === 0) return null;
                                    return (
                                        <div className="hierarchy-menu-buttons" onClick={(e) => e.stopPropagation()} style={{ color: '#106ebe' }}>
                                            {primaryItems.map((mi) => {
                                                const iconName = mi.iconProps?.iconName || mi.button?.Icon || mi.button?.icon || 'Edit';
                                                const actionTestId = mi.key ? `commandbar-${mi.key}` : undefined;
                                                return (
                                                    <TooltipHost key={mi.key} content={mi.text} calloutProps={{ gapSpace: 10 }}>
                                                        <button
                                                            type="button"
                                                            className="hierarchy-menu-icon-button"
                                                            onClick={(e) => {
                                                                e.stopPropagation();
                                                                props.routeAction(mi.button, item);
                                                            }}
                                                            aria-label={mi.text}
                                                            data-testid={actionTestId}
                                                        >
                                                            <Icon iconName={iconName} />
                                                        </button>
                                                    </TooltipHost>
                                                );
                                            })}
                                            <TooltipHost content={TranslateTag('@GenMor@', props.language)} calloutProps={{ gapSpace: 10 }}>
                                                <button
                                                    type="button"
                                                    className="hierarchy-menu-icon-button"
                                                    onClick={(e) => openInlineMenu(e, item)}
                                                    aria-label={TranslateTag('@GenMor@', props.language)}
                                                    data-testid={props.viewName && id != null ? `${props.viewName}-row-${id}-more` : undefined}
                                                >
                                                    <Icon iconName="More" />
                                                </button>
                                            </TooltipHost>
                                        </div>
                                    );
                                })() : (
                                    getFieldValue(item, col.FieldName || col.fieldName)
                                )}
                            </td>
                        );
                    })}
                </tr>
                {hasChildren && isExpanded && (
                    node.children.map(child => renderTreeNode(child, depth + 1))
                )}
            </React.Fragment>
        );
    };

    if (!props.isDataLoaded) {
        return (
            <div className="hierarchy-view">
                <Shimmer />
            </div>
        );
    }

    const headerStyles = {
        background: theme.semanticColors.bodyBackground,
        borderBottom: `1px solid ${theme.semanticColors.bodyDivider}`,
        color: theme.semanticColors.bodyText,
        fontFamily: theme.fonts.small.fontFamily,
        fontSize: theme.fonts.small.fontSize,
        fontWeight: theme.fonts.small.fontWeight
    };

    return (
        <div className="hierarchy-view" data-testid={props.viewName ? `manage-list-${props.viewName}` : 'manage-list'}>
            <table className="hierarchy-table">
                <thead>
                    <tr className="hierarchy-header-row" style={headerStyles}>
                        <th className="hierarchy-header-expand-cell" />
                        {displayColumns.map(col => {
                            const colName = col.Name || col.name;
                            const translated = TranslateTag(colName, props.language);
                            return (
                                <th
                                    key={col.Key || col.key || colName || 'col'}
                                    className="hierarchy-header-cell"
                                    style={{
                                        minWidth: col.MinWidth || 80,
                                        maxWidth: col.MaxWidth || 400
                                    }}
                                >
                                    {translated || colName}
                                </th>
                            );
                        })}
                    </tr>
                </thead>
                <tbody>
                    {treeData.length === 0 ? (
                        <tr>
                            <td colSpan={displayColumns.length + 1} className="hierarchy-empty-cell">
                                No items to display
                            </td>
                        </tr>
                    ) : (
                        treeData.map(node => renderTreeNode(node))
                    )}
                </tbody>
            </table>
            {contextMenu.visible && contextMenu.target && (
                <ContextualMenu
                    target={contextMenu.target}
                    items={getMenuItemsForItem(contextMenu.item)}
                    onDismiss={() => setContextMenu({ visible: false, target: null, item: null })}
                />
            )}
            <InlineMenu
                showMenu={showInlineMenu.visible}
                closeMenu={closeInlineMenu}
                target={showInlineMenu.target}
                items={showInlineMenu.menuItems}
            />
        </div>
    );
};

export default HierarchyView;
