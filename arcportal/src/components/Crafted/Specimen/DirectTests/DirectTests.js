import React, {useState, useRef, useCallback} from 'react';
import { tryBeginFormSave, clearFormSaveInFlight } from '../../../../Utils/General/formSaveInFlight';
import {connect} from 'react-redux';
import './DirectTests.css';
import InvokeFormHandler, {DataRetrievedHandler, PrepareDataFormSavingHandler, NextButtonClickHandler, PreviousButtonClickHandler, FieldContentsChangeHandler, InvokeFormHandlerUsingForm} from '../../../../Utils/General/FormStateHandler';
import SinglePagePanel from '../../../Forms/SinglePagePanel/SinglePagePanel';
import PostEvent from '../../../../Data/PostEvents';
import SimpleCard from '../../../General/SimpleCard/SimpleCard';
import ErrorMessage from '../../../General/ErrorMessage/ErrorMessage';
import { Icon, Spinner } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import TransformDatesInJson from '../../../../Utils/Local/TransformDatesInJson';
import PopulateFieldsWithNewContents from '../../../../Utils/PageStructure/PopulateFieldsWithNewContents';
import { SetTestAndCultureTypeDisplays } from '../../../../Utils/PageStructure/PopulateCraftedWithNewContents';
import { fetchTestListForParent } from '../../../../Utils/Specimen/fetchTestListForParent';
import resolveCultureTypeId from '../../../../Utils/Specimen/resolveCultureTypeId';
import useTestListRefreshState from '../../../../Utils/Specimen/useTestListRefreshState';
import { createTestListSyncPayload } from '../../../../Utils/Specimen/testListSync';

/**
 * Builds a minimal form config for the panel shell while test form data is loading.
 * @param {Object|undefined} formEntry - Form definition from config.
 * @param {Array} pages - Page definitions from config.
 * @returns {{ shellFormDef: Object, shellCurrentPage: Object }}
 */
const buildShellFromForm = (formEntry, pages) => {
    if (!formEntry) {
        return { shellFormDef: { Pages: [{}] }, shellCurrentPage: {} };
    }
    const rawPages = formEntry.Pages;
    const pageNames = Array.isArray(rawPages) && rawPages.length > 0 ? rawPages : [];
    let shellCurrentPage = {};
    if (pageNames.length > 0 && Array.isArray(pages)) {
        const first = pageNames[0];
        const firstName = typeof first === 'string' ? first : (first?.Name ?? first?.name);
        if (firstName) {
            const resolved = pages.filter((p) => (p.Name ?? p.name) === firstName);
            if (resolved.length > 0) {
                shellCurrentPage = resolved[0];
            }
        }
    }
    return {
        shellFormDef: {
            Pages: pageNames.length > 0 ? pageNames : [{}],
            SuppressRecordView: formEntry.SuppressRecordView,
            RecordView: formEntry.RecordView,
        },
        shellCurrentPage,
    };
};

