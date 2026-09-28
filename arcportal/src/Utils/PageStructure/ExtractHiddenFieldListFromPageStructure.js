
const ExtractHiddenFieldListFromPageStructure = (pageStructure) => {

    let fieldList = [];

    for (const page of pageStructure) {
        if (page.Columns !== undefined) {
            for (const column of page.Columns) {
                for (const formGroup of column.FormGroups) {
                    if (formGroup.visible === false) {
                        for (const field of formGroup.Fields) {
                            fieldList.push(field.Id);
                        }
                    }
                }
            }
        }
    }

    return fieldList;
}

export default ExtractHiddenFieldListFromPageStructure;
