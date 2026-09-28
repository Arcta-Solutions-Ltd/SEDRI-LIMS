/**
 * Pure helpers for absolute section element grouping, reordering, and collapse remapping.
 * An element is one entry in the flat Lines[] array; horizontal position is controlled by Left.
 */

/**
 * Builds a stable identity key for an element (excludes Line and Left which change during drag).
 * @param {{ Field?: string, Text?: string, FontSize?: number, Bold?: boolean }} element - Line element.
 * @returns {string} Identity key.
 */
const buildElementIdentityKey = (element) =>
    `${element.Field || ''}|${element.Text || ''}|${element.FontSize ?? ''}|${element.Bold ?? false}`;

/**
 * Returns elements on a line sorted by Left ascending, each annotated with originalIndex.
 * @param {Array<{ Line: number, Left: number }>} lines - Flat array of line elements.
 * @param {number} lineNumber - Line number to filter by.
 * @returns {Array<{ originalIndex: number }>} Sorted elements with originalIndex.
 */
export const getElementsOnLineSorted = (lines, lineNumber) => {
    if (!lines || lines.length === 0) {
        return [];
    }

    return lines
        .map((element, index) => ({ ...element, originalIndex: index }))
        .filter((element) => element.Line === lineNumber)
        .sort((a, b) => a.Left - b.Left);
};

/**
 * Swaps Left values between two elements in the flat lines array (same-line reorder semantics).
 * @param {Array<{ Left: number }>} lines - Flat array of line elements.
 * @param {number} indexA - Flat-array index of the first element.
 * @param {number} indexB - Flat-array index of the second element.
 * @returns {Array} Updated lines array.
 */
export const swapElementLeftValues = (lines, indexA, indexB) => {
    if (
        indexA === indexB
        || indexA < 0
        || indexB < 0
        || indexA >= lines.length
        || indexB >= lines.length
    ) {
        return lines;
    }

    const updated = [...lines];
    const leftA = updated[indexA].Left;
    updated[indexA] = { ...updated[indexA], Left: updated[indexB].Left };
    updated[indexB] = { ...updated[indexB], Left: leftA };
    return updated;
};

/**
 * Moves an element to a different line, preserving its Left position.
 * @param {Array<{ Line: number, Left: number }>} lines - Flat array of line elements.
 * @param {number} fromIndex - Flat-array index of the element being moved.
 * @param {number} targetLineNumber - Destination line number.
 * @returns {Array} Updated lines array.
 */
export const moveElementToLine = (lines, fromIndex, targetLineNumber) => {
    if (
        fromIndex < 0
        || fromIndex >= lines.length
        || lines[fromIndex].Line === targetLineNumber
    ) {
        return lines;
    }

    const updated = [...lines];
    updated[fromIndex] = { ...updated[fromIndex], Line: targetLineNumber };
    return updated;
};

/**
 * Remaps collapsed element indices after a lines-array mutation.
 * Matches elements by stable identity (Field, Text, FontSize, Bold) rather than index.
 * @param {Set<number>} collapsedElements - Set of collapsed flat-array indices.
 * @param {Array} oldLines - Lines array before the mutation.
 * @param {Array} newLines - Lines array after the mutation.
 * @returns {Set<number>} Updated collapse state.
 */
export const remapCollapsedElementIndices = (collapsedElements, oldLines, newLines) => {
    const remapped = new Set();

    collapsedElements.forEach((oldIndex) => {
        const oldElement = oldLines[oldIndex];
        if (!oldElement) {
            return;
        }

        const identityKey = buildElementIdentityKey(oldElement);
        const newIndex = newLines.findIndex(
            (element) => buildElementIdentityKey(element) === identityKey
        );

        if (newIndex !== -1) {
            remapped.add(newIndex);
        }
    });

    return remapped;
};
