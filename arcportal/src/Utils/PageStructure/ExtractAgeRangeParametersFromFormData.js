const AGE_RANGE_SUFFIXES = ['Years', 'Months', 'Days', 'Hours'];
const AGE_RANGE_PREFIXES = ['AgeFrom', 'AgeTo'];

/**
 * Extracts patient-search age range sub-field parameters from form data.
 * ArcAge writes AgeFromYears, AgeToYears, etc.; the query layer expects these keys, not AgeFrom/AgeTo.
 *
 * @param {Object|undefined|null} formData - Form data object (formDef.data).
 * @returns {Array<{key: string, value: string|number}>} Non-empty age sub-field entries for query Parameters.
 */
const ExtractAgeRangeParametersFromFormData = (formData) => {
    if (!formData || typeof formData !== 'object') {
        return [];
    }

    const results = [];
    for (const prefix of AGE_RANGE_PREFIXES) {
        for (const suffix of AGE_RANGE_SUFFIXES) {
            const key = prefix + suffix;
            const property = Object.keys(formData).find(
                (k) => k.toLowerCase() === key.toLowerCase()
            );
            if (property === undefined) {
                continue;
            }
            const raw = formData[property];
            if (raw === undefined || raw === null || raw === '') {
                continue;
            }
            results.push({ key: property, value: raw });
        }
    }

    return results;
};

export default ExtractAgeRangeParametersFromFormData;
