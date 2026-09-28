const CULTURE_TYPE_FIELD_NAMES = ['typeid', 'TypeId', 'culturetypeid', 'CultureTypeId'];

/**
 * Reads a culture type list-item id from a record object (culture row or form payload).
 * @param {Object|undefined|null} record - Record that may carry culture type id fields.
 * @returns {string|null} Culture type id as string, or null when not present.
 */
const readCultureTypeIdFromRecord = (record) => {
    if (record == null || typeof record !== 'object') {
        return null;
    }

    for (const fieldName of CULTURE_TYPE_FIELD_NAMES) {
        const value = record[fieldName];
        if (value != null && value !== '') {
            return String(value);
        }
    }

    return null;
};

/**
 * Resolves the culture type list-item id for isolate test filtering.
 * Checks currentRecord (culture) first, then form payload, then selectedRecord fallbacks.
 * @param {Object|undefined|null} startConfig - Form start config with currentRecord and selectedRecord.
 * @param {Object|undefined|null} formData - Form data returned from the selection query.
 * @returns {string|null} Culture type id as string, or null when unknown.
 */
export function resolveCultureTypeId(startConfig, formData) {
    return readCultureTypeIdFromRecord(startConfig?.currentRecord)
        ?? readCultureTypeIdFromRecord(formData)
        ?? readCultureTypeIdFromRecord(startConfig?.selectedRecord);
}

export default resolveCultureTypeId;
