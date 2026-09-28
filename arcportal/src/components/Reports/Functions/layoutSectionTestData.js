/**
 * Builds synthetic report data for layout section designer preview.
 */

/**
 * Generates table rows filled with placeholder values for preview.
 * @param {string|number|Array} width - Grid width specification.
 * @returns {string[]} Three pipe-separated preview rows.
 */
const generateTableRows = (width) => {
    let columnCount = 1;

    if (Array.isArray(width)) {
        columnCount = width.length > 0 ? width.length : 1;
    } else if (typeof width === 'string' || typeof width === 'number') {
        const parts = String(width)
            .split('|')
            .map((part) => part.trim())
            .filter((part) => part.length > 0);
        columnCount = parts.length > 0 ? parts.length : 1;
    }

    const row = Array(columnCount).fill('X').join('|');
    return [row, row, row];
};

/**
 * Adds a standard data key if not already present.
 * @param {Array} standardEntries - Mutable Standard array.
 * @param {Set<string>} added - Keys already added.
 * @param {string} key - Field value id.
 */
const addStandardKey = (standardEntries, added, key) => {
    if (!key) {
        return;
    }

    const normalized = String(key);
    if (added.has(normalized)) {
        return;
    }

    added.add(normalized);
    standardEntries.push({ Key: normalized, Value: 'X' });
};

/**
 * Extracts standard field keys from a ContentsConfig section.
 * @param {object} section - Printable section config.
 * @param {Array} standardEntries - Mutable Standard array.
 * @param {Set<string>} added - Keys already added.
 */
const extractStandardFieldsFromSection = (section, standardEntries, added) => {
    const columns = [section?.Column1, section?.Column2, section?.Column].filter(Boolean);

    columns.forEach((column) => {
        (column?.Fields || []).forEach((field) => {
            addStandardKey(standardEntries, added, field?.Value);
        });
    });

    (section?.Lines || []).forEach((line) => {
        addStandardKey(standardEntries, added, line?.Field);
    });
};

/**
 * Extracts table data from a grid ContentsConfig.
 * @param {object} section - Grid table ContentsConfig.
 * @param {Array} tableEntries - Mutable Tables array.
 * @param {Set<string>} tableKeys - Table keys already added.
 */
const extractTableFromSection = (section, tableEntries, tableKeys) => {
    const sectionType = section?.Type ? String(section.Type).toLowerCase() : '';
    if (sectionType !== 'table') {
        return;
    }

    const key = section?.Data ? String(section.Data) : '';
    if (!key || tableKeys.has(key)) {
        return;
    }

    tableKeys.add(key);
    tableEntries.push({
        Key: key,
        Rows: generateTableRows(section?.Width),
    });
};

/**
 * Builds synthetic Standard and Tables payloads for layout section preview.
 * @param {object[]} contents - Translated ContentsConfig entries for one section.
 * @param {object|null} dataSection - Data section definition for fallback field keys.
 * @param {object|null} [section] - Live section definition from the designer (bound fields).
 * @returns {{ Standard: Array, Tables: Array }} Preview data payload.
 */
export const generateLayoutSectionTestData = (contents, dataSection, section) => {
    const standardEntries = [];
    const added = new Set();
    const tableEntries = [];
    const tableKeys = new Set();

    (section?.Fields || []).forEach((field) => {
        addStandardKey(standardEntries, added, field?.Value ?? field?.value);
    });

    (contents || []).forEach((sectionEntry) => {
        extractStandardFieldsFromSection(sectionEntry, standardEntries, added);
        extractTableFromSection(sectionEntry, tableEntries, tableKeys);
    });

    (dataSection?.Fields || []).forEach((field) => {
        addStandardKey(standardEntries, added, field?.Value ?? field?.value);
    });

    if (tableEntries.length === 0 && dataSection?.Grids?.length > 0) {
        dataSection.Grids.forEach((grid) => {
            const key = grid?.data ?? grid?.Data;
            if (!key || tableKeys.has(key)) {
                return;
            }

            tableKeys.add(key);
            tableEntries.push({
                Key: key,
                Rows: generateTableRows('300|80'),
            });
        });
    }

    return {
        Standard: standardEntries,
        Tables: tableEntries,
    };
};
