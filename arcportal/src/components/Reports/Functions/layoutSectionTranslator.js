/**
 * Translates layout section designer state into printable ContentsConfig entries.
 * Mirrors ReportTranslator.TranslateSectionGroupAsync for a single section.
 */

import {
    computeRowSlots,
    getNaturalBounds,
    layoutRowKey,
    MINIMUM_GRID_COLUMN_WIDTH,
    scaleFieldColumn,
    scaleGridWidthDefinition,
} from './layoutRowGeometry';
import {
    hasSideBySideRow,
    isFieldsArea,
    normaliseAreaId,
    resolveLayoutRows,
} from './layoutRowResolver';

/**
 * Normalises a grid or field identifier for id-based comparison.
 * @param {string} id - The identifier to normalise.
 * @returns {string} Trimmed, space-stripped, lowercased id.
 */
const normaliseId = (id) => String(id ?? '').trim().replace(/\s/g, '').toLowerCase();

/**
 * Returns true when HeadingText should be rendered as a section heading.
 * @param {string} headingText - Section HeadingText value.
 * @returns {boolean} True when non-empty after trim.
 */
const hasSectionHeading = (headingText) =>
    String(headingText ?? '').trim().length > 0;

/**
 * Normalises HeadingText for persistence and preview.
 * @param {string} headingText - Raw heading text.
 * @returns {string} Trimmed text or empty string.
 */
const normalizeSectionHeading = (headingText) =>
    hasSectionHeading(headingText) ? String(headingText).trim() : '';

/**
 * True when the format defines a heading slot (non-empty Heading array).
 * @param {object} format - Custom format model from the designer.
 * @returns {boolean} Whether format heading geometry applies.
 */
export const formatHeadingEnabled = (format) =>
    (format?.Heading?.length ?? 0) > 0;

/**
 * Builds the printable heading line from section text and format geometry.
 * @param {string} headingText - Section HeadingText.
 * @param {object} format - Custom format model from the designer.
 * @returns {object|null} Heading line config or null when no heading text.
 */
export const buildPrintHeadingLine = (headingText, format) => {
    const text = normalizeSectionHeading(headingText);
    if (!hasSectionHeading(text)) {
        return null;
    }

    const enabled = formatHeadingEnabled(format);
    const formatHeading = enabled ? format.Heading[0] : null;

    return {
        Line: enabled && formatHeading?.Line > 0 ? formatHeading.Line : 1,
        Left: enabled && formatHeading?.Left > 0 ? formatHeading.Left : 20,
        Text: text,
        FontSize: enabled && formatHeading?.FontSize > 0 ? formatHeading.FontSize : 14,
        Bold: enabled ? !!formatHeading?.Bold : true,
    };
};

/**
 * Reads a numeric column index from a field binding.
 * @param {object} field - Section field binding.
 * @returns {number} 1-based column index, or 0 when absent.
 */
const fieldColumnNumber = (field) => {
    const column = Number(field?.Column ?? field?.column ?? 0);
    return column > 0 ? column : 1;
};

/**
 * Maps section field bindings into a column configuration.
 * @param {Array} fields - Section Fields array.
 * @param {number} columnNumber - 1-based column index.
 * @param {number} left - Column left position in points.
 * @param {number} width - Column width in points.
 * @param {number} labelWidth - Label width in points.
 * @returns {object} Column config with Fields array.
 */
export const defineColumn = (fields, columnNumber, left, width, labelWidth) => ({
    Left: left,
    Width: width,
    LabelWidth: labelWidth,
    Fields: (fields || [])
        .filter((f) => fieldColumnNumber(f) === columnNumber)
        .sort((a, b) => Number(a.Order ?? a.order ?? 0) - Number(b.Order ?? b.order ?? 0))
        .map((f) => ({
            Label: f.Label ?? f.label,
            Value: f.Value ?? f.value,
            Image: f.Image ?? f.image,
            Width: f.Width ?? f.width,
            Height: f.Height ?? f.height,
            Format: f.Format ?? f.format,
            Text: f.Text ?? f.text,
            NoBox: !!(f.NoBox ?? f.noBox),
        })),
});

/**
 * Picks format grid geometry for a section grid binding index.
 * @param {Array} formatGrids - Format Grids array.
 * @param {number} gridIndex - Zero-based binding index.
 * @returns {object|null} Format grid or null when none defined.
 */
export const getFormatGridForIndex = (formatGrids, gridIndex) => {
    if (!formatGrids || formatGrids.length === 0) {
        return null;
    }

    return gridIndex < formatGrids.length
        ? formatGrids[gridIndex]
        : formatGrids[formatGrids.length - 1];
};

/**
 * Normalises a grid width pipe-separated string to integer segments.
 * @param {string} widthDefinition - Raw width string (e.g. "300|80").
 * @param {number} headingCount - Number of column headings.
 * @returns {string} Normalised width string.
 */
