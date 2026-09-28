import TranslatePageStructureIntoReferencedFieldList from "./TranslatePageStructureIntoReferencedFieldList";

const RemoveHierarchicalElements = (key, value, changes, pageStructure) => {
    
    let  fields = TranslatePageStructureIntoReferencedFieldList(pageStructure);
    fields = fields.filter(f => f.Type === "dropdown" || f.Type === "filteredcombo" || f.Type === "combobox");

    for (const field of fields) {
        if (field.ParentList !== "" && field.ParentList  === key) {
            let foundInChangeList = false;
            for (const change of changes) {
                if (change.key === field.Id) {
                    change.value = null;
                    foundInChangeList = true;
                }
            }
            if (!foundInChangeList) {
                changes.push({ key: field.Id, value: null});
            }
        }
    }

    return changes;
}

export default RemoveHierarchicalElements;