import Post from '../../Data/Post';
import ApplyFormStateRules from '../State/ApplyFormStateRules';
import AddJsonValuesIntoPageStructure from '../PageStructure/AddJsonValuesIntoPageStructure';
import ApplyDependentDefaultsFromPage from '../PageStructure/ApplyDependentDefaultsFromPage';
import GetDefaultValuesFromPage from '../PageStructure/GetDefaultValuesFromPage';
import AddValuesIntoArrayList from '../Forms/AddValuesIntoArrayList';
import AddValuesIntoValueList from '../Forms/AddValuesIntoValueList';
import RequiredFieldCheck from './RequiredFieldCheck';
import GetNextPage from '../PageStructure/GetNextPage';
import GetPreviousPage from '../PageStructure/GetPreviousPage';
import PopulateDynamicLists from '../General/PopulateDynamicLists';
import GetDataEntryFormFromFormNameConfig from '../Forms/GetDataEntryFormFromFormNameConfig';
import PopulateDynamicListsOnLoad from './PopulateDynamicListsOnLoad';
import GetDynamicListNames, {AddDynamicListsIntoForm} from '../Forms/GetDynamicListNames';
import {AddMetaDataToParameters} from '../../components/Containers/FormHandler/Functions/AddFilterMetaData';

/**
 * Builds filteredget Parameters for form InitialQuery: always includes id, then optional extras (e.g. Source, TestName from record navigation).
 * Excludes duplicate keys (case-insensitive); id always comes from recordId.
 */
const buildInitialQueryParameterList = (recordId, filters, initialQueryExtras) => {
    const params = [{ Key: 'id', Value: recordId }];
    if (!initialQueryExtras || !Array.isArray(initialQueryExtras) || initialQueryExtras.length === 0) {
        return AddMetaDataToParameters(filters, params);
    }
    const seen = new Set(['id']);
    for (const p of initialQueryExtras) {
        if (!p || p.Key === undefined || p.Key === null) continue;
        const kl = String(p.Key).toLowerCase();
        if (seen.has(kl)) continue;
        seen.add(kl);
        params.push({ Key: p.Key, Value: p.Value === undefined || p.Value === null ? '' : String(p.Value) });
    }
    return AddMetaDataToParameters(filters, params);
};
import {AddBlankCraftedStructureForPagesWithoutData} from '../../components/Crafted/CraftedFormHandler';
import SpecialCalculations from './SpecialCalculations';
import RemoveHierarchicalElements from './RemoveHierarchicalElements';
import NullInvisiblePageElements from './NullInvisiblePageElements';
import NullInvisibleFormGroupElements from './NullInvisibleFormGroupElements';
import NullOtherDetailsWhenNotSelected from './NullOtherDetailsWhenNotSelected';
import { appendOtherOptionToAllPageFields } from '../Forms/OtherOptionConstants';
import NormalizeMutuallyExclusiveAntibioticFields from './NormalizeMutuallyExclusiveAntibioticFields';
import { CalculateStateFromFormStateRules, getDistinctEffects } from '../State/ApplyFormStateRules';
import DeriveGrowthTypeParentId from '../Forms/DeriveGrowthTypeParentId';
import ApplyFormDataRules from '../FormDataRules/ApplyFormDataRules';
import syncCalculatedDisplayFieldsToFieldChanges, { stripCalculatedDisplayFieldsFromSaveData } from '../FormDataRules/SyncCalculatedDisplayFields';
import BuildCraftedPageInputData from '../Forms/BuildCraftedPageInputData';
import NormalizeCraftedPageContents from '../Crafted/NormalizeCraftedPageContents';


/**
 * Returns true when localFormValues contains at least one own property (fieldgrid row pass-through).
 * @param {object|null|undefined} localFormValues - Row values from parent FieldGrid.
 * @returns {boolean}
 */
const hasLocalFormValues = (localFormValues) =>
    localFormValues !== undefined &&
    localFormValues !== null &&
    typeof localFormValues === 'object' &&
    !Array.isArray(localFormValues) &&
    Object.keys(localFormValues).length > 0;

