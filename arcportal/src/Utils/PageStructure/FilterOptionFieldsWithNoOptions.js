
const FilterOptionFieldsWithNoOptions = (fieldList, pageStructure) => {

    for (const fieldName of fieldList) {
        for (const page of pageStructure) {
            if (page.Columns !== undefined) {
                for (const column of page.Columns) {
                    for (const formGroup of column.FormGroups) {
                        for (const field of formGroup.Fields) {
                            if (field.Id === fieldName &&
                                field.OptionsName !== undefined && field.OptionsName !== null && field.OptionsName !== "" &&
                                field.Options.length === 0)
                            {
                                fieldList = fieldList.filter(f => f !== fieldName);
                            }
                        }
                    }
                }
            }
        }
    }

    return fieldList;
}

export default FilterOptionFieldsWithNoOptions;