
const ExtractFieldListFromPageStructure = (pageStructure) => {

    let fieldList = [];

    for (const page of pageStructure) {
        if (page.Columns !== undefined) {
            for (const column of page.Columns) {
                for (const formGroup of column.FormGroups) {
                    for (const field of formGroup.Fields) {
                        if (field.value !== undefined) {
                            fieldList.push({key: field.Id, value:field.value})
                        }
                    }
                }
            }
        }
    }

    return fieldList;
}

export default ExtractFieldListFromPageStructure;