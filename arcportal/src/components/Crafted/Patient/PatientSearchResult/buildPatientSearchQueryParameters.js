import ExtractAgeRangeParametersFromFormData from '../../../../Utils/PageStructure/ExtractAgeRangeParametersFromFormData';

/**
 * Builds query/filteredget Parameters for patient search, merging age sub-fields from the input array.
 *
 * @param {Array<{key?: string, Key?: string, value?: *, Value?: *}>|undefined|null} inputData - Crafted page input from workflow navigation.
 * @returns {Array<{key: string, value: *}>} Parameters array including AgeFromYears/AgeToYears when present in input.
 */
const buildPatientSearchQueryParameters = (inputData) => {
    const params = [...(inputData || [])];
    const asObject = {};
    for (const item of params) {
        const k = item.key ?? item.Key;
        if (k == null) {
            continue;
        }
        asObject[k] = item.value ?? item.Value;
    }
    const ageExtras = ExtractAgeRangeParametersFromFormData(asObject);
    for (const ageParam of ageExtras) {
        const existingIndex = params.findIndex(
            (item) => (item.key ?? item.Key)?.toLowerCase() === ageParam.key.toLowerCase()
        );
        if (existingIndex === -1) {
            params.push(ageParam);
        } else {
            params[existingIndex] = ageParam;
        }
    }
    return params;
};

export default buildPatientSearchQueryParameters;