/**
 * @param {object|undefined} formDef - Form definition from config.
 * @returns {string} InitialQuery name, or empty string when unset.
 */
const getFormInitialQuery = (formDef) => {
    const query = formDef?.InitialQuery ?? formDef?.initialQuery ?? '';
    return query === undefined || query === null ? '' : String(query);
};

/**
 * @param {object|undefined} formDef - Form definition from config.
 * @returns {boolean} True when fieldgrid edit should pass parent row data without fetching.
 */
const isDeferredFieldGridForm = (formDef) =>
    String(formDef?.SaveOperation ?? formDef?.saveOperation ?? '').toLowerCase() === 'updategrid';

/**
 * Opens a form: deferred fieldgrid pass-through, InitialQuery fetch, or UseListData defaults.
 * @param {object} [localFormValues] - Row values from parent FieldGrid ({@link localFormData.values}); may be {} for add-from-grid.
 */
const InvokeFormHandlerUsingForm = (form, recordId, dataRetrievedSuccessfully, errorWhenRetrievingData, button, workflowStateId, filters, selectedRecord, initialQueryExtras, localFormValues) => {
    if (form[0] !== undefined) {
        const formDef = form[0];
        const initialQuery = getFormInitialQuery(formDef);
        const extraInfo = { button: button, id: recordId, form: formDef.Name, workflowStateId: workflowStateId };

        if (initialQuery !== '' && !isDeferredFieldGridForm(formDef)) {
            const parameters = buildInitialQueryParameterList(recordId, filters, initialQueryExtras);
            const criteria = { Name: initialQuery, Parameters: parameters };
            Post('query/filteredget', criteria, dataRetrievedSuccessfully, errorWhenRetrievingData, extraInfo);
        } else if (hasLocalFormValues(localFormValues)) {
            dataRetrievedSuccessfully(localFormValues, extraInfo);
        } else if (initialQuery !== '') {
            const parameters = buildInitialQueryParameterList(recordId, filters, initialQueryExtras);
            const criteria = { Name: initialQuery, Parameters: parameters };
            Post('query/filteredget', criteria, dataRetrievedSuccessfully, errorWhenRetrievingData, extraInfo);
        } else {
            let dataToUse = null;
            if (localFormValues !== undefined && localFormValues !== null) {
                dataToUse = localFormValues;
            } else {
                dataToUse = formDef.UseListData === undefined || !formDef.UseListData ? null : selectedRecord;
            }
            dataRetrievedSuccessfully(dataToUse, extraInfo);
        }
    } else {
        errorWhenRetrievingData("Definition for form " + form.Name + " cannot be found");
    }
}

const InvokeFormHandler = (button, recordId, uievents, forms, dataRetrievedSuccessfully, errorWhenRetrievingData, workflowStateId, filters, selectedRecord, initialQueryExtras, localFormValues) => {
    const action = uievents.filter(a => a.Name === button.UIEvent);

    if (action.length > 0) {
        const form = forms.filter(f => f.Name === action[0].Action);

        InvokeFormHandlerUsingForm(form, recordId, dataRetrievedSuccessfully, errorWhenRetrievingData, button, workflowStateId, filters, selectedRecord, initialQueryExtras, localFormValues);
    }
}

/**
 * Returns crafted page contents as the key/value array shape crafted components expect.
 * @param {{Contents: *}|undefined} craftedItem - Entry from formDef.data.Crafted for the page being shown.
 * @returns {Array} Normalized key/value entries for the page.
 */
const GetCraftedPageContents = (craftedItem) => NormalizeCraftedPageContents(craftedItem?.Contents);

