import ExtractFieldListFromPageStructure from '../PageStructure/ExtractFieldListFromPageStructure';
import ExtractHiddenFieldListFromPageStructure from '../PageStructure/ExtractHiddenFieldListFromPageStructure';
import GetFieldLabelFromFieldName from '../PageStructure/GetFieldLabelFromFieldName';
import FilterOptionFieldsWithNoOptions from '../PageStructure/FilterOptionFieldsWithNoOptions';

const RequiredFieldCheck = (fieldsToChange, requiredList, requiredRule, requiredError, pages) => {

    const fieldsToCheck = [...fieldsToChange];
    const existingList = ExtractFieldListFromPageStructure(pages);
    const hiddenList = ExtractHiddenFieldListFromPageStructure(pages);
    for (const field of existingList) {
        const foundfield = fieldsToCheck.filter(f => f.key.toLowerCase() === field.key.toLowerCase());
        if (foundfield.length === 0) {
            fieldsToCheck.push(field);
        }
    }
    
    let fieldList = requiredList.split(",");

    fieldList = fieldList.filter(f => {
        return hiddenList.indexOf(f) === -1; }
    );

    if (fieldList.length > 0) {
        fieldList = FilterOptionFieldsWithNoOptions(fieldList, pages);
    }

    for (const field of fieldList) {
        const fieldEntered = fieldsToCheck.filter(f => f.key.toLowerCase() === field.trim().toLowerCase());

        let found = fieldEntered.length > 0 && fieldEntered[0].value !== undefined && fieldEntered[0].value !== "" && fieldEntered[0].value !== null && fieldEntered[0].value.toString().trim().length !== 0;

        if (requiredRule.toLowerCase() === "or") {
            if (found) {
                return "";
            }
        }
        if (requiredRule.toLowerCase() === "and") {
            if ( ! found) {
                var fieldLabel = GetFieldLabelFromFieldName(field.trim(), pages);
                return fieldLabel;
            }
        }
    } 

    if (requiredRule.toLowerCase() === "or" && fieldList.length > 1 && requiredError !== null && requiredError !== "") {
        return requiredError;
    }

    return (requiredRule.toLowerCase() === "and" || fieldList.length === 0) ? "" : GetFieldLabelFromFieldName(fieldList[0].trim(), pages);
}

export default RequiredFieldCheck;
