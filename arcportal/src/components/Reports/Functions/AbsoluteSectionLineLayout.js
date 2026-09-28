import { getEffectiveFontSize } from '../ReportPdfTextMetrics';

/** Line spacing between absolute section rows (matches LineWriter.js). */
export const ABSOLUTE_LINE_SPACING = 4;

/** Bottom margin added after the last line (matches LineWriter.js). */
export const ABSOLUTE_MARGIN_BOTTOM = 7.5;

/** Default font size when FontSize is omitted or 0 (matches LineWriter.js). */
export const ABSOLUTE_DEFAULT_FONT_SIZE = 11;

/** Space before the horizontal rule below a header/footer section. */
export const ABSOLUTE_HORIZONTAL_RULE_SPACING = 10;

/**
 * Returns line height in points for a font size, matching jsPDF getLineHeight() (fontSize * 1.15).
 * @param {number|undefined|null} fontSize - Font size in points.
 * @param {number} [defaultFontSize=ABSOLUTE_DEFAULT_FONT_SIZE] - Fallback when fontSize is 0 or omitted.
 * @returns {number} Line height in points.
 */
export function getAbsoluteLineHeight(fontSize, defaultFontSize = ABSOLUTE_DEFAULT_FONT_SIZE) {
    const effectiveFontSize = getEffectiveFontSize(fontSize ?? defaultFontSize);
    return effectiveFontSize * 1.15;
}

/**
 * Sorts line elements in the same order as LineWriter / SortLines (line asc, left asc).
 * @param {Object[]} lines - Line element definitions.
 * @returns {Object[]} Sorted copy of lines.
 */
export function sortAbsoluteSectionLines(lines) {
    if (!lines || !Array.isArray(lines)) {
        return [];
    }

    return [...lines].sort((a, b) => {
        if (a.Line !== b.Line) {
            return a.Line - b.Line;
        }
        return (a.Left || 0) - (b.Left || 0);
    });
}

/**
 * Computes the final baseline Y after writing all lines, matching LineWriter.writeLines().
 * @param {Object[]} lines - Line element definitions.
 * @param {Object} [options] - Layout options.
 * @param {number} [options.startingY=0] - Initial baseline Y in points.
 * @param {number} [options.lineSpacing=ABSOLUTE_LINE_SPACING] - Spacing between lines.
 * @param {number} [options.marginBottom=ABSOLUTE_MARGIN_BOTTOM] - Trailing margin after last line.
 * @param {number} [options.defaultFontSize=ABSOLUTE_DEFAULT_FONT_SIZE] - Default font size.
 * @param {boolean} [options.invertYDirection=false] - Whether Y decreases for each new line (footers).
 * @param {number|null} [options.initialLineNumber=null] - Starting line number (defaults to min Line).
 * @returns {number} Final baseline Y in points after margin handling.
 */
export function calculateAbsoluteSectionFinalBaselineY(lines, options = {}) {
    const {
        startingY = 0,
        lineSpacing = ABSOLUTE_LINE_SPACING,
        marginBottom = ABSOLUTE_MARGIN_BOTTOM,
        defaultFontSize = ABSOLUTE_DEFAULT_FONT_SIZE,
        invertYDirection = false,
        initialLineNumber = null,
    } = options;

    if (!lines || lines.length === 0) {
        return startingY;
    }

    const sortedLines = sortAbsoluteSectionLines(lines);
    const firstLineNumber = initialLineNumber ?? Math.min(...lines.map((line) => line.Line));
    let currentLine = firstLineNumber;
    let currentLinePosition = startingY;

    for (const line of sortedLines) {
        if (currentLine !== line.Line) {
            const fontSize =
                !line.FontSize || line.FontSize === 0 ? defaultFontSize : line.FontSize;
            const lineHeight = getAbsoluteLineHeight(fontSize, defaultFontSize);

            if (invertYDirection) {
                currentLinePosition -= lineHeight + lineSpacing;
            } else {
                currentLinePosition += lineHeight + lineSpacing;
            }
        }

        currentLine = line.Line;
    }

    if (invertYDirection) {
        const lastLine = sortedLines[sortedLines.length - 1];
        const fontSize =
            !lastLine.FontSize || lastLine.FontSize === 0 ? defaultFontSize : lastLine.FontSize;
        currentLinePosition -= getAbsoluteLineHeight(fontSize, defaultFontSize) + lineSpacing;
    } else {
        currentLinePosition += marginBottom;
    }

    return currentLinePosition;
}

