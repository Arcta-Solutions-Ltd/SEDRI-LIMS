/**
 * PageOrderEditor - reorders the pages on a form, showing page groups as cards.
 *
 * Group cards cannot be moved. Anchor rows are locked. Member rows move only inside their
 * group and never above an anchor. Ungrouped rows move freely and skip over a whole group
 * card in one step so they cannot land inside a group.
 *
 * Rows are matched by page name id, never by translated label.
 *
 * @param {Object} props
 * @param {Array} props.value - PageOrderRowModel list from editpagesquery
 * @param {Function} props.onChange - Callback with the updated row list
 * @param {Array} [props.language] - Language array for TranslateTag
 */
import React, { useRef } from 'react';
import { Icon, IconButton, TooltipHost } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { getStandardTooltipProps } from '../../../../Utils/General/StandardTooltipProps';
import { reorderList } from '../../../Forms/ArcSelector/ArcSelectorReorderUtils';
import './PageOrderEditor.css';

const rowId = (row) => row?.id ?? row?.Id ?? '';

const groupIdOf = (row) => row?.groupId ?? row?.GroupId ?? '';

const isLocked = (row) => row?.locked === true || row?.Locked === true;

const normalizeRows = (value) => {
    if (!Array.isArray(value)) {
        return [];
    }
    return value.map((row) => ({
        id: rowId(row),
        label: row.label ?? row.Label ?? '',
        groupId: groupIdOf(row) || '',
        groupTitle: row.groupTitle ?? row.GroupTitle ?? '',
        locked: isLocked(row),
    }));
};

const toPayload = (rows) => rows.map((row) => ({
    Id: row.id,
    Label: row.label,
    GroupId: row.groupId || null,
    GroupTitle: row.groupTitle || null,
    Locked: row.locked === true,
}));

const firstIndexOfGroup = (rows, groupId) => rows.findIndex((row) => row.groupId === groupId);

const lastIndexOfGroup = (rows, groupId) => {
    for (let i = rows.length - 1; i >= 0; i--) {
        if (rows[i].groupId === groupId) {
            return i;
        }
    }
    return -1;
};

const memberRange = (rows, groupId) => {
    const first = firstIndexOfGroup(rows, groupId);
    const last = lastIndexOfGroup(rows, groupId);
    if (first < 0) {
        return { start: -1, end: -1 };
    }
    let start = first;
    while (start <= last && rows[start].locked) {
        start += 1;
    }
    return { start, end: last };
};

const wouldLandInsideGroup = (rows, pageId) => {
    const index = rows.findIndex((row) => row.id === pageId);
    if (index < 0) {
        return false;
    }
    const previous = rows[index - 1];
    const next = rows[index + 1];
    return Boolean(previous?.groupId && next?.groupId && previous.groupId === next.groupId);
};

const canMoveUp = (rows, index) => {
    const row = rows[index];
    if (!row || row.locked || index <= 0) {
        return false;
    }
    if (!row.groupId) {
        return true;
    }
    const range = memberRange(rows, row.groupId);
    return index > range.start;
};

const canMoveDown = (rows, index) => {
    const row = rows[index];
    if (!row || row.locked || index >= rows.length - 1) {
        return false;
    }
    if (!row.groupId) {
        return true;
    }
    const range = memberRange(rows, row.groupId);
    return index < range.end;
};

const moveRow = (rows, index, direction) => {
    const row = rows[index];
    if (!row || row.locked) {
        return rows;
    }

    if (row.groupId) {
        const target = direction === 'up' ? index - 1 : index + 1;
        const range = memberRange(rows, row.groupId);
        if (target < range.start || target > range.end) {
            return rows;
        }
        return reorderList(rows, index, target);
    }

    if (direction === 'up') {
        const previous = rows[index - 1];
        if (!previous) {
            return rows;
        }
        if (!previous.groupId) {
            return reorderList(rows, index, index - 1);
        }
        return reorderList(rows, index, firstIndexOfGroup(rows, previous.groupId));
    }

    const next = rows[index + 1];
    if (!next) {
        return rows;
    }
    if (!next.groupId) {
        return reorderList(rows, index, index + 1);
    }
    return reorderList(rows, index, lastIndexOfGroup(rows, next.groupId));
};

const isValidDrop = (rows, fromIndex, toIndex) => {
    if (fromIndex === toIndex || fromIndex < 0 || toIndex < 0) {
        return false;
    }
    const source = rows[fromIndex];
    if (!source || source.locked) {
        return false;
    }

    if (source.groupId) {
        const range = memberRange(rows, source.groupId);
        return toIndex >= range.start && toIndex <= range.end && !rows[toIndex].locked;
    }

    const proposed = reorderList(rows, fromIndex, toIndex);
    return !wouldLandInsideGroup(proposed, source.id);
};

