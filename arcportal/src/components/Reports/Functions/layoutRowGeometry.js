/**
 * Geometry for placing a layout section's field block and grids side by side on a row.
 *
 * JavaScript twin of arc.common/ExtensionMethods/ReportSectionLayoutExtensions.cs. The designer preview and
 * the printed report must agree to the point, so any change here needs the same change there.
 *
 * A layout section's horizontal geometry normally comes straight from its format, which assumes each area
 * gets the full width to itself. Two areas on one row would therefore overlap: in the seeded
 * DoubleColumnWithTwoGrids format both grid positions are identical, so the second grid would land exactly
 * on top of the first. These helpers divide a row's natural span into one slot per area and rescale each
 * area's own geometry into its slot, never reaching beyond the span the areas already occupied.
 */

/**
 * Stable area type identifiers, matching arc.common LayoutAreaTypes.
 */
export const LAYOUT_AREA_TYPES = {
    FIELDS: 'Fields',
    GRID: 'Grid',
};

/** Gap in points left between two areas that share a row. */
export const ROW_AREA_GUTTER = 10;

/** Narrowest grid column in points. Anything narrower cannot hold a readable value once scaled. */
export const MINIMUM_GRID_COLUMN_WIDTH = 12;

/** Narrowest slot in points an area may be given before the row falls back to stacked rendering. */
export const MINIMUM_AREA_WIDTH = 40;

/** Narrowest value column in points left over after a field column's label column is scaled. */
export const MINIMUM_FIELD_VALUE_WIDTH = 16;

/**
 * Rounds half away from zero, matching Math.Round(MidpointRounding.AwayFromZero) in the C# twin.
 * @param {number} value - The value to round.
 * @returns {number} The rounded value.
 */
const roundAwayFromZero = (value) =>
    value < 0 ? -Math.round(-value) : Math.round(value);

/**
 * Builds the row grouping key that tags every printable entry belonging to one row.
 * @param {string} sectionName - The section the row belongs to.
 * @param {number} rowIndex - Zero based row index within the section.
 * @returns {string} A key unique to this section and row.
 */
export const layoutRowKey = (sectionName, rowIndex) =>
    `${String(sectionName ?? '').trim().replace(/\s/g, '').toLowerCase()}#${rowIndex}`;

/**
 * Reads a pipe delimited grid width definition into its column widths.
 * @param {string} widthDefinition - A pipe delimited width string such as "150|150|80".
 * @returns {Array<number>} The column widths, or an empty array when the definition is unreadable.
 */
export const parseGridWidths = (widthDefinition) => {
    if (widthDefinition === null || widthDefinition === undefined || String(widthDefinition).trim() === '') {
        return [];
    }

    const widths = [];

    for (const segment of String(widthDefinition).split('|')) {
        const trimmed = segment.trim();
        if (trimmed.length === 0) {
            continue;
        }

        const parsed = Number(trimmed);
        if (Number.isNaN(parsed)) {
            return [];
        }

        widths.push(roundAwayFromZero(parsed));
    }

    return widths;
};

/**
 * Adds whatever rounding lost or gained to the widest entry so the widths add up exactly.
 * @param {Array<number>} widths - Widths to correct in place.
 * @param {number} targetTotal - The total the widths must add up to.
 */
const absorbRoundingDrift = (widths, targetTotal) => {
    const drift = targetTotal - widths.reduce((total, width) => total + width, 0);
    if (drift === 0) {
        return;
    }

    let widestIndex = 0;
    for (let index = 1; index < widths.length; index++) {
        if (widths[index] > widths[widestIndex]) {
            widestIndex = index;
        }
    }

    widths[widestIndex] += drift;
};

/**
 * Turns requested width percentages into shares that add up to 100.
 * @param {Array<number>} widthPercents - Requested shares, where zero means an equal share of the remainder.
 * @returns {Array<number>} One share per area, summing to 100.
 */
const normaliseWidthShares = (widthPercents) => {
    const areaCount = widthPercents.length;
    const automaticCount = widthPercents.filter((percent) => !(percent > 0)).length;

    if (automaticCount === areaCount) {
        return widthPercents.map(() => 100 / areaCount);
    }

    const explicitTotal = widthPercents
        .filter((percent) => percent > 0)
        .reduce((total, percent) => total + Math.min(percent, 100), 0);

    if (automaticCount === 0) {
        return widthPercents.map((percent) => (100 * Math.min(percent, 100)) / explicitTotal);
    }

    const cappedExplicitTotal = Math.min(explicitTotal, 100 - automaticCount);
    const automaticShare = (100 - cappedExplicitTotal) / automaticCount;

    return widthPercents.map((percent) =>
        percent > 0
            ? (cappedExplicitTotal * Math.min(percent, 100)) / explicitTotal
            : automaticShare);
};

