import React, {useState, useEffect, useMemo, useRef} from 'react';
import { tryBeginFormSave, clearFormSaveInFlight } from '../../../../Utils/General/formSaveInFlight';
import './DirectTestsGrid.css';
import { connect } from 'react-redux';
import * as actionTypes from '../../../../store/actions';
import ListView from '../../../General/ListView/ListView';
import ColumnPicker from '../../../General/ColumnPicker/ColumnPicker';
import { IconButton, TooltipHost, Spinner } from '@fluentui/react';
import { applyColumnLayout } from '../../../../Utils/General/ApplyColumnLayout';
import { getColumnLayoutFromPreferences } from '../../../../Utils/General/GetColumnLayoutFromPreferences';
import { useColumnLayoutChange } from '../../../../Utils/General/useColumnLayoutChange';
import Post from '../../../../Data/Post';
import MapButtonsToContextMenu from '../../../../Utils/Forms/MapButtonsToContextMenu';
import {InvokeFormHandlerUsingForm, DataRetrievedHandler, PrepareDataFormSavingHandler, NextButtonClickHandler, PreviousButtonClickHandler, FieldContentsChangeHandler} from '../../../../Utils/General/FormStateHandler';
import PostEvent from '../../../../Data/PostEvents';
import RightHelp from '../../../General/RightHelp/RightHelp';
import ErrorMessage from '../../../General/ErrorMessage/ErrorMessage';
import SinglePagePanel from '../../../Forms/SinglePagePanel/SinglePagePanel';
import TransformDatesInJson from '../../../../Utils/Local/TransformDatesInJson';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import PopulateFieldsWithNewContents from '../../../../Utils/PageStructure/PopulateFieldsWithNewContents';
import useTestListRefreshState from '../../../../Utils/Specimen/useTestListRefreshState';
import { fetchTestListForParent } from '../../../../Utils/Specimen/fetchTestListForParent';
import { getTestGridLoadingState } from '../../../../Utils/Specimen/testGridLoadingPolicy';
import { invalidateCalloutCacheForTest } from '../../../../Utils/Specimen/testCalloutLoader';
import resolveSpecimenWorkflowStateForTestGrid, { isTestGridMenuStateAllowed } from '../../../../Utils/Specimen/resolveSpecimenWorkflowStateForTestGrid';