const DataRetrievedHandler = (data, extrainfo, forms, pages, lists, laboratoryId, laboratory, specimentypeid) => {
    const formDef = GetDataEntryFormFromFormNameConfig(extrainfo.form, forms, pages, lists, laboratoryId, laboratory, specimentypeid);

    ApplyFormStateRules(formDef, formDef.StartState);

    if (data === undefined || data === null) {
        data = GetDefaultValuesFromPage(formDef.Pages, null);
    } else {
        if (!Array.isArray(data)) {
            NormalizeMutuallyExclusiveAntibioticFields(data);
        }
        var keys = Object.keys(data);
        if (keys.length === 1 && keys[0] === 'Crafted') { 
            var crafted = data["Crafted"];
            data = GetDefaultValuesFromPage(formDef.Pages, null);
            data["Crafted"] = crafted;
        } else {

            if (! Array.isArray(data)) {
                var defaults = GetDefaultValuesFromPage(formDef.Pages, keys);
                data = {...data, ...defaults};
            }
        }
    }

    formDef.Pages = AddJsonValuesIntoPageStructure(formDef.Pages,data, true);

    const growthTypeParentId = DeriveGrowthTypeParentId(lists, data?.growthid ?? data?.GrowthId);
    if (growthTypeParentId !== undefined) {
        data = { ...data, growthTypeParentId };
    }
    formDef.data = data;
    const newPage = formDef.Pages[0];
    if (formDef.data !== undefined && formDef.data.Crafted !== undefined) {
        for (const craft of formDef.data.Crafted) {
            craft.Contents = JSON.parse(craft.Contents);
        }
    }
    AddBlankCraftedStructureForPagesWithoutData(formDef.Pages, data);

    if (formDef.data?.Crafted !== undefined) {
        for (const craft of formDef.data.Crafted) {
            craft.Contents = NormalizeCraftedPageContents(craft.Contents);
        }
    }

    // Read the first page's crafted contents only once the blank structure is in place, because a form can
    // open straight onto a crafted page that the initial query knows nothing about.
    if (newPage.Crafted && formDef.data?.Crafted !== undefined) {
        const newPageData = formDef.data.Crafted.filter((item) => item.Name.toLowerCase() === newPage.Name.toLowerCase());
        formDef.craftedPageData = GetCraftedPageContents(newPageData[0]);
        formDef.inputData = BuildCraftedPageInputData(formDef);
    }

    ApplyFormDataRules(formDef, { trigger: 'initialLoad' });

    PopulateDynamicListsOnLoad(formDef.data, formDef.Pages, lists);
    appendOtherOptionToAllPageFields(formDef.Pages);
    const dynamicLists = GetDynamicListNames(formDef);

    const additionalData = {};
    if (extrainfo.form === "approvereportform" || extrainfo.form === "unapprovereportform") {
        extrainfo.id = data.Id;
        additionalData.reportDisplayConfig = data;
    }

    return { id: extrainfo.id, formDef: formDef, currentState: formDef.StartState, currentPage: newPage, workflowStateId: extrainfo.workflowStateId, dynamicLists: dynamicLists, additionalData: additionalData, rootElements: [] };
}

const IncludeDynamicListsInForm = (state, lists) => {
    AddDynamicListsIntoForm(state.formDef, lists)
    return {...state};
}

/** Save events whose payloads must not include read-only query metadata from initial load. */
const CONFIG_FORM_GROUP_SAVE_EVENTS = new Set([
    'editformgroup',
    'addformgroup',
    'reorderformgroups',
    'movefield',
    'moveformgroup',
    'editpagerules',
    'addfield',
    'editfield',
    'addexistingfield',
]);

/** Query-only keys stripped from all configuration form-group save payloads. */
const CONFIG_QUERY_ONLY_KEYS = [
    'fieldoptions',
    'effectoptions',
    'key',
    'name',
    'stateoptions',
    'statelocked',
    'childlists',
    'pagelistfields',
    'existingfieldoptions',
    'targetpagetitle',
    'targettable',
    'pageoptions',
    'formgroupoptions',
    'subsectionlabel',
];

