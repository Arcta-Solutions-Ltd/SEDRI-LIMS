
const GetFieldFromFieldName = (fieldName, pageStructure) => {

    for (const page of pageStructure) {
        if (page.Columns !== undefined) {
            for (const column of page.Columns) {
                for (const formGroup of column.FormGroups) {
                    for (const field of formGroup.Fields) {
                        if (field.Id === fieldName) {
                            return field;
                        }
                    }
                }
            }
        }
    }

    return null;
}

export default GetFieldFromFieldName;