/**
 * Divides a row's horizontal span into one slot per area.
 * @param {number} spanLeft - Leftmost point of the row span, taken from the areas' natural geometry.
 * @param {number} spanRight - First point beyond the right edge of the row span.
 * @param {Array<number>} widthPercents - Each area's requested share of the row, where zero means an equal share.
 * @returns {Array<{Left: number, Width: number}>|null} One slot per area ordered left to right, or null when
 * the span cannot give every area at least MINIMUM_AREA_WIDTH points.
 */
export const computeRowSlots = (spanLeft, spanRight, widthPercents) => {
    const areaCount = widthPercents?.length ?? 0;
    if (areaCount === 0) {
        return [];
    }

    const availableWidth = spanRight - spanLeft - ROW_AREA_GUTTER * (areaCount - 1);
    if (availableWidth < MINIMUM_AREA_WIDTH * areaCount) {
        return null;
    }

    const shares = normaliseWidthShares(widthPercents);
    const widths = shares.map((share) => roundAwayFromZero((availableWidth * share) / 100));

    absorbRoundingDrift(widths, availableWidth);

    if (widths.some((width) => width < MINIMUM_AREA_WIDTH)) {
        return null;
    }

    const slots = [];
    let left = spanLeft;

    for (let index = 0; index < areaCount; index++) {
        slots.push({ Left: left, Width: widths[index] });
        left += widths[index] + ROW_AREA_GUTTER;
    }

    return slots;
};

/**
 * Rescales a pipe delimited grid width definition so its columns add up to a target width.
 * @param {string} widthDefinition - Natural column widths from the format, such as "150|150|80".
 * @param {number} targetWidth - Total width in points the scaled columns must add up to.
 * @returns {string|null} Scaled pipe delimited widths summing exactly to targetWidth, or null when any column
 * would fall below MINIMUM_GRID_COLUMN_WIDTH.
 */
export const scaleGridWidthDefinition = (widthDefinition, targetWidth) => {
    const naturalWidths = parseGridWidths(widthDefinition);
    if (naturalWidths.length === 0 || targetWidth <= 0) {
        return null;
    }

    const naturalTotal = naturalWidths.reduce((total, width) => total + width, 0);
    if (naturalTotal <= 0 || targetWidth < MINIMUM_GRID_COLUMN_WIDTH * naturalWidths.length) {
        return null;
    }

    const scaled = naturalWidths.map((width) =>
        roundAwayFromZero((targetWidth * width) / naturalTotal));

    absorbRoundingDrift(scaled, targetWidth);

    return scaled.some((width) => width < MINIMUM_GRID_COLUMN_WIDTH)
        ? null
        : scaled.join('|');
};

/**
 * Rescales one field column's geometry into the slot its field block was given on a shared row.
 * @param {object} column - The column's natural geometry, with Left, Width and LabelWidth in points.
 * @param {number} naturalSpanLeft - Leftmost point of the whole field block's natural geometry.
 * @param {number} naturalSpanWidth - Width of the whole field block's natural geometry.
 * @param {{Left: number, Width: number}} slot - The slot the field block was allocated on the row.
 * @returns {{Left: number, Width: number, LabelWidth: number}} The column's geometry inside the slot, keeping
 * the relative spacing of the format's columns so a two column block stays two columns, just narrower.
 */
export const scaleFieldColumn = (column, naturalSpanLeft, naturalSpanWidth, slot) => {
    const naturalLeft = Number(column?.Left ?? 0);
    const naturalWidth = Number(column?.Width ?? 0);
    const naturalLabelWidth = Number(column?.LabelWidth ?? 0);

    if (!slot || naturalSpanWidth <= 0) {
        return { Left: naturalLeft, Width: naturalWidth, LabelWidth: naturalLabelWidth };
    }

    const scale = slot.Width / naturalSpanWidth;
    const scaledWidth = Math.max(
        MINIMUM_FIELD_VALUE_WIDTH + 1,
        roundAwayFromZero(naturalWidth * scale)
    );
    const scaledLabelWidth = roundAwayFromZero(naturalLabelWidth * scale);

    return {
        Left: slot.Left + roundAwayFromZero((naturalLeft - naturalSpanLeft) * scale),
        Width: scaledWidth,
        LabelWidth: Math.min(
            Math.max(scaledLabelWidth, 1),
            scaledWidth - MINIMUM_FIELD_VALUE_WIDTH
        ),
    };
};

/**
 * Reads the horizontal extent a printable entry occupies before any row scaling.
 * @param {object} entry - A field block or grid ContentsConfig entry.
 * @returns {{Left: number, Width: number}} The entry's left edge and width in points.
 */
export const getNaturalBounds = (entry) => {
    const columns = [entry?.Column1, entry?.Column2].filter((column) => column);

    if (columns.length > 0) {
        const left = Math.min(...columns.map((column) => Number(column.Left ?? 0)));
        const right = Math.max(...columns.map((column) => Number(column.Left ?? 0) + Number(column.Width ?? 0)));

        return { Left: left, Width: right - left };
    }

    return {
        Left: Number(entry?.Left ?? 0),
        Width: parseGridWidths(entry?.Width).reduce((total, width) => total + width, 0),
    };
};
