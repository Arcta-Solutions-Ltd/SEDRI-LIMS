
const GetValueFromPageStructure = (fieldName,pageStructure) => {

    let returnValue = "";

    for (const page of pageStructure) {
        for (const column of page.Columns) {
            for (const formGroup of column.FormGroups) {
                for (const field of formGroup.Fields) {
    
                    if (field.Id === fieldName) {
                        returnValue = field.value;
                    }
                }
            }
        }
    }

    return returnValue;

}

export default GetValueFromPageStructure;