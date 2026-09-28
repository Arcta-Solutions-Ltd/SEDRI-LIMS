import React from 'react';
import { IconButton } from '@fluentui/react';
import buildTreeFromFlatData from '../../../Utils/General/buildTreeFromFlatData';
import './ArcHierarchyPicker.css';

/**
 * Renders a hierarchical tree for picker selection.
 * Uses buildTreeFromFlatData + expand/collapse. Multi-select: click adds (no checkboxes).
 * Single-select: click selects and highlights.
 * @param {Object} props
 * @param {string} [props.fieldId] - Parent field id; used to build stable DOM ids on each option row (`{fieldId}-option-{key}`).
 * @param {string} [props.idField='key'] - Field name for item id
 * @param {string} [props.parentIdField='ParentKey'] - Field name for parent id
 * @param {string} [props.textField='text'] - Field name for display text
 * @param {Set} [props.expandedIds] - Set of expanded node ids
 * @param {Function} props.onToggleExpand - Called when expand/collapse icon clicked
 * @param {Function} props.onSelect - Called when a selectable item is clicked
 * @param {Set|Array} [props.selectedKeys] - Currently selected keys (single-select only)
 * @param {boolean} [props.multiSelect=false] - Whether multiple selection is allowed
 * @param {boolean} [props.leavesOnly=false] - When true, only leaf nodes (items with no children) are selectable; parent rows remain expandable but not selectable
 * @param {Set} [props.parentIdsFromFullTree] - When leavesOnly is true, Set of IDs that have children in the full hierarchy; used to keep parents non-selectable even when search filters options
 * @param {boolean} [props.useContextSelection=false] - When true, single click highlights context for add-child; double click selects value
 * @param {string|number} [props.contextSelectedKey] - Highlighted node key for add-child context
 * @param {Function} [props.onContextSelect] - Called when a row is clicked for add-child context
 */
const TreePickerBody = (props) => {
    const {
        fieldId,
        options = [],
        idField = 'key',
        parentIdField = 'ParentKey',
        textField = 'text',
        expandedIds = new Set(),
        onToggleExpand,
        onSelect,
        selectedKeys,
        multiSelect = false,
        leavesOnly = false,
        parentIdsFromFullTree,
        useContextSelection = false,
        contextSelectedKey,
        onContextSelect,
    } = props;

    const selectedSet = selectedKeys != null
        ? (selectedKeys instanceof Set ? selectedKeys : new Set(selectedKeys || []))
        : null;
    const showSelection = !multiSelect && selectedSet != null;

    const getItemId = (item) => {
        const key = Object.keys(item || {}).find(k => k.toLowerCase() === (idField || 'key').toLowerCase());
        return key ? (item[key] ?? item?.key ?? item?.Key) : null;
    };

    const getText = (item) => {
        if (!item) return '';
        const key = Object.keys(item).find(k => k.toLowerCase() === (textField || 'text').toLowerCase());
        return key ? (item[key] ?? '') : (item.text ?? item.Text ?? '');
    };

    const treeData = React.useMemo(() => {
        return buildTreeFromFlatData(options, idField, parentIdField);
    }, [options, idField, parentIdField]);

    const isSelected = (item) => {
        if (!showSelection || !selectedSet) return false;
        const id = getItemId(item);
        return id != null && selectedSet.has(String(id));
    };

    const isContextSelected = (item) => {
        if (!useContextSelection || contextSelectedKey == null) return false;
        const id = getItemId(item);
        return id != null && String(id) === String(contextSelectedKey);
    };

    const handleRowClick = (item, isSelectable) => {
        if (!isSelectable) return;
        if (useContextSelection) {
            onContextSelect?.(item);
            return;
        }
        onSelect(item);
    };

    const handleRowDoubleClick = (item, isSelectable) => {
        if (!isSelectable) return;
        if (useContextSelection) {
            onSelect(item);
        }
    };

    const handleNodeKeyDown = (e, ctx) => {
        const { hasChildren, isExpanded, id, isSelectable, item } = ctx;

        if (e.key === "Enter" || e.key === " ") {
            e.preventDefault();
            if (hasChildren) onToggleExpand(id);
            else if (isSelectable) {
                if (useContextSelection) onSelect(item);
                else onSelect(item);
            }
        }

        if (e.key === "ArrowRight" && hasChildren && !isExpanded) {
            e.preventDefault();
            onToggleExpand(id);
        }

        if (e.key === "ArrowLeft" && hasChildren && isExpanded) {
            e.preventDefault();
            onToggleExpand(id);
        }
    };

    const renderTreeNode = (node, depth = 0) => {
        const item = node.item;
        const id = getItemId(item);
        const hasChildren = node.children && node.children.length > 0;
        const isExpanded = id != null && expandedIds.has(String(id));
        const selected = isSelected(item);
        const contextSelected = isContextSelected(item);

        const isSelectable = leavesOnly && parentIdsFromFullTree
            ? !parentIdsFromFullTree.has(String(id))
            : !leavesOnly || !hasChildren;
        return (
            <React.Fragment key={id}>
                <div
                    id={fieldId && id != null ? `${fieldId}-option-${id}` : undefined}
                    className={`hierarchy-picker-row ${hasChildren ? 'hierarchy-has-children' : 'hierarchy-leaf'} ${selected ? 'hierarchy-node-selected' : ''} ${contextSelected ? 'hierarchy-node-context' : ''} ${!isSelectable ? 'hierarchy-picker-row-not-selectable' : ''}`}
                    onClick={() => handleRowClick(item, isSelectable)}
                    onDoubleClick={() => handleRowDoubleClick(item, isSelectable)}
                    data-depth={depth}
                    tabIndex={0}
                    onKeyDown={(e) => handleNodeKeyDown(e, { hasChildren, isExpanded, id, isSelectable, item })}
                >
                    <div className="hierarchy-picker-expand-cell" style={{ paddingLeft: `${depth * 24 + 8}px` }}>
                        {hasChildren ? (
                            <IconButton
                                iconProps={{ iconName: isExpanded ? 'ChevronDown' : 'ChevronRight' }}
                                onClick={(e) => {
                                    e.stopPropagation();
                                    onToggleExpand(id);
                                }}
                                styles={{ root: { width: 24, height: 24 } }}
                                ariaLabel={isExpanded ? 'Collapse' : 'Expand'}
                            />
                        ) : (
                            <span className="hierarchy-node-spacer" style={{ width: 24, display: 'inline-block' }} />
                        )}
                    </div>
                    <div className="hierarchy-picker-cell">
                        {getText(item)}
                    </div>
                </div>
                {hasChildren && isExpanded && node.children.map(child => renderTreeNode(child, depth + 1))}
            </React.Fragment>
        );
    };

    if (!options || options.length === 0) {
        return (
            <div className="hierarchy-picker-empty">
                No items to display
            </div>
        );
    }

    return (
        <div className="hierarchy-picker-body">
            {treeData.map(node => renderTreeNode(node))}
        </div>
    );
};

export default TreePickerBody;
