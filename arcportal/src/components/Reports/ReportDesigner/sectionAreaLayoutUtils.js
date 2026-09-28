import { getNaturalBounds } from '../Functions/layoutRowGeometry';
import {
    isFieldsArea,
    LAYOUT_AREA_TYPES,
    normaliseAreaId,
    resolveLayoutRows,
} from '../Functions/layoutRowResolver';
import { getGridDisplayText } from './gridLayoutDisplay';

/**
 * Editor-side helpers for the section area layout control: labelling areas, moving them between rows, and
 * working out the schematic geometry the control draws.
 *
 * Matching is by stable id throughout. A grid's translated description is used for display only, so an
 * arrangement keeps working when the system is translated into another language.
 */

/**
 * Deep copies an arrangement so a drag can be built up without mutating editor state.
 * @param {Array<object>} rows - Layout rows.
 * @returns {Array<{Areas: Array<object>}>} A detached copy.
 */
export const cloneLayoutRows = (rows) =>
    (rows || []).map((row) => ({
        Areas: (row?.Areas || row?.areas || []).map((area) => ({
            Type: area?.Type ?? area?.type,
            Name: area?.Name ?? area?.name ?? null,
            WidthPercent: Number(area?.WidthPercent ?? area?.widthPercent ?? 0),
        })),
    }));

/**
 * Builds the arrangement the control should show for the current section state.
 * @param {Array<object>} layoutRows - The arrangement held in editor state.
 * @param {Array<object>} grids - The section's grid bindings.
 * @param {object} format - The resolved section format.
 * @param {Array<object>} fields - The section's field bindings.
 * @returns {Array<{Areas: Array<object>}>} A complete arrangement covering every current area once.
 */
export const resolveEditorLayoutRows = (layoutRows, grids, format, fields) =>
    resolveLayoutRows(layoutRows, grids, sectionHasFieldBlock(format, fields));

/**
 * Determines whether the section renders a scalar field block at all.
 * @param {object} format - The resolved section format.
 * @param {Array<object>} fields - The section's field bindings.
 * @returns {boolean} True when the format defines field columns and the section places at least one field.
 */
export const sectionHasFieldBlock = (format, fields) => {
    const columnCount = (format?.Columns ?? format?.columns ?? []).length;
    const formatType = format?.Type ?? format?.type;

    return columnCount > 0 && formatType !== 'Table' && (fields || []).length > 0;
};

/**
 * Returns the label shown on an area card.
 * @param {object} area - A layout area.
 * @param {Array<object>} dataSectionGrids - Grids offered by the data section, for description lookup.
 * @param {Array<object>} language - Login language catalogue.
 * @returns {string} "Fields" for the field block, or the grid's translated description.
 */
export const getAreaLabel = (area, dataSectionGrids, language = []) => {
    if (isFieldsArea(area?.Type)) {
        return 'Fields';
    }

    const gridId = normaliseAreaId(area?.Name);
    const dataGrid = (dataSectionGrids || []).find(
        (grid) => normaliseAreaId(grid?.Name ?? grid?.name) === gridId
    );

    return dataGrid ? getGridDisplayText(dataGrid, language) : area?.Name || 'Grid';
};

/**
 * Counts the columns an area card should draw in its glyph.
 * @param {object} area - A layout area.
 * @param {object} format - The resolved section format.
 * @param {Array<object>} grids - The section's grid bindings.
 * @returns {number} Field columns for the field block, or grid columns for a grid.
 */
export const getAreaColumnCount = (area, format, grids) => {
    if (isFieldsArea(area?.Type)) {
        return (format?.Columns ?? format?.columns ?? []).length || 1;
    }

    const gridIndex = (grids || []).findIndex(
        (grid) => normaliseAreaId(grid?.Name ?? grid?.name) === normaliseAreaId(area?.Name)
    );
    const formatGrids = format?.Grids ?? format?.grids ?? [];

    if (formatGrids.length === 0) {
        return 1;
    }

    const formatGrid = gridIndex >= 0 && gridIndex < formatGrids.length
        ? formatGrids[gridIndex]
        : formatGrids[formatGrids.length - 1];
    const widthDefinition = String(formatGrid?.Width ?? formatGrid?.width ?? '');

    return widthDefinition.split('|').filter((segment) => segment.trim().length > 0).length || 1;
};

/**
 * Moves an area onto an existing row, placing it at the end of that row.
 * @param {Array<object>} rows - The current arrangement.
 * @param {number} sourceRowIndex - Row the area is coming from.
 * @param {number} sourceAreaIndex - Position of the area within its row.
 * @param {number} targetRowIndex - Row the area is going to.
 * @returns {Array<object>} A new arrangement with empty rows removed.
 */
export const moveAreaToRow = (rows, sourceRowIndex, sourceAreaIndex, targetRowIndex) => {
    const next = cloneLayoutRows(rows);
    const sourceRow = next[sourceRowIndex];
    const targetRow = next[targetRowIndex];

    if (!sourceRow || !targetRow || sourceRow === targetRow) {
        return next;
    }

    const [moved] = sourceRow.Areas.splice(sourceAreaIndex, 1);
    if (!moved) {
        return next;
    }

    // Explicit shares belonged to the row the area left, so both rows fall back to equal shares.
    targetRow.Areas.push({ ...moved, WidthPercent: 0 });
    targetRow.Areas.forEach((area) => {
        area.WidthPercent = 0;
    });
    sourceRow.Areas.forEach((area) => {
        area.WidthPercent = 0;
    });

    return next.filter((row) => row.Areas.length > 0);
};