export const normalizeGridWidth = (widthDefinition, headingCount) => {
    if (!widthDefinition || String(widthDefinition).trim() === '') {
        return widthDefinition;
    }

    const parsedWidths = String(widthDefinition)
        .split('|')
        .map((segment) => segment.trim())
        .filter((segment) => segment.length > 0)
        .map((segment) => {
            const parsed = parseInt(segment, 10);
            return Number.isNaN(parsed) ? null : parsed;
        })
        .filter((value) => value !== null);

    if (headingCount > 0 && parsedWidths.length > 0 && headingCount !== parsedWidths.length) {
        // Width/heading mismatch is non-fatal for preview; backend logs this at Info level.
    }

    return parsedWidths.length > 0 ? parsedWidths.join('|') : widthDefinition;
};

/**
 * Finds a data section grid by stable id (Name), never by description.
 * @param {Array} dataSectionGrids - Grids from the data section definition.
 * @param {string} sectionGridName - Section grid binding Name.
 * @returns {object|undefined} Matching data grid or undefined.
 */
export const findDataGridById = (dataSectionGrids, sectionGridName) =>
    (dataSectionGrids || []).find(
        (grid) => normaliseId(grid.name ?? grid.Name) === normaliseId(sectionGridName)
    );

/**
 * Translates designer section state into printable ContentsConfig entries.
 * @param {object} params - Translation inputs.
 * @param {object} params.section - Section definition from editor state.
 * @param {object} params.format - Resolved custom format model.
 * @param {object|null} params.dataSection - Data section definition.
 * @returns {{ contents: object[], warnings: string[] }} Translated contents and non-fatal warnings.
 */
