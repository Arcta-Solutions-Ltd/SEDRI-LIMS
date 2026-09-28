import Undefined from '../General/Undefined';

const PopulateFieldsWithNewContents = (pageStructure, fieldChanges) => {

    for (const page of pageStructure) {
        for (const column of page.Columns) {
            for (const formGroup of column.FormGroups) {
                for (const field of formGroup.Fields) {
                    const changedValue = fieldChanges.findIndex(r => r.key.toLowerCase() === field.Id.toLowerCase());
                    if (changedValue !== -1) {
                        let newValue = fieldChanges[changedValue].value;
                        if (! Undefined(fieldChanges[changedValue].value) && ! Undefined(fieldChanges[changedValue].value.Key)) {
                            newValue = fieldChanges[changedValue].value.value;
                        }
                        field.value = newValue;
                    }
                }
            }
        }
    }
   
}

export default PopulateFieldsWithNewContents;