/**
 * Moves an area onto a new row inserted at the given position.
 * @param {Array<object>} rows - The current arrangement.
 * @param {number} sourceRowIndex - Row the area is coming from.
 * @param {number} sourceAreaIndex - Position of the area within its row.
 * @param {number} insertAtRowIndex - Index the new row is inserted at.
 * @returns {Array<object>} A new arrangement with empty rows removed.
 */
export const moveAreaToNewRow = (rows, sourceRowIndex, sourceAreaIndex, insertAtRowIndex) => {
    const next = cloneLayoutRows(rows);
    const sourceRow = next[sourceRowIndex];

    if (!sourceRow) {
        return next;
    }

    // A row holding one area is already on a row of its own, so moving it next to itself changes nothing.
    if (sourceRow.Areas.length === 1
        && (insertAtRowIndex === sourceRowIndex || insertAtRowIndex === sourceRowIndex + 1)) {
        return next;
    }

    const [moved] = sourceRow.Areas.splice(sourceAreaIndex, 1);
    if (!moved) {
        return next;
    }

    sourceRow.Areas.forEach((area) => {
        area.WidthPercent = 0;
    });
    next.splice(insertAtRowIndex, 0, { Areas: [{ ...moved, WidthPercent: 0 }] });

    return next.filter((row) => row.Areas.length > 0);
};

/**
 * Reorders an area within its own row.
 * @param {Array<object>} rows - The current arrangement.
 * @param {number} rowIndex - The row being reordered.
 * @param {number} fromAreaIndex - Current position of the area.
 * @param {number} toAreaIndex - Position to move it to.
 * @returns {Array<object>} A new arrangement.
 */
export const moveAreaWithinRow = (rows, rowIndex, fromAreaIndex, toAreaIndex) => {
    const next = cloneLayoutRows(rows);
    const row = next[rowIndex];

    if (!row || toAreaIndex < 0 || toAreaIndex >= row.Areas.length) {
        return next;
    }

    const [moved] = row.Areas.splice(fromAreaIndex, 1);
    row.Areas.splice(toAreaIndex, 0, moved);

    return next;
};

/**
 * Sets the width split either side of a divider on a row.
 * @param {Array<object>} rows - The current arrangement.
 * @param {number} rowIndex - The row being resized.
 * @param {number} dividerIndex - Index of the area immediately left of the divider.
 * @param {Array<number>} percents - The share for every area on the row, summing to 100.
 * @returns {Array<object>} A new arrangement.
 */
export const setRowWidthPercents = (rows, rowIndex, dividerIndex, percents) => {
    const next = cloneLayoutRows(rows);
    const row = next[rowIndex];

    if (!row || row.Areas.length !== percents.length) {
        return next;
    }

    row.Areas.forEach((area, index) => {
        area.WidthPercent = percents[index];
    });

    return next;
};

/**
 * Reads the geometry the schematic should draw, which is the geometry the report will use.
 * @param {object} params - Geometry inputs.
 * @param {Array<object>} params.rows - The resolved arrangement.
 * @param {Array<object>} params.contents - Translated printable entries for the section.
 * @returns {Array<{areas: Array<{area: object, Left: number, Width: number}>, fits: boolean}>} Per row, each
 * area's left edge and width in points, and whether the areas on the row actually clear one another.
 * @remarks
 * Reads the already translated entries rather than recomputing anything, because the translator has already
 * divided each row's span between its areas. That makes it impossible for the control to draw a layout the
 * renderer would not produce. A row the translator could not lay out keeps its natural geometry, which shows
 * up here as overlapping areas and is reported as not fitting.
 */
export const computeSchematicGeometry = ({ rows, contents }) => {
    const entryForArea = buildAreaEntryLookup(contents);

    return (rows || []).map((row) => {
        const areas = row.Areas.map((area) => {
            const entry = entryForArea(area);
            const bounds = entry ? getNaturalBounds(entry) : { Left: 0, Width: 0 };

            return { area, Left: bounds.Left, Width: bounds.Width };
        });

        return { areas, fits: !hasOverlappingAreas(areas) };
    });
};

/**
 * Determines whether any two areas on a row occupy the same horizontal space.
 * @param {Array<{Left: number, Width: number}>} areas - The areas on one row.
 * @returns {boolean} True when two areas overlap, or an area has no width.
 */
const hasOverlappingAreas = (areas) => {
    if (areas.length < 2) {
        return areas.some(({ Width }) => Width <= 0);
    }

    const ordered = [...areas].sort((left, right) => left.Left - right.Left);

    return ordered.some((area, index) =>
        area.Width <= 0
        || (index > 0 && area.Left < ordered[index - 1].Left + ordered[index - 1].Width));
};

/**
 * Builds a lookup from a layout area to the printable entry it represents.
 * @param {Array<object>} contents - Translated printable entries for the section.
 * @returns {Function} Given an area, returns its entry or undefined.
 * @remarks
 * Grid entries are matched on their stable grid id, not on their position in the contents list, because the
 * arrangement reorders that list.
 */
const buildAreaEntryLookup = (contents) => {
    const fieldBlockEntry = (contents || []).find(
        (entry) => entry?.Column1 || entry?.Column2
    );

    return (area) => {
        if (isFieldsArea(area?.Type)) {
            return fieldBlockEntry;
        }

        const gridId = normaliseAreaId(area?.Name);

        return (contents || []).find(
            (entry) => entry?.Theme === 'grid' && normaliseAreaId(entry?.Name) === gridId
        );
    };
};

export { LAYOUT_AREA_TYPES };
