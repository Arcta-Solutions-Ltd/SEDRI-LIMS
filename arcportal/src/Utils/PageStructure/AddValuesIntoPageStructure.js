
const AddValuesIntoPageStructure = (pageStructure, valuesList, wipeIfNotFound = false) => {

    let returnStructure = [...pageStructure];
    if (valuesList !== undefined) {
        for (const page of returnStructure) {
            if (!page.Columns) { continue; }
            for (const column of page.Columns) {
                for (const formGroup of column.FormGroups) {
                    for (const field of formGroup.Fields) {
                        var newValueList = valuesList.map((val) => { return val.key.toLowerCase()} );
                        var index = newValueList.indexOf(field.Id.toLowerCase());
                        if (index !== -1) {
                            switch (field.Type) {
                                case 'date':
                                    field.value = valuesList[index].value === "" ? "" : new Date(valuesList[index].value);
                                    break;
                                case 'dropdown':
                                case 'combobox':
                                case 'hierarchicalpicker':
                                    field.value = valuesList[index].value == null ? "" : String(valuesList[index].value);
                                    break;
                                default:
                                    field.value = valuesList[index].value;
                            }
                        } else {
                            field.value = wipeIfNotFound ? undefined : field.value;
                        }
                    }
                }
            }
    
        }
    }    
    
    return returnStructure;

}

export default AddValuesIntoPageStructure;