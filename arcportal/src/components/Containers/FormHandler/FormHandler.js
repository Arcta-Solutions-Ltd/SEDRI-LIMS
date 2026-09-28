import React, {useState, useEffect, useRef} from 'react';
import { tryBeginFormSave, clearFormSaveInFlight } from '../../../Utils/General/formSaveInFlight';
import {connect} from 'react-redux';
import InvokeFormHandler, {DataRetrievedHandler, PrepareDataFormSavingHandler, NextButtonClickHandler, PreviousButtonClickHandler, FieldContentsChangeHandler, InvokeFormHandlerUsingForm, IncludeDynamicListsInForm} from '../../../Utils/General/FormStateHandler';
import AddFilterMetaData from './Functions/AddFilterMetaData';
import SinglePagePanel from '../../Forms/SinglePagePanel/SinglePagePanel';
import PostEvent from '../../../Data/PostEvents';
import PopulateFieldsWithNewContents from '../../../Utils/PageStructure/PopulateFieldsWithNewContents';
import syncCalculatedDisplayFieldsToFieldChanges from '../../../Utils/FormDataRules/SyncCalculatedDisplayFields';
import PopulateCraftedWithNewContents from '../../../Utils/PageStructure/PopulateCraftedWithNewContents';
import GetWithNoParams from '../../../Data/GetWithNoParams';
import {PostList} from '../../../Data/Post';
import TransformDatesInJson from '../../../Utils/Local/TransformDatesInJson';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import AdjustPageToHandleMultiPageView from '../../../Utils/General/MultiPageHandler';
import { SetDynamicDefaultValues } from '../../../Utils/PageStructure/GetDefaultValuesFromPage';
import RequiredFieldCheck from '../../../Utils/General/RequiredFieldCheck';
import { SetTestAndCultureTypeDisplays } from '../../../Utils/PageStructure/PopulateCraftedWithNewContents';
import resolveCultureTypeId from '../../../Utils/Specimen/resolveCultureTypeId';
import ResetFormForRepeat from '../../../Utils/Forms/ResetFormForRepeat';
import { getRepeatFromPage } from '../../../Utils/Forms/RepeatConfig';
import RepeatPrompt from '../../Forms/RepeatPrompt/RepeatPrompt';

