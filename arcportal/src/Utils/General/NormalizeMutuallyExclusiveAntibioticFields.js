/**
 * Returns true when a form data value is unset, null, blank, or numeric zero.
 * @param {*} value - Form field value.
 * @returns {boolean}
 */
const isEmptyIdValue = (value) => {
    if (value === undefined || value === null || value.toString().trim() === '') {
        return true;
    }

    const numeric = Number(value);
    return !Number.isNaN(numeric) && numeric === 0;
};

/**
 * Reads a form data property case-insensitively.
 * @param {object} data - Form data object.
 * @param {string} fieldId - Field id to read.
 * @returns {*} Field value when present.
 */
const getFieldValue = (data, fieldId) => {
    if (!data || fieldId === undefined || fieldId === null) {
        return undefined;
    }

    const property = Object.keys(data).find(key => key.toLowerCase() === String(fieldId).toLowerCase());
    return property === undefined ? undefined : data[property];
};

/**
 * Clears a form data property case-insensitively.
 * @param {object} data - Form data object to mutate.
 * @param {string} fieldId - Field id to clear.
 */
const clearFieldValue = (data, fieldId) => {
    const property = Object.keys(data).find(key => key.toLowerCase() === String(fieldId).toLowerCase());
    if (property !== undefined) {
        data[property] = null;
    }
};

/**
 * Normalizes expert rule antibiotic vs antibiotic group fields when both ids are populated.
 * Treats 0 as empty for id fields. Prefers antibiotic group so the edit form can display the group combobox after legacy dual-value rows.
 * @param {object} data - Loaded form data.
 * @returns {object} Normalized form data.
 */
const NormalizeMutuallyExclusiveAntibioticFields = (data) => {
    if (!data || Array.isArray(data)) {
        return data;
    }

    const antibioticId = getFieldValue(data, 'antibioticid');
    const antibioticGroupId = getFieldValue(data, 'antibioticgroupid');

    if (isEmptyIdValue(antibioticId)) {
        clearFieldValue(data, 'antibioticid');
    }

    if (isEmptyIdValue(antibioticGroupId)) {
        clearFieldValue(data, 'antibioticgroupid');
    }

    const antibioticAfterNormalize = getFieldValue(data, 'antibioticid');
    const groupAfterNormalize = getFieldValue(data, 'antibioticgroupid');

    if (!isEmptyIdValue(antibioticAfterNormalize) && !isEmptyIdValue(groupAfterNormalize)) {
        clearFieldValue(data, 'antibioticid');
    }

    return data;
};

export default NormalizeMutuallyExclusiveAntibioticFields;