export const translateLayoutSectionToContents = ({ section, format, dataSection }) => {
    const warnings = [];

    if (!format || !format.Type) {
        return { contents: [], warnings: ['No format selected'] };
    }

    const printHeading = buildPrintHeadingLine(section?.HeadingText, format);
    const sectionGrids = section?.Grids || [];
    const boundGridCount = sectionGrids.length;
    const formatType = format.Type ?? format.type;
    const formatColumns = format.Columns ?? format.columns ?? [];

    const newConfig = {
        Name: section?.Name || 'PreviewSection',
        Heading: printHeading
            ? [{
                Line: printHeading.Line,
                Left: printHeading.Left,
                Text: printHeading.Text,
                FontSize: printHeading.FontSize,
                Bold: printHeading.Bold,
            }]
            : null,
        Type: formatType,
        Dynamic: section?.Dynamic || false,
        Separator: section?.Separator,
    };

    if (formatColumns.length > 0) {
        formatColumns.forEach((column, index) => {
            const columnNumber = index + 1;
            const columnConfig = defineColumn(
                section?.Fields || [],
                columnNumber,
                column.Left ?? column.left,
                column.Width ?? column.width,
                column.LabelWidth ?? column.labelwidth ?? column.labelWidth
            );

            if (columnNumber === 1) {
                newConfig.Column1 = columnConfig;
            } else if (columnNumber === 2) {
                newConfig.Column2 = columnConfig;
            }
        });

        const placedValues = new Set();
        [newConfig.Column1, newConfig.Column2].forEach((columnConfig) => {
            (columnConfig?.Fields || []).forEach((field) => {
                if (field?.Value) {
                    placedValues.add(String(field.Value));
                }
            });
        });

        const unplacedFields = (section?.Fields || []).filter((field) => {
            const value = field?.Value ?? field?.value;
            return value && !placedValues.has(String(value));
        });

        if (unplacedFields.length > 0 && newConfig.Column1) {
            const fallbackColumn = formatColumns[0];
            unplacedFields
                .sort((a, b) => Number(a.Order ?? a.order ?? 0) - Number(b.Order ?? b.order ?? 0))
                .forEach((field) => {
                    newConfig.Column1.Fields.push({
                        Label: field.Label ?? field.label,
                        Value: field.Value ?? field.value,
                        Image: field.Image ?? field.image,
                        Width: field.Width ?? field.width,
                        Height: field.Height ?? field.height,
                        Format: field.Format ?? field.format,
                        Text: field.Text ?? field.text,
                        NoBox: !!(field.NoBox ?? field.noBox),
                    });
                });

            if (fallbackColumn) {
                newConfig.Column1.Left = newConfig.Column1.Left ?? fallbackColumn.Left ?? fallbackColumn.left;
                newConfig.Column1.Width = newConfig.Column1.Width ?? fallbackColumn.Width ?? fallbackColumn.width;
                newConfig.Column1.LabelWidth = newConfig.Column1.LabelWidth
                    ?? fallbackColumn.LabelWidth
                    ?? fallbackColumn.labelwidth
                    ?? fallbackColumn.labelWidth;
            }
        }
    }

    if (format.Grids && boundGridCount > 0 && formatType !== 'Table') {
        newConfig.LinkedSections = boundGridCount;
    }

    if (section?.Lines?.length > 0) {
        newConfig.Lines = section.Lines;
    }

    if (section?.Images?.length > 0) {
        newConfig.Images = section.Images;
    }

    const contents = [];
    const gridEntriesByGridId = new Map();
    let thisSectionOnlyContainsATable = true;

    if (formatType !== 'Table') {
        contents.push(newConfig);
        thisSectionOnlyContainsATable = false;
    }

    const dataSectionGrids = dataSection?.Grids || [];
    const formatGrids = format.Grids || [];

    if (formatGrids.length > 0 || sectionGrids.length > 0) {
        if (dataSectionGrids.length > sectionGrids.length) {
            warnings.push(
                `Section places ${sectionGrids.length} of ${dataSectionGrids.length} grid(s) offered by data section`
            );
        }

        if (formatGrids.length > 0 && sectionGrids.length > formatGrids.length) {
            warnings.push(
                `Section binds ${sectionGrids.length} grid(s) but format defines ${formatGrids.length} position(s)`
            );
        }

        for (let gridIndex = 0; gridIndex < sectionGrids.length; gridIndex++) {
            const formatGrid = getFormatGridForIndex(formatGrids, gridIndex);
            const sectionGrid = sectionGrids[gridIndex];

            if (!formatGrid) {
                warnings.push(`Format defines no grid positions; unable to place grid '${sectionGrid.Name}'`);
                continue;
            }

            const dataGridDefinition = findDataGridById(dataSectionGrids, sectionGrid.Name);
            if (!dataGridDefinition) {
                warnings.push(`Unable to find data grid '${sectionGrid.Name}' in data section`);
                continue;
            }

            const rawHeadings = sectionGrid.Head || [];
            const hasAnyHeading = rawHeadings.some((h) => String(h ?? '').trim().length > 0);
            const columnHeadings = hasAnyHeading
                ? rawHeadings.map((h) => String(h ?? '').trim())
                : [];

            const gridWidth = formatGrid.Width ?? formatGrid.width;
            const normalizedWidth = normalizeGridWidth(gridWidth, columnHeadings.length);
            const dataKey = dataGridDefinition.data ?? dataGridDefinition.Data;
            const gridName = dataGridDefinition.name ?? dataGridDefinition.Name;
            const description = section?.Description || '';

            const gridConfig = {
                Name: gridName,
                Data: dataKey,
                Left: formatGrid.Left ?? formatGrid.left,
                Width: normalizedWidth,
                Type: 'Table',
                Theme: 'grid',
                Group: null,
                Head: columnHeadings.length > 0 ? columnHeadings : null,
                Colour: description.includes('Comments') ? 'manatee' : '#287fba',
                NoBox: !!(sectionGrid.NoBox ?? sectionGrid.noBox),
            };

            if (thisSectionOnlyContainsATable && gridIndex === 0 && printHeading) {
                gridConfig.Heading = newConfig.Heading;
            }

            contents.push(gridConfig);
            gridEntriesByGridId.set(normaliseAreaId(sectionGrid.Name ?? sectionGrid.name), gridConfig);
        }
    }

    const arranged = applyLayoutRows({
        section,
        fieldBlockEntry: thisSectionOnlyContainsATable ? null : newConfig,
        gridEntriesByGridId,
        contents,
        warnings,
    });

    return { contents: arranged, warnings };
};

/**
 * Arranges a section's printable entries into its area rows, rewriting the geometry of any areas that share a
 * row so they sit side by side instead of on top of one another.
 * @param {object} params - Arrangement inputs.
 * @param {object} params.section - Section definition from editor state.
 * @param {object|null} params.fieldBlockEntry - The section's field block entry, or null when grid-only.
 * @param {Map<string, object>} params.gridEntriesByGridId - Grid entries keyed by normalised section grid id.
 * @param {Array<object>} params.contents - The entries in their default stacked order.
 * @param {Array<string>} params.warnings - Non-fatal warnings to add to.
 * @returns {Array<object>} The entries in row order, untouched when no row holds more than one area.
 */
