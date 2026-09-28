import ApplyDependentDefaultsFromPage from '../PageStructure/ApplyDependentDefaultsFromPage';
import GetDefaultValuesFromPage from '../PageStructure/GetDefaultValuesFromPage';

const deleteDataKeyCaseInsensitive = (data, keyName) => {
    const match = Object.keys(data).find((k) => k.toLowerCase() === keyName.toLowerCase());
    if (match !== undefined) {
        delete data[match];
    }
};

/**
 * Collects field ids declared on a single page.
 * @param {object} page
 * @returns {string[]}
 */
const getFieldIdsFromPage = (page) => {
    const ids = [];
    if (page.Columns === undefined) { return ids; }

    for (const column of page.Columns) {
        for (const formGroup of column.FormGroups) {
            for (const field of formGroup.Fields) {
                ids.push(field.Id);
            }
        }
    }
    return ids;
};

/**
 * Resets bound values on a page so a subsequent data merge can re-apply defaults.
 * @param {object} page
 */
const resetPageFieldValues = (page) => {
    if (page.Columns === undefined) { return; }

    for (const column of page.Columns) {
        for (const formGroup of column.FormGroups) {
            for (const field of formGroup.Fields) {
                field.value = undefined;
            }
        }
    }
};

/**
 * Returns a set of lower-cased field ids on pages from startIndex onward.
 * @param {object[]} pages
 * @param {number} startIndex
 * @returns {Set<string>}
 */
const getClearedPageFieldIds = (pages, startIndex) => {
    const ids = new Set();
    for (let index = startIndex; index < pages.length; index++) {
        for (const fieldId of getFieldIdsFromPage(pages[index])) {
            ids.add(fieldId.toLowerCase());
        }
    }
    return ids;
};

/**
 * Removes form data for fields on cleared pages and merges config-driven defaults.
 * Static defaults, defaultToNow, and dependent placeholders are resolved the same way
 * as on initial form open via GetDefaultValuesFromPage and SetDynamicDefaultValues.
 *
 * @param {object[]} pages - Full form page list.
 * @param {number} startIndex - First cleared page index (inclusive).
 * @param {object} data - Mutable form data object.
 * @returns {Record<string, string>} Static and defaultToNow defaults merged into data.
 */
const ApplyDefaultsToClearedPages = (pages, startIndex, data) => {
    const clearedPages = pages.slice(startIndex);
    const clearedFieldIds = getClearedPageFieldIds(pages, startIndex);

    for (const page of clearedPages) {
        for (const fieldId of getFieldIdsFromPage(page)) {
            deleteDataKeyCaseInsensitive(data, fieldId);
        }
        resetPageFieldValues(page);
    }

    const staticDefaults = GetDefaultValuesFromPage(clearedPages, Object.keys(data));
    for (const [key, value] of Object.entries(staticDefaults)) {
        data[key] = value;
    }

    for (let index = 0; index < startIndex; index++) {
        const page = pages[index];
        const dynamicDefaults = ApplyDependentDefaultsFromPage(pages, page, data);
        for (const defValue of dynamicDefaults) {
            if (clearedFieldIds.has(defValue.id.toLowerCase())) {
                data[defValue.id] = defValue.value;
            }
        }
    }

    return staticDefaults;
};

export default ApplyDefaultsToClearedPages;
