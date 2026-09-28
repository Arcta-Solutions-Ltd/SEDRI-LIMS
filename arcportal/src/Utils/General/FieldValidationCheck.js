import ExtractFieldListFromPageStructure from '../PageStructure/ExtractFieldListFromPageStructure';
import GetFieldFromFieldName from '../PageStructure/GetFieldFromFieldName';

/**
 * Validates fields to ensure they do not exceed specified maximum values.
 * 
 * This function checks the given fields and pages for any fields that need to be validated.
 * If a field exceeds the specified maximum value, an error message is returned.
 * 
 * @param {Array} fieldsToChange - An array of fields that need to be validated.
 * @param {Array} pages - An array of pages containing field structures.
 * @returns {string} - An error message if a validation check fails, otherwise an empty string.
 */
const FieldValidationCheck = (fieldsToChange, pages) => {
    const fieldsToCheck = [...fieldsToChange];
    const existingList = ExtractFieldListFromPageStructure(pages);

    for (const field of existingList) {
        const foundField = fieldsToCheck.find(f => f.key.toLowerCase() === field.key.toLowerCase());
        if (!foundField) {
            fieldsToCheck.push(field);
        }
    }

    for (const fieldKVP of fieldsToCheck) {
        const field = GetFieldFromFieldName(fieldKVP.key, pages);
        if (field.Max) {
            switch (field.Type) {
                case "singleline":
                    if (parseInt(field.value, 10) > parseInt(field.Max, 10)) {
                        return "Value exceeds maximum limit.";
                    }
                    break;
                // Add additional field type cases here if needed
                default:
                    break;
            }
        }
    }

    return "";
};

export default FieldValidationCheck;
