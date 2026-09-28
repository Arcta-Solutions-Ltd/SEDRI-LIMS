/** Synthetic list key for the configurable "Other" option (matches backend OtherOptionConstants.Key). */
export const OTHER_OPTION_KEY = '__ARC_OTHER__';

export const OTHER_OPTION_LABEL_TAG = '@GenOther@';

export const OTHER_DETAILS_LABEL_TAG = '@ConOtherDetails@';

/**
 * Reads a form data value using case-insensitive key matching.
 * @param {object} data
 * @param {string} fieldId
 * @returns {*}
 */
export const getFormDataValue = (data, fieldId) => {
    if (data === undefined || data === null || fieldId === undefined || fieldId === null) {
        return undefined;
    }
    const property = Object.keys(data).find(key => key.toLowerCase() === String(fieldId).toLowerCase());
    if (property !== undefined) {
        return data[property];
    }
    return data[fieldId];
};

/**
 * Collects parent list field ids that have an auto-generated Other-details companion on the page.
 * @param {object[]} pages
 * @returns {Set<string>}
 */
export const collectParentFieldIdsWithOtherCompanion = (pages) => {
    const parentIds = new Set();
    if (!Array.isArray(pages)) {
        return parentIds;
    }
    for (const page of pages) {
        for (const column of page.Columns || page.columns || []) {
            for (const formGroup of column.FormGroups || column.formGroups || []) {
                for (const field of formGroup.Fields || formGroup.fields || []) {
                    const parentId = field.OtherDetailsFor ?? field.otherDetailsFor;
                    if (parentId) {
                        parentIds.add(String(parentId).toLowerCase());
                    }
                }
            }
        }
    }
    return parentIds;
};

/**
 * @param {object} field
 * @param {Set<string>} [parentIdsWithCompanion]
 * @returns {boolean}
 */
export const fieldHasAllowOther = (field, parentIdsWithCompanion) => {
    if (field?.AllowOther === true || field?.allowOther === true
        || field?.AllowOther === 'Yes' || field?.allowOther === 'Yes') {
        return true;
    }
    const otherOptionKey = field?.OtherOptionKey ?? field?.otherOptionKey;
    if (otherOptionKey !== undefined && otherOptionKey !== null && otherOptionKey !== '') {
        return true;
    }
    const fieldId = field?.Id ?? field?.id;
    if (fieldId && parentIdsWithCompanion?.has(String(fieldId).toLowerCase())) {
        return true;
    }
    return false;
};

/**
 * @param {object} field
 * @returns {string}
 */
export const getOtherOptionKey = (field) =>
    field?.OtherOptionKey ?? field?.otherOptionKey ?? OTHER_OPTION_KEY;

/**
 * @param {object} field
 * @param {object} data
 * @param {object[]} [fields]
 * @returns {boolean}
 */
export const isOtherDetailsFieldVisible = (field, data, fields) => {
    const parentId = field?.OtherDetailsFor ?? field?.otherDetailsFor;
    if (!parentId) {
        return true;
    }
    let parentValue = getFormDataValue(data, parentId);
    const parentField = Array.isArray(fields)
        ? fields.find(f => String(f.Id ?? f.id).toLowerCase() === String(parentId).toLowerCase())
        : null;
    if ((parentValue === undefined || parentValue === null || parentValue === '') && parentField) {
        parentValue = parentField.value;
    }
    const otherKey = getOtherOptionKey(parentField ?? field);
    return String(parentValue) === otherKey;
};

/**
 * Appends the runtime "Other" option to every eligible field on each page.
 * @param {object[]} pages
 */
export const appendOtherOptionToAllPageFields = (pages) => {
    if (!Array.isArray(pages)) {
        return;
    }
    const parentIdsWithCompanion = collectParentFieldIdsWithOtherCompanion(pages);
    for (const page of pages) {
        for (const column of page.Columns || page.columns || []) {
            for (const formGroup of column.FormGroups || column.formGroups || []) {
                for (const field of formGroup.Fields || formGroup.fields || []) {
                    appendOtherOptionIfNeeded(field, parentIdsWithCompanion);
                    if (field.Type === 'fieldgrid' && Array.isArray(field.GridFields)) {
                        for (const gridField of field.GridFields) {
                            appendOtherOptionIfNeeded(gridField, parentIdsWithCompanion);
                        }
                    }
                }
            }
        }
    }
};

/**
 * Appends the runtime "Other" option when the field has AllowOther enabled.
 * @param {object} field
 * @param {Set<string>} [parentIdsWithCompanion]
 */
export const appendOtherOptionIfNeeded = (field, parentIdsWithCompanion) => {
    if (!fieldHasAllowOther(field, parentIdsWithCompanion)) {
        return;
    }
    const otherKey = getOtherOptionKey(field);
    if (!Array.isArray(field.Options)) {
        field.Options = [];
    }
    if (field.Options.some(o => String(o.key) === otherKey)) {
        return;
    }
    field.Options = [...field.Options, { key: otherKey, text: OTHER_OPTION_LABEL_TAG }];
};
