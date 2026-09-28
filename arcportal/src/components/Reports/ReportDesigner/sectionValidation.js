/**
 * Consistency checks for a report section, its chosen format and the data section behind it.
 *
 * The data section is the catalogue of what a test actually captures: its Fields and Grids are the
 * fields and fieldgrids on the test form. The format supplies the geometry - how many field columns
 * and how many grid positions the section has to place them in. A section that binds fewer grids
 * than its test provides, or a format with fewer grid positions than the section binds, is not an
 * error the save can reject, but the user needs to see it because the report will not show what
 * they expect.
 *
 * Grids and fields are matched on their ids (a grid's Name, a field's Value), never on Description
 * or Label, because those are language catalogue tokens that change with the user's language.
 */

/**
 * Identifiers for the issues {@link validateSection} can report. Used as the suffix of the DOM id
 * each issue renders under, so tests can address a specific issue rather than matching on text.
 */
export const ISSUE_TYPES = {
    GRID_CAPACITY: 'grid-capacity',
    GRID_UNPLACED: 'grid-unplaced',
    GRID_UNKNOWN: 'grid-unknown',
    FIELD_UNKNOWN: 'field-unknown',
    GRIDS_NOT_SUPPORTED: 'grids-not-supported',
    FIELDS_NOT_SUPPORTED: 'fields-not-supported',
    COLUMN_LIMIT: 'column-limit'
};

/**
 * The number of field columns the report renderer fills. A format may define more, but only the
 * first two are written out, so fields placed beyond them would not appear.
 */
const RENDERED_COLUMN_LIMIT = 2;

/**
 * Normalises a grid or field identifier so the form, data section and section binding spellings of
 * the same thing compare equal.
 * @param {string} id - The identifier to normalise.
 * @returns {string} The identifier trimmed, stripped of spaces and lower cased.
 */
const normaliseId = (id) => String(id ?? '').trim().replace(/\s/g, '').toLowerCase();

/**
 * Returns section grid bindings capped to the number of positions the format defines.
 * @param {Array} sectionGrids - Bindings from the section editor state.
 * @param {object} format - The resolved format, with a Grids array describing positions.
 * @returns {Array} Bindings trimmed to format capacity.
 */
export const trimSectionGridsToFormat = (sectionGrids, format) => {
    const capacity = format?.Grids?.length ?? 0;
    return (sectionGrids || []).slice(0, capacity);
};

/**
 * Returns the number of width slots a format grid position defines.
 * @param {object} formatGrid - A format grid with a pipe-separated Width string.
 * @returns {number} Column count for the grid position.
 */
const getFormatGridColumnCount = (formatGrid) => {
    if (!formatGrid?.Width) {
        return 0;
    }

    return formatGrid.Width.split('|').filter(width => width !== '').length;
};

/**
 * Drops section fields placed beyond the format's field-column count.
 * A format with zero columns clears all scalar field placements.
 * @param {Array} sectionFields - Field bindings from the section editor state.
 * @param {object} format - The resolved format, with a Columns array describing geometry.
 * @returns {Array} Fields capped to format column capacity.
 */
export const trimSectionFieldsToFormat = (sectionFields, format) => {
    const capacity = format?.Columns?.length ?? 0;

    if (capacity === 0) {
        return [];
    }

    return (sectionFields || []).filter(field => Number(field?.Column) >= 1 && Number(field?.Column) <= capacity);
};

/**
 * After removing a format column at removedIndex (0-based), removes fields in that column and
 * decrements Column for fields placed in higher columns.
 * @param {Array} sectionFields - Field bindings from the section editor state.
 * @param {number} removedIndex - Zero-based index of the removed format column.
 * @returns {Array} Reconciled field bindings.
 */
export const reconcileSectionFieldsAfterColumnRemoved = (sectionFields, removedIndex) => {
    const removedColumnNumber = removedIndex + 1;

    return (sectionFields || [])
        .filter(field => Number(field?.Column) !== removedColumnNumber)
        .map(field => {
            const column = Number(field?.Column);
            if (column > removedColumnNumber) {
                return { ...field, Column: column - 1 };
            }

            return field;
        });
};

/**
 * Trims each section grid's Head array to the width-slot count its format position defines.
 * @param {Array} sectionGrids - Grid bindings from the section editor state.
 * @param {object} format - The resolved format, with a Grids array describing positions.
 * @returns {Array} Grid bindings with Head arrays trimmed to format width capacity.
 */
export const trimSectionGridHeadsToFormat = (sectionGrids, format) => {
    const formatGrids = format?.Grids || [];
    const capacity = formatGrids.length;

    return (sectionGrids || []).slice(0, capacity).map((grid, index) => {
        const formatGrid = index < formatGrids.length
            ? formatGrids[index]
            : formatGrids[formatGrids.length - 1];
        const columnCount = getFormatGridColumnCount(formatGrid);
        const currentHead = grid?.Head || [];

        if (currentHead.length <= columnCount) {
            return grid;
        }

        return {
            ...grid,
            Head: currentHead.slice(0, columnCount)
        };
    });
};

/**
 * Detects which format column was removed when the column count shrinks by one.
 * @param {Array} previousColumns - Columns before the format edit.
 * @param {Array} newColumns - Columns after the format edit.
 * @returns {number} Zero-based removed index, or -1 when not exactly one column was removed.
 */
const findRemovedColumnIndex = (previousColumns, newColumns) => {
    const previous = previousColumns || [];
    const next = newColumns || [];

    if (next.length !== previous.length - 1) {
        return -1;
    }

    for (let index = 0; index < previous.length; index += 1) {
        const previousColumn = previous[index];
        const nextColumn = next[index];

        if (!nextColumn
            || previousColumn?.Left !== nextColumn?.Left
            || previousColumn?.Width !== nextColumn?.Width
            || previousColumn?.LabelWidth !== nextColumn?.LabelWidth) {
            return index;
        }
    }

    return previous.length - 1;
};

