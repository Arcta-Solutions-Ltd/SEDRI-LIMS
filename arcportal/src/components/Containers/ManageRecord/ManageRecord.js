import React, { useState, useEffect, useRef, useCallback } from 'react';
import { connect } from 'react-redux';
import { PrimaryButton, DefaultButton, IconButton, TooltipHost, Spinner } from '@fluentui/react';
import ManageListEmbedded from '../ManageListEmbedded/ManageListEmbedded';
import Post from '../../../Data/Post';
import GeneralViewer from '../../General/GeneralViewer/GeneralViewer';
import PrintPreview from '../PrintPreview/PrintPreview';
import GetVisibleButtons from '../../../Utils/Forms/GetVisibleButtons';
import ArraySorter from '../../../Utils/General/ArraySorter';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import TopbarMenu from '../../General/TopbarMenu/TopBarMenu';
import { getStandardTooltipProps } from '../../../Utils/General/StandardTooltipProps';
import CraftedRegionFactory from '../../Crafted/CraftedRegionFactory';
import FormHandler from '../FormHandler/FormHandler';
import { Separator } from '@fluentui/react';
import Diary from '../Diary/Diary';
import PrintBarcode from '../PrintBarcode/PrintBarcode';
import ManageRecordDates from './ManageRecordDates';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import './ManageRecord.css';
import AlertList from '../../General/AlertList/AlertList';
import { GetWorkflowFromSpecimenType } from '../../../Utils/State/GetEntryStatesForRecord';
import { BespokeMenuRemoval } from '../../../Utils/Forms/GetVisibleButtons';
import ReportContainer from '../../Reports/ReportDesigner/ReportContainer';
import WorkflowContainer from '../../Configuration/WorkflowDesigner/WorkflowContainer';
import { findRecordViewDefinition } from '../../../Utils/Configuration/resolveRecordViewDefinition';
import findListRowIndex from '../../../Utils/General/FindListRowIndex';

/**
 * View names on the Configuration > Views screen that show every configuration region rather than
 * the Forms region alone. Direct tests, culture tests, reports and workflows belong to specimens.
 */
const FULLY_CONFIGURABLE_VIEWS = ['specimens'];

/**
 * Determines whether the open view configuration record shows every configuration region.
 * Falls back to Forms only when the view name cannot be resolved, which is the safe default.
 *
 * @param {Array|undefined} listData - Rows from the Views list, each carrying the view Name.
 * @param {string|number|undefined} itemId - Configuration record id of the open view.
 * @returns {boolean} True when all regions should be shown.
 */
const isFullyConfigurableView = (listData, itemId) => {
    const rowIndex = findListRowIndex(listData, itemId);
    if (rowIndex < 0) {
        return false;
    }

    const row = listData[rowIndex];
    const viewName = row?.Name ?? row?.name;
    if (typeof viewName !== 'string') {
        return false;
    }

    return FULLY_CONFIGURABLE_VIEWS.includes(viewName.toLowerCase());
};

/**
 * Displays a single record in view or edit mode, and orchestrates secondary display
 * modes such as print preview, report designer, and workflow designer.
 *
 * The bottom bar (Exit / Previous / Next buttons) is hidden when either
 * `passive` is true or `displayMode` is `"reportdesigner"`, so it does not
 * appear beneath the report designer or its full-screen preview overlay.
 *
 * @param {string}   props.type            - Record-view type key used to resolve the view definition.
 * @param {string|number} props.itemId     - The ID of the record to display.
 * @param {string}   props.action          - Initial display mode (e.g. `"normal"`, `"printpreview"`,
 *                                           `"printreport"`, `"reportdesigner"`, `"workflowdesigner"`).
 * @param {Function} props.cancel          - Callback invoked when the user exits the record view.
 * @param {Array}    [props.listData]      - Optional array of sibling records; enables Previous / Next navigation.
 * @param {Function} [props.updateItemRow] - Optional callback invoked with the new item ID after navigating
 *                                           to a sibling record, so the parent list can highlight the correct row.
 * @param {boolean}  [props.passive]       - When `true`, hides the bottom bar (Exit / Previous / Next) and the
 *                                           top-bar action menu, rendering the record in a read-only context.
 * @param {Object}   [props.recordInfo]    - Optional additional key/value pairs merged into the initial data
 *                                           request as extra query parameters.
 * @param {Object}   [props.selectedRecord] - The currently selected record object, used for workflow resolution.
 * @param {string}   [props.language]      - Active language code passed to translation utilities.
 */
