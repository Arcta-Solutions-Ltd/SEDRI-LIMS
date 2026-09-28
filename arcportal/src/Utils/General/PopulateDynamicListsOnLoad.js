import TranslatePageStructureIntoReferencedFieldList from './TranslatePageStructureIntoReferencedFieldList';
import IsOptionUnderParent from './IsOptionUnderParent';
import resolveQueryDropdownOptions from './ResolveQueryDropdownOptions';
import { appendOtherOptionIfNeeded, collectParentFieldIdsWithOtherCompanion } from '../Forms/OtherOptionConstants';

/**
 * Applies query-sourced dropdown options onto fields during initial load.
 * @param {Object} retrievedData - Initial query result.
 * @param {Object[]} fields - Flattened field list from page structure.
 */
const applyQueryDropdownOptionsOnLoad = (retrievedData, fields, parentIdsWithCompanion) => {
    for (const field of fields) {
        const optionsName = field.OptionsName || field.optionsName || '';
        const queryOptions = resolveQueryDropdownOptions(retrievedData, optionsName, field);
        if (queryOptions != null) {
            field.Options = queryOptions;
            appendOtherOptionIfNeeded(field, parentIdsWithCompanion);
        }
    }
};

/**
 * Filters dropdown/combobox options by parent field value on initial form load.
 * Only runs when ParentList or ParentValue is a non-empty string.
 * @param {Object} retrievedData - Initial query result keyed by field id.
 * @param {Object[]} pageStructure - Form pages with field definitions.
 * @param {Object[]} lists - Static list configs from Redux.
 */
const PopulateDynamicListsOnLoad = (retrievedData, pageStructure, lists) => {

    const parentIdsWithCompanion = collectParentFieldIdsWithOtherCompanion(pageStructure);

    let  fields = TranslatePageStructureIntoReferencedFieldList(pageStructure);
    fields = fields.filter(f => f.Type === "dropdown" || f.Type === "combobox" || f.Type === "filteredcombo" || f.Type === "hierarchicalpicker" || f.Type === "radio");

    applyQueryDropdownOptionsOnLoad(retrievedData, fields, parentIdsWithCompanion);

    for (const field of fields) {
        const parentList = field.ParentList ?? field.parentList ?? '';
        const optionsName = field.OptionsName || field.optionsName || '';

        if (parentList !== '') {
            const list = lists.filter(l => (l.Name || l.name || '').toLowerCase() === optionsName.toLowerCase());
            if (list.length === 0) {
                continue;
            }
            let parentField = fields.filter(f => f.Id === parentList);
            if (parentField.length > 0) {
                field.Options = list[0].Options.filter(f => IsOptionUnderParent(f, parentField[0].value)).map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}});
                appendOtherOptionIfNeeded(field, parentIdsWithCompanion);
            } else {
                let retrievedKeys = Object.keys(retrievedData);
                parentField = retrievedKeys.filter(r => r === parentList);
                if (parentField.length > 0) {
                    field.Options = list[0].Options.filter(f => IsOptionUnderParent(f, retrievedData[parentField[0]])).map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}});
                    appendOtherOptionIfNeeded(field, parentIdsWithCompanion);
                }
            }
        }
        else {
            const parentValue = field.ParentValue ?? field.parentValue ?? '';
            if (parentValue !== '') {
                const list = lists.filter(l => (l.Name || l.name || '').toLowerCase() === optionsName.toLowerCase());
                if (list.length > 0) {
                    field.Options = list[0].Options.filter(f => IsOptionUnderParent(f, parentValue)).map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}});
                    appendOtherOptionIfNeeded(field, parentIdsWithCompanion);
                }
            }
        }
        appendOtherOptionIfNeeded(field, parentIdsWithCompanion);
    }
}

export default PopulateDynamicListsOnLoad;