/**
 * Additional query-only keys stripped for specific save events only.
 * FieldList is stripped for addformgroup (new subsections are empty); editformgroup and
 * reorderformgroups must post FieldList for field/subsection order persistence.
 */
const CONFIG_QUERY_ONLY_KEYS_BY_EVENT = {
    addformgroup: ['fieldlist'],
};

/**
 * Removes read-only query metadata from configuration form-group save payloads.
 * FieldList is preserved for editformgroup and reorderformgroups; stripped only for addformgroup.
 * @param {Object} dataToSave - Merged form data about to be posted.
 * @param {string|undefined} saveEvent - Form SaveEvent name.
 * @returns {Object} Payload with query-only keys removed when applicable.
 */
const StripConfigurationQueryMetadata = (dataToSave, saveEvent) => {
    if (!saveEvent || !CONFIG_FORM_GROUP_SAVE_EVENTS.has(String(saveEvent).toLowerCase())) {
        return dataToSave;
    }

    const eventKey = String(saveEvent).toLowerCase();
    const extraKeys = CONFIG_QUERY_ONLY_KEYS_BY_EVENT[eventKey] ?? [];
    const keysToStrip = [...CONFIG_QUERY_ONLY_KEYS, ...extraKeys];

    const stripped = { ...dataToSave };
    for (const key of Object.keys(stripped)) {
        if (keysToStrip.includes(key.toLowerCase())) {
            delete stripped[key];
        }
    }
    return stripped;
};

/**
 * Prepares merged form data for save, clearing hidden form-group and invisible page field values first.
 * Configuration form-group saves omit read-only query metadata (fieldOptions, effectOptions,
 * existingFieldOptions). FieldList is omitted only for addformgroup; editformgroup and
 * reorderformgroups must include FieldList for order persistence.
 * rootElements are merged only on crafted pages; non-crafted saves use fieldChanges only.
 */
const PrepareDataFormSavingHandler = (state, viewName) => {

    if (state.currentPage.Crafted) {
        const craftedItem = state.formDef.data.Crafted.findIndex((item) => item.Name === state.currentPage.Name);

        state.formDef.data.Crafted[craftedItem].Contents = AddValuesIntoArrayList(state.formDef.data.Crafted[craftedItem].Contents, state.fieldChanges);
        if (Array.isArray(state.rootElements)) {
            state.formDef.data = AddValuesIntoValueList(state.formDef.data, state.rootElements);
            state.formDef.Pages = AddJsonValuesIntoPageStructure(state.formDef.Pages,state.formDef.data); 
            state.rootElements = [];
        }
    } else {
        state.formDef.data = AddValuesIntoValueList(state.formDef.data, state.fieldChanges);
    }

    const saveEventName = String(state.formDef.SaveEvent ?? state.formDef.saveEvent ?? '').toLowerCase();
    const isRecordEditSave = saveEventName === 'editrequest' || saveEventName === 'editadmission';

    if (!isRecordEditSave) {
        state.formDef.data = NullInvisibleFormGroupElements(state.formDef, state.currentPage?.Name ?? state.currentPage?.name);
        state.formDef.data = NullOtherDetailsWhenNotSelected(state.formDef);
        if (!getFormInitialQuery(state.formDef)) {
            state.formDef.data = NullInvisiblePageElements(state.formDef);
        }
    }
    let dataToSave = { ...state.formDef.data };
    delete dataToSave.growthTypeParentId;

    // A crafted page a form opens straight onto is seeded with a copy of the record rather than a selection
    // list. The backend reads Crafted as lists of selections, so those entries are left out of the payload.
    if (Array.isArray(dataToSave.Crafted)) {
        dataToSave.Crafted = dataToSave.Crafted.filter((item) => Array.isArray(item.Contents));
    }

    if (state.formDef.SaveEvent !== undefined && state.formDef.SaveEvent !== "") {
        dataToSave["Event"] = state.formDef.SaveEvent;
        dataToSave["View"] = viewName;
        dataToSave["FormName"] = state.formDef.Name ?? state.formDef.name;
        if (state.id !== undefined && state.id !== 0  ) { dataToSave["Id"] = state.id; }
        if (state.workflowStateId > 0) { dataToSave["StateId"] = state.workflowStateId; }
    }

    dataToSave = StripConfigurationQueryMetadata(dataToSave, state.formDef.SaveEvent);
    dataToSave = stripCalculatedDisplayFieldsFromSaveData(dataToSave);

    return {...state, data: dataToSave};
}