const ManageRecord = (props) => {

    const [rxdItemId, setRxdItemId] = useState(props.itemId);
    const [displayMode, setDisplayMode] = useState(props.action);
    const [itemId, setItemId] = useState(props.itemId);
    const [regionData, _setRegionData] = useState([]); 
    const regionDataRef = useRef(regionData);
    const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
    const [embeddedRefreshGeneration, setEmbeddedRefreshGeneration] = useState(0);
    const [testListSyncPayload, setTestListSyncPayload] = useState(null);
    /** Specimen state from updateItemRow callback before listData re-render (Add Culture refresh race). */
    const [pendingParentStateId, setPendingParentStateId] = useState(null);
    /**
     * Synchronous specimen workflow state for embedded list injection (avoids React batching lag
     * between updateItemRow and triggerEmbeddedRefresh after Add Culture save).
     */
    const specimenContextStateIdRef = useRef(null);

    /**
     * Extracts workflow state id from a single-specimen list row payload.
     * @param {Object|Array|undefined} rowData - Row from singlespecimenforspecimenlist.
     * @returns {string|number|undefined}
     */
    const extractSpecimenStateIdFromRow = (rowData) => {
        const row = Array.isArray(rowData) ? rowData[0] : rowData;
        if (!row || typeof row !== 'object') {
            return undefined;
        }
        return row.stateid ?? row.StateId;
    };

    /**
     * Stores fresh parent specimen state for embedded lists and schedules embedded refresh after paint.
     * @param {Object|Array|undefined} rowData - Row from updateItemRow / singlespecimenforspecimenlist.
     */
    const applyFreshSpecimenContextStateAndRefreshEmbedded = (rowData) => {
        const nextState = extractSpecimenStateIdFromRow(rowData);
        if (nextState !== undefined && nextState !== null && nextState !== '') {
            specimenContextStateIdRef.current = nextState;
            setPendingParentStateId(nextState);
        }
        requestAnimationFrame(() => {
            triggerEmbeddedRefresh();
        });
    };

    /**
     * Increments the embedded-list refresh signal for regions that still need a network re-fetch.
     */
    const triggerEmbeddedRefresh = useCallback(() => {
        setEmbeddedRefreshGeneration((generation) => generation + 1);
    }, []);

    /**
     * Handles embedded list refresh completion. Only {@code 'embeddedrefresh'} requests a new fetch;
     * {@code false} means a fetch finished and must not re-trigger another refresh (avoids refetch loops).
     * @param {boolean|string|undefined} signal - {@code 'embeddedrefresh'} to refresh, {@code false} when done.
     */
    const handleEmbeddedRefreshDone = useCallback((signal) => {
        if (signal === 'embeddedrefresh') {
            triggerEmbeddedRefresh();
        }
    }, [triggerEmbeddedRefresh]);
    const [rowChanged, setRowChanged] = useState(false);
    const [formStartConfig, setFormStartConfig] = useState({});
    const [listVisibility, setListVisibility] = useState(true);
    const [viewDiary, setViewDiary] = useState({display: false});
    const [viewRecord, setViewRecord] = useState({display: false});
    const [childRecordView, setChildRecordView] = useState(null);
    const [printBarcode, setPrintBarcode] = useState({display: false});
    const [alertMessages, setAlertMessages] = useState([]);
    const [reportConfiguration, setReportConfiguration] = useState({});
    const [isDataLoaded, setIsDataLoaded] = useState(false);

    let topBarMenu = (null);
    let recordDisplay = (null);

    /**
     * Handles API errors when fetching record or alert data. Shows the error message and,
     * for the role record view, hides the loading overlay.
     * @param {Object} response - The error response (may have a data property with the message).
     */
    const errorWhenRetrievingData = (response) => {
        const errorMessage = response.data !== undefined ? response.data : response;
        updateErrorStatus({visible: true, message: errorMessage});
        if (props.type === 'roles') {
            setIsDataLoaded(true);
        }
    }

    const setRegionData = data => {
        regionDataRef.current = data;
        _setRegionData(data);
    };

    const alertDataRetrievedSuccessfully = (data) => {
        setAlertMessages(data);
    }

    useEffect(() => {
        if (props.itemId == null) {
            return;
        }
        if (String(props.itemId) !== String(itemId)) {
            setItemId(props.itemId);
            setRxdItemId(props.itemId);
        }
    }, [props.itemId]);

    useEffect(() => {
        if (pendingParentStateId == null || props.listData === undefined) {
            return;
        }
        const latestIndex = findListRowIndex(props.listData, itemId);
        if (latestIndex < 0) {
            return;
        }
        const rowState = props.listData[latestIndex].stateid ?? props.listData[latestIndex].StateId;
        if (rowState !== undefined && String(rowState) === String(pendingParentStateId)) {
            setPendingParentStateId(null);
        }
    }, [pendingParentStateId, props.listData, itemId]);

    useEffect(() => {
        if (props.parentSpecimenStateId == null) {
            specimenContextStateIdRef.current = null;
        }
    }, [itemId, props.parentSpecimenStateId]);

    /**
     * Callback when region data is received from the record query. Populates regionData and
     * recursively fetches the next region if configured. For the role record view, hides
     * the loading overlay when all regions have been loaded.
     * @param {Object} data - The fetched region data.
     * @param {number} index - The region index (0-based).
     */
    const viewDataRetrievedSuccessfully = useCallback((data, index) => {

        ManageRecordDates(data);
        var newRegionData = [...regionDataRef.current];
        newRegionData[index] = {...data};
        setRegionData(newRegionData);

        var currentView = findRecordViewDefinition(props.type, props.recordviews);

        var resultantItemId = itemId;
        if (props.itemId !== rxdItemId) {
            setItemId(props.itemId);
            setRxdItemId(props.itemId);
            resultantItemId = props.itemId;
        }

        let nextFetchableIndex = index + 1;
        const regions = currentView?.Regions;
        while (
            regions &&
            nextFetchableIndex < regions.length &&
            !regions[nextFetchableIndex]?.QueryName
        ) {
            nextFetchableIndex++;
        }
        const hasNextRegion =
            regions && nextFetchableIndex < regions.length && regions[nextFetchableIndex]?.QueryName;
        if (hasNextRegion) {
            let params = [{ Key: 'id', Value: resultantItemId }];
            if (props.recordInfo) {
                Object.entries(props.recordInfo).forEach(([k, v]) => {
                    if (v !== undefined && v !== null && k.toLowerCase() !== 'listdata' && k.toLowerCase() !== 'stateid') {
                        params.push({ Key: k, Value: String(v) });
                    }
                });
            }
            let criteria = { Name: regions[nextFetchableIndex].QueryName, Parameters: params };
            Post('query/filteredget', criteria, viewDataRetrievedSuccessfully, errorWhenRetrievingData, nextFetchableIndex);
        } else if (props.type === 'roles') {
            setIsDataLoaded(true);
        }
        resetAlerts(currentView?.Name);

    }, [rxdItemId, itemId, props.itemId, props.recordviews, props.views, props.type]);

    /**
     * Updates a single record-view region without chaining fetches to subsequent regions.
     * Used after embedded saves when only specimen details (region 0) should reload, and
     * after form saves on the role record view (Menu Permissions, Event Permissions, Clone Role).
     * For the role record view, also clears the loading overlay when the refresh query succeeds.
     * @param {Object} data - The fetched region data.
     * @param {number} index - The region index (0-based).
     */
    const refreshSingleRegionSuccessfully = useCallback((data, index) => {
        ManageRecordDates(data);
        const newRegionData = [...regionDataRef.current];
        newRegionData[index] = { ...data };
        setRegionData(newRegionData);
        if (props.type === 'roles') {
            setIsDataLoaded(true);
        }
        resetAlerts(findRecordViewDefinition(props.type, props.recordviews)?.Name);
    }, [props.recordviews, props.type]);

    useEffect(() => {
        var newView = findRecordViewDefinition(props.type, props.recordviews);

        if (newView?.Regions?.length > 0 && newView.Regions[0].QueryName !== '') {
            if (props.type === 'roles') {
                setIsDataLoaded(false);
            }
            let params = [{ Key: 'id', Value: itemId }];
            if (props.recordInfo) {
                Object.entries(props.recordInfo).forEach(([k, v]) => {
                    if (v !== undefined && v !== null && k.toLowerCase() !== 'listdata' && k.toLowerCase() !== 'stateid') {
                        params.push({ Key: k, Value: String(v) });
                    }
                });
            }
            const criteria = { Name: newView.Regions[0].QueryName, Parameters: params };
            Post('query/filteredget', criteria, viewDataRetrievedSuccessfully, errorWhenRetrievingData, 0);
        } else if (props.type === 'roles') {
            setIsDataLoaded(true);
        }

    }, [props.recordviews, props.views, props.type, viewDataRetrievedSuccessfully, itemId, props.itemId, props.recordInfo]);

    /**
     * Extra InitialQuery parameters from parent record navigation (e.g. Source, TestName on test record view) and embedded list click payload.
     * id is always supplied separately as recordId; duplicate id keys from objects are ignored.
     */
    const buildFormInitialQueryExtras = (recordId, clickRecordInfo, parentRecordInfo) => {
        const skip = new Set(['listdata', 'stateid', 'button']);
        const out = [];
        const pushFrom = (obj) => {
            if (!obj || typeof obj !== 'object') return;
            Object.entries(obj).forEach(([k, v]) => {
                if (v === undefined || v === null) return;
                if (skip.has(String(k).toLowerCase())) return;
                if (String(k).toLowerCase() === 'id') return;
                out.push({ Key: k, Value: String(v) });
            });
        };
        pushFrom(parentRecordInfo);
        pushFrom(clickRecordInfo);
        const seen = new Set();
        const deduped = [];
        for (const p of out) {
            const kl = String(p.Key).toLowerCase();
            if (seen.has(kl)) continue;
            seen.add(kl);
            deduped.push(p);
        }
        return deduped;
    };

    const setFormConfig = (button, recordId, clickRecordInfo) => {
        const extras = buildFormInitialQueryExtras(recordId, clickRecordInfo, props.recordInfo);
        extras.push({ Key: 'recordView', Value: props.type ?? '' });
        setFormStartConfig({
            button: button,
            id: recordId,
            view: props.type,
            refresh: refreshForm,
            containerVisibility: setListVisibility,
            moveToNextItem: moveToNextFormItem,
            changeHandler: formChangeHandler,
            selectedRecord: props.selectedRecord,
            currentRecord: (() => {
                const idx = findListRowIndex(props.listData, itemId);
                return idx >= 0 ? props.listData[idx] : undefined;
            })(),
            setFormStartConfig: setFormStartConfig,
            initialQueryParameters: extras
        })
    }

    const resetAlerts = (name) => {
        if (name === "specimenrecordview" || name === "cultures") {
            const criteria = { Parameters: [{ Key: 'id', Value: itemId }, { Key: 'view', Value: name }] };
            Post('alert/get', criteria, alertDataRetrievedSuccessfully, errorWhenRetrievingData);
        }
    }

    /**
     * Re-queries standard record-view regions and optionally signals embedded lists to refresh.
     * @param {{ skipEmbeddedListsRefresh?: boolean }} [options] - When true, embedded crafted/list
     *   regions are not re-fetched (e.g. direct tests grid received an in-memory testListSync payload).
     */
    const refreshAfterReturningFromForm = (options = {}) => {
        const skipEmbeddedListsRefresh = options.skipEmbeddedListsRefresh === true;

        var newView = findRecordViewDefinition(props.type, props.recordviews);

        if (newView?.Regions?.length > 0 && newView.Regions[0].QueryName !== '') {
            if (props.type === 'roles') {
                setIsDataLoaded(false);
            }
            let params = [{ Key: 'id', Value: itemId }];
            if (props.recordInfo) {
                Object.entries(props.recordInfo).forEach(([k, v]) => {
                    if (v !== undefined && v !== null && k.toLowerCase() !== 'listdata' && k.toLowerCase() !== 'stateid') {
                        params.push({ Key: k, Value: String(v) });
                    }
                });
            }
            const criteria = { Name: newView.Regions[0].QueryName, Parameters: params };
            Post('query/filteredget', criteria, refreshSingleRegionSuccessfully, errorWhenRetrievingData, 0);
        }

        if (!skipEmbeddedListsRefresh) {
            triggerEmbeddedRefresh();
        }
    }

    const moveToNextFormItem = () => {

    }

    const reportFilterFieldKeys = ['ReportFilter', 'DirectTestSelector', 'SelectorGrid', 'CommentGrid'];

    const formChangeHandler = (state) => {
        if (reportFilterFieldKeys.includes(state.key)) {
            const filterUpdate = state.value ?? {};
            setReportConfiguration(prev => ({
                ...prev,
                ...filterUpdate,
                value: filterUpdate,
            }));
        }
    }

    const clickButtonHandler = (button, id, recordInfo) => {
        const buttonToUse = { ...button };
        if (props.type === 'testrecordview' && String(buttonToUse.Key ?? buttonToUse.key).toLowerCase() === 'edittest') {
            const testName = recordInfo?.TestName ?? props.recordInfo?.TestName;
            if (testName) {
                buttonToUse.UIEvent = testName.slice(0, -4) + 'uievent';
            }
        }

        const action = props.uievents.filter(
            (a) =>
                a.Name &&
                buttonToUse.UIEvent &&
                String(a.Name).toLowerCase() === String(buttonToUse.UIEvent).toLowerCase()
        );
        if (action.length === 0 || !action[0]) {
            return;
        }
        if (action[0].Type === 'form' ) {
            setFormConfig(buttonToUse, id, recordInfo);
        }  else if (action[0].Type === 'diary')
        {
            setViewDiary({display: true, type: action[0].Action });
            setListVisibility(false);
        } else if (action[0].Type === 'view-record')
        {
            setViewRecord({display: true, type: action[0].Action, info: recordInfo });
            setListVisibility(false)
        } else if (action[0].Type === 'print-barcode')
        {
            setPrintBarcode({ display: true, type: action[0].Action });
        } else if (action[0].Type === 'printpreview')
        {
            const newDisplayMode = displayMode == "printpreview" ? "normal" : "printpreview";
            setDisplayMode(newDisplayMode)
        } else if (action[0].Type === 'printreport')
        {
            const newDisplayMode = displayMode == "printreport" ? "normal" : "printreport";
            setDisplayMode(newDisplayMode)
        } else if (action[0].Type === 'reportdesigner')
        {
            const newDisplayMode = displayMode == "reportdesigner" ? "normal" : "reportdesigner";
            setViewRecord({display: false, type: action[0].Action, info: recordInfo });
            setDisplayMode(newDisplayMode)
        } else if (action[0].Type === 'workflowdesigner')
        {
            const newDisplayMode = displayMode == "workflowdesigner" ? "normal" : "workflowdesigner";
            setViewRecord({display: false, type: action[0].Action, info: recordInfo });
            setDisplayMode(newDisplayMode)
        }
    }

    const buttonClickHandler = (button) => {
        clickButtonHandler(button, itemId);
    }

    /**
     * Refreshes the record view after a form save or embedded list action.
     * When {@code embeddedrefresh} is received and the record view has
     * {@link RefreshRecordOnEmbeddedSave}, standard regions (e.g. specimen details) are
     * re-queried and the parent list row is updated via {@code updateItemRow} so workflow
     * state on toolbar buttons and embedded lists stays in sync.
     *
     * @param {Object|string|undefined} button - Finish action name (e.g. {@code 'embeddedrefresh'}),
     *   or a button config when opening a nested workflow action.
     * @param {number|string|undefined} recordId - Record id from FormHandler (optional).
     * @param {Object|undefined} refreshContext - Optional context; {@code testListSync} hands fresh test rows to the grid.
     */
    const refreshForm = (button, recordId, refreshContext) => {
        if (button !== undefined && button.UIEvent !== undefined) {
            buttonClickHandler(button);
        }

        var newView = findRecordViewDefinition(props.type, props.recordviews);
        const testListSync = refreshContext?.testListSync;

        if (button === "embeddedrefresh") {
            if (testListSync) {
                setTestListSyncPayload(testListSync);
            }
            const refreshParentRecord =
                newView?.RefreshRecordOnEmbeddedSave ?? newView?.refreshRecordOnEmbeddedSave;
            if (refreshParentRecord) {
                refreshAfterReturningFromForm({ skipEmbeddedListsRefresh: !!testListSync });
                if (props.updateItemRow !== undefined) {
                    props.updateItemRow(itemId, applyFreshSpecimenContextStateAndRefreshEmbedded);
                }
            } else if (!testListSync) {
                triggerEmbeddedRefresh();
            }
        } else if (button === 'refresh' || button === 'update') {
            const queryName =
                newView &&
                (newView.SingleQuery === ''
                    ? newView.Regions?.[0]?.QueryName
                    : newView.SingleQuery);

            if (queryName) {
                refreshAfterReturningFromForm({ skipEmbeddedListsRefresh: true });
            }

            if (props.updateItemRow !== undefined) {
                props.updateItemRow(itemId, applyFreshSpecimenContextStateAndRefreshEmbedded);
            } else if (queryName) {
                triggerEmbeddedRefresh();
            }
        } else {
            if (props.updateItemRow !== undefined) {
                props.updateItemRow(itemId);
            }
    
            const queryName =
                newView &&
                (newView.SingleQuery === ''
                    ? newView.Regions?.[0]?.QueryName
                    : newView.SingleQuery);
    
            if (newView && queryName !== undefined && queryName !== "" && props.listData !== undefined) {
                const criteria = { Name: queryName, Parameters: [{ Key: 'id', Value: itemId }] };
                Post('query/filteredget', criteria, singleViewDataRetrievedSuccessfully, errorWhenRetrievingData, { button: button });
            }
        }

        resetAlerts(newView?.Name);

        if (props.onRefresh !== undefined) {
            props.onRefresh();
        }
    }

    const singleViewDataRetrievedSuccessfully = (data, extraInfo) => {
        const currentIndex = findListRowIndex(props.listData, itemId);
        if (currentIndex >= 0 && data?.stateid !== undefined) {
            props.listData[currentIndex].stateid = data.stateid;
        }

        refreshAfterReturningFromForm();
    }

    const listClickHandler = (recordInfo) => {
        clickButtonHandler(recordInfo.button, recordInfo.id, recordInfo);
    }

    const errorCloseHandler = () => {
        updateErrorStatus({visible: false, message: ''});
    }

    /**
     * Navigates to the previous record in `props.listData` relative to the currently displayed item.
     * If the current item is already the first in the list, no action is taken.
     * When `props.updateItemRow` is provided and the current record has unsaved changes,
     * it is called with the new item ID so the parent list can update its highlighted row.
     */
    const selectPreviousItem = () => {
        let currentIndex = findListRowIndex(props.listData, itemId);
        if (currentIndex > 0) {
            let newItemId = props.listData[--currentIndex].id;
            if (newItemId === undefined) {
                newItemId = props.listData[currentIndex].Id; 
            }
            setItemId(newItemId);
            if (props.updateItemRow !== undefined && rowChanged) {
                props.updateItemRow(newItemId);
                setRowChanged(false);
            }
        }
    }

    /**
     * Navigates to the next record in `props.listData` relative to the currently displayed item.
     * If the current item is already the last in the list, no action is taken.
     * When `props.updateItemRow` is provided and the current record has unsaved changes,
     * it is called with the new item ID so the parent list can update its highlighted row.
     */
    const selectNextItem = () => {
        let currentIndex = findListRowIndex(props.listData, itemId);
        if (currentIndex >= 0 && currentIndex < props.listData.length - 1) {
            let newItemId = props.listData[++currentIndex].id; 
            if (newItemId === undefined) {
                newItemId = props.listData[currentIndex].Id; 
            }
            setItemId(newItemId) 
            if (props.updateItemRow !== undefined && rowChanged) {
                props.updateItemRow(newItemId);
                setRowChanged(false);
            }
        }
    }

    const onDiaryCancel = () => {
        setListVisibility(true);
        setViewDiary({display: false});
    }

    const onPrintBarcodeDone = () => {
        setPrintBarcode({display: false});
    }

    const onRecordCancel = () => {
        setViewRecord({display: false});
        setListVisibility(true);
    }

    /**
     * Handles the cancel/back action from the report designer view.
     * Resets `displayMode` to `"normal"`, returning the component to its standard
     * record display and restoring the bottom bar.
     */
    const onReportDesignerCancel = () => {
        setDisplayMode("normal");
    }

    const getListView = (listViewName) => {
        return props.views.filter((view) => {
            return view.Name === listViewName;
        })[0];
    }

    const filterOutUnwantedRegions = (regions, workflow) => {
        if (!workflow || !regions || regions.length === 0) {
            return regions;
        }
    
        return regions.filter(region => {
            if (!workflow.IncludeCulture && region.Id === "cultures") {
                return false;
            }
            if (!workflow.IncludeInstrument && region.Id === "instrumentresults") {
                return false;
            }
            return true;
        });
    };

    var viewConfig = findRecordViewDefinition(props.type, props.recordviews);

    if (!viewConfig) {
        return (
            <div className='managerecord-page'>
                <ErrorMessage
                    visible={true}
                    dismissHandler={errorCloseHandler}
                    error="Record view configuration is not available for this account."
                />
            </div>
        );
    }

    // On the Configuration > Views record view, only the specimens view owns direct tests, culture
    // tests, reports and workflows. Every other view (patients included) shows the Forms region
    // alone, so its forms can still be configured. Resolved by view name rather than record id so
    // that enabling a new view for configuration needs no change here.
    let regionsToDisplay = viewConfig.Regions;
    let buttonsToDisplay = viewConfig.Buttons;
    if (props.type === "views" && !isFullyConfigurableView(props.listData, props.itemId)) {
        regionsToDisplay = viewConfig.Regions.filter((r) => {return r.Id === "forms"});
        buttonsToDisplay = [];
    }

    for (var region of viewConfig.Regions) {
        if (region.Type === 'listview' && region.ListViewName !== '') {
            region.ListView = getListView(region.ListViewName);
        }
    }

    var listContentCss = "managerecord-list-content";
    listContentCss += listVisibility ? "" : " app-invisible";

    let unsortedVisibleButtons;
    let currentIndex = -1;
    let stateId = props.stateId ?? 0;

    if (props.listData !== undefined) {
        currentIndex = findListRowIndex(props.listData, itemId);
        const currentItem = currentIndex >= 0 ? props.listData[currentIndex] : undefined;
        if (currentItem !== undefined) {
            stateId =
                currentItem.stateid !== undefined
                    ? currentItem.stateid
                    : currentItem.StateId ?? props.stateId ?? 0;
        }
    }

    unsortedVisibleButtons = GetVisibleButtons(
        buttonsToDisplay,
        true,
        stateId,
        undefined,
        undefined,
        props.selectedRecord?.specimentypeid,
        props.selectedRecord?.laboratoryid,
        props.laboratory
    );

    let visibleButtons = unsortedVisibleButtons.sort(ArraySorter("primaryAction"));
    if (props.listData && currentIndex >= 0) {
        visibleButtons = BespokeMenuRemoval(visibleButtons, props.listData[currentIndex]);
    }

    topBarMenu = <TopbarMenu
                    suppressFarItems={true}
                    inlineActions={true}
                    embeddedInForm={props.type === 'testrecordview'}
                    showFilterIcon={false}
                    displayGridView={false}
                    buttons={visibleButtons}
                    language={props.language}
                    clickButton={buttonClickHandler}
                    selectedRecord={props.selectedRecord}>
                </TopbarMenu>

    const onNavigateToRecordView = (type, id, recordInfo) => {
        setChildRecordView({ type, id, recordInfo });
        setListVisibility(false);
    };

    const onBackFromChildView = () => {
        setChildRecordView(null);
        setListVisibility(true);
    };

    // Nested record shell must use ConnectedManageRecord so Redux injects recordviews/forms/lists/uievents.
    // Do not forward Redux-derived props with possibly undefined values: that overwrites merged store props.
    if (childRecordView) {
        recordDisplay = <ConnectedManageRecord
                            itemId={childRecordView.id}
                            type={childRecordView.type}
                            recordInfo={childRecordView.recordInfo}
                            cancel={onBackFromChildView}
                            onRefresh={refreshForm}
                        >
                        </ConnectedManageRecord>;
    } else if (viewRecord.display) {
        recordDisplay = <ConnectedManageRecord
                            listData={viewRecord.info.listdata}
                            itemId={viewRecord.info.id}
                            selectedRecord={props.selectedRecord}
                            stateId={viewRecord.info.stateid}
                            parentSpecimenStateId={
                                props.selectedRecord?.stateid
                                ?? props.selectedRecord?.StateId
                                ?? props.stateId
                            }
                            type={viewRecord.type}
                            buttonClickHandler={buttonClickHandler}
                            cancel={onRecordCancel}
                            recordviews={props.recordviews}
                            views={props.views}
                            uievents={props.uievents}
                            language={props.language}
                            laboratory={props.laboratory}
                            workflow={props.workflow}
                            onRefresh={refreshForm}
                            onNavigateToRecordView={onNavigateToRecordView}
                        >
                        </ConnectedManageRecord>
    } else if (viewDiary.display) {
        recordDisplay = <Diary
            cancel={onDiaryCancel}
            type={viewDiary.type}
            itemId={itemId}
            stateId={props.stateId}
            onRefresh={refreshForm}
            >
        </Diary>
    } else if (printBarcode.display) {
        recordDisplay = <PrintBarcode
            type={printBarcode.type}
            done={onPrintBarcodeDone}
            selectedRecord={currentIndex >= 0 ? props.listData[currentIndex] : props.selectedRecord}
        >
        </PrintBarcode>
    }

    let stateid = specimenContextStateIdRef.current ?? pendingParentStateId ?? props.stateId ?? 0;
    if (specimenContextStateIdRef.current == null && pendingParentStateId == null && props.listData !== undefined) {
        const latestIndex = findListRowIndex(props.listData, itemId);
        if (latestIndex >= 0) {
            const latestRow = props.listData[latestIndex];
            stateid = latestRow.stateid === undefined
                ? parseInt(latestRow.StateId)
                : latestRow.stateid;
            if (Number.isNaN(stateid)) {
                stateid = props.stateId ?? 0;
            }
        }
    }

    let mainContent;
    let topAlert = (null);
    let bottomAlert = (null);

    const workflow = GetWorkflowFromSpecimenType(props.selectedRecord?.specimentypeid, props.selectedRecord?.laboratoryid, props.laboratory, props.workflow);
    regionsToDisplay = filterOutUnwantedRegions(regionsToDisplay, workflow);
    
    if (displayMode === "reportdesigner") {
        mainContent = <ReportContainer config={viewRecord.info} cancel={onReportDesignerCancel} language={props.language}/>
    } else if (displayMode === "workflowdesigner") {
        mainContent = <WorkflowContainer config={props.recordInfo ?? viewRecord.info}/>
    } else if (displayMode === "printpreview" || displayMode === "printreport") {
        mainContent = <div className='managerecord-content'><PrintPreview id={itemId} history={props.action === "printreport"} reportConfiguration={reportConfiguration}></PrintPreview></div>
    } else {
        topAlert = <AlertList messages={alertMessages} position="top"></AlertList>
        bottomAlert = <AlertList messages={alertMessages} position="bottom"></AlertList>
        mainContent = (
            <div className='managerecord-content'>
                {regionsToDisplay.map((region, index) => {
    
                    switch (region.Type) {
                        case "standard":
                            if (regionData[index] !== undefined && regionData[index].Sections !== undefined) {
                                return (<GeneralViewer key={region.Id} data={regionData[index]} language={props.language}></GeneralViewer>)
                            } else return (null) 
                        case "listview":
                            return (
                                <div key={region.Id} id={region.Id} data-testid={`record-section-${region.Id}`}>
                                    <div>
                                        <ManageListEmbedded
                                            config={region.ListView}
                                            parentId={region.ListView.ParentId}
                                            itemId={itemId}
                                            recordInfo={props.recordInfo}
                                            stateid={stateid}
                                            specimenContextStateIdRef={specimenContextStateIdRef}
                                            toggleFullScreen={false}
                                            region={index}
                                            refresh={embeddedRefreshGeneration}
                                            passive={props.passive}
                                            containerVisibility={setListVisibility}
                                            onButtonClick={listClickHandler}
                                            onRefreshDone={handleEmbeddedRefreshDone}
                                            title={region.Title}
                                            selectedRecord={props.selectedRecord}
                                        >
                                        </ManageListEmbedded>
                                    </div>
                                </div>
                            )
                        case "crafted":
                            return(
                                <div key={region.Id} id={region.Id} data-testid={`record-section-${region.Id}`}>
                                    {region.Title !== undefined && region.Title !== '' ? (
                                    <div className='managerecord-sectiontitle'>
                                        {region.Title}
                                    </div>
                                    ) : (null)}
                                    <div className='managerecord-section-table'>
                                        <CraftedRegionFactory
                                            config={region}
                                            region={index}
                                            containerVisibility={setListVisibility}
                                            refresh={embeddedRefreshGeneration}
                                            passive={props.passive}
                                            suppressRowMenus={
                                                props.passive === true
                                                && region.Name?.toLowerCase() !== 'directtestsgrid'
                                            }
                                            language={props.language}
                                            data={regionData}
                                            onRefresh={refreshForm}
                                            id={itemId}
                                            stateid={stateid}
                                            parentSpecimenStateId={
                                                props.parentSpecimenStateId
                                                ?? props.selectedRecord?.stateid
                                                ?? props.selectedRecord?.StateId
                                            }
                                            onButtonClick={listClickHandler}
                                            onRefreshDone={handleEmbeddedRefreshDone}
                                            testListSync={testListSyncPayload}
                                            selectedRecord={props.selectedRecord}
                                            specimenContextStateIdRef={specimenContextStateIdRef}
                                            forms={props.forms}
                                            pages={props.pages}
                                            lists={props.lists}
                                            recordInfo={props.recordInfo}
                                            onNavigateToRecordView={onNavigateToRecordView}
                                            onBackFromChildView={onBackFromChildView}
                                        >
                                        </CraftedRegionFactory>
                                    </div>
                                </div>
                                
                            )
                        default: return (null)
                    }
                })}
            </div>
        );
    }
    
    let titleBar = (        
        <div className='managerecord-top-buttons'>
            <div className='managerecord-titlebar-left'>
                                    <div className='managerecord-title'>
                        {viewConfig.Title}
                    </div>
            </div>
        </div>
    );

    if (props.recordInfo && props.cancel) {
        const backTooltip = TranslateTag("@GenExi@", props.language);
        titleBar = (
            <div className='managerecord-top-buttons'>
                <div className='managerecord-titlebar-left'>
                    <div className='managerecord-button'>
                        <TooltipHost
                            content={backTooltip}
                            tooltipProps={getStandardTooltipProps()}
                            calloutProps={{ gapSpace: 10 }}
                        >
                            <IconButton
                                id="managerecord-back"
                                iconProps={{iconName: 'Back'}}
                                ariaLabel={backTooltip}
                                onClick={() => props.cancel(itemId)} 
                            />
                        </TooltipHost>
                    </div>
                    <div className='managerecord-title'>
                        {viewConfig.Title}
                    </div>
                </div>
            </div>
        );
    } else if (props.listData !== undefined) {
        const backTooltip = TranslateTag("@GenExi@", props.language);
        const previousTooltip = TranslateTag("@GenPre@", props.language) + ' ' + viewConfig.SingleItemName;
        const nextTooltip = TranslateTag("@GenNex@", props.language) + ' ' + viewConfig.SingleItemName;
        titleBar = (
            <div className='managerecord-top-buttons'>
                <div className='managerecord-titlebar-left'>
                    <div className='managerecord-button'>
                        <TooltipHost
                            content={backTooltip}
                            tooltipProps={getStandardTooltipProps()}
                            calloutProps={{ gapSpace: 10 }}
                        >
                            <IconButton
                                id="managerecord-back"
                                iconProps={{iconName: 'Back'}}
                                ariaLabel={backTooltip}
                                onClick={() => props.cancel(itemId)} 
                            />
                        </TooltipHost>
                    </div>
                    <div className='managerecord-title'>
                        {viewConfig.Title}
                    </div>
                </div>
                <div className='managerecord-right-icon-buttons'>
                    <div className='managerecord-button'>
                        <TooltipHost
                            content={previousTooltip}
                            tooltipProps={getStandardTooltipProps()}
                            calloutProps={{ gapSpace: 10 }}
                        >
                            <IconButton
                                id="managerecord-previous"
                                iconProps={{iconName: 'Previous'}}
                                ariaLabel={previousTooltip}
                                onClick={()=>{selectPreviousItem()}}
                            />
                        </TooltipHost>
                    </div>
                    &nbsp;&nbsp;
                    <div className='managerecord-button'>
                        <TooltipHost
                            content={nextTooltip}
                            tooltipProps={getStandardTooltipProps()}
                            calloutProps={{ gapSpace: 10 }}
                        >
                            <IconButton
                                id="managerecord-next"
                                iconProps={{iconName: 'Next'}}
                                ariaLabel={nextTooltip}
                                onClick={()=>{selectNextItem()}}
                            />
                        </TooltipHost>
                    </div>
                </div>
            </div>
        );
    }

    /**
     * Bottom-bar button group rendered inside `managerecord-footer`.
     * Always contains an Exit button. When `props.listData` is provided, also
     * renders Previous and Next buttons for navigating between sibling records.
     *
     * This element is only mounted when `props.passive` is false AND
     * `displayMode` is not `"reportdesigner"`, ensuring it does not appear
     * beneath the report designer or its print preview overlay.
     */
    let footerButtons = (
        <div className='managerecord-bottom-buttons'>
            <div className='managerecord-button'>
                <DefaultButton
                    text={TranslateTag("@GenExi@", props.language)}
                    onClick={() => props.cancel(itemId)}
                    styles={{
                        root: { border: '0px', padding: '2px', backgroundColor: '#ddd'},
                        rootHovered: { backgroundColor: '#ccc' },
                        label: {  }}}
                />
            </div>
            {props.listData !== undefined && (
                <div className='managerecord-right-buttons'>
                    <div className='managerecord-button'>
                        <PrimaryButton
                            text={TranslateTag("@GenPre@", props.language) + ' ' + viewConfig.SingleItemName}
                            onClick={()=>{selectPreviousItem()}}
                        />
                    </div>
                    <div className='managerecord-button'>
                        <PrimaryButton
                            text={TranslateTag("@GenNex@", props.language) + ' ' + viewConfig.SingleItemName}
                            onClick={()=>{selectNextItem()}}
                        />
                    </div>
                </div>
            )}
        </div>
    );

    // Experimental hot-keys.
    document.onkeydown = function(e) {
        switch (e.key) {
            case "ArrowLeft":
                selectPreviousItem();
                break;
            case "ArrowRight":
                selectNextItem();
                break;
            case "Escape":
                if(props.cancel !== undefined){
                    props.cancel(itemId);
                }
                break;
            default:
                break;
        }
    };


    const pageTestId = props.type === 'testrecordview' ? 'test-record-view' : undefined;

    return (
        <div className='managerecord-page' data-testid={pageTestId}>
            {props.type === 'roles' && !isDataLoaded && (
                <div className='managerecord-loading-overlay' data-testid="managerecord-role-loading" role="status" aria-live="polite">
                    <Spinner styles={{ circle: { width: '100px', height: '100px', borderWidth: '10px' } }} />
                </div>
            )}
            {recordDisplay}
            <div className={listContentCss}>
                <div>
                    {displayMode !== "reportdesigner" && (
                        <div>
                            {titleBar}
                            {props.passive !== true ? (
                                <div data-testid="managerecord-topbar-menu">
                                    {topBarMenu}
                                    <div className='managerecord-topbar-separator'>
                                        <Separator></Separator>
                                    </div>
                                </div>
                            ) : (null)}
                        </div>
                    )}
                    {topAlert}
                    {mainContent}
                    {bottomAlert}
                </div>
                <div className='managerecord-footer'>
                    {!props.passive && displayMode !== "reportdesigner" ? (
                        <div>
                            {footerButtons}
                        </div>
                    ) : (null)}
                </div>
            </div>
            <FormHandler startConfig={formStartConfig} refresh={refreshForm} reportConfiguration={reportConfiguration}></FormHandler>
            <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message} ></ErrorMessage>
        </div>
    );
};

const mapStateToProps = state => {
    return {
        currentView: state.display.currentView,
        views: state.config.views,
        recordviews: state.config.recordviews,
        uievents: state.config.uievents,
        forms: state.config.forms,
        pages: state.config.pages,
        lists: state.config.lists,
        language: state.config.language,
        workflow: state.config.workflow,
        laboratory: state.config.laboratory
    };
}

const ConnectedManageRecord = connect(mapStateToProps)(ManageRecord);
export default ConnectedManageRecord;