/**
 * Computes baseline Y positions for each line element using the same algorithm as LineWriter.writeLines().
 * @param {Object[]} lines - Line element definitions in designer/storage order.
 * @param {Object} [options] - Layout options.
 * @param {number} [options.startingY=0] - Initial baseline Y in points.
 * @param {number} [options.lineSpacing=ABSOLUTE_LINE_SPACING] - Spacing between lines.
 * @param {number} [options.defaultFontSize=ABSOLUTE_DEFAULT_FONT_SIZE] - Default font size.
 * @param {boolean} [options.invertYDirection=false] - Whether Y decreases for each new line (footers).
 * @param {number|null} [options.initialLineNumber=null] - Starting line number (defaults to min Line).
 * @returns {Array<{ index: number, lineNumber: number, baselineY: number, fontSize: number }>}
 */
export function calculateAbsoluteSectionBaselines(lines, options = {}) {
    const {
        startingY = 0,
        lineSpacing = ABSOLUTE_LINE_SPACING,
        defaultFontSize = ABSOLUTE_DEFAULT_FONT_SIZE,
        invertYDirection = false,
        initialLineNumber = null,
    } = options;

    if (!lines || lines.length === 0) {
        return [];
    }

    const sortedEntries = lines
        .map((line, index) => ({ line, index }))
        .sort((a, b) => {
            if (a.line.Line !== b.line.Line) {
                return a.line.Line - b.line.Line;
            }
            return (a.line.Left || 0) - (b.line.Left || 0);
        });

    const firstLineNumber = initialLineNumber ?? Math.min(...lines.map((line) => line.Line));
    let currentLine = firstLineNumber;
    let baselineY = startingY;
    const results = new Array(lines.length);

    for (const { line, index } of sortedEntries) {
        if (currentLine !== line.Line) {
            const fontSize =
                !line.FontSize || line.FontSize === 0 ? defaultFontSize : line.FontSize;
            const lineHeight = getAbsoluteLineHeight(fontSize, defaultFontSize);

            if (invertYDirection) {
                baselineY -= lineHeight + lineSpacing;
            } else {
                baselineY += lineHeight + lineSpacing;
            }
        }

        const fontSize =
            !line.FontSize || line.FontSize === 0 ? defaultFontSize : line.FontSize;

        results[index] = {
            index,
            lineNumber: line.Line,
            baselineY,
            fontSize,
        };

        currentLine = line.Line;
    }

    return results;
}

/**
 * Returns total section height in points (content bottom + horizontal rule spacing + optional padding).
 * Mirrors writeHeader / writeFooter height calculation.
 * @param {Object[]} lines - Line element definitions.
 * @param {Object[]} [images=[]] - Image placements with Y and Height.
 * @param {Object} [options] - Layout options.
 * @param {number} [options.startingY=0] - Initial baseline Y in points.
 * @param {number} [options.lineSpacing=ABSOLUTE_LINE_SPACING] - Spacing between lines.
 * @param {number} [options.marginBottom=ABSOLUTE_MARGIN_BOTTOM] - Trailing margin after last line.
 * @param {number} [options.horizontalRuleSpacing=ABSOLUTE_HORIZONTAL_RULE_SPACING] - Space before rule.
 * @param {number} [options.bottomPadding=0] - Extra padding below the rule (preview visual clearance).
 * @param {number} [options.defaultFontSize=ABSOLUTE_DEFAULT_FONT_SIZE] - Default font size.
 * @param {boolean} [options.invertYDirection=false] - Whether Y decreases for each new line (footers).
 * @returns {number} Total section height in points.
 */
export function calculateAbsoluteSectionHeight(lines, images = [], options = {}) {
    const {
        startingY = 0,
        lineSpacing = ABSOLUTE_LINE_SPACING,
        marginBottom = ABSOLUTE_MARGIN_BOTTOM,
        horizontalRuleSpacing = ABSOLUTE_HORIZONTAL_RULE_SPACING,
        bottomPadding = 0,
        defaultFontSize = ABSOLUTE_DEFAULT_FONT_SIZE,
        invertYDirection = false,
    } = options;

    let maxBottom = calculateAbsoluteSectionFinalBaselineY(lines, {
        startingY,
        lineSpacing,
        marginBottom,
        defaultFontSize,
        invertYDirection,
    });

    if (images && images.length > 0) {
        const maxImageBottom = Math.max(
            ...images.map((image) => (image.Y || 0) + (image.Height || 0))
        );
        maxBottom = Math.max(maxBottom, maxImageBottom);
    }

    return maxBottom + horizontalRuleSpacing + bottomPadding;
}

/**
 * Returns the Y coordinate where the horizontal rule is drawn (without bottom padding).
 * @param {Object[]} lines - Line element definitions.
 * @param {Object[]} [images=[]] - Image placements with Y and Height.
 * @param {Object} [options] - Layout options (same as calculateAbsoluteSectionHeight).
 * @returns {number} Section boundary Y in points.
 */
export function calculateAbsoluteSectionBoundaryY(lines, images = [], options = {}) {
    return calculateAbsoluteSectionHeight(lines, images, {
        ...options,
        bottomPadding: 0,
    });
}
