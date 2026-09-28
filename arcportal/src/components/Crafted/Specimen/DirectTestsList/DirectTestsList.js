import React, {useState, useEffect} from 'react';
import {connect} from 'react-redux';
import * as actionTypes from '../../../../store/actions';
import TopbarMenu from '../../../General/TopbarMenu/TopBarMenu';
import ListView from '../../../General/ListView/ListView';
import ColumnPicker from '../../../General/ColumnPicker/ColumnPicker';
import ErrorMessage from '../../../General/ErrorMessage/ErrorMessage';
import AddListsIntoFilters, { AddDynamicFilterList,GetDynamicListsFromDatabase } from '../../../../Utils/Forms/AddListsIntoFilters';
import GetVisibleButtons from '../../../../Utils/Forms/GetVisibleButtons';
import MapButtonsToContextMenu from '../../../../Utils/Forms/MapButtonsToContextMenu';
import ArraySorter from '../../../../Utils/General/ArraySorter';
import CombinedFilter from '../../../General/Filter/CombinedFilter/CombinedFilter';
import { hasListViewFilterControls, shouldShowListViewFilterUi } from '../../../../Utils/Forms/ListViewFilterUtils';
import Post from '../../../../Data/Post';
import PostEvent from '../../../../Data/PostEvents';
import RefreshFilterList from '../../../Containers/ManageList/Functions/RefreshFilterList';
import { applyColumnLayout } from '../../../../Utils/General/ApplyColumnLayout';
import { useColumnLayoutChange } from '../../../../Utils/General/useColumnLayoutChange';
import { SetInitialSortedColumn, SetDefaultPreset, UpdateSortedColumn, SelectPreset, ChangeFilterCondition, SetDefaultFiltersPassedIn } from '../../../Containers/ManageList/Functions/FilterState';
import TransformDatesInJson from '../../../../Utils/Local/TransformDatesInJson';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import FormHandler from '../../../Containers/FormHandler/FormHandler';
import { AddMetaDataToParameters } from '../../../Containers/FormHandler/Functions/AddFilterMetaData';
import IsValidEntryStateForButton from '../../../../Utils/State/IsValidEntryStateForButton';