const buildSegments = (rows) => {
    const segments = [];
    let currentGroup = null;

    rows.forEach((row, index) => {
        if (row.groupId) {
            if (!currentGroup || currentGroup.groupId !== row.groupId) {
                currentGroup = {
                    type: 'group',
                    groupId: row.groupId,
                    groupTitle: row.groupTitle,
                    rows: [],
                };
                segments.push(currentGroup);
            }
            currentGroup.rows.push({ row, index });
            return;
        }

        currentGroup = null;
        segments.push({ type: 'page', row, index });
    });

    return segments;
};

const PageOrderEditor = (props) => {
    const language = props.language || [];
    const rows = normalizeRows(props.value);
    const draggedIndexRef = useRef(null);

    const translate = (tag, fallback) => TranslateTag(tag, language) || fallback;

    const emit = (nextRows) => {
        if (typeof props.onChange === 'function') {
            props.onChange(toPayload(nextRows));
        }
    };

    const onMove = (index, direction) => {
        emit(moveRow(rows, index, direction));
    };

    const onDragStart = (index) => {
        const row = rows[index];
        if (!row || row.locked || (!row.groupId && !canMoveUp(rows, index) && !canMoveDown(rows, index))) {
            draggedIndexRef.current = null;
            return;
        }
        if (row.locked) {
            draggedIndexRef.current = null;
            return;
        }
        draggedIndexRef.current = index;
    };

    const onDrop = (index) => {
        const fromIndex = draggedIndexRef.current;
        draggedIndexRef.current = null;
        if (fromIndex == null || !isValidDrop(rows, fromIndex, index)) {
            return;
        }
        emit(reorderList(rows, fromIndex, index));
    };

    const renderRow = (row, index) => {
        const movable = !row.locked && (row.groupId ? canMoveUp(rows, index) || canMoveDown(rows, index) : true);
        const upEnabled = canMoveUp(rows, index);
        const downEnabled = canMoveDown(rows, index);
        const lockTitle = translate('@ConPagM@', 'This page is fixed at the start of its group and cannot be moved.');

        return (
            <div
                key={row.id || index}
                id={`pageorder-row-${row.id}`}
                className={movable ? 'pageorder-row pageorder-row-movable' : 'pageorder-row'}
                draggable={movable}
                onDragStart={movable ? () => onDragStart(index) : undefined}
                onDragOver={(event) => event.preventDefault()}
                onDrop={(event) => {
                    event.preventDefault();
                    onDrop(index);
                }}
            >
                <span className="pageorder-row-label">{TranslateTag(row.label, language) || row.label}</span>
                <span className="pageorder-row-actions">
                    {row.locked && (
                        <TooltipHost content={lockTitle} tooltipProps={getStandardTooltipProps()} calloutProps={{ gapSpace: 8 }}>
                            <span id={`pageorder-row-${row.id}-lock`} className="pageorder-lock">
                                <Icon iconName="Lock" />
                            </span>
                        </TooltipHost>
                    )}
                    {!row.locked && (
                        <>
                            <TooltipHost content={translate('@MovUp@', 'Move up')} tooltipProps={getStandardTooltipProps()} calloutProps={{ gapSpace: 8 }}>
                                <IconButton
                                    id={`pageorder-row-${row.id}-up`}
                                    iconProps={{ iconName: 'Up' }}
                                    ariaLabel={translate('@MovUp@', 'Move up')}
                                    disabled={!upEnabled}
                                    onClick={() => onMove(index, 'up')}
                                />
                            </TooltipHost>
                            <TooltipHost content={translate('@MovDow@', 'Move down')} tooltipProps={getStandardTooltipProps()} calloutProps={{ gapSpace: 8 }}>
                                <IconButton
                                    id={`pageorder-row-${row.id}-down`}
                                    iconProps={{ iconName: 'Down' }}
                                    ariaLabel={translate('@MovDow@', 'Move down')}
                                    disabled={!downEnabled}
                                    onClick={() => onMove(index, 'down')}
                                />
                            </TooltipHost>
                        </>
                    )}
                </span>
            </div>
        );
    };

    const helperText = translate('@ConPagO@', 'Pages in this group must stay together.');

    return (
        <div className="pageorder-editor" id="pageorder-editor">
            {buildSegments(rows).map((segment, segmentIndex) => {
                if (segment.type === 'group') {
                    return (
                        <div
                            key={segment.groupId || segmentIndex}
                            className="pageorder-group"
                            id={`pageorder-group-${segment.groupId}`}
                        >
                            <div className="pageorder-group-header">
                                {TranslateTag(segment.groupTitle, language) || segment.groupTitle}
                            </div>
                            <div className="pageorder-group-help">{helperText}</div>
                            {segment.rows.map((entry) => renderRow(entry.row, entry.index))}
                        </div>
                    );
                }
                return renderRow(segment.row, segment.index);
            })}
        </div>
    );
};

export default PageOrderEditor;
