/**
 * Reorder helpers for ArcSelector / ArcFieldSelector list rows.
 * All matching uses record id (value), not display label.
 */

const getRecordId = (record) => record.id ?? record.value ?? record.Value;

/**
 * Moves one item from fromIndex to toIndex in a list (0-based).
 * @param {Array} data - Current list.
 * @param {number} fromIndex - Source index.
 * @param {number} toIndex - Target index.
 * @returns {Array} Reordered list.
 */
export function reorderList(data, fromIndex, toIndex) {
    if (!Array.isArray(data) || fromIndex === toIndex || fromIndex < 0 || toIndex < 0) {
        return data;
    }
    const updated = [...data];
    const [dragged] = updated.splice(fromIndex, 1);
    if (dragged === undefined) {
        return data;
    }
    updated.splice(toIndex, 0, dragged);
    return updated;
}

/**
 * Moves the matching record to the top of the list.
 * @param {Array} data - Current list.
 * @param {Object} record - Row to move.
 * @returns {Array} Reordered list.
 */
export function moveTop(data, record) {
    const recordId = getRecordId(record);
    const newData = data.filter((r) => getRecordId(r) === recordId);
    for (const line of data) {
        if (getRecordId(line) !== recordId) {
            newData.push(line);
        }
    }
    return newData;
}

/**
 * Moves the matching record up one position.
 * @param {Array} data - Current list.
 * @param {Object} record - Row to move.
 * @returns {Array} Reordered list.
 */
export function moveUp(data, record) {
    const recordId = getRecordId(record);
    const position = data.findIndex((r) => getRecordId(r) === recordId);
    if (position <= 0) {
        return data;
    }

    const lineToMove = data.find((r) => getRecordId(r) === recordId);
    const newData = [];
    let count = 0;
    for (const line of data) {
        if (position - 1 === count) {
            newData.push(lineToMove);
        }
        if (getRecordId(line) !== recordId) {
            newData.push(line);
        }
        count++;
    }
    return newData;
}

/**
 * Moves the matching record down one position.
 * @param {Array} data - Current list.
 * @param {Object} record - Row to move.
 * @returns {Array} Reordered list.
 */
export function moveDown(data, record) {
    const recordId = getRecordId(record);
    const position = data.findIndex((r) => getRecordId(r) === recordId);
    if (position < 0 || position >= data.length - 1) {
        return data;
    }

    const lineToMove = data.find((r) => getRecordId(r) === recordId);
    const newData = [];
    let count = 0;
    let found = false;
    for (const line of data) {
        if (position + 2 === count) {
            newData.push(lineToMove);
            found = true;
        }
        if (getRecordId(line) !== recordId) {
            newData.push(line);
        }
        count++;
    }
    if (!found) {
        newData.push(lineToMove);
    }
    return newData;
}

/**
 * Moves the matching record to the bottom of the list.
 * @param {Array} data - Current list.
 * @param {Object} record - Row to move.
 * @returns {Array} Reordered list.
 */
export function moveBottom(data, record) {
    const recordId = getRecordId(record);
    const newData = [];
    for (const line of data) {
        if (getRecordId(line) !== recordId) {
            newData.push(line);
        }
    }
    const recordToMove = data.find((r) => getRecordId(r) === recordId);
    if (recordToMove) {
        newData.push(recordToMove);
    }
    return newData;
}
