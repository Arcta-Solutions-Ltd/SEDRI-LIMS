/**
 * Calculated display field ids written by form data rules (not persisted on save).
 */
export const CALCULATED_DISPLAY_FIELD_KEYS = ['PatientAgeDisplay'];

/**
 * Merges calculated display values from formDef.data into fieldChanges so
 * PopulateFieldsWithNewContents can refresh read-only fields on the page.
 *
 * @param {Object} formDef - Form definition with data.
 * @param {Array<{key: string, value: *}>} fieldChanges - Existing field changes.
 * @returns {Array<{key: string, value: *}>} Updated field changes.
 */
const syncCalculatedDisplayFieldsToFieldChanges = (formDef, fieldChanges) => {
    const result = [...(fieldChanges || [])];
    const data = formDef?.data;
    if (!data) {
        return result;
    }

    for (const key of CALCULATED_DISPLAY_FIELD_KEYS) {
        const value = data[key];
        if (value === undefined) {
            continue;
        }
        const displayIdx = result.findIndex((r) => (r.key || '').toLowerCase() === key.toLowerCase());
        const entry = { key, value };
        if (displayIdx !== -1) {
            result[displayIdx] = entry;
        } else {
            result.push(entry);
        }
    }

    return result;
};

/**
 * Removes calculated display fields from a save payload.
 *
 * @param {Object} dataToSave - Merged form data about to be posted.
 * @returns {Object} Payload without calculated display-only fields.
 */
export const stripCalculatedDisplayFieldsFromSaveData = (dataToSave) => {
    if (!dataToSave) {
        return dataToSave;
    }
    const stripped = { ...dataToSave };
    for (const key of CALCULATED_DISPLAY_FIELD_KEYS) {
        delete stripped[key];
    }
    return stripped;
};

export default syncCalculatedDisplayFieldsToFieldChanges;