const NextButtonClickHandler = (page, fieldChangesToUse, sourceOfClick, state, lists, laboratory, workflow, language) => {
    let errorField = "";
    if (sourceOfClick !== "customnovalidation") {
        errorField = RequiredFieldCheck(state.fieldChanges,page.Required, page.RequiredRule, page.RequiredError, state.formDef.Pages);
    } 
    let newPage;
    let newState = state.currentState;

    if (errorField === "") {

        newState = CalculateStateFromFormStateRules(page, state, sourceOfClick);

        ApplyFormStateRules(state.formDef, newState);
        const specimenTypeIdForWorkflow = state.formDef.data.SpecimenTypeId ?? state.formDef.data.specimentypeid;
        const laboratoryIdForWorkflow = state.formDef.data.LaboratoryId ?? state.formDef.data.laboratoryid;
        newPage = GetNextPage(page, state.formDef.Pages, specimenTypeIdForWorkflow, laboratory, workflow, laboratoryIdForWorkflow, language);
        state = NavButtonClickHandler(page, newPage, fieldChangesToUse, newState,state, lists);
    }

    return {...state, currentPage: newPage, error: errorField, currentState: newState};
}

const PreviousButtonClickHandler = (page, state) => {
        const newPage = GetPreviousPage(page,state.formDef.Pages);

        let newState = state.currentState;
        newState = newPage.EntryState !== undefined && newPage.EntryState !== "" ? newPage.EntryState : newState;
        ApplyFormStateRules(state.formDef, newState);  
        state = NavButtonClickHandler(page, newPage, undefined, newState, state);

        return state;
    }

const FullScreenButtonClickHander = (state) => {
    state = NavButtonClickHandler(state.currentPage, state.currentPage, undefined, undefined, state);
    return state;
}

/**
 * Merges departing-page field changes into form data, applies dependent field defaults
 * (<:fieldId:> placeholders), then runs page-navigation form data rules.
 */
const NavButtonClickHandler = (oldPage, newPage, fieldListToUse, newState, state, lists) => {

        let craftedItem;

        if (oldPage.Crafted) {
            craftedItem = state.formDef.data.Crafted.findIndex((item) => item.Name === oldPage.Name);
            if (Array.isArray(fieldListToUse)) {
                state = MultipleFieldContentsChangeHandler("multiplechanges", fieldListToUse, { rootCopy: true}, lists, state);
            }

            if (Array.isArray(state.rootElements)) {
                state.formDef.data = AddValuesIntoValueList(state.formDef.data,state.rootElements);
                state.formDef.Pages = AddJsonValuesIntoPageStructure(state.formDef.Pages,state.formDef.data); 
                state.rootElements = [];
            }
        } else {
            state.formDef.data = AddValuesIntoValueList(state.formDef.data,state.fieldChanges);
            state.formDef.Pages = AddJsonValuesIntoPageStructure(state.formDef.Pages,state.formDef.data); 
        }

        const dependentDefaults = ApplyDependentDefaultsFromPage(state.formDef.Pages, oldPage, state.formDef.data);
        for (const defValue of dependentDefaults) {
            state.formDef.data[defValue.id] = defValue.value;
        }
        if (dependentDefaults.length > 0) {
            state.formDef.Pages = AddJsonValuesIntoPageStructure(state.formDef.Pages, state.formDef.data);
        }

        state.formDef = ApplyFormDataRules(state.formDef, { trigger: 'pageNavigation', newPage });

        if (newPage !== undefined && newPage.Crafted) {
            const newPageData = state.formDef.data.Crafted.filter((item) => item.Name === newPage.Name);
            state.formDef.craftedPageData = GetCraftedPageContents(newPageData[0]);
            state.formDef.inputData = BuildCraftedPageInputData(state.formDef);
        }

        newState = newState === undefined ? state.currentState : newState;
        return { ...state, currentState: newState, currentPage: newPage, fieldChanges: []};
    }

