const GetIndividualFieldDetails = (formName, forms, pages, value) => {
    const form = forms.filter((f) => f.Name === formName)[0];

    if (form !== undefined) {
        return FindFieldInForm(form, pages, value);
    }
};

const GetIndividualFieldAcrossAllForms = (forms, pages, value) => {
    for (const form of forms) {
        const field = FindFieldInForm(form, pages, value);
        if (field !== undefined) {
            return field;
        }
    }
};

const FindFieldInForm = (form, pages, value) => {
    if (!value) return;
    for (const page of form.Pages) {
        let newPage = pages.find((p) => p.Name === page);
        const isCrafted = newPage.Crafted === undefined ? false : newPage.Crafted;
        if (newPage && ! isCrafted && Array.isArray(newPage.Columns)) {
            for (const formGroup of newPage.Columns[0].FormGroups) {
                const field = formGroup.Fields.find(field => field.Id.toLowerCase() === value.toLowerCase());
                if (field) return field;
            }
        }
    }
};

export default GetIndividualFieldDetails;
export { GetIndividualFieldAcrossAllForms };
