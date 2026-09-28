/**
 * Resolves grid data IDs to display text using option lists from the form pages.
 * Mutates data in place, replacing item.value (ID) with option.Text where a match is found.
 * Only replaces items whose key maps to the current list (avoids collisions when IDs overlap).
 * Supports both PascalCase and camelCase page JSON (`Columns`/`columns`, `FormGroups`/`formGroups`, `Fields`/`fields`).
 * @param {Array<{key: string, value: string|number}>} data - Grid row data as {key, value} pairs.
 * @param {Array<{Name?: string, name?: string, Options?: Array, options?: Array}>} lists - Option lists (e.g. antibiotic, testresult, testmethod).
 * @param {Array} pages - Page definitions containing Fields with OptionsName/optionsName to determine which lists apply.
 * @returns {Array} The data array (mutated with resolved text values).
 */
const GetTextForListItemsInGrid = (data, lists, pages) => {

    if (!Array.isArray(data) || !Array.isArray(lists)) {
        return data ?? [];
    }

    const safePages = Array.isArray(pages) ? pages : [];

    const pageColumns = (page) => page?.Columns ?? page?.columns ?? [];
    const columnFormGroups = (column) => column?.FormGroups ?? column?.formGroups ?? [];
    const groupFields = (group) => group?.Fields ?? group?.fields ?? [];

    const addFieldToMap = (field) => {
        const optionName = field.OptionsName || field.optionsName;
        if (optionName) {
            const fieldKey = (field.Id || field.id || '').toString().toLowerCase();
            if (fieldKey) {
                keyToListMap[fieldKey] = optionName.toLowerCase();
            }
        }
    };

    const keyToListMap = {};
    safePages.forEach(page =>
        pageColumns(page).forEach(column =>
            columnFormGroups(column).forEach(group =>
                groupFields(group).forEach(field => {
                    addFieldToMap(field);
                    (field.GridFields ?? field.gridfields ?? []).forEach(gf => addFieldToMap(gf));
                })
            )
        )
    );

    const getOptionNames = (field) => {
        const names = [];
        const opt = field.OptionsName || field.optionsName;
        if (opt) names.push(opt);
        (field.GridFields ?? field.gridfields ?? []).forEach(gf => {
            const gOpt = gf.OptionsName || gf.optionsName;
            if (gOpt) names.push(gOpt);
        });
        return names;
    };

    let formLists = safePages.flatMap(page =>
        pageColumns(page).flatMap(column =>
            columnFormGroups(column).flatMap(group =>
                groupFields(group).flatMap(field => getOptionNames(field)).filter(optionName => !!optionName)
            )
        )
    );

    const listName = (list) => String(list.Name ?? list.name ?? '').toLowerCase();
    let listsToUse = lists.filter(list =>
        formLists.some(f => String(f).toLowerCase() === listName(list))
    );

    listsToUse.forEach(list => {
        const currentListName = listName(list);
        (list.Options ?? list.options ?? []).forEach(option => {
            const optionKey = option?.Key ?? option?.key;
            const optionText = option?.Text ?? option?.text;
            const match = data.find(item =>
                (String(item.value) === String(optionKey) || item.value == optionKey) &&
                keyToListMap[(item.key || '').toLowerCase()] === currentListName
            );
            if (match && optionText !== undefined) {
                match.value = optionText;
            }
        });
    });

    return data;
}

export default GetTextForListItemsInGrid;