/**
 * Collects dynamic list names required by a form definition for a PostList request.
 * @param {Object} formDef - Form definition with Pages containing dynamic fields.
 * @returns {Array<{name: string, includeFixed: boolean, translate: boolean}>}
 */
const GetDynamicListNames = (formDef) => {

    let dynamicLists = [];

    for (const page of formDef.Pages) {
        for (const column of page.Columns) {
            for (const formGroup of column.FormGroups) {
                for (const field of formGroup.Fields) {
                    if (field.Dynamic) {
                        const optionsName = field.OptionsName || field.optionsName;
                        dynamicLists.push({
                            name: optionsName,
                            includeFixed: !field.RemoveFixed,
                            translate: field.TranslateOptions ?? field.translateOptions
                        });
                     }
                }
            }
        }
    }

    return dynamicLists;
}

/**
 * Injects options from PostList responses into dynamic form fields that have no parent list.
 * Always assigns an array to field.Options when a matching list is found.
 * @param {Object} formDef - Form definition to mutate.
 * @param {Array} lists - List configs returned from list/get.
 * @returns {string}
 */
const AddDynamicListsIntoForm = (formDef, lists) => {

    let dynamicLists = "";

    for (const page of formDef.Pages) {
        for (const column of page.Columns) {
            for (const formGroup of column.FormGroups) {
                for (const field of formGroup.Fields) {
                    if (field.Dynamic) {
                        const parentList = field.ParentList ?? field.parentList ?? '';
                        if (parentList === '') {
                            const optionsName = field.OptionsName || field.optionsName;
                            const list = lists.filter(l =>
                                (l.name || l.Name || '').toLowerCase() === (optionsName || '').toLowerCase()
                            );
                            if (list.length > 0) {
                                const listOptions = list[0].options || list[0].Options || [];
                                const options = Array.isArray(listOptions)
                                    ? listOptions.map((option) => ({
                                        key: option.key ?? option.Key,
                                        text: option.text ?? option.Text ?? '',
                                        ParentKey: option.parentkey ?? option.ParentKey ?? option.parentKey
                                    }))
                                    : [];
                                field.Options = options;
                            }
                        }
                     }
                }
            }
        }
    }

    return dynamicLists;
}

export default GetDynamicListNames;
export {AddDynamicListsIntoForm};
