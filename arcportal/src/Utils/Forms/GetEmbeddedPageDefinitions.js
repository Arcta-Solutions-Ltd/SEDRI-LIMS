const GetEmbeddedPageDefinitions = (formDef, props) => {

    //Look up ui events on form pages
    let uiEvents = [];
    const pagesToScan = Array.isArray(formDef.Pages)
        ? formDef.Pages.map(p => typeof p === 'string' ? (props.pages ?? []).find(fp => fp.Name === p) : p).filter(Boolean)
        : [];
    pagesToScan.forEach(page => {
        (page?.Columns ?? []).forEach(column => {
            (column?.FormGroups ?? []).forEach(group => {
                (group?.Fields ?? []).forEach(field => {
                    field.FormUIEvent !== null && field.FormUIEvent !== undefined && field.FormUIEvent !== "" ? uiEvents.push(field.FormUIEvent) : null;
                    field.AddFormUIEvent !== null && field.AddFormUIEvent !== undefined && field.AddFormUIEvent !== "" ? uiEvents.push(field.AddFormUIEvent) : null;
                })
            })
        })
    })

    //Use the ui events list to retrieve the corresponding embedded form names
    let formNames = [];
    props.uievents.forEach(uievent => {
        if (uiEvents.includes(uievent.Name)){
            formNames.push(uievent.Action)
        }
    })

    //Retrieve the array of form definitions from props and create the temporary array of pages
    let forms = props.forms.filter(form => formNames.includes(form.Name));
    let pages = [];
    forms.forEach(form => {
        form.Pages.forEach(page => {
            pages.push(page);
        })
    })

    //Extract and return the array of complete page definitions
    return props.pages.filter(page => pages.includes(page.Name));
}

export default GetEmbeddedPageDefinitions;