const DirectTestsList = (props) => {

    const [listTypeState, setListTypeState] = useState("grid");
    const [displayFilterState, setDisplayFilterState] = useState(true);
    const [buttonState, setButtonState] = useState({viewGrid: true});
    const [listData, setListDataState] = useState({});
    const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
    const [selectionStatus, setSelectionStatus] = useState(false);
    const [selectedRecord, setSelectedRecord] = useState({});
    const [selectedIndex, setSelectedIndex] = useState({});
    const [listViewRefresh, setListViewRefresh] = useState(false);
    const [selectionState, setSelectionState] = useState(null);
    const [listVisibility, setListVisibility] = useState(true);
    const [isDataLoaded, setIsDataLoaded] = useState(false);
    const [filterState, setFilterState] = useState({});
    const [showNextButton, setShowNextButton] = useState(false);
    const [formStartConfig, setFormStartConfig] = useState({});

    // Part of tentative fix for shimmer delay.
    const [queryName, setQueryName] = useState("");
    const [showColumnPicker, setShowColumnPicker] = useState(false);
    const [filterPresets, setFilterPresets] = useState([]);

    const columnLayout = props.config.ColumnLayout || props.config.columnLayout;
    const onColumnLayoutChange = useColumnLayoutChange({
        viewKey: props.config.Name,
        columnLayout,
        onUpdate: props.onUpdateColumnLayout,
        onSaveError: (response) => updateErrorStatus({ visible: true, message: response }),
    });

    let title = (null);
    let headerText = (null);
    let headerDivider = (null);
    let topBarMenu = (null);
    let filter = (null);
    let displayType = (null);
    let savedButton;

    useEffect(() => {

        const ResetViewToDefaults = () => {
            setListVisibility(true);
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

    }, [props.config, props.lists]);

    // Tentative fix for shimmer delay.
    if (queryName !== props.config.QueryName) {
        setQueryName(props.config.QueryName);
        setIsDataLoaded(false);
    }

    const updateSortedColumn = (fieldName) => {
        const state = UpdateSortedColumn(fieldName, filterState);
        setFilterState(state);
        refreshList(state.filters, state.textSearch, state.sortedColumn, state.descending);
    }

    const clearFilter = () => {
        const filters = filterState.filters.map((f) => {
            return ( { ...f, values: [] });
        });;
        const state = SetDefaultPreset(props.config, {...filterState, filters: filters});

        setFilterState(state);
        RefreshFilterList(state.filters, "", state.sortedColumn, state.descending, props.config.SearchFields, props.config.QueryName, userDataReceivedHandler, errorWhenRetrievingData);
    }

    const filterDropDownHandler = (event, option, key) => {
        const state = ChangeFilterCondition(option, key, filterState);
        refreshList(state.filters, state.textSearch, state.sortedColumn, state.descending);
        setFilterState(state);
    }

    const filterPresetClickHandler = (preset) => {
        const state = SelectPreset(preset, filterState);
        refreshList(state.filters, state.searchText, state.sortedColumn, state.descending);
        setFilterState(state);
    }

    const saveFilterPresetHandler = (newPreset) => {
        const newPresets = [...(filterPresets || []), newPreset];
        setFilterPresets(newPresets);
        const payload = { Event: 'savefilterpresetsevent', Id: '0', ViewName: props.config.Name, FilterPresets: newPresets };
        PostEvent(payload, () => props.onUpdateFilterPresets(props.config.Name, newPresets), errorWhenRetrievingData);
    }

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

    const refreshList = (filters, searchText, orderBy, orderDescending) => {
        RefreshFilterList(filters, searchText, orderBy, orderDescending, props.config.SearchFields, props.config.QueryName, userDataReceivedHandler, errorWhenRetrievingData);
    }

    const searchChangeHandler = (event, newValue) => {
        setFilterState({...filterState, textSearch: newValue})
        refreshList(filterState.filters, newValue, filterState.sortedColumn, filterState.descending);
    }

    const dateChangeHandler = (event, newValue) => {
        setFilterState({...filterState, textSearch: newValue})
        refreshList(filterState.filters, newValue, filterState.sortedColumn, filterState.descending);
    }

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

    const recordDisplay = (record) => {
        const form = props.forms.filter(f => f.Name === record.TestName);
        record.stateid = form.length === 0 ? "noteditable" : "edit";
        if (record.stateid === "edit") {
            const deleteForm = props.config.Id === "directtests" ? props.forms.filter(f => f.Name === 'removedirecttestform') : props.forms.filter(f => f.Name === 'removeculturetestform');
            record.stateid = deleteForm.length === 0 ? "edit" : "editanddelete";
        }
        record.Status = record.Status === "Complete" ? TranslateTag("@GenComC@", props.language) : TranslateTag("@GenReq@", props.language);

        return record;
    }

    const userDataReceivedHandler = (data) => {

        TransformDatesInJson(data);
        for (let record of data) {
            record = recordDisplay(record);
        }
        setListDataState(data);
        setIsDataLoaded(true);
        setShowNextButton(data.length > 1);
    }

    const buttonClickHandler = (button, recordOverride) => {
        const record = recordOverride !== undefined ? recordOverride : selectedRecord;
        savedButton = button;
        if (button === undefined || button === null) { return }
        if (button.Key === "viewtest") {
            return;
        }
        if (button.Key === "deletetest") {
            button.UIEvent = props.config.Name === "tests" ? "removedirecttestuievent" : "removeculturetestuievent";
            //savedButton = undefined;
        } else {
            if (record !== undefined && record.TestName !== undefined) {
                button.UIEvent = record.TestName.slice(0, -4) + "uievent";
                //savedButton = undefined;
            }
        }
        const action = props.uievents.filter(a => a.Name === button.UIEvent);

        if (action.length === 0) {
            return;
        } else if (action[0].Type === 'form' ) {
            let recordId = record !== undefined && record.id !== undefined ? record.id : 0;
            recordId = recordId === 0 && record !== undefined && record.Id !== undefined ? record.Id : recordId;
            setFormConfig(button, recordId, record);
        } 
    }

    const embeddedModeButtonHandler = (item, button) => {
        if (button.Key === "viewtest") {
            if (props.onNavigateToRecordView) {
                const id = item.id !== undefined ? item.id : item.Id;
                const source = props.config.Name === "culturetests" ? "culture" : "direct";
                props.onNavigateToRecordView("testrecordview", id, { TestName: item.TestName, Source: source });
            } else {
                updateErrorStatus({ visible: true, message: "View not available for this test." });
            }
            return;
        }
        setSelectedRecord(item);
        buttonClickHandler(button, item);
    }

    const setFormConfig = (button, recordId, record) => {
        setFormStartConfig({
            button: button, 
            id: recordId, 
            view: props.config.Name, 
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility,
            moveToNextItem: moveToNextFormItem, 
            selectedRecord: record,
            viewData: props.config
        })
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
            setSelectedRecord([]);
        } 
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
                setFormConfig(button, listData[newRecordIndex].id, listData[newRecordIndex]);
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

    const errorWhenRetrievingData = (response) => {
        updateErrorStatus({visible: true, message: response});
    }

    const singleViewDataRetrievedSuccessfully = (rowData, extraInfo) => {
        if (selectedRecord !== undefined && rowData !== undefined && rowData !== null && rowData !== "") {
            const rowToChange = listData.findIndex(r => r.id === extraInfo.Id || r.Id === extraInfo.Id);
            TransformDatesInJson(rowData);
            rowData = recordDisplay(rowData);
            const mergedRow = rowToChange >= 0 ? { ...listData[rowToChange], ...rowData } : rowData;
            listData[rowToChange] = mergedRow;
            setSelectedRecord({ ...mergedRow });
            setListViewRefresh(!listViewRefresh);
        }
    }

    const itemInvokedHandler = (item) => {
        const viewButton = props.config.Buttons.filter(b => b.Key === 'view')[0];
        if (viewButton) {
            buttonClickHandler(viewButton, item);
        }
    }

    const menuButtonClickForCard = (button, row) => {
        embeddedModeButtonHandler(row, button);
    };

    // From DataList.
    const selectionChangeHandler = (rxdSelectionState) => {
        setSelectionState(rxdSelectionState);
        updateSelectionStates(rxdSelectionState);
    }

    const updateSelectionStates = (selection) => {
        if (selection !== null) {
            setSelectedRecord(selection.getSelection()[0]);
            setSelectedIndex(selection.getSelectedIndices());
            setSelectionStatus(selection.getSelectedCount() > 0);
        }
    }

    // From CardList.
    const itemSelected = (item, selected) => {
        if (selected) {
            setSelectedRecord(item);
            setSelectionStatus(true);

            if (savedButton !== undefined) {

                if (savedButton.Key !== "deletetest") {
                    savedButton.UIEvent = item.TestName.slice(0, -4) + "uievent";
                }

                const action = props.uievents.filter(a => a.Name === savedButton.UIEvent);

                if (action.length === 0) {
                    return;
                } else if (action[0].Type === 'form' ) {
                    let recordId = item !== undefined && item.id !== undefined ? item.id : 0;
                    recordId = recordId === 0 && item !== undefined && item.Id !== undefined ? item.Id : recordId;
                    setFormConfig(savedButton, recordId, item);
                } 
                savedButton = undefined;
            }
        } else {
            setSelectionStatus(false);
        }
    }

    // const updateItem = (id) => {
    //     if (id !== undefined) {
    //         if (props.config.SingleQuery !== undefined && props.config.SingleQuery !== "") {
    //             const criteria = { Name: props.config.SingleQuery, Parameters: [{ Key: 'id', Value: id }] };
    //             Post('query/filteredget', criteria, singleViewDataRetrievedSuccessfully, errorWhenRetrievingData, { Id: id });
    //         } 
    //     }
    // }

    // const selectItem = (id) => {
    //     let newRecordIndex = listData.findIndex(r => r.id === id);
    //     if (newRecordIndex > 0) {
    //         updateSelectionStates(selectionState);
    //     }
    // }

    const refreshForm = (button) => {
        buttonClickHandler(button);
    }

    //let unsortedVisibleButtons = GetVisibleButtons(props.config.Buttons, selectionStatus, selectedRecord === undefined ? undefined : selectedRecord.stateid, filterState.filters);
    const visibleButtons = []//unsortedVisibleButtons.sort(ArraySorter("primaryAction"));

    const menuButtons = [
        {Icon: "RedEye", Key: "viewtest", PrimaryAction: true, Text: TranslateTag("@GenVieC@", props.language), OnFinish: "none", EntryStates: "edit, editanddelete, noteditable", Workflow: false},
        {Icon: "Edit", Key: "edittest", PrimaryAction: true, Text: TranslateTag("@GenEdi@",props.language), OnFinish: "update", EntryStates: "edit, editanddelete", Workflow: true},
        {Icon: "Delete", Key: "deletetest", PrimaryAction: true, Text: TranslateTag("@GenDel@",props.language), OnFinish: "refresh", EntryStates: "editanddelete", Workflow: true}
    ];
    const menuItems = MapButtonsToContextMenu(menuButtons, buttonClickHandler);

    const baseColumns = (props.config.GridColumns || []).map((column) => ({
        Key: column.Key,
        Name: column.Name,
        FieldName: column.FieldName,
        MinWidth: column.MinWidth,
        MaxWidth: column.MaxWidth,
        IsResizable: column.IsResizable !== false,
        IsCollapsible: column.IsCollapsible,
        IsSorted: column.FieldName === filterState.sortedColumn,
        IsSortedDescending: column.FieldName === filterState.sortedColumn ? filterState.descending : false,
        Highlight: column.Highlight,
        defaultHidden: column.defaultHidden,
        DefaultHidden: column.DefaultHidden,
    }));

    const tatColumn = baseColumns.find(
        (col) => String(col.Key || col.key || '').toLowerCase() === 'turnaroundtime'
    );
    const turnaroundTimeFieldName = tatColumn ? (tatColumn.FieldName || tatColumn.fieldName) : null;

    const columns = applyColumnLayout(baseColumns, columnLayout);
    const cardFieldNames = columns
        .filter((col) => {
            const key = (col.Key || col.key || '').toLowerCase();
            const fieldName = col.FieldName || col.fieldName || '';
            return fieldName && key !== 'menu' && key !== 'turnaroundtime';
        })
        .map((col) => ({ key: col.FieldName || col.fieldName, name: col.Name || col.name }));

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
                    searchChangeHandler={searchChangeHandler}
                    dateChangeHandler={dateChangeHandler}
                    filterSearch={props.config.FilterSearch}
                    dateSearch={props.config.DateSearch}>
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
                        selectedRecord={selectedRecord}>
                    </TopbarMenu>

    displayType =
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
            basicModeButtonHandler={embeddedModeButtonHandler}
            laboratoryConfig={props.laboratory}
            forms={props.forms}
            selectionModel={selectionState}
            isDataLoaded={isDataLoaded}
            displaySummary={props.config.DisplaySummary === "true"}
            language={props.language}
            refresh={listViewRefresh}
            viewName={props.config.Name}
            columnLayout={columnLayout}
            onColumnLayoutChange={onColumnLayoutChange}
            baseColumns={baseColumns}
            turnaroundTimeFieldName={turnaroundTimeFieldName}
            onMenuButtonClick={menuButtonClickForCard}>
        </ListView>
        

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
            <FormHandler startConfig={formStartConfig} filters={filterState.filters} refresh={refreshForm} showNextButton={showNextButton} ></FormHandler>
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
        onUpdateColumnLayout: (viewName, columnLayout) =>
            dispatch({ type: actionTypes.UPDATE_VIEW_COLUMN_LAYOUT, viewName, columnLayout }),
        onUpdateFilterPresets: (viewName, presets) =>
            dispatch({ type: actionTypes.UPDATE_VIEW_FILTER_PRESETS, viewName, filterPresets: presets }),
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(DirectTestsList);
