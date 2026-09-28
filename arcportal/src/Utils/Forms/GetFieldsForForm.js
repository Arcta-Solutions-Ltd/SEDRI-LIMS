const GetFieldsForForm = (formName, forms, pages, includeGridFields) => {

    const form = forms.filter(f => f.Name === formName)[0];
    includeGridFields = includeGridFields === undefined ? true : false;

    let fieldList = [];

    if (form !== undefined) {
        for (const page of form.Pages) {
            let newPage = pages.filter(p => p.Name === page)[0];
            if (newPage !== undefined) {
                if (newPage.Columns !== undefined && newPage.Columns !== null && newPage.Columns !== "" &&
                    newPage.Columns[0] !== undefined && newPage.Columns[0] !== null && newPage.Columns[0] !== "" &&
                    newPage.Columns[0].FormGroups !== undefined && newPage.Columns[0].FormGroups !== null && newPage.Columns[0].FormGroups !== "") {
                    for (const formGroup of newPage.Columns[0].FormGroups) {
                        for (const field of formGroup.Fields) {
                            if (field.Type !== "fieldgrid" || (field.Type === "fieldgrid" && includeGridFields)) {
                                fieldList.push({id: field.Id, field: field.Label});
                            }
                        }
                    }
                }
            }
        }
    }

    return fieldList;
}

export default GetFieldsForForm;