const DirectTestsGrid = (props) => {

    const [displayHelpState, setDisplayHelpState] = useState(false);
    const [userFormState, setUserFormState] = useState({isFormOpen: false, fullScreen: false});
    const [listData, setListDataState] = useState({});
    const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
    const [dataEntryErrorStatus, updatedataEntryErrorStatus] = useState({visible: false, message: ''});
    const [selectedRecord, setSelectedRecord] = useState({});
    const [selectedIndex, setSelectedIndex] = useState({});
    const [finishAction, setFinishAction] = useState("");
    const [dataEntryState, setDataEntryState] = useState();
    const [saveEnabled, setSaveEnabled] = useState(true);
    const saveInFlightRef = useRef(false);
    const [showColumnPicker, setShowColumnPicker] = useState(false);
    // When we update a single row after an edit, we still let the parent refresh run
    // (specimen details, alerts, other embedded lists). This ref tells the refresh
    // effect to skip the resulting full re-fetch of the test list so that editing one
    // test stays O(1) regardless of how many tests are provisioned on the specimen.
    const skipNextRefreshFetch = useRef(false);
    const fetchGenerationRef = useRef(0);
    const lastAppliedSyncGenerationRef = useRef(0);
    const { listRefreshInFlight, beginListRefresh, endListRefresh } = useTestListRefreshState();

    const parentType = props.config.Id === 'culturetests' ? 'culture' : 'specimen';

    /**
     * Applies test list rows to grid state after formatting each record for display.
     * @param {Array} rows - Raw rows from a list query or sync payload.
     */
    const applyTestListRows = (rows) => {
        TransformDatesInJson(rows);
        const nextRows = Array.isArray(rows) ? rows.map((row) => ({ ...row })) : [];
        for (const record of nextRows) {
            formatRecord(record);
        }
        setListDataState(nextRows);
        endListRefresh();
    };

    /**
     * Fetches the full test list for this grid, ignoring stale responses from superseded requests.
     * @param {number} [generation] - When set, only applies the result if it matches the current generation.
     */
    const fetchFullTestList = (generation) => {
        fetchTestListForParent(
            parentType,
            props.id,
            (data) => {
                if (generation !== undefined && generation !== fetchGenerationRef.current) {
                    endListRefresh();
                    return;
                }
                TransformDatesInJson(data);
                const rows = Array.isArray(data) ? data : [];
                // #region agent log
                console.info('DEBUG', {
                    location: 'DirectTestsGrid.fetchFullTestList',
                    message: 'networkFetchComplete',
                    data: { parentId: props.id, rowCount: rows.length },
                    timestamp: Date.now(),
                });
                // #endregion
                for (const record of rows) {
                    formatRecord(record);
                }
                setListDataState(rows);
                endListRefresh();
                if (props.onRefreshDone !== undefined) {
                    props.onRefreshDone(false);
                }
            },
            (response) => {
                if (generation !== undefined && generation !== fetchGenerationRef.current) {
                    endListRefresh();
                    return;
                }
                endListRefresh();
                if (props.onRefreshDone !== undefined) {
                    props.onRefreshDone(false);
                }
                updateErrorStatus({ visible: true, message: response?.data !== undefined ? response.data : response });
            }
        );
    };

    let displayType = (null);
    let recordDisplay = (null);

    useEffect(() => {
        const sync = props.testListSync;
        if (sync && sync.generation !== lastAppliedSyncGenerationRef.current) {
            lastAppliedSyncGenerationRef.current = sync.generation;
            // #region agent log
            console.info('DEBUG', {
                location: 'DirectTestsGrid.testListSync',
                message: 'applySyncPayload',
                data: { generation: sync.generation, rowCount: sync.rows?.length ?? 0 },
                timestamp: Date.now(),
            });
            // #endregion
            applyTestListRows(sync.rows);
            return;
        }
    }, [props.testListSync]);

    useEffect(() => {
        if (skipNextRefreshFetch.current) {
            skipNextRefreshFetch.current = false;
            if (props.onRefreshDone !== undefined) {
                props.onRefreshDone(false);
            }
            return;
        }
        const generation = ++fetchGenerationRef.current;
        beginListRefresh();
        fetchFullTestList(generation);
    }, [props.config.QueryName, props.id, props.refresh]);

    const closeHelpHandler = () => {
        setDisplayHelpState(false);
    }

    /**
     * Closes the test form overlay and resets save state.
     */
    const closeUserFormHandler = () => {
        setUserFormState({...userFormState, isFormOpen: false, fullScreen: false, formDef: {}});
        props.containerVisibility(true);
        clearFormSaveInFlight(saveInFlightRef);
        setSaveEnabled(true);
        errorCloseHandler();
        dataEntryErrorCloseHandler();
    }

    const errorCloseHandler = () => {
        updateErrorStatus({visible: false, message: ''});
    }

    const errorWhenSavingData = (response) => {
        clearFormSaveInFlight(saveInFlightRef);
        setSaveEnabled(true);
        updatedataEntryErrorStatus({visible: true, message: response.data});
    }

    const dataEntryErrorCloseHandler = (response) => {
        updatedataEntryErrorStatus({visible: false, message: ""});
    }

    /**
     * Applies workflow state and status display formatting to a single test row.
     * Enriches rows with parent specimen context when absent so per-row menu filtering works.
     * @param {Object} record - A test row from the test list query.
     * @returns {Object} The same record, mutated with stateid and a translated Status.
     */
    const formatRecord = (record) => {
        const isComplete = record.Status === "Complete";
        const form = props.forms.filter(f => f.Name === record.TestName);
        record.stateid = form.length === 0 ? "noteditable" : "edit";
        if (record.stateid === "edit") {
            const deleteForm = props.config.Id === "directtests" ? props.forms.filter(f => f.Name === 'removedirecttestform') : props.forms.filter(f => f.Name === 'removeculturetestform');
            record.stateid = deleteForm.length === 0 ? "edit" : "editanddelete";
        }
        if (!isComplete) {
            record.stateid = record.stateid + "-incomplete";
        }
        const parentRecord = props.selectedRecord;
        if (parentRecord != null) {
            if (record.specimentypeid == null && record.SpecimenTypeId == null) {
                const specimenTypeId = parentRecord.specimentypeid ?? parentRecord.SpecimenTypeId;
                if (specimenTypeId != null) {
                    record.specimentypeid = specimenTypeId;
                }
            }
            if (record.laboratoryid == null && record.LaboratoryId == null) {
                const laboratoryId = parentRecord.laboratoryid ?? parentRecord.LaboratoryId;
                if (laboratoryId != null) {
                    record.laboratoryid = laboratoryId;
                }
            }
        }
        record.Status = isComplete ? TranslateTag("@GenComC@", props.language) : TranslateTag("@GenReq@", props.language);
        return record;
    }

    const dataReceivedHandler = (data) => {
        TransformDatesInJson(data);

        for (const record of data) {
            formatRecord(record);
        }
        setListDataState(data);
        endListRefresh();
        if (props.onRefreshDone !== undefined) {
            props.onRefreshDone(false);
        }
    }

    const errorWhenRetrievingData = (response) => {
        endListRefresh();
        if (props.onRefreshDone !== undefined) {
            props.onRefreshDone(false);
        }
        updateErrorStatus({visible: true, message: response?.data !== undefined ? response.data : response});
    }

    /**
     * Fetches just the edited test and replaces that one row in the grid, instead of
     * re-fetching and re-formatting every provisioned test. Keeps edit time independent
     * of the number of tests on the specimen.
     * @param {number|string} editedId - Id of the test that was just saved.
     */
    const updateSingleRow = (editedId) => {
        if (editedId === undefined || editedId === null) {
            let parameters = [{ Key: 'id', Value: props.id }];
            const criteria = { Name: props.config.QueryName, Parameters: parameters };
            Post('query/filteredget', criteria, dataReceivedHandler, errorWhenRetrievingData);
            return;
        }

        const singleRowQuery = props.config.Id === 'directtests'
            ? 'activetestbyidfortestlistquery'
            : 'activeculturetestbyidfortestlistquery';
        const criteria = { Name: singleRowQuery, Parameters: [{ Key: 'id', Value: editedId }] };
        Post('query/filteredget', criteria, (row) => singleRowReceivedHandler(row, editedId), errorWhenRetrievingData);
    }

    const singleRowReceivedHandler = (row, editedId) => {
        if (!row) {
            return;
        }
        TransformDatesInJson(row);
        const formatted = formatRecord(row);

        setListDataState((current) => {
            if (!Array.isArray(current)) {
                return current;
            }
            const idx = current.findIndex(r => String(r.Id ?? r.id) === String(editedId));
            if (idx < 0) {
                return current;
            }
            const existing = current[idx];
            // The single-row query does not enrich turnaround colour; keep the existing value.
            if (formatted.TurnAroundTimeColour === undefined || formatted.TurnAroundTimeColour === null || formatted.TurnAroundTimeColour === '') {
                formatted.TurnAroundTimeColour = existing.TurnAroundTimeColour;
            }
            const updated = [...current];
            updated[idx] = { ...existing, ...formatted };
            return updated;
        });
        setSelectedRecord([]);
        invalidateCalloutCacheForTest(editedId);
    }

    const embeddedModeButtonHandler = (item, button) => {
        itemSelected(item, true);
        buttonClickHandler(button, item);
    }

    /**
     * Handles button clicks for Edit, Delete, and View actions on test rows.
     * @param {Object} button - Button config with Key, OnFinish
     * @param {Object} item - The test record (id, TestName, etc.)
     */
    const buttonClickHandler = (button, item) => {
        if (listRefreshInFlight || !saveEnabled) {
            return;
        }
        setFinishAction(button.OnFinish);
        if (button.Key === "deletetest") {
            const form = props.config.Id === "directtests" ? props.forms.filter(f => f.Name === 'removedirecttestform') : props.forms.filter(f => f.Name === 'removeculturetestform');
            InvokeFormHandlerUsingForm(form, item.Id, dataRetrievedSuccessfully, errorWhenRetrievingData);
        } else if (button.Key === "viewtest") {
            if (props.onNavigateToRecordView) {
                const source = props.config.Id === 'culturetests' ? 'culture' : 'direct';
                props.onNavigateToRecordView('testrecordview', item.Id, { TestName: item.TestName, Source: source });
            } else {
                updateErrorStatus({ visible: true, message: "View not available for this test." });
            }
        } else {
            const form = props.forms.filter(f => f.Name === item.TestName);
            InvokeFormHandlerUsingForm(form, item.Id, dataRetrievedSuccessfully, errorWhenRetrievingData);
        }
    }

    /**
     * Handles form data retrieved from the API when opening a test form for edit.
     * @param {Object} data - Form data from the API
     * @param {Object} extraInfo - Extra info including button config
     */
    const dataRetrievedSuccessfully = (data, extraInfo) => {
        const state = DataRetrievedHandler(data, extraInfo, props.forms, props.pages, props.lists);
        setDataEntryState({...state, button: extraInfo.button, fieldChanges: [] });
        setUserFormState({...userFormState, isFormOpen: true, formDef: state.formDef, fullScreen: state.formDef.fullScreenOnly});
    }

    /**
     * Handles form save: disables Save button, prepares data, and POSTs to backend.
     */
    const formSaveHandler = () => {
        if (!saveEnabled || !tryBeginFormSave(saveInFlightRef)) {
            return;
        }
        setSaveEnabled(false);
        const state = PrepareDataFormSavingHandler(dataEntryState, "specimens");

        const refresh = finishAction === 'update' ? 'update' : 'refresh';
        const editedId = dataEntryState !== undefined ? dataEntryState.id : undefined;

        PostEvent(state.data, () => { eventPostedSuccessfully(refresh, editedId) }, errorWhenSavingData, state );
    }
    
    const eventPostedSuccessfully = (refresh, editedId) => {
        closeUserFormHandler();

        if (refresh === 'update') {
            updateSingleRow(editedId);
            // Let the parent refresh specimen-level regions/alerts, but skip the
            // resulting full re-fetch of the test list (handled by updateSingleRow).
            if (props.onRefresh !== undefined) {
                skipNextRefreshFetch.current = true;
                props.onRefresh();
            }
            return;
        }

        if (refresh === 'refresh') {
            if (editedId !== undefined && editedId !== null) {
                setListDataState((current) => {
                    if (!Array.isArray(current)) {
                        return current;
                    }
                    return current.filter((r) => String(r.Id ?? r.id) !== String(editedId));
                });
                setSelectedRecord([]);
                invalidateCalloutCacheForTest(editedId);
                if (props.onRefresh !== undefined) {
                    skipNextRefreshFetch.current = true;
                    props.onRefresh();
                }
                return;
            }

            const generation = ++fetchGenerationRef.current;
            beginListRefresh();
            fetchFullTestList(generation);
            setSelectedRecord([]);
        }

        if (props.onRefresh !== undefined) {
            skipNextRefreshFetch.current = true;
            props.onRefresh();
        }
    }

    const nextButtonClickHandler = (page, fieldChangesToUse, sourceOfClick) => {
        const state = NextButtonClickHandler(page, fieldChangesToUse, sourceOfClick, dataEntryState);
 
        if (state.error !== "") {
            updatedataEntryErrorStatus({visible: true, message: state.error + TranslateTag("@ValMes@",props.language)});
        } else {
            setDataEntryState(state);
        }
    }

    const previousButtonClickHandler = (page) => {
        const state = PreviousButtonClickHandler(page, dataEntryState);
        setDataEntryState(state);
    }

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
    }

    const refreshForm = (button) => {
        buttonClickHandler(button);
    }

    const itemInvokedHandler = (item) => {
        let viewButton = props.config.Buttons.filter(b => b.Key === 'view')[0];
    }

    const selectionChangeHandler = (selectionState) => {
        setSelectedRecord(selectionState.getSelection());
        setSelectedIndex(selectionState.getSelectedIndices());
    }

    const itemSelected = (item, selected) => {
        if (selected) {
            var arrayOfSelectedItems = [];
            arrayOfSelectedItems[0] = item;
            setSelectedRecord(arrayOfSelectedItems);
        }
    }

    let menuButtons = [];
    let menuItems = [];

    const resolvedSpecimenState = resolveSpecimenWorkflowStateForTestGrid(props);
    const rowMenusAllowed = isTestGridMenuStateAllowed(resolvedSpecimenState);

    if (rowMenusAllowed) {
        menuButtons = [
            {Icon: "RedEye", Key: "viewtest", PrimaryAction: true, Text: TranslateTag("@GenVieC@", props.language), OnFinish: "none", EntryStates: "edit, editanddelete, noteditable, edit-incomplete, editanddelete-incomplete", Workflow: true},
            {Icon: "Edit", Key: "edittest", PrimaryAction: true, Text: TranslateTag("@GenEdi@", props.language), OnFinish: "update", EntryStates: "edit, editanddelete, edit-incomplete, editanddelete-incomplete", Workflow: true},
            {Icon: "Delete", Key: "deletetest", PrimaryAction: true, Text: TranslateTag("@GenDel@", props.language), OnFinish: "refresh", EntryStates: "editanddelete, editanddelete-incomplete", Workflow: true}
        ];
        menuItems = MapButtonsToContextMenu(menuButtons, buttonClickHandler);
    } else {
        // #region agent log
        console.log('DEBUG', {
            location: 'DirectTestsGrid.buildMenuItems',
            message: 'menusSuppressed',
            data: {
                parentId: props.id,
                parentType,
                resolvedSpecimenState,
                propsStateId: props.stateid,
                parentSpecimenStateId: props.parentSpecimenStateId,
                rowCount: Array.isArray(listData) ? listData.length : 0,
            },
            timestamp: Date.now(),
        });
        // #endregion
    }

    const baseColumns = [
        { Key: 'alert', Name: '', FieldName: '', MinWidth: 20, MaxWidth: 20, IsResizable: false, IsCollapsible: false },
        { Key: 'turnaroundtime', Name: '', FieldName: 'TurnAroundTimeColour', MinWidth: 28, MaxWidth: 28, IsResizable: false, IsCollapsible: false },
        { Key: 'column1', Name: TranslateTag("@GenTesC@",props.language), FieldName: 'TestDescription', MinWidth: 180, MaxWidth: 180, IsResizable: true, IsCollapsible: false },
        { Key: 'menu', Name: '', FieldName: '', MinWidth: 120, MaxWidth: 120, IsResizable: true, IsCollapsible: false },
        { Key: 'column2', Name: TranslateTag("@GenStaA@",props.language), FieldName: 'Status', MinWidth: 200, MaxWidth: 200, IsResizable: true, IsCollapsible: true },
        { Key: 'column3', Name: TranslateTag("@GenReq@",props.language), FieldName: 'Requested', MinWidth: 180, MaxWidth: 180, IsResizable: true, IsCollapsible: true },
        { Key: 'column4', Name: TranslateTag("@GenComC@",props.language), FieldName: 'Completed', MinWidth: 200, MaxWidth: 200, IsResizable: true, IsCollapsible: true }
    ];

    let baseColumnsFiltered = [...baseColumns];
    if (props.suppressRowMenus === true) {
        baseColumnsFiltered = baseColumnsFiltered.filter((c) => c.Key !== 'menu');
    }

    const testRowMenuIdPrefix = parentType === 'culture' ? 'culturetest' : 'directtest';

    const viewKey = props.config.Name || props.config.QueryName;
    const columnLayout = useMemo(() => getColumnLayoutFromPreferences(props.preferences, viewKey), [props.preferences, viewKey]);
    const onColumnLayoutChange = useColumnLayoutChange({
        viewKey,
        columnLayout,
        onUpdate: props.onUpdatePreferencesColumnLayout,
        onSaveError: errorWhenRetrievingData,
    });

    const columns = applyColumnLayout(baseColumnsFiltered, columnLayout);

    // Filter view based on state.
    const listContentCss = ! userFormState.fullScreen ? "" : "app-invisible"
    const columnLabel = TranslateTag('@GenCol@', props.language);

    const gridLoadingState = getTestGridLoadingState(listData, listRefreshInFlight);
    const showGridArea = listRefreshInFlight || (Array.isArray(listData) && listData.length > 0);

    if (showGridArea) {
        displayType =
            <div
                data-testid="directtestsgrid-list"
                className={`directtestsgrid-list-container${gridLoadingState.blockInteraction ? ' directtestsgrid-list-refreshing' : ''}`}
            >
                <div className="directtestsgrid-toolbar">
                    <TooltipHost content={columnLabel}>
                        <IconButton
                            iconProps={{ iconName: 'ColumnOptions' }}
                            ariaLabel={columnLabel}
                            onClick={() => setShowColumnPicker(true)}
                        />
                    </TooltipHost>
                </div>
                <ListView 
                    type={"grid"}
                    columns={columns}
                    listData={Array.isArray(listData) ? listData : []}
                    menuItems={menuItems}
                    itemSelected={itemSelected}
                    selectedRecord={selectedIndex}
                    itemInvokedHandler={itemInvokedHandler}
                    selectionChanged={selectionChangeHandler}
                    isDataLoaded={gridLoadingState.isDataLoaded}
                    basic={true}
                    displaySummary={true}
                    laboratoryConfig={props.laboratory}
                    basicModeButtonHandler={embeddedModeButtonHandler}
                    forms={props.forms}
                    viewName={viewKey}
                    columnLayout={columnLayout}
                    onColumnLayoutChange={onColumnLayoutChange}
                    baseColumns={baseColumnsFiltered}
                    calloutLazyLoad={true}
                    calloutParentType={parentType}
                    testRowMenuIdPrefix={testRowMenuIdPrefix}>
                </ListView>
                {gridLoadingState.showBlockingOverlay && (
                    <div className="directtestsgrid-list-loading" data-testid="directtestsgrid-loading">
                        <Spinner label={TranslateTag("@GenLoa@", props.language)} />
                    </div>
                )}
            </div>
    } else {
        displayType = (
            <div className='directtestsgrid-no-list'>
                ----- {TranslateTag("@GenNon@", props.language)} -----
            </div>
        );
    }

    let dataEntryPanel = (null);
    if (userFormState.isFormOpen) {
        dataEntryPanel = (<SinglePagePanel
            id={dataEntryState.id}
            visible={true}
            currentPage={dataEntryState.currentPage}
            close={closeUserFormHandler}
            save={formSaveHandler}
            next={nextButtonClickHandler}
            previous={previousButtonClickHandler}
            config={userFormState.formDef}
            fullScreen={userFormState.fullScreen}
            changeHandler={fieldContentsChangeHandler}
            errorCloseHandler={dataEntryErrorCloseHandler}
            refresh={refreshForm}
            language={props.language}
            error={dataEntryErrorStatus}
            saveEnabled={saveEnabled}
            isSaving={!saveEnabled}
            state={dataEntryState}>
        </SinglePagePanel>);
    }

    return (
        <div className='directtestsgrid-content'>
            <div className={listContentCss}>
                {displayType}
            </div>
            {recordDisplay}
            {dataEntryPanel}
            <RightHelp visible={displayHelpState} closed={closeHelpHandler}></RightHelp>
            <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
            {showColumnPicker && (
                <ColumnPicker
                    isOpen={showColumnPicker}
                    onDismiss={() => setShowColumnPicker(false)}
                    columns={baseColumnsFiltered}
                    columnLayout={columnLayout}
                    onColumnLayoutChange={onColumnLayoutChange}
                    onReset={() => {
                        const payload = { Event: 'savecolumnlayoutsevent', Id: '0', ViewName: viewKey, ClearLayout: true };
                        PostEvent(payload, () => props.onUpdatePreferencesColumnLayout(viewKey, null), errorWhenRetrievingData);
                    }}
                    language={props.language}
                />
            )}
        </div>
    )
};

const mapStateToProps = state => {
    return {
        uievents: state.config.uievents,
        forms: state.config.forms,
        pages: state.config.pages,
        lists: state.config.lists,
        laboratory: state.config.laboratory,
        showFullScreen: state.display.showFullScreen,
        preferences: state.config.preferences,
    };
}

const mapDispatchToProps = dispatch => {
    return {
        onUpdatePreferencesColumnLayout: (viewKey, columnLayout) =>
            dispatch({ type: actionTypes.UPDATE_PREFERENCES_COLUMN_LAYOUT, viewKey, columnLayout }),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(DirectTestsGrid);