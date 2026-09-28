import React, { useMemo, useRef, useState } from 'react';
import { Icon } from '@fluentui/react';
import { ReportPageDimensions } from '../ReportPageDimensions';
import { areaDomId, isFieldsArea } from '../Functions/layoutRowResolver';
import {
    computeSchematicGeometry,
    getAreaColumnCount,
    getAreaLabel,
    moveAreaToNewRow,
    moveAreaToRow,
    moveAreaWithinRow,
    setRowWidthPercents,
} from './sectionAreaLayoutUtils';
import './SectionAreaLayoutEditor.css';

const { PAGE_WIDTH } = ReportPageDimensions;

/** Drag payload type, matching the convention used by the absolute section designer. */
const AREA_DRAG_MIME = 'application/x-sectionarealayout-area';

/** Narrowest share of a row a divider drag may leave an area with. */
const MINIMUM_DIVIDER_PERCENT = 15;

/**
 * Builds the percentage split a divider drag produces.
 * @param {Array<{Width: number}>} areas - The areas on the row with their current widths.
 * @param {number} dividerIndex - Index of the area immediately left of the divider.
 * @param {number} deltaPoints - How far the divider moved, in points.
 * @returns {Array<number>|null} A share per area summing to 100, or null when the move is not allowed.
 */
const buildDividerPercents = (areas, dividerIndex, deltaPoints) => {
    const total = areas.reduce((sum, area) => sum + area.Width, 0);
    if (total <= 0) {
        return null;
    }

    const widths = areas.map((area) => area.Width);
    widths[dividerIndex] += deltaPoints;
    widths[dividerIndex + 1] -= deltaPoints;

    const percents = widths.map((width) => Math.round((width / total) * 100));
    if (percents.some((percent) => percent < MINIMUM_DIVIDER_PERCENT)) {
        return null;
    }

    const drift = 100 - percents.reduce((sum, percent) => sum + percent, 0);
    percents[dividerIndex] += drift;

    return percents;
};

/**
 * Graphical control for arranging a layout section's field block and grids into rows.
 *
 * Draws the section to scale: rows are bands across the page width and each area is a card at its real
 * offset and proportional width, so the schematic reads as a simplified version of the preview above it.
 * Dropping a card on another row puts the two areas side by side; dropping it on the line between two rows
 * moves it onto a row of its own.
 *
 * @param {object} props - Component props.
 * @param {Array<object>} props.rows - The resolved arrangement to display.
 * @param {Array<object>} props.contents - Translated printable entries, which carry the computed geometry.
 * @param {object} props.format - The resolved section format.
 * @param {Array<object>} props.grids - The section's grid bindings.
 * @param {Array<object>} props.dataSectionGrids - Grids offered by the data section, for display labels.
 * @param {Array<object>} props.language - Login language catalogue.
 * @param {Function} props.onChange - Called with a new arrangement whenever the user rearranges areas.
 */
