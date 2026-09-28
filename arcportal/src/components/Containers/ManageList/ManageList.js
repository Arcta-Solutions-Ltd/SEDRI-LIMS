import React, {useState, useEffect, useRef, useCallback} from 'react';
import {connect} from 'react-redux';
import * as actionTypes from '../../../store/actions';
import './ManageList.css';
import TopbarMenu from '../../General/TopbarMenu/TopBarMenu';
import ListView from '../../General/ListView/ListView';
import ColumnPicker from '../../General/ColumnPicker/ColumnPicker';
import HierarchyView from '../../General/HierarchyView/HierarchyView';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import AddListsIntoFilters, { AddDynamicFilterList,GetDynamicListsFromDatabase } from '../../../Utils/Forms/AddListsIntoFilters';
import GetVisibleButtons from '../../../Utils/Forms/GetVisibleButtons';
import MapButtonsToContextMenu from '../../../Utils/Forms/MapButtonsToContextMenu';
import IsValidEntryStateForButton from '../../../Utils/State/IsValidEntryStateForButton';
import ArraySorter from '../../../Utils/General/ArraySorter';
import ManageRecord from '../ManageRecord/ManageRecord';
import PrintBarcode from '../PrintBarcode/PrintBarcode';
import CombinedFilter from '../../General/Filter/CombinedFilter/CombinedFilter';
import { hasListViewFilterControls, shouldShowListViewFilterUi } from '../../../Utils/Forms/ListViewFilterUtils';
import Post from '../../../Data/Post';
import PostEvent from '../../../Data/PostEvents';
import Diary from '../Diary/Diary';
import FormHandler from '../FormHandler/FormHandler';
import RefreshFilterList from './Functions/RefreshFilterList';
import {AddMetaDataToParameters} from '../FormHandler/Functions/AddFilterMetaData';
import {SetInitialSortedColumn, SetDefaultPreset, UpdateSortedColumn, SelectPreset, ChangeFilterCondition, SetDefaultFiltersPassedIn} from './Functions/FilterState';
import TransformDatesInJson from '../../../Utils/Local/TransformDatesInJson';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { GetPropertyValueIgnoreCase } from '../../../Utils/General/FindCaseInsensitiveProperty';
import findListRowIndex from '../../../Utils/General/FindListRowIndex';
import { applyColumnLayout } from '../../../Utils/General/ApplyColumnLayout';
import { useColumnLayoutChange } from '../../../Utils/General/useColumnLayoutChange';

