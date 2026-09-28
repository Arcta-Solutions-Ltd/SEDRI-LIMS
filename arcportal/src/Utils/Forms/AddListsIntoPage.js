import LaboratoryList from "../../Classes/Laboratory/LaboratoryList";
import TokenInfo from "../../Classes/Security/TokenInfo";
import { appendOtherOptionIfNeeded, collectParentFieldIdsWithOtherCompanion } from "./OtherOptionConstants";

const AddListsIntoPage = (page, lists, laboratoryId, laboratory, specimentypeid, formName) => {

    let returnPage = structuredClone(page);
    const parentIdsWithCompanion = collectParentFieldIdsWithOtherCompanion([returnPage]);

    const mapOptions = (options, optionsName, fieldType) => {
        const type = (fieldType && String(fieldType).toLowerCase()) || '';
        const isTag = optionsName && optionsName.toLowerCase() === 'tag';
        const useValueAsKey = false;
        const mapped = options.map((option) => {
            const text = option.Text ?? option.Key;
            const key = useValueAsKey ? text : option.Key;
            return { key, text, ParentKey: option.ParentKey };
        });
        if (type === 'hierarchicalpicker' || isTag) {
            return mapped;
        }
        return mapped;
    };

    for (const column of returnPage.Columns) {
        for (const formGroup of column.FormGroups) {
            for (const field of formGroup.Fields) {
                const optionsName = field.OptionsName || field.optionsName;
                if (optionsName !== undefined && optionsName !== "" && ! field.Dynamic) {
                    if (field.ParentList === undefined || field.ParentList === "") {
                        const list = lists.filter(l => l.Name.toLowerCase() === optionsName.toLowerCase());
                        RemoveCultureTypeOptionsBasedUponLaboratory(list, laboratoryId, laboratory, specimentypeid);
                        RemoveSpecimenTypeOptionsBasedUponForm(list, formName, laboratoryId, laboratory);
                        field.Options = mapOptions(list[0].Options, optionsName, field.Type);
                    }
                } else if (Array.isArray(field.Options) && field.Options.length > 0 && field.Options[0].Key !== undefined) {
                    field.Options = mapOptions(field.Options, '', field.Type);
                }
                appendOtherOptionIfNeeded(field, parentIdsWithCompanion);
                if (field.Type === "fieldgrid" && field.GridFields !== undefined) {
                    for (const gridField of field.GridFields) {
                        if (gridField.Type === "dropdown" || gridField.Type === "filteredcombo" || gridField.Type === "combobox" || gridField.Type === "hierarchicalpicker") {
                            const list = lists.filter(l => l.Name.toLowerCase() === gridField.OptionsName.toLowerCase());
                            gridField.Options = mapOptions(list[0].Options, gridField.OptionsName, gridField.Type);
                        }
                    }
                }
            }
        }
    }

    return returnPage;
}

const RemoveCultureTypeOptionsBasedUponLaboratory = (list, laboratoryId, laboratory, specimentypeid) => {
    const newOptions = [];
    const listIndex = list.findIndex((l) => l.Name === "culturetype");
    if (listIndex !== -1 && laboratoryId !== undefined) {
        const newListEntry = structuredClone(list[listIndex]);
        const labConfig = new LaboratoryList(laboratory, specimentypeid);
        const currentLaboratory = labConfig.getLaboratory(laboratoryId);
        if (currentLaboratory.CultureTypeOptions !== undefined) {
            for (const entry of newListEntry.Options) {
                if (currentLaboratory.CultureTypeOptions.Values.includes(entry.Key)) {
                    newOptions.push(entry);
                }
            }
            newListEntry.Options = newOptions;
        } else {
            newListEntry.Options = list[0].Options;
        }
        list[listIndex] = newListEntry;
    }
}

/**
 * Narrows the specimen type list to the types the laboratory has configured for this form. A form with no
 * configured entry is left unrestricted, which is what keeps every form that predates this restriction working.
 */
const RemoveSpecimenTypeOptionsBasedUponForm = (list, formName, laboratoryId, laboratory) => {
    const listIndex = list.findIndex((l) => l.Name.toLowerCase() === "specimentype");
    if (listIndex === -1 || formName === undefined || formName === "") { return; }

    const resolvedLaboratoryId = ResolveLaboratoryId(laboratoryId);
    if (resolvedLaboratoryId === undefined) { return; }

    const currentLaboratory = new LaboratoryList(laboratory, null).getLaboratory(resolvedLaboratoryId);
    const allowed = currentLaboratory?.FormSpecimenTypeOptions?.[formName.toLowerCase()];
    if (allowed === undefined || allowed.length === 0) { return; }

    const newListEntry = structuredClone(list[listIndex]);
    newListEntry.Options = newListEntry.Options.filter((option) => allowed.includes(option.Key));
    list[listIndex] = newListEntry;
}

/**
 * A create form has no selected record to take the laboratory from, so fall back to the laboratory the user is
 * signed in against.
 */
const ResolveLaboratoryId = (laboratoryId) => {
    if (laboratoryId !== undefined && laboratoryId !== null && laboratoryId !== "") { return laboratoryId; }

    try {
        return new TokenInfo("arctoken").LaboratoryId;
    } catch {
        return undefined;
    }
}

export default AddListsIntoPage;