const FormHandler = (props) => {

    const [dataEntryErrorStatus, updateDataEntryErrorStatus] = useState({visible: false, message: ''});
    const [dataEntryState, setDataEntryState] = useState();
    const [formShellVisible, setFormShellVisible] = useState(false);
    const [formDataReady, setFormDataReady] = useState(false);
    const [shellFormDef, setShellFormDef] = useState(null);
    const [fullScreen, setFullScreen] = useState(false);
    const [displayModeSwitchable, setDisplayModeSwitchable] = useState(true);
    const [finishAction, setFinishAction] = useState("");
    const [refreshConfig, setRefreshConfig] = useState(false);
    const [saveEnabled, setSaveEnabled] = useState(true);
    const [repeatPrompt, setRepeatPrompt] = useState(undefined);
    const saveInFlightRef = useRef(false);
    const formLoadGenerationRef = useRef(0);
    const dataEntryStateRef = useRef(undefined);

    useEffect(() => {
        dataEntryStateRef.current = dataEntryState;
    }, [dataEntryState]);

    /**
     * Applies dynamic list options to form state when list/get returns.
     * Ignores stale async callbacks from a prior form load (generation mismatch).
     * Preserves rootElements from the current form session only when form name and record id match.
     * @param {object} data - Dynamic list payloads from list/get.
     * @param {object} extraInfo - Form state snapshot from when PostList was invoked (includes loadGeneration).
     */
    const refreshCalculatedDisplayFields = (formDef, fieldChanges) => {
        const syncedFieldChanges = syncCalculatedDisplayFieldsToFieldChanges(formDef, fieldChanges);
        PopulateFieldsWithNewContents(formDef.Pages, syncedFieldChanges);
        return syncedFieldChanges;
    };

    const dynamicListsRetrieved = (data, extraInfo) => {
        if (extraInfo?.loadGeneration !== formLoadGenerationRef.current) {
            return;
        }

        IncludeDynamicListsInForm(extraInfo, data);

        const currentState = dataEntryStateRef.current;
        if (
            currentState !== undefined &&
            currentState.formDef?.Name === extraInfo.formDef?.Name &&
            currentState.id === extraInfo.id &&
            Array.isArray(currentState.rootElements) &&
            currentState.rootElements.length > 0
        ) {
            extraInfo.rootElements = currentState.rootElements;
        }

        extraInfo.fieldChanges = refreshCalculatedDisplayFields(extraInfo.formDef, extraInfo.fieldChanges ?? []);
        setDataEntryState(extraInfo);
        setFormDataReady(true);
    };

    useEffect(() => {
        const dataRetrievedSuccessfully = (data, extraInfo) => {
            if (extraInfo.form === "monitoringjsonviewer") {
                TransformDatesInJson(data);
            }

            const state = DataRetrievedHandler(data, extraInfo, props.forms, props.pages, props.lists, props.startConfig.selectedRecord?.laboratoryid, props.laboratory, props.startConfig.selectedRecord?.specimentypeid);

            if (extraInfo.form === "ackreceiptform" || extraInfo.form === "testselectionform" || extraInfo.form === "culturetestselectionform") {
                const rawSpecimenTypeId = extraInfo.form === "ackreceiptform"
                    ? data.SpecimenTypeId
                    : (props.startConfig.selectedRecord?.specimentypeid
                        ?? props.startConfig.selectedRecord?.SpecimenTypeId
                        ?? data?.SpecimenTypeId
                        ?? data?.specimentypeid);
                if (rawSpecimenTypeId != null && rawSpecimenTypeId !== "") {
                    const specimentypeid = String(rawSpecimenTypeId);
                    let cultureTypeId = null;

                    // For culture test selection, get the culture type ID from the selected record or data
                    if (extraInfo.form === "culturetestselectionform") {
                        cultureTypeId = resolveCultureTypeId(props.startConfig, data);
                        SetTestAndCultureTypeDisplays(data.Crafted, specimentypeid, props.laboratory, props.startConfig.selectedRecord.laboratoryid, false, cultureTypeId);
                    } else {
                        SetTestAndCultureTypeDisplays(data.Crafted, specimentypeid, props.laboratory, props.startConfig.selectedRecord.laboratoryid, true);
                    }
                }
            }

            const initialFieldChanges = refreshCalculatedDisplayFields(state.formDef, []);
            setDataEntryState({...state, button: extraInfo.button, fieldChanges: initialFieldChanges, rootElements: state.rootElements ?? [] });

            if (state.dynamicLists.length > 0) {
                PostList(
                    state.dynamicLists,
                    dynamicListsRetrieved,
                    errorWhenRetrievingData,
                    {
                        ...state,
                        button: extraInfo.button,
                        fieldChanges: initialFieldChanges,
                        rootElements: state.rootElements ?? [],
                        loadGeneration: formLoadGenerationRef.current,
                    },
                    props.startConfig.id
                );
            } else {
                setFormDataReady(true);
            }
        }

        if (props.startConfig !== undefined && props.startConfig.button !== undefined) {
            formLoadGenerationRef.current += 1;

            setFinishAction(props.startConfig.button.OnFinish !== undefined ? props.startConfig.button.OnFinish : props.startConfig.button.onFinish);
            setRefreshConfig(props.startConfig.button.refreshConfig);

            let formForShell = [];
            if (props.startConfig.button.UIEvent === undefined) {
                formForShell = props.forms.filter(f =>
                    f.Name === props.startConfig.formName || f.Title === props.startConfig.formName);
            } else {
                const actionForShell = props.uievents.filter((a) => a.Name === props.startConfig.button.UIEvent);
                if (actionForShell.length > 0) {
                    formForShell = props.forms.filter((f) => f.Name === actionForShell[0].Action);
                }
            }
            const rawPages = formForShell[0]?.Pages;
            const pages = Array.isArray(rawPages) && rawPages.length > 0 ? rawPages : [{}];
            setShellFormDef({
                Pages: pages,
                SuppressRecordView: formForShell[0]?.SuppressRecordView,
                RecordView: formForShell[0]?.RecordView,
            });
            setFormShellVisible(true);
            setFormDataReady(false);
            setDataEntryState(undefined);

            if (props.startConfig.button.UIEvent === undefined) {
                const form = props.forms.filter(f =>
                    f.Name === props.startConfig.formName || f.Title === props.startConfig.formName);
                InvokeFormHandlerUsingForm(form, props.startConfig.id, dataRetrievedSuccessfully, errorWhenRetrievingData,undefined,undefined,props.filters, props.startConfig.selectedRecord, props.startConfig.initialQueryParameters, props.startConfig.localFormData?.values);
            } else {
                InvokeFormHandler(props.startConfig.button, props.startConfig.id, props.uievents, props.forms, dataRetrievedSuccessfully, errorWhenRetrievingData, undefined,  props.filters, props.startConfig.selectedRecord, props.startConfig.initialQueryParameters, props.startConfig.localFormData?.values);
            }

            const action = props.uievents.filter(a => a.Name === props.startConfig.button.UIEvent);

            if (action.length > 0) {
                const form = props.forms.filter(f => f.Name === action[0].Action);

                // Display stuff
                let overlayOnly = false;
                let fullScreenOnly = false;
                let fullScreenDefault = false;
                let displaySettings = form[0]?.DisplaySettings;
                if (displaySettings !== undefined &&
                    displaySettings !== null &&
                    displaySettings !== "")
                {
                    displaySettings = displaySettings.toLowerCase();
                    overlayOnly = displaySettings === "overlayonly"; 
                    fullScreenOnly = displaySettings === "fullscreenonly";
                    fullScreenDefault = displaySettings ===  "fullscreendefault";
                }
    
                if (props.startConfig.showFullScreenButton === undefined || props.startConfig.showFullScreenButton === true) {
                    setDisplayModeSwitchable(!overlayOnly && !fullScreenOnly);
                } else {
                    setDisplayModeSwitchable(false);
                }

                const fullScreenPreference = props.preferences !== undefined && props.preferences.DataFullScreen === "Yes";
                const goFullScreen = fullScreenDefault || fullScreenOnly || fullScreenPreference;
                setFullScreen(goFullScreen);

                if (props.startConfig.containerVisibility !== undefined) {
                    props.startConfig.containerVisibility(!goFullScreen);
                }
                // End of display stuff
            }

        } else {
            resetForm();
        }
    }, [props.startConfig, props.forms, props.lists, props.pages, props.uievents]);

    const errorWhenRetrievingData = (response) => {
        const errorMessage = response.data !== undefined ? response.data : response;
        updateDataEntryErrorStatus({visible: true, message: errorMessage});
    }

    const nextButtonClickHandler = (page, fieldChangesToUse, sourceOfClick) => {
        dataEntryErrorCloseHandler();
        if (document.activeElement?.blur) {
            document.activeElement.blur();
        }
        const currentState = dataEntryStateRef.current ?? dataEntryState;
        const state = NextButtonClickHandler(page, fieldChangesToUse, sourceOfClick, currentState, props.lists, props.laboratory, props.workflow, props.language);
  
        if (state.error !== "") {
            updateDataEntryErrorStatus({visible: true, message: state.error + TranslateTag("@ValMes@",props.language)});
        } else {
            const fieldChanges = refreshCalculatedDisplayFields(state.formDef, state.fieldChanges ?? []);
            const updatedState = {...state, fieldChanges};
            dataEntryStateRef.current = updatedState;
            setDataEntryState(updatedState);
        }
    }
  
    const previousButtonClickHandler = (page) => {
        dataEntryErrorCloseHandler();
        const state = PreviousButtonClickHandler(page, dataEntryState);
        const fieldChanges = refreshCalculatedDisplayFields(state.formDef, state.fieldChanges ?? []);
        setDataEntryState({...state, fieldChanges});
    }

    const formSaveHandler = (moveToNextItem) => {
        if (dataEntryState === undefined || dataEntryState.formDef === undefined) {
            return;
        }
        let errorField = "";
        if (true) {
            errorField = RequiredFieldCheck(dataEntryState.fieldChanges, dataEntryState.currentPage.Required, dataEntryState.currentPage.RequiredRule, dataEntryState.currentPage.RequiredError, dataEntryState.formDef.Pages);       
        } 

        if (errorField !== "") {
            updateDataEntryErrorStatus({visible: true, message: errorField + TranslateTag("@ValMes@",props.language)});
            return;
        }

        if (!saveEnabled || !tryBeginFormSave(saveInFlightRef)) {
            return;
        }
        setSaveEnabled(false);

        const state = PrepareDataFormSavingHandler(dataEntryState, props.startConfig.view);
        state.data = AddFilterMetaData(props.filters, state.data);

        if (Array.isArray(props.startConfig.allSelectedRecords) && props.startConfig.allSelectedRecords.length > 1) {
            state.data.selectedItems = props.startConfig.allSelectedRecords;
        }

        if (props.startConfig?.deferSave === true) {
            const deferSaveExcludeKeys = new Set(['event', 'view', 'stateid', 'selecteditems']);
            const completeDeferSave = () => {
                const savedArray = Object.entries(state.data)
                    .filter(([key]) => !deferSaveExcludeKeys.has(String(key).toLowerCase()))
                    .map(([key, val]) => ({
                        key,
                        value: val != null && val !== undefined ? String(val) : ''
                    }));
                if (props.startConfig.refresh) {
                    props.startConfig.refresh(savedArray, state.formDef.Pages);
                }
                if (props.startConfig?.onSave !== undefined) {
                    props.startConfig.onSave();
                }
                closeFormHandler();
            };

            const saveEvent = state.formDef.SaveEvent ?? state.formDef.saveEvent;
            const saveOperation = state.formDef.SaveOperation ?? state.formDef.saveOperation;
            const deferToParentGrid = String(saveOperation ?? '').toLowerCase() === 'updategrid';
            if (saveEvent !== undefined && saveEvent !== '' && !deferToParentGrid) {
                const postedData = { ...state.data, Id: state.data.ReportDisplayId ?? state.data.Id };
                PostEvent(postedData, completeDeferSave, errorWhenSavingData, state);
                dataEntryErrorCloseHandler();
                return;
            }

            completeDeferSave();
            return;
        }

        const postedData = {...state.data, Id: state.data.ReportDisplayId ?? state.data.Id};
        PostEvent(postedData, moveToNextItem === true ? eventPostSuccessNextItem : eventPostedSuccessfully, errorWhenSavingData, state );       
        dataEntryErrorCloseHandler();       
    }

    const fieldContentsChangeHandler = (id, value, extraInfo) => {
        let state = {...dataEntryState};
 
        if (id === "multiplechanges") {
            for (const item of value) {
                state = FieldContentsChangeHandler(item.key,item.value,extraInfo, props.lists, state);
            }
        } else {
            state = FieldContentsChangeHandler(id,value,extraInfo, props.lists, dataEntryState);
        }

        PopulateFieldsWithNewContents(state.formDef.Pages, state.fieldChanges);

        PopulateCraftedWithNewContents(state.formDef, state.currentPage.Name, value, id.toLowerCase(), props.laboratory);

        if (props.startConfig.changeHandler !== undefined && Array.isArray(state.fieldChanges))  {
            props.startConfig.changeHandler(state.fieldChanges[0]);
        }

        dataEntryStateRef.current = state;
        setDataEntryState(state);
    }

    const dataEntryErrorCloseHandler = () => {
        updateDataEntryErrorStatus({visible: false, message: ""});
    }

    const errorWhenSavingData = (response) => {
        const errorMessage = response.data !== undefined ? response.data : response;
        updateDataEntryErrorStatus({visible: true, message: errorMessage});
        clearFormSaveInFlight(saveInFlightRef);
        setSaveEnabled(true);
    }

    const eventPostedSuccessfully = (id, extraInfo) => {
        const repeat = dataEntryState?.formDef?.Repeat;
        if (getRepeatFromPage(repeat) && !refreshConfig) {
            setRepeatPrompt({ repeat: repeat, createdIds: id });
            return;
        }

        if (refreshConfig) {
            GetWithNoParams('/config/get',configRetrievedSuccessfully, errorWhenSavingData);
        } else {
            if (finishAction === "update" || finishAction === "refresh" || finishAction === "refreshfilter" || finishAction === "updatefromform" || finishAction === "embeddedrefresh") {
                const idToUse = dataEntryState?.additionalData?.reportDisplayConfig?.ReportDisplayId ?? dataEntryState.id;
                props.startConfig.refresh(finishAction, idToUse, extraInfo.data);
            }
            if (props.startConfig != undefined && props.startConfig.onSave != undefined) {
                props.startConfig.onSave();
            }
            closeFormHandler();
        }
    }

    /**
     * The record saved, and the user asked to register another against the same parents. Refresh the list
     * behind the form so the saved record shows, then rewind the form to its repeat page.
     */
    const repeatFormHandler = () => {
        const { repeat, createdIds } = repeatPrompt;

        setDataEntryState((previousState) => ResetFormForRepeat({ ...previousState }, repeat, createdIds));
        clearFormSaveInFlight(saveInFlightRef);
        setSaveEnabled(true);
        dataEntryErrorCloseHandler();

        // Defer closing the dialog and refreshing the list so the click does not pass through to the form panel.
        window.setTimeout(() => {
            setRepeatPrompt(undefined);

            if (finishAction === "update" || finishAction === "refresh" || finishAction === "refreshfilter" || finishAction === "updatefromform" || finishAction === "embeddedrefresh") {
                props.startConfig.refresh(finishAction, dataEntryState?.id);
            }
        }, 0);
    }

    /**
     * The record saved and the user is done, so complete the save in the way a form without a repeat would.
     */
    const finishAfterRepeatPromptHandler = () => {
        setRepeatPrompt(undefined);

        if (finishAction === "update" || finishAction === "refresh" || finishAction === "refreshfilter" || finishAction === "updatefromform" || finishAction === "embeddedrefresh") {
            props.startConfig.refresh(finishAction, dataEntryState.id);
        }
        if (props.startConfig != undefined && props.startConfig.onSave != undefined) {
            props.startConfig.onSave();
        }
        closeFormHandler();
    }

    const configRetrievedSuccessfully = (configData) => {
        props.updateConfig(configData);
        if (finishAction === "update" || finishAction === "refresh") {
            props.startConfig.refresh(finishAction, dataEntryState.id);
        }
        closeFormHandler();
    }

    const eventPostSuccessNextItem = () => {
        if (finishAction === "update" || finishAction === "refresh") {
            props.startConfig.refresh(finishAction, dataEntryState.id);
        }
        props.startConfig.moveToNextItem(props.startConfig.button, dataEntryState.id);
        clearFormSaveInFlight(saveInFlightRef);
        setSaveEnabled(true);
    }

    const closeFormHandler = () => {
        setFormShellVisible(false);
        setFormDataReady(false);
        setShellFormDef(null);
        setDataEntryState(undefined);
        setRepeatPrompt(undefined);
        resetFullScreen(false);

        if (props.startConfig !== undefined && props.startConfig.containerVisibility !== undefined) {
            props.startConfig.containerVisibility(true);
        }

        clearFormSaveInFlight(saveInFlightRef);
        setSaveEnabled(true);
        dataEntryErrorCloseHandler();

        if (props.startConfig !== undefined && props.startConfig.button !== undefined && props.startConfig.button.onFinish === "alwaysrefresh") {
            props.refresh("embeddedrefresh");
        }

        if (props.startConfig?.onClose !== undefined && typeof props.startConfig.onClose === 'function') {
            props.startConfig.onClose();
        }
    }

    const loadErrorCloseHandler = () => {
        closeFormHandler();
    };

    const resetForm = () => {
        setFormShellVisible(false);
        setFormDataReady(false);
        setShellFormDef(null);
        setDataEntryState(undefined);
        setRepeatPrompt(undefined);
        resetFullScreen(false);
        clearFormSaveInFlight(saveInFlightRef);
        setSaveEnabled(true);
        dataEntryErrorCloseHandler();
    }

    const fullScreenClickHandler = () => {
        if (displayModeSwitchable) {
            const isFullScreen = props.fullScreen === undefined ? fullScreen : props.fullScreen;
            const fullScreenValue = !isFullScreen;
            props.startConfig.containerVisibility(! fullScreenValue);
            resetFullScreen(fullScreenValue);
        }
    }

    const resetFullScreen = (value) => {
        setFullScreen(value);
        if (props.fullScreenClick !== undefined) {
            props.fullScreenClick(value);
        }
    }

    const focusOutHandler = (id, value) => {
        if (dataEntryState === undefined || dataEntryState.formDef === undefined) {
            return;
        }
        if (value !== undefined) {
            const defaultValues = SetDynamicDefaultValues(dataEntryState.formDef.Pages, id, value);
            if (defaultValues.length > 0) {
                for (const defValue of defaultValues) {
                    fieldContentsChangeHandler(defValue.id, defValue.value);
                }
            }
        }
    }

    let dataEntryPanel = (null);
    if (formShellVisible && props.startConfig !== undefined) {

        const isFullScreen = props.fullScreen === undefined ? fullScreen : props.fullScreen;

        if (props.startConfig.containerVisibility !== undefined) {
            props.startConfig.containerVisibility(! isFullScreen);
        }

        const reportConfiguration = props.reportConfiguration;
        if (dataEntryState?.additionalData?.reportDisplayConfig !== undefined) {
            reportConfiguration.displayId = dataEntryState.additionalData.reportDisplayConfig.ReportDisplayId;
        }

        const shellLoading = !formDataReady && !dataEntryErrorStatus.visible;
        const loadFailed = !formDataReady && dataEntryErrorStatus.visible;

        let pageToDisplay;
        let configToUse;
        let stateForPanel = dataEntryState;
        let idForPanel = props.startConfig.id;

        if (formDataReady && dataEntryState !== undefined) {
            pageToDisplay = AdjustPageToHandleMultiPageView(dataEntryState.currentPage, dataEntryState.formDef.Groups, isFullScreen);
            configToUse = dataEntryState.formDef;
        } else {
            const firstPage = shellFormDef?.Pages?.[0] || {};
            pageToDisplay = firstPage;
            configToUse = shellFormDef || { Pages: [{}] };
            stateForPanel = undefined;
        }

        dataEntryPanel = (<SinglePagePanel
            id={idForPanel}
            visible={true}
            suppressDismiss={repeatPrompt !== undefined}
            currentPage={pageToDisplay}
            close={closeFormHandler}
            save={formSaveHandler}
            next={nextButtonClickHandler}
            previous={previousButtonClickHandler}
            config={configToUse}
            fullScreen={isFullScreen}
            displayModeSwitchable={displayModeSwitchable}
            fullScreenClick={fullScreenClickHandler}
            fieldFocusOut={focusOutHandler}
            changeHandler={fieldContentsChangeHandler}
            errorCloseHandler={dataEntryErrorCloseHandler}
            error={dataEntryErrorStatus}
            language={props.language}
            refresh={props.refresh}
            recordType={formDataReady && dataEntryState !== undefined ? dataEntryState.formDef.RecordView : configToUse.RecordView}
            saveEnabled={saveEnabled}
            isSaving={!saveEnabled}
            startConfig={props.startConfig}
            records={props.startConfig.records}
            showNextButton={props.showNextButton}
            state={stateForPanel}
            reportConfiguration={props.reportConfiguration}
            lists={props.lists}
            allPages={props.pages}
            uievents={props.uievents}
            forms={props.forms}
            shellLoading={shellLoading}
            loadFailed={loadFailed}
            loadErrorCloseHandler={loadErrorCloseHandler}
            >
        </SinglePagePanel>);
    }

    return (
        <React.Fragment>
            {dataEntryPanel}
            <RepeatPrompt
                visible={repeatPrompt !== undefined}
                repeat={repeatPrompt?.repeat}
                language={props.language}
                onRepeat={repeatFormHandler}
                onFinish={finishAfterRepeatPromptHandler}
            />
        </React.Fragment>
    )
}

const mapStateToProps = state => {
    return {
        uievents: state.config.uievents,
        forms: state.config.forms,
        pages: state.config.pages,
        views: state.config.views,
        lists: state.config.lists,
        language: state.config.language,
        preferences: state.config.preferences,
        laboratory: state.config.laboratory,
        workflow: state.config.workflow
    };
}


export default connect(mapStateToProps)(FormHandler)