const ManageList = (props) => {

    const [listTypeState, setListTypeState] = useState("grid");
    const [displayFilterState, setDisplayFilterState] = useState(true);
    const [buttonState, setButtonState] = useState({viewGrid: true});
    const [listData, setListDataState] = useState({});
    const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
    const [selectionStatus, setSelectionStatus] = useState(false);
    const [batchStatus, setBatchStatus] = useState(false);
    const [selectedRecord, setSelectedRecord] = useState({});
    const [allSelectedRecords, setAllSelectedRecords] = useState({});
    const [selectedIndex, setSelectedIndex] = useState({});
    const [viewRecord, setViewRecord] = useState({display: false});
    const viewRecordRef = useRef(viewRecord);
    viewRecordRef.current = viewRecord;
    const [viewDiary, setViewDiary] = useState({display: false});
    const [listViewRefresh, setListViewRefresh] = useState(false);
    const [selectionState, setSelectionState] = useState(null);
    const [formStartConfig, setFormStartConfig] = useState({});
    const [listVisibility, setListVisibility] = useState(true);
    const [isDataLoaded, setIsDataLoaded] = useState(false);
    const [filterState, setFilterState] = useState({});
    const [printBarcode, setPrintBarcode] = useState({display: false});
    const [showNextButton, setShowNextButton] = useState(false);
    const [queryName, setQueryName] = useState("");
    const [reportConfiguration, setReportConfiguration] = useState({});
    const [filterPresets, setFilterPresets] = useState([]);
    const [showColumnPicker, setShowColumnPicker] = useState(false);
    const prevViewNameRef = useRef(null);

    let title = (null);
    let headerText = (null);
    let headerDivider = (null);
    let topBarMenu = (null);
    let filter = (null);
    let displayType = (null);
    let recordDisplay = (null);
    let reportDisplay = (null);
    let savedButton;

    useEffect(() => {
        const viewName = props.config?.Name;
        const viewChanged = prevViewNameRef.current !== viewName;
        prevViewNameRef.current = viewName;

        if (viewChanged) {
            const ResetViewToDefaults = () => {
                setViewRecord({display: false});
                setViewDiary({display: false});
                setListVisibility(true);
                setPrintBarcode({display: false});
                setFormStartConfig({});
                return AddListsIntoFilters(props.config.Filters, props.lists);
            }
            setIsDataLoaded(false);
            let state = { filters: ResetViewToDefaults() };
            state = SetInitialSortedColumn(props.config, state);
            state = SetDefaultPreset(props.config, state);
            if (props.data !== undefined) {
                state = SetDefaultFiltersPassedIn(props.data, state)
            }
            setFilterState(state);
            RefreshFilterList(state.filters, "", state.sortedColumn, state.descending, props.config.SearchFields, props.config.QueryName, userDataReceivedHandler, errorWhenRetrievingData);

            GetDynamicListsFromDatabase(state, dynamicFilterListsRetrieved, errorWhenRetrievingData);
        }

        setFilterPresets(props.config.FilterPresets || []);

    }, [props.config, props.lists]);

    // Tentative fix for shimmer delay.
    if (queryName !== props.config.QueryName) {
        setQueryName(props.config.QueryName);
        setIsDataLoaded(false);
    }

    const updateSortedColumn = (fieldName) => {
        setFilterState((prevState) => {
            const newState = UpdateSortedColumn(fieldName, prevState);
            refreshList(newState.filters, newState.textSearch, newState.sortedColumn, newState.descending);
            return newState;
        });
    }

    const clearFilter = () => {
        const filters = filterState.filters.map((f) => {
            return ( { ...f, values: [] });
        });;
        const state = SetDefaultPreset(props.config, {...filterState, filters: filters});
        state.startDate = undefined;
        state.endDate = undefined;

        setFilterState(state);
        RefreshFilterList(state.filters, "", state.sortedColumn, state.descending, props.config.SearchFields, props.config.QueryName, userDataReceivedHandler, errorWhenRetrievingData, undefined, undefined);
    }

    const filterDropDownHandler = (event, option, key) => {
        const state = ChangeFilterCondition(option, key, filterState);
        refreshList(state.filters, state.textSearch, state.sortedColumn, state.descending);
        setFilterState(state);
    }

    const filterPresetClickHandler = (preset) => {
        const state = SelectPreset(preset, filterState);
        refreshList(state.filters, state.textSearch, state.sortedColumn, state.descending, state.startDate, state.endDate);
        setFilterState(state);
    }

    /**
     * Saves a new filter preset to the database and updates the Redux config store.
     * @param {Object} newPreset - The preset to add (Key, Name, Default, Fields)
     */
    const saveFilterPresetHandler = (newPreset) => {
        const newPresets = [...(filterPresets || []), newPreset];
        setFilterPresets(newPresets);
        const payload = { Event: 'savefilterpresetsevent', Id: '0', ViewName: props.config.Name, FilterPresets: newPresets };
        PostEvent(payload, () => props.onUpdateFilterPresets(props.config.Name, newPresets), errorWhenRetrievingData);
    }

    /**
     * Removes a filter preset from the database and updates the Redux config store.
     * @param {Object} presetToRemove - The preset to remove
     */
    const removeFilterPresetHandler = (presetToRemove) => {
        const newPresets = (filterPresets || []).filter((p) => p.Key !== presetToRemove.Key);
        setFilterPresets(newPresets);
        const payload = { Event: 'savefilterpresetsevent', Id: '0', ViewName: props.config.Name, FilterPresets: newPresets };
        PostEvent(payload, () => props.onUpdateFilterPresets(props.config.Name, newPresets), errorWhenRetrievingData);
    }

    const dynamicFilterListsRetrieved = (data, state) => {
        state.filters = AddDynamicFilterList(state.filters,data);
        setFilterState(state);       
    };

    const handleRefreshButton = () => {
        refreshList(filterState.filters, filterState.textSearch, filterState.sortedColumn, filterState.descending);
    }

    // const refreshList = (filters, searchText, orderBy, orderDescending) => {
    //     RefreshFilterList(filters, searchText, orderBy, orderDescending, props.config.SearchFields, props.config.QueryName, userDataReceivedHandler, errorWhenRetrievingData);
    // }

    const refreshList = (filters, searchText, orderBy, orderDescending, startDate, endDate) => {
        RefreshFilterList(filters, searchText, orderBy, orderDescending, props.config.SearchFields, props.config.QueryName, userDataReceivedHandler, errorWhenRetrievingData, startDate, endDate);
    }


    const searchChangeHandler = (event, newValue) => {
        setFilterState({...filterState, textSearch: newValue})
        refreshList(filterState.filters, newValue, filterState.sortedColumn, filterState.descending);
    }

    const filterChangeHandler = (key, newValue) => {
        let filters = {...filterState};
        let matchingFilter = filters.filters.filter((f) => f.Key === key);
        if (matchingFilter.length > 0) {
            matchingFilter[0].values = newValue;
        } else {
            filters.filters.push({Key: key, FieldName: key, values: newValue});
        }
        setFilterState(filters);
        refreshList(filterState.filters, "", filterState.sortedColumn, filterState.descending);
    }

    // const dateChangeHandler = (event, newValue) => {
    //     setFilterState({...filterState, textSearch: newValue})
    //     refreshList(filterState.filters, newValue, filterState.sortedColumn, filterState.descending);
    // }

    const dateChangeHandler = (key, newValue) => {
        let startDate = filterState.startDate;
        let endDate = filterState.endDate;
        if (key === "StartDate") {
            startDate = newValue;
            setFilterState({...filterState, startDate: newValue});
        } else {
            endDate = newValue;
            setFilterState({...filterState, endDate: newValue});
        }

        refreshList(filterState.filters, filterState.textSearch, filterState.sortedColumn, filterState.descending, startDate, endDate);
    }

    /**
     * Toggles between grid (table/hierarchy) and card view.
     * For hierarchy configs, grid shows HierarchyView and card shows CardList as a mobile-friendly fallback.
     */
    const listTypeClickHandler = () => {
        if (listTypeState === "card") {
            setListTypeState("grid");
        } else {
            setListTypeState("card");
        }
        setSelectionStatus(false);
    }

    const filterClickHandler = () => {
        setDisplayFilterState((prev) => !prev);
    }

    const errorCloseHandler = () => {
        updateErrorStatus({visible: false, message: ''});
    }

    const userDataReceivedHandler = (data) => {
        TransformDatesInJson(data);

        for (var i = 0; i < data.length; i++) {
            for (let key in data[i]) {
                data[i][key] = data[i][key] === 'Yes' ? TranslateTag("@GenYesA@", props.language) : data[i][key] === 'No' ? TranslateTag("@GenNo@", props.language) : data[i][key];
              }
        }

        setListDataState(data);
        setIsDataLoaded(true);
        setShowNextButton(data.length > 1);

        const vr = viewRecordRef.current;
        if (vr.display && vr.viewedId != null) {
            const row = data.find(
                (r) => String(r.id ?? r.Id) === String(vr.viewedId)
            );
            if (row) {
                setSelectedRecord({ ...row });
            }
        }
    }

    /**
     * Configures and opens a form for the given button action.
     * @param {Object} button - The button config (Key, UIEvent, OnFinish, etc.)
     * @param {number} recordId - The record ID (0 for add forms)
     * @param {Object} [record] - The selected record; when button has addChildContext and prefillFormFields, used to pre-fill form fields
     */
    const setFormConfig = (button, recordId, record) => {
        const addChildContext = button.AddChildContext || button.addChildContext;
        const prefillMap = button.PrefillFormFields || button.prefillFormFields;

        let localFormData;
        if (addChildContext && record && prefillMap && typeof prefillMap === 'object') {
            const values = {};
            for (const [formField, recordField] of Object.entries(prefillMap)) {
                const val = GetPropertyValueIgnoreCase(record, recordField);
                if (val !== undefined) values[formField] = val;
            }
            if (Object.keys(values).length > 0) localFormData = { values };
        }

        setFormStartConfig({
            button: button,
            id: recordId ?? 0,
            view: props.config.Name,
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility,
            moveToNextItem: moveToNextFormItem,
            allSelectedRecords: allSelectedRecords,
            selectedRecord: record,
            viewData: props.config,
            changeHandler: formChangeHandler,
            localFormData
        });
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

    const updateTableRetrievedHandler = (data) => {
        const currentList = props.lists.filter(l => l.Name.toLowerCase() === data.Name.toLowerCase());
        if (currentList.length > 0) {
            currentList[0].Options = data.Options;
        }
    }

    const refreshAfterReturningFromForm = (action, id, savedData) => {
        if (action === 'update') {
            if (id !== undefined) {
                const parameters = AddMetaDataToParameters(filterState.filters, [{ Key: 'id', Value: id }]);
                const criteria = { Name: props.config.SingleQuery, Parameters: parameters };
                Post('query/filteredget', criteria, singleViewDataRetrievedSuccessfully, errorWhenRetrievingData, { Id: id });
            }
        } else if (action === 'refresh') {
            refreshList(filterState.filters, filterState.textSearch, filterState.sortedColumn, filterState.descending);
            // When viewing a record, don't touch selectedRecord - form close should not change the selected id
            if (!viewRecord.display) {
                setSelectedRecord({});
                // List data will be replaced; clear Fluent selection so the next row click syncs toolbar OnSelect buttons
                // with the new items (matches list selection to selectedRecord for GetVisibleButtons).
                if (selectionState !== null) {
                    selectionState.setAllSelected(false);
                    updateSelectionStates(selectionState);
                } else {
                    setSelectionStatus(false);
                    setBatchStatus(false);
                }
            }
        } else if (action === "refreshfilter") {
            GetDynamicListsFromDatabase(filterState, dynamicFilterListsRetrieved, errorWhenRetrievingData);
        } else if (action === "updatefromform") {
            let rowToChange;
            if (savedData.Key === undefined) {
                rowToChange = listData.findIndex(r => r.key === savedData.key);
            } else {
                rowToChange = listData.findIndex(r => r.Key === savedData.Key);
            }
            listData[rowToChange] = savedData;
            setSelectedRecord({...savedData});
        }

        if (savedData !== undefined && savedData.Event !== undefined && (savedData.Event === "addtableentry" || savedData.Event === "edittableentry" || savedData.Event === "deletetableentry")) {
            const criteria = { Name: "UpdateTableListQuery", Parameters: [{ Key: 'metaflistid', Value: savedData.metaflistid }] };
            Post('query/filteredget', criteria, updateTableRetrievedHandler, errorWhenRetrievingData );
        }
    }

    const buttonClickHandler = (button, record) => {
        routeAction(button, record ?? selectedRecord);
        // savedButton = button;
        // if (button === undefined || button === null) { return }
        // const action = props.uievents.filter(a => a.Name === button.UIEvent);

        // if (action.length === 0) {
        //     return;
        // } else if (action[0].Type === 'form' ) {
        //     let recordId = selectedRecord !== undefined && selectedRecord.id !== undefined ? selectedRecord.id : 0;
        //     recordId = recordId === 0 && selectedRecord !== undefined && selectedRecord.Id !== undefined ? selectedRecord.Id : recordId;
        //     setFormConfig(button, recordId, selectedRecord);
        // } else if (action[0].Type === 'view-record' || action[0].Type === 'printpreview' || action[0].Type === 'printreport')
        // {
        //     setViewRecord({display: true, type: action[0].Action, action: action[0].Type });
        //     setListVisibility(false);
        // } else if (action[0].Type === 'diary')
        // {
        //     setViewDiary({display: true, type: action[0].Action });
        //     setListVisibility(false);
        // } 
        // else if (action[0].Type === 'print-barcode')
        // {
        //     setPrintBarcode({ display: true, type: action[0].Action });
        // }
    }

    const multiSelectButtonClickHandler = (item, button) => {
        setSelectedRecord(item);
        routeAction(button, item);
    }

    const routeAction = (button, item) => {
        savedButton = button;
        if (button === undefined || button === null) { return }
        const action = props.uievents.filter(a => a.Name === button.UIEvent);

        if (action.length === 0) {
            return;
        } else if (action[0].Type === 'form' ) {
            let recordId = item !== undefined && item.id !== undefined ? item.id : 0;
            recordId = recordId === 0 && item !== undefined && item.Id !== undefined ? item.Id : recordId;
            setFormConfig(button, recordId, item);
        } else if (action[0].Type === 'view-record' || action[0].Type === 'printpreview' || action[0].Type === 'printreport')
        {
            const viewedId = item !== undefined && item !== null ? (item.id ?? item.Id) : undefined;
            setViewRecord({ display: true, type: action[0].Action, action: action[0].Type, viewedId });
            setListVisibility(false);
        } else if (action[0].Type === 'diary')
        {
            setViewDiary({display: true, type: action[0].Action });
            setListVisibility(false);
        } 
        else if (action[0].Type === 'print-barcode')
        {
            setPrintBarcode({ display: true, type: action[0].Action });
        }
    }

    const refreshForm = (button) => {
        buttonClickHandler(button);
    }

    const errorWhenRetrievingData = (response) => {
        const errorMessage = response.data !== undefined ? response.data : response;
        updateErrorStatus({visible: true, message: errorMessage});
    }

    const moveToNextFormItem = (button, id) => {
        let showNextButton = false;
        let currrentRecordIndex = listData.findIndex(r => r.id === id);

        if (currrentRecordIndex === -1) {
            setShowNextButton(false);
            return;
        }

        let newRecordIndex = currrentRecordIndex + 1;
        while (listData.length > newRecordIndex) {
            if (IsValidEntryStateForButton(button, listData[newRecordIndex].stateid)) {
                setFormConfig(button, listData[newRecordIndex].id);
                selectionState.setIndexSelected(newRecordIndex, true, true);
                updateSelectionStates(selectionState);
                showNextButton = (listData.length > newRecordIndex + 1) ? true : false;
                break;
            } else {
                newRecordIndex++;
            }
        }
        setShowNextButton(showNextButton);
    }

    /**
     * Merges a single-specimen list row refresh into listData and selectedRecord.
     * @param {Object} rowData - Row payload from the single-item list query.
     * @param {{ Id?: string|number, onComplete?: Function }} extraInfo - Target row id and optional callback.
     */
    const singleViewDataRetrievedSuccessfully = (rowData, extraInfo) => {
        if (selectedRecord !== undefined && rowData !== undefined && rowData !== null && rowData !== "") {
            TransformDatesInJson(rowData);
            setListDataState(prev => {
                const rowToChange = findListRowIndex(prev, extraInfo.Id);
                if (rowToChange < 0) return prev;
                const next = [...prev];
                next[rowToChange] = { ...prev[rowToChange], ...rowData };
                return next;
            });
            setSelectedRecord(prev => ({ ...prev, ...rowData }));
            setListViewRefresh(!listViewRefresh);
        }
        if (typeof extraInfo?.onComplete === 'function') {
            extraInfo.onComplete(rowData);
        }
    }

    const itemInvokedHandler = (item) => {
        const viewButton = props.config.Buttons.filter(b => b.Key === 'view')[0];
        if (viewButton) {
            buttonClickHandler(viewButton, item);
        }
    }

    const menuButtonClickForCard = (button, row) => {
        buttonClickHandler(button, row);
    };

    // From DataList.
    const selectionChangeHandler = (rxdSelectionState) => {
        setSelectionState(rxdSelectionState);
        updateSelectionStates(rxdSelectionState);
    }

    const updateSelectionStates = (selection) => {
        if (selection !== null) {
            const newSelection = selection.getSelection()[0] ?? {};
            const newHasId = newSelection && typeof newSelection === 'object' && !Array.isArray(newSelection) && (newSelection.id !== undefined || newSelection.Id !== undefined);
            const newId = newHasId ? (newSelection.id ?? newSelection.Id) : undefined;

            const syncSelectionIndices = () => {
                setAllSelectedRecords(selection.getSelection());
                setSelectedIndex(selection.getSelectedIndices());
                setSelectionStatus(selection.getSelectedCount() === 1);
                setBatchStatus(selection.getSelectedCount() > 1);
            };

            // While viewing a record, anchor selectedRecord to the opened row (viewedId), not arbitrary grid selection after refresh.
            if (viewRecord.display) {
                const viewedId = viewRecord.viewedId;
                if (viewedId !== undefined && viewedId !== null) {
                    const matchesViewed = newId !== undefined && String(newId) === String(viewedId);
                    if (!matchesViewed) {
                        syncSelectionIndices();
                        return;
                    }
                    setSelectedRecord(newSelection);
                    syncSelectionIndices();
                    return;
                }
                if (!newHasId) {
                    syncSelectionIndices();
                    return;
                }
            }

            setSelectedRecord(newSelection);
            syncSelectionIndices();
        }
    }

    const itemSelected = (item, selected) => {
        if (selected) {
            setSelectedRecord(item);
            setSelectionStatus(true);
            
            if (savedButton !== undefined) {
                let recordId = item !== undefined && item.id !== undefined ? item.id : 0;
                recordId = recordId === 0 && item !== undefined && item.Id !== undefined ? item.Id : recordId;
                setFormConfig(savedButton, recordId, item);
                savedButton = undefined;
            }
        } else {
            setSelectionStatus(false);
        }
    }

    /**
     * Re-queries the parent manage-list row for {@code id} and merges fresh fields (e.g. workflow state).
     * @param {string|number} id - Record id to refresh.
     * @param {Function} [onComplete] - Optional callback after listData/selectedRecord merge.
     */
    const updateItem = (id, onComplete) => {
        if (id !== undefined) {
            if (props.config.SingleQuery !== undefined && props.config.SingleQuery !== "") {
                const criteria = { Name: props.config.SingleQuery, Parameters: [{ Key: 'id', Value: id }] };
                Post('query/filteredget', criteria, singleViewDataRetrievedSuccessfully, errorWhenRetrievingData, { Id: id, onComplete });
            } else if (typeof onComplete === 'function') {
                onComplete();
            }
        }
    }

    const selectItem = (id) => {
        const newRecordIndex = listData.findIndex(r => r.id === id || r.Id === id);
        if (newRecordIndex >= 0 && selectionState !== null) {
            updateSelectionStates(selectionState);
        }
    }

    const onRecordCancel = (id) => {
        setViewRecord({ display: false });
        setListVisibility(true);
        selectItem(id);
    }

    const onDiaryCancel = () => {
        setListVisibility(true);
        setViewDiary({display: false});
    }

    const onPrintBarcodeDone = () => {
        setPrintBarcode({display: false});
    }

    let unsortedVisibleButtons = GetVisibleButtons(props.config.Buttons, selectionStatus, GetPropertyValueIgnoreCase(selectedRecord,"stateid"), filterState.filters, batchStatus, selectedRecord?.specimentypeid,selectedRecord?.laboratoryid, props.laboratory);
    const visibleButtons = unsortedVisibleButtons.sort(ArraySorter("primaryAction"));
    const menuButtons =  props.config.Buttons.filter((button) => { return button.OnSelect; });
    let unsortedMenuItems = MapButtonsToContextMenu(menuButtons, buttonClickHandler, props.config.Name ?? props.config.name);
    const menuItems = unsortedMenuItems.sort(ArraySorter("primaryAction"));

    const isHierarchy = !!(props.config.ParentId || props.config.parentId);
    const parentIdField = props.config.ParentId || props.config.parentId || 'parentorganisationid';
    const idField = props.config.IdField || props.config.idField || 'id';

    const baseColumns = (props.config.GridColumns || props.config.gridColumns || []).map((column) => {
        const fieldName = column.FieldName || column.fieldName;
        const base = {
            Key: column.Key || column.key,
            Name: column.Name || column.name,
            FieldName: fieldName,
            MinWidth: column.MinWidth || column.minWidth,
            MaxWidth: column.MaxWidth || column.maxWidth,
            IsResizable: (column.IsResizable ?? column.isResizable) !== false,
            IsCollapsible: column.IsCollapsible ?? column.isCollapsible,
            IsSorted: fieldName && fieldName === filterState.sortedColumn,
            IsSortedDescending: fieldName && fieldName === filterState.sortedColumn ? filterState.descending : false,
            Highlight: column.Highlight || column.highlight,
            defaultHidden: column.defaultHidden,
            DefaultHidden: column.DefaultHidden
        };
        if (isHierarchy) {
            base.HideInHierarchy = column.HideInHierarchy || column.hideInHierarchy;
        }
        return base;
    });

    const tatColumn = baseColumns.find(
        (col) => String(col.Key || col.key || '').toLowerCase() === 'turnaroundtime'
    );
    const turnaroundTimeFieldName = tatColumn ? (tatColumn.FieldName || tatColumn.fieldName) : null;

    const columnLayout = props.config.ColumnLayout || props.config.columnLayout;
    const columns = applyColumnLayout(baseColumns, columnLayout);
    const onColumnLayoutChange = useColumnLayoutChange({
        viewKey: props.config.Name,
        columnLayout,
        onUpdate: props.onUpdateColumnLayout,
        onSaveError: errorWhenRetrievingData,
    });

    // Build from layout-filtered columns so card view matches grid visibility
    const cardFieldNames = columns
        .filter((col) => {
            const key = (col.Key || col.key || '').toLowerCase();
            const fieldName = col.FieldName || col.fieldName || '';
            return fieldName && key !== 'menu' && key !== 'turnaroundtime';
        })
        .map((col) => ({ key: col.FieldName || col.fieldName, name: col.Name || col.name }));

    if (viewRecord.display) {
        const recordViewItemId =
            viewRecord.viewedId ?? selectedRecord.id ?? selectedRecord.Id;
        recordDisplay = <ManageRecord
                            listData={listData}
                            itemId={recordViewItemId}
                            selectedRecord={selectedRecord}
                            stateId={GetPropertyValueIgnoreCase(selectedRecord, "stateid")}
                            type={viewRecord.type}
                            action={viewRecord.action}
                            buttonClickHandler={buttonClickHandler}
                            cancel={onRecordCancel}
                            updateItemRow={updateItem}
                            selectItemRow={selectItem}
                        >
                        </ManageRecord>
    }

    if (viewDiary.display) {
        recordDisplay = <Diary
            cancel={onDiaryCancel}
            type={viewDiary.type}
            itemId={selectedRecord.id}
            stateId={selectedRecord.stateid}
        >
        </Diary>
    }

    if (printBarcode.display) {
        recordDisplay = <PrintBarcode
            type={printBarcode.type}
            done={onPrintBarcodeDone}
            selectedRecord={selectedRecord}
        >
        </PrintBarcode>
    }

    // Display list controls.
    var listContentCss = "managelist-list-content";
    listContentCss += listVisibility ? "" : " app-invisible";
    
    if (props.config.Title !== undefined)  {
        title = <h1 className='managelist-title'>{props.config.Title}</h1>
    }

    if (props.config.HeaderText !== undefined) {
        headerText = <div className='managelist-heading-text'>{props.config.HeaderText}</div>
    }

    const showFilterControls = hasListViewFilterControls(props.config);
    const showFilterUi = shouldShowListViewFilterUi(props.config, filterPresets);

    if (displayFilterState && showFilterUi) {
        filter = <CombinedFilter
                    config={filterState}
                    viewConfig={props.config}
                    filterPresets={filterPresets.length > 0 ? filterPresets : (props.config.FilterPresets || [])}
                    view={props.config.Name}
                    refresh={handleRefreshButton}
                    filterDropDownHandler={filterDropDownHandler}
                    onPresetClick={filterPresetClickHandler}
                    onSavePreset={saveFilterPresetHandler}
                    onRemovePreset={removeFilterPresetHandler}
                    language={props.language}
                    onClear={clearFilter}
                    searchText={filterState.textSearch}
                    startDate={filterState.startDate}
                    endDate={filterState.endDate}
                    searchChangeHandler={searchChangeHandler}
                    dateChangeHandler={dateChangeHandler}
                    filterChangeHandler={filterChangeHandler}
                    filterSearch={props.config.FilterSearch}
                    numberRanges={props.config.NumberRanges}
                    dateSearch={props.config.DateSearch}
                    dateSearchLabel={props.config.DateSearchLabel}>
                 </CombinedFilter>
    }

    topBarMenu =    <TopbarMenu 
                        showFilterIcon={showFilterControls}
                        onListTypeClick={listTypeClickHandler}
                        onFilterClick={filterClickHandler}
                        onFullScreenClick={props.toggleFullScreen}
                        displayGridView={buttonState.viewGrid}
                        showCardIcon={true}
                        showColumnIcon={listTypeState === 'grid'}
                        onColumnPickerClick={() => setShowColumnPicker(true)}
                        buttons={visibleButtons}
                        language={props.language}
                        listType={listTypeState}
                        clickButton={buttonClickHandler}
                        selectedRecord={selectedRecord}
                        inlineActions={true}>
                    </TopbarMenu>

    /* For hierarchy views, card view renders ListView with type='card' as a mobile-friendly fallback. */
    displayType = isHierarchy ? (
        listTypeState === "card" ? (
            <ListView
                key={props.config.Name}
                type="card"
                cardFieldNames={cardFieldNames}
                listData={listData}
                menuItems={menuItems}
                itemSelected={itemSelected}
                itemInvokedHandler={itemInvokedHandler}
                laboratoryConfig={props.laboratory}
                language={props.language}
                turnaroundTimeFieldName={turnaroundTimeFieldName}
                onMenuButtonClick={menuButtonClickForCard}
            />
        ) : (
            <HierarchyView
                key={props.config.Name}
                viewName={props.config.Name}
                data={listData}
                idField={idField}
                parentIdField={parentIdField}
                columns={columns}
                menuItems={menuItems}
                routeAction={routeAction}
                onItemSelected={itemSelected}
                selectedItem={selectedRecord}
                isDataLoaded={isDataLoaded}
                laboratoryConfig={props.laboratory}
                language={props.language}
            />
        )
    ) : (
        <ListView 
            key={props.config.Name}
            type={listTypeState}
            cardFieldNames={cardFieldNames}
            columns={columns}
            listData={listData}
            menuItems={menuItems}
            itemSelected={itemSelected}
            selectedRecord={selectedIndex}
            itemInvokedHandler={itemInvokedHandler}
            selectionChanged={selectionChangeHandler}
            updateSortedColumn={updateSortedColumn}
            basic={false}
            selectionModel={selectionState}
            isDataLoaded={isDataLoaded}
            displaySummary={props.config.DisplaySummary === "true"}
            language={props.language}
            multiSelect={props.config.MultiSelect}
            laboratoryConfig={props.laboratory}
            basicModeButtonHandler={multiSelectButtonClickHandler}
            refresh={listViewRefresh}
            viewName={props.config.Name}
            columnLayout={columnLayout}
            onColumnLayoutChange={onColumnLayoutChange}
            baseColumns={baseColumns}
            turnaroundTimeFieldName={turnaroundTimeFieldName}
            onMenuButtonClick={menuButtonClickForCard}>
        </ListView>
    );

    return (
        <div className='managelist-content'>
            <div className={listContentCss}>
                <div>
                    {!props.showFullScreen ? (
                        <hgroup>
                            {title}
                            {headerText}
                        </hgroup>
                    ) : (null)}
                    {topBarMenu}
                    {filter}
                    {headerDivider}
                </div>
                <div className='managelist-list'>
                    {displayType}
                </div>
            </div>
            {recordDisplay}
            {reportDisplay}
            <FormHandler startConfig={formStartConfig} filters={filterState.filters} refresh={refreshForm} showNextButton={showNextButton} reportConfiguration={reportConfiguration}></FormHandler>
            <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
            {showColumnPicker && (
                <ColumnPicker
                    isOpen={showColumnPicker}
                    onDismiss={() => setShowColumnPicker(false)}
                    columns={baseColumns}
                    columnLayout={columnLayout}
                    onColumnLayoutChange={onColumnLayoutChange}
                    onReset={() => {
                        const payload = { Event: 'savecolumnlayoutsevent', Id: '0', ViewName: props.config.Name, ClearLayout: true };
                        PostEvent(payload, () => props.onUpdateColumnLayout(props.config.Name, null), errorWhenRetrievingData);
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
        language: state.config.language,
        laboratory: state.config.laboratory,
        showFullScreen: state.display.showFullScreen
    };
}

const mapDispatchToProps = dispatch => {
    return {
        onFilterSelect: (value) => dispatch({type: actionTypes.SETFILTERPRESET, value: value}),
        onUpdateFilterPresets: (viewName, filterPresets) =>
            dispatch({ type: actionTypes.UPDATE_VIEW_FILTER_PRESETS, viewName, filterPresets }),
        onUpdateColumnLayout: (viewName, columnLayout) =>
            dispatch({ type: actionTypes.UPDATE_VIEW_COLUMN_LAYOUT, viewName, columnLayout }),
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(ManageList);
