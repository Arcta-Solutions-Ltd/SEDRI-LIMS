
const TranslatePageStructureIntoReferencedFieldList = (pageStructure) => {

    let fieldList = [];

    for (const page of pageStructure) {
        for (const column of page.Columns) {
            for (const formGroup of column.FormGroups) {
                fieldList = fieldList.concat(formGroup.Fields)
            }
        }
    }
    return fieldList;

}

export default TranslatePageStructureIntoReferencedFieldList;