const applyLayoutRows = ({ section, fieldBlockEntry, gridEntriesByGridId, contents, warnings }) => {
    const resolvedRows = resolveLayoutRows(
        section?.LayoutRows ?? section?.layoutRows,
        section?.Grids || [],
        fieldBlockEntry !== null
    );

    if (!hasSideBySideRow(resolvedRows)) {
        return contents;
    }

    const arranged = [];

    resolvedRows.forEach((row, rowIndex) => {
        const rowEntries = [];
        const rowPercents = [];

        row.Areas.forEach((area) => {
            const entry = isFieldsArea(area.Type)
                ? fieldBlockEntry
                : gridEntriesByGridId.get(normaliseAreaId(area.Name));

            if (!entry) {
                return;
            }

            rowEntries.push(entry);
            rowPercents.push(area.WidthPercent);
        });

        if (rowEntries.length === 0) {
            return;
        }

        arranged.push(...rowEntries);

        if (rowEntries.length > 1) {
            applyRowGeometry({
                section,
                rowIndex,
                rowEntries,
                rowPercents,
                fieldBlockEntry,
                warnings,
            });
        }
    });

    // Anything the arrangement did not mention still has to render, or a grid would silently disappear.
    arranged.push(...contents.filter((entry) => !arranged.includes(entry)));

    moveGridOnlyHeadingToFirstEntry(fieldBlockEntry, contents, arranged);

    return arranged;
};

/**
 * Rewrites the geometry of the areas on one row so they divide the row's natural span between them.
 * @param {object} params - Row inputs.
 * @param {object} params.section - Section definition from editor state.
 * @param {number} params.rowIndex - Zero based index of the row within the section.
 * @param {Array<object>} params.rowEntries - The entries sharing the row, ordered left to right.
 * @param {Array<number>} params.rowPercents - Each entry's requested share of the row width.
 * @param {object|null} params.fieldBlockEntry - The section's field block entry.
 * @param {Array<string>} params.warnings - Non-fatal warnings to add to.
 * @remarks
 * Every value is staged and only committed once the whole row is known to fit, so a row that cannot be laid
 * out is left completely untouched and falls back to stacked rendering rather than rendering half rescaled.
 */
const applyRowGeometry = ({ section, rowIndex, rowEntries, rowPercents, fieldBlockEntry, warnings }) => {
    const naturalBounds = rowEntries.map(getNaturalBounds);
    const spanLeft = Math.min(...naturalBounds.map((bounds) => bounds.Left));
    const spanRight = Math.max(...naturalBounds.map((bounds) => bounds.Left + bounds.Width));

    const slots = computeRowSlots(spanLeft, spanRight, rowPercents);
    if (!slots) {
        warnings.push(
            `Row ${rowIndex + 1} cannot fit ${rowEntries.length} areas across ${spanRight - spanLeft}pt; areas remain stacked`
        );
        return;
    }

    const scaledGridWidths = rowEntries.map((entry, index) =>
        entry === fieldBlockEntry ? null : scaleGridWidthDefinition(entry.Width, slots[index].Width));

    const unscalableIndex = rowEntries.findIndex((entry, index) =>
        entry !== fieldBlockEntry && scaledGridWidths[index] === null);

    if (unscalableIndex >= 0) {
        warnings.push(
            `Row ${rowIndex + 1} grid '${rowEntries[unscalableIndex].Name}' cannot be narrowed to ${slots[unscalableIndex].Width}pt without a column below ${MINIMUM_GRID_COLUMN_WIDTH}pt; areas remain stacked`
        );
        return;
    }

    const rowKey = layoutRowKey(section?.Name, rowIndex);

    rowEntries.forEach((entry, index) => {
        entry.LayoutRowKey = rowKey;
        entry.LayoutRowAreaCount = rowEntries.length;

        if (entry === fieldBlockEntry) {
            [1, 2].forEach((columnNumber) => {
                const column = entry[`Column${columnNumber}`];
                if (!column) {
                    return;
                }

                const scaled = scaleFieldColumn(
                    column,
                    naturalBounds[index].Left,
                    naturalBounds[index].Width,
                    slots[index]
                );
                column.Left = scaled.Left;
                column.Width = scaled.Width;
                column.LabelWidth = scaled.LabelWidth;
            });

            // The deferred heading hands a mixed section's heading to its first grid, which assumes the grid
            // prints below the fields. On a shared row the heading belongs with the field block.
            entry.LinkedSections = 0;
            return;
        }

        entry.Left = slots[index].Left;
        entry.Width = scaledGridWidths[index];
    });
};

/**
 * Moves a grid-only section's heading onto whichever grid the arrangement put first.
 * @param {object|null} fieldBlockEntry - The field block entry, or null when the section is grid-only.
 * @param {Array<object>} contents - The entries in their default stacked order.
 * @param {Array<object>} arranged - The entries in row order.
 */
const moveGridOnlyHeadingToFirstEntry = (fieldBlockEntry, contents, arranged) => {
    if (fieldBlockEntry !== null || contents.length === 0 || arranged.length === 0) {
        return;
    }

    const headingHolder = contents[0];
    if (!headingHolder.Heading || headingHolder === arranged[0]) {
        return;
    }

    arranged[0].Heading = headingHolder.Heading;
    headingHolder.Heading = null;
};