const SectionAreaLayoutEditor = ({
    rows,
    contents,
    format,
    grids,
    dataSectionGrids,
    language = [],
    onChange,
}) => {
    const [draggedArea, setDraggedArea] = useState(null);
    const [dropTarget, setDropTarget] = useState(null);
    const draggedAreaRef = useRef(null);
    const dividerDragRef = useRef(null);

    const geometry = useMemo(
        () => computeSchematicGeometry({ rows, contents }),
        [rows, contents]
    );

    const hasUnfittableRow = geometry.some((row) => !row.fits && row.areas.length > 1);
    const areaCount = (rows || []).reduce((total, row) => total + row.Areas.length, 0);

    if (areaCount === 0) {
        return null;
    }

    /**
     * Records which area a drag started from.
     * @param {DragEvent} event - Drag start event.
     * @param {number} rowIndex - Row the area is on.
     * @param {number} areaIndex - Position of the area within its row.
     */
    const handleDragStart = (event, rowIndex, areaIndex) => {
        const payload = { rowIndex, areaIndex };
        draggedAreaRef.current = payload;
        setDraggedArea(payload);
        event.dataTransfer.effectAllowed = 'move';
        event.dataTransfer.setData(AREA_DRAG_MIME, JSON.stringify(payload));
    };

    /**
     * Clears drag state once the pointer is released, whether or not a drop happened.
     */
    const handleDragEnd = () => {
        draggedAreaRef.current = null;
        setDraggedArea(null);
        setDropTarget(null);
    };

    /**
     * Reads the dragged area, preferring component state so a drop works even where the browser withholds
     * drag data until the drop event.
     * @param {DragEvent} event - Drop event.
     * @returns {{rowIndex: number, areaIndex: number}|null} The dragged area, or null.
     */
    const readDragPayload = (event) => {
        if (draggedAreaRef.current) {
            return draggedAreaRef.current;
        }

        try {
            return JSON.parse(event.dataTransfer.getData(AREA_DRAG_MIME));
        } catch {
            return null;
        }
    };

    /**
     * Highlights a drop target while a card is dragged over it.
     * @param {DragEvent} event - Drag over event.
     * @param {object} target - The target being hovered.
     */
    const handleDragOver = (event, target) => {
        if (!draggedAreaRef.current) {
            return;
        }

        event.preventDefault();
        event.stopPropagation();
        event.dataTransfer.dropEffect = 'move';
        setDropTarget(target);
    };

    /**
     * Moves the dragged area onto an existing row so it sits beside the areas already there.
     * @param {DragEvent} event - Drop event.
     * @param {number} targetRowIndex - Row that was dropped on.
     */
    const handleDropOnRow = (event, targetRowIndex) => {
        event.preventDefault();
        event.stopPropagation();

        const payload = readDragPayload(event);
        handleDragEnd();

        if (!payload || payload.rowIndex === targetRowIndex) {
            return;
        }

        onChange(moveAreaToRow(rows, payload.rowIndex, payload.areaIndex, targetRowIndex));
    };

    /**
     * Moves the dragged area onto a new row inserted at the drop position.
     * @param {DragEvent} event - Drop event.
     * @param {number} insertAtRowIndex - Index the new row is inserted at.
     */
    const handleDropOnInsertLine = (event, insertAtRowIndex) => {
        event.preventDefault();
        event.stopPropagation();

        const payload = readDragPayload(event);
        handleDragEnd();

        if (!payload) {
            return;
        }

        onChange(moveAreaToNewRow(rows, payload.rowIndex, payload.areaIndex, insertAtRowIndex));
    };

    /**
     * Moves an area with the keyboard so the control is usable without a mouse.
     * @param {KeyboardEvent} event - Key down event on a card.
     * @param {number} rowIndex - Row the area is on.
     * @param {number} areaIndex - Position of the area within its row.
     */
    const handleAreaKeyDown = (event, rowIndex, areaIndex) => {
        const rowCount = rows.length;

        switch (event.key) {
            case 'ArrowLeft':
                event.preventDefault();
                onChange(moveAreaWithinRow(rows, rowIndex, areaIndex, areaIndex - 1));
                break;
            case 'ArrowRight':
                event.preventDefault();
                onChange(moveAreaWithinRow(rows, rowIndex, areaIndex, areaIndex + 1));
                break;
            case 'ArrowUp':
                event.preventDefault();
                if (rowIndex > 0) {
                    onChange(moveAreaToRow(rows, rowIndex, areaIndex, rowIndex - 1));
                }
                break;
            case 'ArrowDown':
                event.preventDefault();
                if (rowIndex < rowCount - 1) {
                    onChange(moveAreaToRow(rows, rowIndex, areaIndex, rowIndex + 1));
                }
                break;
            case 'Enter':
            case ' ':
                event.preventDefault();
                onChange(moveAreaToNewRow(rows, rowIndex, areaIndex, rowIndex + 1));
                break;
            default:
                break;
        }
    };

    /**
     * Starts a divider drag, which re-splits the width between the two areas either side of it.
     * @param {MouseEvent} event - Mouse down event on a divider.
     * @param {number} rowIndex - Row being resized.
     * @param {number} dividerIndex - Index of the area immediately left of the divider.
     */
    const handleDividerMouseDown = (event, rowIndex, dividerIndex) => {
        event.preventDefault();
        event.stopPropagation();

        const rowGeometry = geometry[rowIndex];
        const scale = event.currentTarget.parentElement.clientWidth / PAGE_WIDTH;

        dividerDragRef.current = {
            rowIndex,
            dividerIndex,
            startX: event.clientX,
            scale: scale > 0 ? scale : 1,
            areas: rowGeometry.areas.map((area) => ({ Width: area.Width })),
        };

        /**
         * Applies the divider position as the pointer moves.
         * @param {MouseEvent} moveEvent - Mouse move event.
         */
        const onMouseMove = (moveEvent) => {
            const drag = dividerDragRef.current;
            if (!drag) {
                return;
            }

            const deltaPoints = (moveEvent.clientX - drag.startX) / drag.scale;
            const percents = buildDividerPercents(drag.areas, drag.dividerIndex, deltaPoints);

            if (percents) {
                onChange(setRowWidthPercents(rows, drag.rowIndex, drag.dividerIndex, percents));
            }
        };

        /**
         * Ends the divider drag.
         */
        const onMouseUp = () => {
            dividerDragRef.current = null;
            window.removeEventListener('mousemove', onMouseMove);
            window.removeEventListener('mouseup', onMouseUp);
        };

        window.addEventListener('mousemove', onMouseMove);
        window.addEventListener('mouseup', onMouseUp);
    };

    /**
     * Renders the thin line between rows that a card can be dropped on to make a new row.
     * @param {number} insertAtRowIndex - Index the new row would be inserted at.
     * @param {string} domId - DOM id for the line.
     * @returns {JSX.Element} The insert line element.
     */
    const renderInsertLine = (insertAtRowIndex, domId) => {
        const isActive = dropTarget?.kind === 'insert' && dropTarget.index === insertAtRowIndex;

        return (
            <div
                id={domId}
                className={`section-area-layout-insert${draggedArea ? ' available' : ''}${isActive ? ' active' : ''}`}
                onDragOver={(event) => handleDragOver(event, { kind: 'insert', index: insertAtRowIndex })}
                onDragLeave={() => setDropTarget(null)}
                onDrop={(event) => handleDropOnInsertLine(event, insertAtRowIndex)}
            >
                <span className="section-area-layout-insert-label">New row</span>
            </div>
        );
    };

    const geometrySummary = geometry.flatMap((row, rowIndex) =>
        row.areas.map(({ area, Left, Width }) => ({
            rowIndex,
            areaId: areaDomId(area),
            left: Left,
            width: Width,
        })));

    return (
        <div
            className="section-area-layout-editor"
            id="sectionarealayouteditor"
            data-layout-geometry={JSON.stringify(geometrySummary)}
        >
            <div className="section-area-layout-header">
                <h4>Area layout</h4>
                <span className="section-area-layout-hint" id="sectionarealayouteditor-hint">
                    Drag an area onto another row to place them side by side
                </span>
            </div>

            {hasUnfittableRow && (
                <div className="section-area-layout-warning" id="sectionarealayouteditor-issue-row-too-narrow">
                    <Icon iconName="Warning" /> There is not enough width to place these areas side by side, so
                    they will print one under the other. Move one onto its own row.
                </div>
            )}

            <div className="section-area-layout-page">
                {renderInsertLine(0, 'sectionarealayouteditor-insert-first')}

                {rows.map((row, rowIndex) => {
                    const rowGeometry = geometry[rowIndex];
                    const isRowTarget = dropTarget?.kind === 'row' && dropTarget.index === rowIndex;

                    return (
                        <React.Fragment key={`sectionarealayout-row-${rowIndex}`}>
                            <div
                                id={`sectionarealayouteditor-row-${rowIndex}`}
                                className={`section-area-layout-row${isRowTarget ? ' drop-target' : ''}${rowGeometry.fits ? '' : ' does-not-fit'}`}
                                onDragOver={(event) => handleDragOver(event, { kind: 'row', index: rowIndex })}
                                onDragLeave={() => setDropTarget(null)}
                                onDrop={(event) => handleDropOnRow(event, rowIndex)}
                            >
                                <span className="section-area-layout-row-number">{rowIndex + 1}</span>

                                <div className="section-area-layout-row-track">
                                    {row.Areas.map((area, areaIndex) => {
                                        const areaGeometry = rowGeometry.areas[areaIndex];
                                        const label = getAreaLabel(area, dataSectionGrids, language);
                                        const columnCount = getAreaColumnCount(area, format, grids);
                                        const isDragging = draggedArea?.rowIndex === rowIndex
                                            && draggedArea?.areaIndex === areaIndex;

                                        return (
                                            <React.Fragment key={`${areaDomId(area)}-${areaIndex}`}>
                                                {areaIndex > 0 && (
                                                    <div
                                                        id={`sectionarealayouteditor-row-${rowIndex}-divider-${areaIndex - 1}`}
                                                        className="section-area-layout-divider"
                                                        role="separator"
                                                        aria-label={`Resize areas either side of divider ${areaIndex} on row ${rowIndex + 1}`}
                                                        onMouseDown={(event) =>
                                                            handleDividerMouseDown(event, rowIndex, areaIndex - 1)}
                                                    />
                                                )}
                                                <div
                                                    id={`sectionarealayouteditor-row-${rowIndex}-area-${areaDomId(area)}`}
                                                    className={`section-area-layout-area${isDragging ? ' dragging' : ''}`}
                                                    style={{ flexGrow: Math.max(areaGeometry.Width, 1) }}
                                                    draggable
                                                    tabIndex={0}
                                                    role="button"
                                                    aria-label={`${label}, row ${rowIndex + 1} of ${rows.length}. Use the arrow keys to move it, or Enter to put it on its own row.`}
                                                    onDragStart={(event) => handleDragStart(event, rowIndex, areaIndex)}
                                                    onDragEnd={handleDragEnd}
                                                    onKeyDown={(event) => handleAreaKeyDown(event, rowIndex, areaIndex)}
                                                >
                                                    <span className="section-area-layout-area-label">
                                                        <Icon iconName={isFieldsArea(area.Type) ? 'TextField' : 'Table'} />
                                                        {label}
                                                    </span>
                                                    <span
                                                        className={`section-area-layout-glyph ${isFieldsArea(area.Type) ? 'fields' : 'grid'}`}
                                                        aria-hidden="true"
                                                    >
                                                        {Array.from({ length: isFieldsArea(area.Type) ? 3 : 2 }).map((ignored, lineIndex) => (
                                                            <span
                                                                className="section-area-layout-glyph-line"
                                                                key={`glyph-line-${lineIndex}`}
                                                            >
                                                                {Array.from({ length: columnCount }).map((alsoIgnored, cellIndex) => (
                                                                    <span
                                                                        className="section-area-layout-glyph-cell"
                                                                        key={`glyph-cell-${cellIndex}`}
                                                                    />
                                                                ))}
                                                            </span>
                                                        ))}
                                                    </span>
                                                    <span className="section-area-layout-area-width">
                                                        {areaGeometry.Width}pt
                                                    </span>
                                                </div>
                                            </React.Fragment>
                                        );
                                    })}
                                </div>
                            </div>

                            {renderInsertLine(rowIndex + 1, `sectionarealayouteditor-row-${rowIndex}-insert-after`)}
                        </React.Fragment>
                    );
                })}
            </div>
        </div>
    );
};

export default SectionAreaLayoutEditor;
