import GetListOfVisiblePageIds from "./GetListOfVisiblePageIds";

/** Field ids that must stay on form data for workflow/lab resolution even when their page is not visible. */
const isWorkflowContextFieldId = (fieldId) => {
    if (fieldId === undefined || fieldId === null) return false;
    const id = String(fieldId).toLowerCase();
    return id === "specimentypeid" || id === "laboratoryid";
};

const NullInvisiblePageElements = (formDef) => {
    
    const visibleFields = GetListOfVisiblePageIds(formDef);
    let newData = {...formDef.data};
    let fieldsToNull = [];
    for (const page of formDef.Pages) {
        if (!page.Visible && ! page.Crafted) {

            if (page.Columns !== undefined && page.Columns !== null && page.Columns.length > 0 ) {
                for (const column of page.Columns) {
                    for (const formGroup of column.FormGroups) {
                        for (const field of formGroup.Fields) {
                            if (! visibleFields.includes(field.Id) && ! isWorkflowContextFieldId(field.Id)) {
                                fieldsToNull.push(field.Id);
                            }
                        }
                    }
                }
            }
        }
    }

    for (const field of fieldsToNull) {
        newData[field] = null;
    }

    return newData;
}

export default NullInvisiblePageElements;
