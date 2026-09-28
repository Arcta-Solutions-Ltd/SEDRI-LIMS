/**
 * Pure helpers for absolute section line grouping, counting, and reordering.
 * A "line" is a vertical row identified by numeric Line; each line may contain
 * multiple horizontal elements sharing the same Line value.
 */

/**
 * Returns the count of distinct vertical line numbers in a lines array.
 * @param {Array<{ Line: number }>} lines - Flat array of line elements.
 * @returns {number} Number of distinct Line values.
 */
export const getDistinctLineCount = (lines) => {
    if (!lines || lines.length === 0) {
        return 0;
    }
    return new Set(lines.map((l) => l.Line)).size;
};

/**
 * Returns distinct line numbers sorted ascending.
 * @param {Array<{ Line: number }>} lines - Flat array of line elements.
 * @returns {number[]} Sorted distinct Line values.
 */
export const getSortedLineNumbers = (lines) => {
    if (!lines || lines.length === 0) {
        return [];
    }
    return [...new Set(lines.map((l) => l.Line))].sort((a, b) => a - b);
};

/**
 * Builds a mapping from old line numbers to new sequential line numbers (1..n)
 * based on the desired order of line groups.
 * @param {number[]} orderedLineNumbers - Line numbers in desired vertical order.
 * @returns {Object<number, number>} Map of old Line → new Line.
 */
const buildLineNumberMap = (orderedLineNumbers) => {
    const map = {};
    orderedLineNumbers.forEach((oldLineNumber, index) => {
        map[oldLineNumber] = index + 1;
    });
    return map;
};

/**
 * Reorders an array of distinct line numbers by moving one group relative to another.
 * When moving down, the dragged line is inserted after the target.
 * @param {number[]} orderedLineNumbers - Current sorted line numbers.
 * @param {number} fromLineNumber - Line number being dragged.
 * @param {number} targetLineNumber - Line number to drop onto.
 * @returns {number[]} Reordered line numbers (not yet renumbered to 1..n).
 */
const reorderLineNumberList = (orderedLineNumbers, fromLineNumber, targetLineNumber) => {
    const fromIndex = orderedLineNumbers.indexOf(fromLineNumber);
    const targetIndex = orderedLineNumbers.indexOf(targetLineNumber);

    if (fromIndex === -1 || targetIndex === -1 || fromIndex === targetIndex) {
        return orderedLineNumbers;
    }

    const result = [...orderedLineNumbers];
    const [removed] = result.splice(fromIndex, 1);
    const insertIndex = fromIndex < targetIndex ? targetIndex : targetIndex;
    result.splice(insertIndex, 0, removed);
    return result;
};

/**
 * Reorders vertical line groups and remaps all element Line values to sequential 1..n.
 * @param {Array<{ Line: number }>} lines - Flat array of line elements.
 * @param {number} fromLineNumber - Line number being dragged.
 * @param {number} targetLineNumber - Line number to drop onto.
 * @returns {{ lines: Array, lineNumberMap: Object<number, number> }} Updated lines and old→new map.
 */
export const reorderLineGroups = (lines, fromLineNumber, targetLineNumber) => {
    const orderedLineNumbers = getSortedLineNumbers(lines);
    const reordered = reorderLineNumberList(orderedLineNumbers, fromLineNumber, targetLineNumber);
    const lineNumberMap = buildLineNumberMap(reordered);

    const updatedLines = lines.map((element) => ({
        ...element,
        Line: lineNumberMap[element.Line]
    }));

    return { lines: updatedLines, lineNumberMap };
};

/**
 * Remaps collapsed line numbers after a reorder/renumber operation.
 * @param {Set<number>} collapsedLines - Set of collapsed line numbers.
 * @param {Object<number, number>} lineNumberMap - Map of old Line → new Line.
 * @returns {Set<number>} Updated collapse state.
 */
export const remapCollapsedLineNumbers = (collapsedLines, lineNumberMap) => {
    const remapped = new Set();
    collapsedLines.forEach((oldLineNumber) => {
        const newLineNumber = lineNumberMap[oldLineNumber];
        if (newLineNumber !== undefined) {
            remapped.add(newLineNumber);
        }
    });
    return remapped;
};