const DirectTests = (props) => {

    const [dataEntryErrorStatus, updatedataEntryErrorStatus] = useState({visible: false, message: ''});
    const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
    const [dataEntryState, setDataEntryState] = useState();
    const [userFormState, setUserFormState] = useState({isFormOpen: false, fullScreen: false, showGrid: true, formDef: undefined});
    const [testPanelShellActive, setTestPanelShellActive] = useState(false);
    const [innerPanelOpen, setInnerPanelOpen] = useState(false);
    const [formDataReady, setFormDataReady] = useState(false);
    const [loadFailed, setLoadFailed] = useState(false);
    const [shellFormDef, setShellFormDef] = useState(null);
    const [shellCurrentPage, setShellCurrentPage] = useState({});
    const [saveEnabled, setSaveEnabled] = useState(true);
    const [localListData, setLocalListData] = useState(Array.isArray(props.data) ? props.data : []);
    const saveInFlightRef = useRef(false);
    const loadInFlightRef = useRef(false);
    const pendingListRefreshRef = useRef(false);
    const pendingParentRefreshRef = useRef(false);
    const { listRefreshInFlight, beginListRefresh, endListRefresh } = useTestListRefreshState();

    const addIcon = <Icon iconName="Add" />;

    /**
     * Refreshes the parent specimen list row or record-view embedded regions after a test change.
     * @param {Array} [freshRows] - When set, passes authoritative list rows to the record-view grid via testListSync.
     */
    const triggerParentContextRefresh = useCallback((freshRows) => {
        if (props.startConfig?.refresh === undefined) {
            return;
        }
        if (props.startConfig.view === 'specimens') {
            props.startConfig.refresh('update', props.id);
        } else if (freshRows !== undefined) {
            props.startConfig.refresh('embeddedrefresh', props.id, {
                testListSync: createTestListSyncPayload(props.type, freshRows),
            });
        } else {
            props.startConfig.refresh('embeddedrefresh', props.id);
        }
    }, [props.startConfig, props.id, props.type]);

    /**
     * Re-queries the test list in place and updates local card data without reopening the View/Update form.
     * @param {boolean} [refreshParentAfter=false] - When true, refreshes the parent record view or list row after data arrives.
     */
    const refreshTestListInPlace = useCallback((refreshParentAfter = false) => {
        beginListRefresh();
        fetchTestListForParent(
            props.type,
            props.id,
            (data) => {
                TransformDatesInJson(data);
                const rows = Array.isArray(data) ? data : [];
                setLocalListData(rows);
                endListRefresh();
                if (refreshParentAfter) {
                    triggerParentContextRefresh(rows);
                }
            },
            (response) => {
                endListRefresh();
                if (refreshParentAfter) {
                    triggerParentContextRefresh();
                }
                const errorMessage = response?.data !== undefined ? response.data : response;
                updateErrorStatus({ visible: true, message: errorMessage });
            }
        );
    }, [props.type, props.id, beginListRefresh, endListRefresh, triggerParentContextRefresh]);

    /**
     * Resets inner test form state after the panel exit animation completes.
     */
    const finalizeCloseFormHandler = () => {
        setTestPanelShellActive(false);
        setInnerPanelOpen(false);
        setFormDataReady(false);
        setLoadFailed(false);
        setShellFormDef(null);
        setShellCurrentPage({});
        setDataEntryState(undefined);
        setUserFormState((prev) => ({
            ...prev,
            isFormOpen: false,
            showGrid: true,
            formDef: undefined,
            fullScreen: false
        }));
        clearFormSaveInFlight(saveInFlightRef);
        loadInFlightRef.current = false;
        setSaveEnabled(true);
        dataEntryErrorCloseHandler();

        if (pendingListRefreshRef.current) {
            const refreshParentAfter = pendingParentRefreshRef.current;
            pendingListRefreshRef.current = false;
            pendingParentRefreshRef.current = false;
            refreshTestListInPlace(refreshParentAfter);
        }
    };

    /**
     * Starts the panel close animation; cleanup runs in finalizeCloseFormHandler.
     */
    const requestCloseFormHandler = () => {
        setInnerPanelOpen(false);
    };

    /**
     * Opens the inner panel immediately with a loading shell, then fetches form data.
     * @param {Array} form - Resolved form definition array.
     * @param {Object|undefined} button - Button config when opening via UI event.
     * @param {number|string} recordId - Record id for the form initial query.
     */
    const beginTestFormLoad = (form, button, recordId) => {
        if (loadInFlightRef.current || listRefreshInFlight) {
            return;
        }
        if (button?.UIEvent === undefined && form.length === 0) {
            return;
        }

        loadInFlightRef.current = true;
        dataEntryErrorCloseHandler();
        setLoadFailed(false);
        setFormDataReady(false);
        setDataEntryState(undefined);

        const { shellFormDef: shell, shellCurrentPage: shellPage } = buildShellFromForm(form[0], props.pages);
        setShellFormDef(shell);
        setShellCurrentPage(shellPage);
        setTestPanelShellActive(true);
        setInnerPanelOpen(true);
        setUserFormState((prev) => ({ ...prev, showGrid: false }));

        if (button !== undefined && button.UIEvent !== undefined) {
            InvokeFormHandler(button, recordId, props.uievents, props.forms, dataRetrievedSuccessfully, errorWhenRetrievingData);
        } else {
            InvokeFormHandlerUsingForm(form, recordId, dataRetrievedSuccessfully, errorWhenRetrievingData);
        }
    };

    /**
     * Opens a test card or add-tests workflow. Blocked while the list is refreshing.
     * @param {Object} button - Card or add button config.
     */
    const buttonClickHandler = (button) => {
        if (listRefreshInFlight) {
            return;
        }
        if (button.UIEvent === undefined) {
            const form = props.forms.filter((f) => f.Name === button.key);
            beginTestFormLoad(form, undefined, button.id);
        } else {
            const action = props.uievents.filter((a) => a.Name === button.UIEvent);
            if (action.length === 0) {
                return;
            }
            const form = props.forms.filter((f) => f.Name === action[0].Action);
            beginTestFormLoad(form, button, props.id);
        }
    };

    /**
     * Handles form data retrieved from the API. Applies test/culture type defaults for selection forms.
     * @param {Object} data - Form data from the API
     * @param {Object} extraInfo - Extra info including form name and button config
     */
    const dataRetrievedSuccessfully = (data, extraInfo) => {
        loadInFlightRef.current = false;
        const state = DataRetrievedHandler(data, extraInfo, props.forms, props.pages, props.lists);

        if (extraInfo.form === "testselectionform" || extraInfo.form === "culturetestselectionform") {
            if (props.startConfig.selectedRecord?.specimentypeid !== undefined) {
                const specimentypeid = props.startConfig.selectedRecord.specimentypeid.toString();
                let cultureTypeId = null;

                if (extraInfo.form === "culturetestselectionform") {
                    cultureTypeId = resolveCultureTypeId(props.startConfig, data);
                    SetTestAndCultureTypeDisplays(data.Crafted, specimentypeid, props.laboratory, props.startConfig.selectedRecord.laboratoryid, false, cultureTypeId);
                } else {
                    SetTestAndCultureTypeDisplays(data.Crafted, specimentypeid, props.laboratory, props.startConfig.selectedRecord.laboratoryid, true);
                }
            }
       }
        setDataEntryState({...state, button: extraInfo.button, fieldChanges: [] });
        setUserFormState((prev) => ({
            ...prev,
            isFormOpen: true,
            formDef: state.formDef,
            fullScreen: state.formDef.fullScreenOnly,
            showGrid: false
        }));
        setFormDataReady(true);
        setLoadFailed(false);
    };

    const errorWhenRetrievingData = (response) => {
        loadInFlightRef.current = false;
        const errorMessage = response?.data !== undefined ? response.data : response;
        updatedataEntryErrorStatus({ visible: true, message: errorMessage });
        setLoadFailed(true);
    };

    const loadErrorCloseHandler = () => {
        dataEntryErrorCloseHandler();
        requestCloseFormHandler();
    };

    const errorCloseHandler = () => {
        updateErrorStatus({visible: false, message: ''});
    };

    const nextButtonClickHandler = (page, fieldChangesToUse, sourceOfClick) => {
        dataEntryErrorCloseHandler();
        const state = NextButtonClickHandler(page, fieldChangesToUse, sourceOfClick, dataEntryState);

        if (state.error !== "") {
            updatedataEntryErrorStatus({visible: true, message: state.error + TranslateTag("@ValMes@",props.language)});
        } else {
            setDataEntryState(state);
        }
    };

    const previousButtonClickHandler = (page) => {
        dataEntryErrorCloseHandler();
        const state = PreviousButtonClickHandler(page, dataEntryState);
        setDataEntryState(state);
    };

    /**
     * Handles form save: disables Save button, prepares data, and POSTs to backend.
     */
    const formSaveHandler = () => {
        if (!formDataReady || dataEntryState === undefined) {
            return;
        }
        dataEntryErrorCloseHandler();
        if (!saveEnabled || !tryBeginFormSave(saveInFlightRef)) {
            return;
        }
        setSaveEnabled(false);
        const state = PrepareDataFormSavingHandler(dataEntryState, "specimens");
        PostEvent(state.data, eventPostedSuccessfully, errorWhenSavingData, state );
    };

    /**
     * Handles field value changes and updates form state.
     * @param {string} id - Field id
     * @param {*} value - New value
     * @param {Object} extraInfo - Extra info for the change
     */
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
        setDataEntryState(state);
    };

    const dataEntryErrorCloseHandler = () => {
        updatedataEntryErrorStatus({visible: false, message: ""});
    };

    /**
     * Handles a successful inner-panel save (add, edit, or delete test).
     * Schedules an in-place list re-query after the panel closes and refreshes the parent context.
     * @param {Object} data - PostEvent response containing the saved record id.
     */
    const eventPostedSuccessfully = (data) => {
        const saveEvent = String(dataEntryState?.formDef?.SaveEvent ?? dataEntryState?.formDef?.saveEvent ?? '').toLowerCase();
        const isRemoveTest = saveEvent === 'removedirecttest' || saveEvent === 'removeculturetest';

        if (isRemoveTest) {
            pendingListRefreshRef.current = true;
            pendingParentRefreshRef.current = true;
        } else {
            const savedId = parseInt(data.id, 10);
            const itemToChange = localListData.filter((item) => item.id === savedId || item.Id === savedId);
            if (itemToChange.length === 0) {
                pendingListRefreshRef.current = true;
                pendingParentRefreshRef.current = true;
            } else {
                setLocalListData((current) =>
                    current.map((item) => {
                        const itemId = item.Id ?? item.id;
                        if (String(itemId) === String(savedId)) {
                            return { ...item, status: 'Complete', Status: 'Complete' };
                        }
                        return item;
                    })
                );
                triggerParentContextRefresh();
            }
        }
        requestCloseFormHandler();
    };

    const errorWhenSavingData = (response) => {
        clearFormSaveInFlight(saveInFlightRef);
        setSaveEnabled(true);
        updatedataEntryErrorStatus({visible: true, message: response.data});
    };

    /**
     * Opens the remove-test confirmation form for the selected card (removedirecttestform or removeculturetestform).
     * @param {Object} recordToDelete - Card data including the test row id.
     */
    const deleteCardHandler = (recordToDelete) => {
        if (listRefreshInFlight) {
            return;
        }
        const form = props.config.Name === "directtestpage" ? props.forms.filter(f => f.Name === 'removedirecttestform') : props.forms.filter(f => f.Name === 'removeculturetestform');
        beginTestFormLoad(form, undefined, recordToDelete.id);
    };

    const listSource = Array.isArray(localListData) ? localListData : [];
    const actionsDisabled = listRefreshInFlight || testPanelShellActive;

    const dataToDisplay = listSource.map((item) => {
        TransformDatesInJson(item);
        var status = item.Status === "Complete" ? TranslateTag("@GenComC@", props.language) : TranslateTag("@GenReq@", props.language);
        const deleteForm = props.config.Name === "directtestpage" ? props.forms.filter(f => f.Name === 'removedirecttestform') : props.forms.filter(f => f.Name === 'removeculturetestform');
        const form = props.forms.filter(f => f.Name === item.TestName);
        const canDelete = deleteForm.length > 0 && form.length > 0 && !actionsDisabled;
        let returnValue = {
            description: item.TestDescription,
            key: item.TestName,
            link: item.TestName,
            status: TranslateTag("@GenStaA@", props.language) + ": " + status,
            requested: TranslateTag("@GenReqA@", props.language) + " " + item.Requested,
            delete: canDelete,
            id: item.Id,
            colour: item.Status === "Complete" ? "green" : "grey",
            lock: false
        };

        if (item.Completed !== undefined && item.Completed !== null ) {
            returnValue.completed = TranslateTag("@GenComD@", props.language) + " " + item.Completed;
        }

        return returnValue;
    });

    const addNewTestButton = {
        id: -1,
        UIEvent: props.type === "specimen" ? "testselectionuievent" : "culturetestselectionuievent",
        space1: "",
        icon: addIcon,
        space2: "",
        text: props.type === "specimen" ? TranslateTag("@TesAddG@", props.language) : TranslateTag("@TesAddH@", props.language),
        colour: "blue",
        centre: true
    };

    dataToDisplay.push(addNewTestButton);

    const cardsVisible = !testPanelShellActive;
    let cardsCss = "directtests-cards";
    if (!cardsVisible) {
        cardsCss += " directtests-cards-hidden";
    } else if (listRefreshInFlight) {
        cardsCss += " directtests-cards-refreshing";
    }
    const contentCss = testPanelShellActive
        ? "directtests-content directtests-content-panel-open"
        : "directtests-content";

    const shellLoading = testPanelShellActive && !formDataReady && !loadFailed;
    const configToUse = formDataReady && userFormState.formDef ? userFormState.formDef : (shellFormDef ?? { Pages: [{}] });
    const pageToUse = formDataReady && dataEntryState ? dataEntryState.currentPage : shellCurrentPage;

    let dataEntryPanel = (null);
    if (testPanelShellActive) {
        dataEntryPanel = (<SinglePagePanel
            visible={innerPanelOpen}
            currentPage={pageToUse}
            close={requestCloseFormHandler}
            onDismissed={finalizeCloseFormHandler}
            save={formSaveHandler}
            next={nextButtonClickHandler}
            previous={previousButtonClickHandler}
            config={configToUse}
            fullScreen={false}
            changeHandler={fieldContentsChangeHandler}
            errorCloseHandler={dataEntryErrorCloseHandler}
            loadErrorCloseHandler={loadErrorCloseHandler}
            language={props.language}
            error={dataEntryErrorStatus}
            saveEnabled={formDataReady && saveEnabled}
            isSaving={formDataReady && !saveEnabled}
            shellLoading={shellLoading}
            loadFailed={loadFailed}
            state={formDataReady ? dataEntryState : undefined}
            id={formDataReady && dataEntryState ? dataEntryState.id : undefined}
            lists={props.lists}
            allPages={props.pages}
            uievents={props.uievents}
            forms={props.forms}>
        </SinglePagePanel>);
    }

    return (
        <div className={contentCss}>
            <div className={cardsCss} data-testid="directtests-cards">
                <div className="directtests-top">
                    {props.type === "specimen" ? TranslateTag("@SpeDir@", props.language) : TranslateTag("@SpeCulC@", props.language)}
                </div>
                <SimpleCard
                    data={dataToDisplay}
                    onClick={buttonClickHandler}
                    onKeySelect={buttonClickHandler}
                    onDelete={deleteCardHandler}
                    deleteDisabled={actionsDisabled}>
                </SimpleCard>
                {listRefreshInFlight && cardsVisible && (
                    <div className="directtests-list-loading" data-testid="directtests-list-loading">
                        <Spinner label="" />
                    </div>
                )}
            </div>
            {dataEntryPanel}
            <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
        </div>
    );
};

const mapStateToProps = state => {
    return {
        uievents: state.config.uievents,
        forms: state.config.forms,
        pages: state.config.pages,
        lists: state.config.lists,
        laboratory: state.config.laboratory
    };
};

export default connect(mapStateToProps)(DirectTests);
