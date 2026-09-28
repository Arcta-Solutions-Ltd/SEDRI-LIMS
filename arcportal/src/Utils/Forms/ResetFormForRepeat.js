import ApplyFormStateRules from '../State/ApplyFormStateRules';
import AddJsonValuesIntoPageStructure from '../PageStructure/AddJsonValuesIntoPageStructure';
import ApplyFormDataRules from '../FormDataRules/ApplyFormDataRules';
import BuildCraftedPageInputData from './BuildCraftedPageInputData';
import NormalizeCraftedPageContents from '../Crafted/NormalizeCraftedPageContents';
import ApplyDefaultsToClearedPages from './ApplyDefaultsToClearedPages';
import { getRepeatFromPage } from './RepeatConfig';

const SERVER_STAMPED_RECORD_KEYS = ['Id', 'AccessionNumber'];
const PARENT_RECORD_KEYS = ['PatientId', 'AdmissionId', 'RequestId'];

/**
 * Rewinds a saved form so it can be run again from the page named by the form's repeat configuration.
 * Pages before that point keep what the user entered, so the next record is registered against the same
 * parents; the repeated pages are cleared and config-driven defaults are re-applied via
 * GetDefaultValuesFromPage.
 *
 * @param {object} state - The form state as it was when the save completed.
 * @param {object} repeat - The Repeat block from the form config.
 * @param {object} createdIds - Save response with record and parent identifiers.
 * @returns {object} A new state positioned on the repeat page.
 */
const ResetFormForRepeat = (state, repeat, createdIds) => {

    const fromPage = getRepeatFromPage(repeat);
    const pages = state.formDef.Pages;
    const startIndex = pages.findIndex((page) => page.Name?.toLowerCase() === fromPage.toLowerCase());
    if (startIndex === -1) {
        return state;
    }

    for (let index = startIndex; index < pages.length; index++) {
        ClearCraftedSelections(pages[index], state.formDef.data.Crafted);
    }

    ApplyDefaultsToClearedPages(pages, startIndex, state.formDef.data);
    ClearServerStampedRecordKeys(state.formDef.data);

    for (const key of PARENT_RECORD_KEYS) {
        const value = ReadCreatedId(createdIds, key);
        if (value !== undefined && value !== null && value !== '') {
            state.formDef.data[key] = value;
        }
    }

    const trimmedState = TrimWorkflowStateForClearedPages(state.currentState, pages, startIndex);
    const repeatPage = pages[startIndex];

    ApplyFormStateRules(state.formDef, trimmedState);
    AddJsonValuesIntoPageStructure(state.formDef.Pages, state.formDef.data, true);
    state.formDef = ApplyFormDataRules(state.formDef, { trigger: 'pageNavigation', newPage: repeatPage });

    if (repeatPage.Crafted && state.formDef.data?.Crafted !== undefined) {
        const newPageData = state.formDef.data.Crafted.filter(
            (item) => item.Name.toLowerCase() === repeatPage.Name.toLowerCase()
        );
        state.formDef.craftedPageData = newPageData.length > 0
            ? NormalizeCraftedPageContents(newPageData[0].Contents)
            : [];
        state.formDef.inputData = BuildCraftedPageInputData(state.formDef);
    }

    return { ...state, currentState: trimmedState, currentPage: repeatPage, fieldChanges: [], rootElements: [] };
};

/**
 * The save response is serialised camel cased by the API, so match on the name without regard to case.
 * @param {object} createdIds
 * @param {string} key
 * @returns {*}
 */
const ReadCreatedId = (createdIds, key) => {
    if (createdIds === undefined || createdIds === null) { return undefined; }

    const match = Object.keys(createdIds).find((name) => name.toLowerCase() === key.toLowerCase());
    return match === undefined ? undefined : createdIds[match];
};

const deleteDataKeyCaseInsensitive = (data, keyName) => {
    const match = Object.keys(data).find((k) => k.toLowerCase() === keyName.toLowerCase());
    if (match !== undefined) {
        delete data[match];
    }
};

const ClearServerStampedRecordKeys = (data) => {
    for (const key of SERVER_STAMPED_RECORD_KEYS) {
        deleteDataKeyCaseInsensitive(data, key);
    }
};

const getPageStateTokens = (page) => {
    const tokens = [];
    const nextButton = page.NextButton ?? page.nextButton;
    const onClickState = nextButton?.OnClickState ?? nextButton?.onclickstate;
    const nextState = onClickState?.State ?? onClickState?.state;
    if (nextState) {
        tokens.push(nextState);
    }
    const entryState = page.EntryState ?? page.entryState;
    if (entryState) {
        tokens.push(entryState);
    }
    return tokens;
};

const TrimWorkflowStateForClearedPages = (currentState, pages, startIndex) => {
    if (currentState === undefined || currentState === null || currentState === '') {
        return currentState ?? '';
    }

    let stateArray = currentState.split(',').map((s) => s.trim()).filter((s) => s !== '');
    for (let index = startIndex; index < pages.length; index++) {
        for (const token of getPageStateTokens(pages[index])) {
            stateArray = stateArray.filter((s) => s.toLowerCase() !== token.toLowerCase());
        }
    }
    return stateArray.join(',');
};

const ClearCraftedSelections = (page, crafted) => {
    if (!page.Crafted || !Array.isArray(crafted)) { return; }

    const entry = crafted.find((item) => item.Name?.toLowerCase() === page.Name?.toLowerCase());
    if (entry === undefined || !Array.isArray(entry.Contents)) { return; }

    for (const item of entry.Contents) {
        if (item.Allowed !== undefined) {
            item.Allowed = 'No';
        }
    }
};

export default ResetFormForRepeat;
