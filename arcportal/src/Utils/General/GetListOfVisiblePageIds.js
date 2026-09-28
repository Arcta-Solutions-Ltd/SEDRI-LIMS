const GetListOfVisiblePageIds = (formDef) => {
    
    let visibleFields = [];
    for (const page of formDef.Pages) {
        if (page.Visible && ! page.Crafted) {
            if (page.Columns !== undefined && page.Columns !== null && page.Columns.length > 0 ) {
                for (const column of page.Columns) {
                    for (const formGroup of column.FormGroups) {
                        for (const field of formGroup.Fields) {
                            visibleFields.push(field.Id);
                        }
                    }
                }
            }
        }
    }

    return visibleFields;
}

export default GetListOfVisiblePageIds;
