import { SetDynamicDefaultValues } from './GetDefaultValuesFromPage';

/**
 * Resolves a field value from form data by stable field id (case-insensitive).
 * @param {object} data
 * @param {string} fieldId
 * @returns {*|undefined}
 */
const getDataValueByFieldId = (data, fieldId) => {
    const dataKey = Object.keys(data).find((k) => k.toLowerCase() === fieldId.toLowerCase());
    if (dataKey === undefined) {
        return undefined;
    }
    return data[dataKey];
};

/**
 * For each populated field on departingPage, runs SetDynamicDefaultValues
 * and returns { id, value } pairs to merge into form data.
 * @param {object[]} pages - Full form page list.
 * @param {object} departingPage - Page being navigated away from.
 * @param {object} data - Merged form data after field changes are applied.
 * @returns {{ id: string, value: * }[]}
 */
const ApplyDependentDefaultsFromPage = (pages, departingPage, data) => {
    const defaults = [];
    if (departingPage?.Columns === undefined || data === undefined) {
        return defaults;
    }

    for (const column of departingPage.Columns) {
        for (const formGroup of column.FormGroups) {
            for (const field of formGroup.Fields) {
                const value = getDataValueByFieldId(data, field.Id) ?? field.value;
                if (value === undefined || value === null || value === '') {
                    continue;
                }

                const dynamicDefaults = SetDynamicDefaultValues(pages, field.Id, value);
                defaults.push(...dynamicDefaults);
            }
        }
    }

    return defaults;
};

export default ApplyDependentDefaultsFromPage;
