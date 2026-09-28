import TranslatePageStructureIntoReferencedFieldList from './TranslatePageStructureIntoReferencedFieldList';
import IsOptionUnderParent from './IsOptionUnderParent';
import resolveQueryDropdownOptions from './ResolveQueryDropdownOptions';

const PopulateDynamicLists = (pageStructure, lists, key, value, queryData) => {

    let currentField;
    let  fields = TranslatePageStructureIntoReferencedFieldList(pageStructure);
    fields = fields.filter(f => f.Type === "dropdown" || f.Type === "filteredcombo" || f.Type === "combobox" || f.Type === "hierarchicalpicker");

    for (const field of fields) {
        if (field.ParentList !== "" && field.ParentList  === key) {

            const optionsName = field.OptionsName || field.optionsName || '';
            const newValue = value === undefined || value.key === undefined ? value : value.value;
            const queryOptions = resolveQueryDropdownOptions(
                queryData,
                optionsName,
                field,
                newValue
            );
            if (queryOptions != null) {
                field.Options = queryOptions;
                field.value = null;
                currentField = field.Id;
                continue;
            }

            const list = lists.filter(l => l.Name.toLowerCase() === field.OptionsName.toLowerCase());

            field.Options = list[0].Options.filter(f => IsOptionUnderParent(f, newValue)).map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}});

            field.value = null;
            currentField = field.Id
        }
        if (currentField !== undefined && field.ParentList === currentField) {
            field.value = null;
            field.Options = [];
            currentField = field.Id
        }
    }

}

export default PopulateDynamicLists;