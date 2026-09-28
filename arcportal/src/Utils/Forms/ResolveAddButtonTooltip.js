/**
 * Maps add-button UI events to language tags when ListViewConfig.AddButtonTooltip is not set.
 * Backend config is authoritative when present; this fallback keeps tooltips correct before config reload.
 */
import TranslateTag from '../Local/TranslateTag';

const ADD_BUTTON_TOOLTIP_BY_UI_EVENT = {
    adddirecttestuievent: '@ConAddB@',
    addculturetestuievent: '@ConAddA@',
    addreportconfiguievent: '@ConAddL@',
    addworkflowuievent: '@ConAddAB@',
    addtestcategoryuievent: '@LabAddA@',
    addculturetypecategoryuievent: '@LabAddD@',
    addspecimentypedirecttestoptionuievent: '@LabAddG@',
    addspecimentypeculturetypeoptionuievent: '@LabAddH@',
    addculturetypeculturetestoptionuievent: '@LabAddI@',
    addspecimentypeworkflowuievent: '@LabAddJ@',
    addspecimentypedirecttestuievent: '@ConAddE@',
    addspecimentypeculturetypeuievent: '@ConAddD@',
    addculturetypeculturetestuievent: '@ConAddZ@',
    addformspecimentypeoptionuievent: '@ConFormSpeAdd@',
    addorganismscopeculturetestoptionuievent: '@LabOrgC@',
    addsectionuievent: '@ConAddN@',
};

/**
 * English fallbacks when a tag is not yet present in the loaded frontend language bundle.
 * Values must match {@link EnglishLanguage.cs}.
 */
const ADD_BUTTON_TOOLTIP_ENGLISH = {
    '@ConAddA@': 'Add Isolate Test',
    '@ConAddAB@': 'Add workflow',
    '@ConAddB@': 'Add Direct Test',
    '@ConAddD@': 'Add Culture Type Default',
    '@ConAddE@': 'Add Direct Test Default',
    '@ConAddJ@': 'Add Field Definition',
    '@ConAddL@': 'Add new report',
    '@ConAddN@': 'Add report section',
    '@ConAddZ@': 'Add Isolate Test Default',
    '@ConFormSpeAdd@': 'Add Form Specimen Type Option',
    '@LabAddA@': 'Add test category rule',
    '@LabAddD@': 'Add culture type category rule',
    '@LabAddG@': 'Add direct test options',
    '@LabAddH@': 'Add culture type options',
    '@LabAddI@': 'Add isolate test options',
    '@LabAddJ@': 'Add specimen type workflow rule',
    '@LabOrgC@': 'Add organism scope isolate test option',
};

/**
 * Resolves the language tag for an embedded list add (+) header tooltip.
 * @param {Object|undefined} config - Embedded list view config from the server.
 * @returns {string} Language tag for the add-button tooltip.
 */
const resolveAddButtonTooltip = (config) => {
    if (config?.AddButtonTooltip) {
        return config.AddButtonTooltip;
    }

    const addButton = config?.AddButton;
    if (addButton) {
        const mapped = ADD_BUTTON_TOOLTIP_BY_UI_EVENT[String(addButton).toLowerCase()];
        if (mapped) {
            return mapped;
        }
    }

    return '@ConAddJ@';
};

/**
 * Translates an add-button tooltip tag using the loaded language bundle.
 * @param {string} tag - Language tag or pre-translated text from server config.
 * @param {Array<Object>|undefined} language - Frontend language items from Redux.
 * @returns {string} Translated tooltip text.
 */
const translateAddButtonTooltip = (tag, language) => {
    if (!tag) {
        return '';
    }
    if (!String(tag).startsWith('@')) {
        return tag;
    }
    return TranslateTag(tag, language) || ADD_BUTTON_TOOLTIP_ENGLISH[tag] || tag;
};

export default resolveAddButtonTooltip;
export { translateAddButtonTooltip };