/**
 * Reconciles section field placements after a format edit changed the column geometry.
 * @param {Array} sectionFields - Field bindings from the section editor state.
 * @param {object} previousFormat - Format before the edit, if known.
 * @param {object} updatedFormat - Format after the edit.
 * @returns {Array} Field bindings adjusted for the new column layout.
 */
export const reconcileSectionFieldsAfterFormatChange = (sectionFields, previousFormat, updatedFormat) => {
    const removedIndex = findRemovedColumnIndex(previousFormat?.Columns, updatedFormat?.Columns);
    const reconciled = removedIndex >= 0
        ? reconcileSectionFieldsAfterColumnRemoved(sectionFields, removedIndex)
        : (sectionFields || []);

    return trimSectionFieldsToFormat(reconciled, updatedFormat);
};

/**
 * Checks a section definition against its format and its data section.
 * @param {object} params - The section, its resolved format and its resolved data section.
 * @param {object} params.section - The section definition, with Fields and Grids.
 * @param {object} params.format - The section's format, with Columns and Grids describing geometry.
 * @param {object} [params.dataSection] - The data section the section reads, listing what the test captures.
 * @returns {Array<{type: string, message: string}>} The issues found, empty when the section is consistent.
 */
export const validateSection = ({ section, format, dataSection }) => {
    const issues = [];

    if (!section || !format) {
        return issues;
    }

    const sectionFields = section.Fields || [];
    const sectionGrids = section.Grids || [];
    const formatColumns = format.Columns || [];
    const formatGrids = format.Grids || [];
    const dataSectionFields = dataSection?.Fields || [];
    const dataSectionGrids = dataSection?.Grids || [];
    const dataSectionName = dataSection?.Name || 'the data section';

    // The count the test provides, which is what the format has to be able to hold. A section that
    // has not placed its grids yet still needs the warning, so this is measured against the data
    // section rather than against the section's own bindings.
    const gridsNeeded = Math.max(dataSectionGrids.length, sectionGrids.length);

    if (gridsNeeded > 0 && formatGrids.length === 0) {
        issues.push({
            type: ISSUE_TYPES.GRIDS_NOT_SUPPORTED,
            message: `Format "${format.Description || format.Name}" has no grid positions, so none of the ${gridsNeeded} grid(s) available from ${dataSectionName} can be shown. Choose or create a format with ${gridsNeeded} grid position(s).`
        });
    } else if (gridsNeeded > formatGrids.length) {
        issues.push({
            type: ISSUE_TYPES.GRID_CAPACITY,
            message: `${dataSectionName} provides ${gridsNeeded} grid(s) but format "${format.Description || format.Name}" has ${formatGrids.length} grid position(s). Choose or create a format with ${gridsNeeded} grid position(s).`
        });
    }

    if (dataSectionGrids.length > sectionGrids.length && formatGrids.length > 0) {
        issues.push({
            type: ISSUE_TYPES.GRID_UNPLACED,
            message: `${dataSectionGrids.length - sectionGrids.length} of the ${dataSectionGrids.length} grid(s) available from ${dataSectionName} are not placed on this section and will not appear on the report.`
        });
    }

    if (dataSectionGrids.length > 0) {
        const availableGridIds = dataSectionGrids.map(grid => normaliseId(grid.name ?? grid.Name));
        const unknownGrids = sectionGrids
            .map(grid => grid?.Name)
            .filter(name => name && !availableGridIds.includes(normaliseId(name)));

        if (unknownGrids.length > 0) {
            issues.push({
                type: ISSUE_TYPES.GRID_UNKNOWN,
                message: `Grid(s) ${unknownGrids.join(', ')} are not provided by ${dataSectionName} and will be skipped.`
            });
        }
    }

    if (dataSectionFields.length > 0) {
        const availableFieldIds = dataSectionFields.map(field => normaliseId(field.Value ?? field.value));
        const unknownFields = sectionFields
            .map(field => field?.Value)
            .filter(value => value && !availableFieldIds.includes(normaliseId(value)));

        if (unknownFields.length > 0) {
            issues.push({
                type: ISSUE_TYPES.FIELD_UNKNOWN,
                message: `Field(s) ${unknownFields.join(', ')} are not provided by ${dataSectionName} and will be blank on the report.`
            });
        }
    }

    if (sectionFields.length > 0 && formatColumns.length === 0 && formatGrids.length > 0) {
        issues.push({
            type: ISSUE_TYPES.FIELDS_NOT_SUPPORTED,
            message: `Format "${format.Description || format.Name}" only supports grids, not fields (section has ${sectionFields.length} field(s)).`
        });
    }

    if (formatColumns.length > RENDERED_COLUMN_LIMIT
        && sectionFields.some(field => Number(field?.Column) > RENDERED_COLUMN_LIMIT)) {
        issues.push({
            type: ISSUE_TYPES.COLUMN_LIMIT,
            message: `Only the first ${RENDERED_COLUMN_LIMIT} columns of a format are rendered, so fields placed beyond column ${RENDERED_COLUMN_LIMIT} will not appear on the report.`
        });
    }

    return issues;
};

/**
 * Returns true when section HeadingText should appear on the printed report.
 * Whitespace-only and empty strings mean no section heading.
 * @param {string} text - The section HeadingText value from configuration.
 * @returns {boolean} True when a section heading should be rendered on print.
 */
export const hasSectionHeadingText = (text) => !!(text && String(text).trim());
