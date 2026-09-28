import AddListsIntoPage from './AddListsIntoPage';

const GetDataEntryFormFromFormNameConfig = (formName, forms, pages, lists, laboratoryId, laboratory, specimentypeid) => {

    let formDef = forms.filter(f => f.Name === formName)[0];

    let pagesList = [...formDef.Pages];

    const newFormDef = {...formDef};

    newFormDef.Pages = [];
    for (const page of pagesList) {
        const pageName = typeof page === 'string' ? page : (page?.Name ?? page?.name);
        let newPage = pages.find(p => String(p.Name ?? p.name).toLowerCase() === String(pageName).toLowerCase());
        if (newPage !== undefined) {
            newPage = AddListsIntoPage(newPage, lists, laboratoryId, laboratory, specimentypeid, formName);
            newFormDef.Pages.push(newPage);
        }
    }

    return newFormDef;
}

export default GetDataEntryFormFromFormNameConfig;