const MultipleFieldContentsChangeHandler = (id, value, extraInfo, lists, state) => {
    if (id === "multiplechanges") {
        for (const item of value) {
            state = FieldContentsChangeHandler(item.key,item.value,extraInfo, lists, state);
        }
    } else {
        state = FieldContentsChangeHandler(id,value,extraInfo, lists, state);
    }

    return state;
}
/**
 * Handles a single field value change, updating fieldChanges and optionally rootElements when rootCopy is set.
 * Crafted components use rootCopy to lift values to the form root for save/navigation on crafted pages.
 * Recomputes currentState from onClickState rules and applies form page visibility so navigation buttons
 * and the progress bar react immediately to branching field changes (e.g. Acknowledge Receipt Action).
 */
const FieldContentsChangeHandler = (id, value, extraInfo, lists, state) => {

    let tempFieldChanges = [...state.fieldChanges];
    const rowToChange = tempFieldChanges.findIndex(r => r.key === id);
    if (rowToChange !== -1) {
        tempFieldChanges[rowToChange] = {key: id, value: value};
    } else {
        tempFieldChanges.push({key: id, value: value});
    }

    if ((id || '').toLowerCase() === 'growthid') {
        const growthTypeParentId = DeriveGrowthTypeParentId(lists, value);
        tempFieldChanges.push({ key: 'growthTypeParentId', value: growthTypeParentId ?? '' });
    }

    tempFieldChanges = SpecialCalculations(id, value, tempFieldChanges);

    // Make a copy of the crafted elements in the object root

    const rootCopy = extraInfo === undefined || extraInfo.rootCopy === undefined ? false : extraInfo.rootCopy;
    const rootElements = state.rootElements === undefined ? [] :  [...state.rootElements];
    if (rootCopy && value !== undefined) {
        rootElements.push({key: id, value: value.value});
    }

//    if (extraInfo !== undefined && extraInfo.parentKey !== undefined) {
        PopulateDynamicLists(state.formDef.Pages, lists, id, value, state.formDef.data);
        tempFieldChanges = RemoveHierarchicalElements(id, value, tempFieldChanges, state.formDef.Pages);
//    }

    state.formDef.data = AddValuesIntoValueList(state.formDef.data,tempFieldChanges);

    state.formDef = ApplyFormDataRules(state.formDef, { trigger: 'fieldChange', changedFieldId: id });
    state.formDef.Pages = AddJsonValuesIntoPageStructure(state.formDef.Pages, state.formDef.data);

    tempFieldChanges = syncCalculatedDisplayFieldsToFieldChanges(state.formDef, tempFieldChanges);

    const onClickState = state.currentPage?.NextButton?.OnClickState;
    const distinctEffects = getDistinctEffects(onClickState?.Rules ?? []);
    let newState = state.currentState;
    if (distinctEffects.length > 1) {
        newState = CalculateStateFromFormStateRules(state.currentPage, state, "page");
        ApplyFormStateRules(state.formDef, newState);
    }

    return {...state, fieldChanges: tempFieldChanges, rootElements: rootElements, currentState: newState };
}

export default InvokeFormHandler;
export { DataRetrievedHandler, PrepareDataFormSavingHandler, NextButtonClickHandler, PreviousButtonClickHandler, FieldContentsChangeHandler, InvokeFormHandlerUsingForm,
     FullScreenButtonClickHander, IncludeDynamicListsInForm, MultipleFieldContentsChangeHandler };
export { default as BuildCraftedPageInputData } from '../Forms/BuildCraftedPageInputData';