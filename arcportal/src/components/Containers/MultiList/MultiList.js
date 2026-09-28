// import React, {useState, useEffect} from 'react';
// import {connect} from 'react-redux';
// import * as actionTypes from '../../../store/actions';
// import './MultiList.css';
// import TopbarMenu from '../../General/TopbarMenu/TopBarMenu';
// import ListView from '../../General/ListView/ListView';
// import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
// import AddListsIntoFilters, { AddDynamicFilterList,GetDynamicListsFromDatabase } from '../../../Utils/Forms/AddListsIntoFilters';
// import GetVisibleButtons from '../../../Utils/Forms/GetVisibleButtons';
// import MapButtonsToContextMenu from '../../../Utils/Forms/MapButtonsToContextMenu';
// // import IsValidEntryStateForButton from '../../../Utils/State/IsValidEntryStateForButton';
// import ArraySorter from '../../../Utils/General/ArraySorter';
// // import ManageRecord from '../ManageRecord/ManageRecord';
// import FormHandler from '../FormHandler/FormHandler';
// import CombinedFilter from '../../General/Filter/CombinedFilter/CombinedFilter';
// // import Post from '../../../Data/Post';
// import RefreshFilterList from '../ManageList/Functions/RefreshFilterList';
// // import {AddMetaDataToParameters} from '../FormHandler/Functions/AddFilterMetaData';
// import {SetInitialSortedColumn, SetDefaultPreset, UpdateSortedColumn, SelectPreset, ChangeFilterCondition} from '../ManageList/Functions/FilterState';
// import TransformDatesInJson from '../../../Utils/Local/TransformDatesInJson';
// import TranslateTag from '../../../Utils/Local/TranslateTag';


// const MultiList = (props) => {

//     const [listTypeState, setListTypeState] = useState("grid");
//     const [displayFilterState, setDisplayFilterState] = useState(true);
//     const [buttonState] = useState({viewGrid: true});
//     const [listData, setListDataState] = useState({});
//     const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
//     const [selectionStatus, setSelectionStatus] = useState(false);
//     const [selectedRecords, setSelectedRecords] = useState({});
//     const [selectedIndex, setSelectedIndex] = useState({});
// //    const [viewRecord, setViewRecord] = useState({display: false});
//     const [listViewRefresh, setListViewRefresh] = useState(false);
//     const [selectionState, setSelectionState] = useState(null);
//     const [listVisibility, setListVisibility] = useState(true);
//     const [isDataLoaded, setIsDataLoaded] = useState(false);
//     const [filterState, setFilterState] = useState({});
//     const [formStartConfig, setFormStartConfig] = useState({});

//     const [queryName, setQueryName] = useState("");

//     let title = (null);
//     let headerText = (null);
//     let headerDivider = (null);
//     let topBarMenu = (null);
//     let filter = (null);
//     let displayType = (null);
//     let recordDisplay = (null);
//     let reportDisplay = (null);


//     useEffect(() => {

//         const ResetViewToDefaults = () => {

// //            setViewRecord({display: false});
//             setListVisibility(true);
//             return AddListsIntoFilters(props.config.Filters, props.lists);
//         }
//         setIsDataLoaded(false);
//         let state = { filters: ResetViewToDefaults() };
//         state = SetInitialSortedColumn(props.config, state);
//         state = SetDefaultPreset(props.config, state);
//         setFilterState(state);
//         RefreshFilterList(state.filters, "", state.sortedColumn, state.descending, props.config.SearchFields, props.config.QueryName, userDataReceivedHandler, errorWhenRetrievingData);

//         GetDynamicListsFromDatabase(state, dynamicFilterListsRetrieved, errorWhenRetrievingData);
//     }, [props.config, props.lists]);

//     // Tentative fix for shimmer delay.
//     if (queryName !== props.config.QueryName) {
//         setQueryName(props.config.QueryName);
//         setIsDataLoaded(false);
//     }

//     const updateSortedColumn = (fieldName) => {
//         const state = UpdateSortedColumn(fieldName, filterState);
//         setFilterState(state);
//         refreshList(state.filters, state.searchText, state.sortedColumn, state.descending, filterState.startDate, filterState.endDate);
//     }

//     const clearFilter = () => {
//         const filters = filterState.filters.map((f) => {
//             return ( { ...f, values: [] });
//         });

//         const state = SetDefaultPreset(props.config, {...filterState, filters: filters});
//         state.startDate = undefined;
//         state.endDate = undefined;

//         setFilterState(state);
//         RefreshFilterList(state.filters, "", state.sortedColumn, state.descending, props.config.SearchFields, props.config.QueryName, userDataReceivedHandler, errorWhenRetrievingData, undefined, undefined);
//     }

//     const filterDropDownHandler = (event, option, key) => {
//         const state = ChangeFilterCondition(option, key, filterState);
//         refreshList(state.filters, state.textSearch, state.sortedColumn, state.descending, filterState.startDate, filterState.endDate);
//         setFilterState(state);
//     }

//     const filterPresetClickHandler = (preset) => {
//         const state = SelectPreset(preset, filterState);
//         refreshList(state.filters, state.searchText, state.sortedColumn, state.descending, filterState.startDate, filterState.endDate);
//         setFilterState(state);
//     }

//     const dynamicFilterListsRetrieved = (data, state) => {
//         state.filters = AddDynamicFilterList(state.filters,data);
//         setFilterState(state);       
//     };

//     const handleRefreshButton = () => {
//         refreshList(filterState.filters, filterState.textSearch, filterState.sortedColumn, filterState.descending, filterState.startDate, filterState.endDate);
//     }

//     const refreshList = (filters, searchText, orderBy, orderDescending, startDate, endDate) => {
//         RefreshFilterList(filters, searchText, orderBy, orderDescending, props.config.SearchFields, props.config.QueryName, userDataReceivedHandler, errorWhenRetrievingData, startDate, endDate);
//     }

//     const searchChangeHandler = (event, newValue) => {
//         setFilterState({...filterState, textSearch: newValue})
//         refreshList(filterState.filters, newValue, filterState.sortedColumn, filterState.descending, filterState.startDate, filterState.endDate);
//     }

//     const dateChangeHandler = (key, newValue) => {
//         let startDate = filterState.startDate;
//         let endDate = filterState.endDate;
//         if (key === "StartDate") {
//             startDate = newValue;
//             setFilterState({...filterState, startDate: newValue});
//         } else {
//             endDate = newValue;
//             setFilterState({...filterState, endDate: newValue});
//         }

//         refreshList(filterState.filters, filterState.textSearch, filterState.sortedColumn, filterState.descending, startDate, endDate);
//     }

//     const listTypeClickHandler = () => {
//         if (listTypeState === "card") {
//             setListTypeState("grid");
//         } else {
//             setListTypeState("card");
//         }
//         setSelectionStatus(false);
//     }

//     const filterClickHandler = () => {
//         setDisplayFilterState(! displayFilterState);
//     }

//     const errorCloseHandler = () => {
//         updateErrorStatus({visible: false, message: ''});
//     }

//     const userDataReceivedHandler = (data) => {

//         TransformDatesInJson(data);

//         for (var i = 0; i < data.length; i++) {
//             for (let key in data[i]) {
//                 data[i][key] = data[i][key] === 'Yes' ? TranslateTag("@GenYesA@", props.language) : data[i][key] === 'No' ? TranslateTag("@GenNo@", props.language) : data[i][key];
//               }
//         }

//         setListDataState(data);
//         setIsDataLoaded(true);
//     }

//     const setFormConfig = (button) => {
//         setFormStartConfig({
//             button: button, 
//             allSelectedRecords: selectedRecords,
//             view: props.config.Name, 
//             refresh: refreshAfterReturningFromForm,
//             containerVisibility: setListVisibility
//         })
//     }

//     const refreshAfterReturningFromForm = (action, id) => {
//         if (action === 'refresh') {
//             refreshList(filterState.filters, filterState.searchText, filterState.sortedColumn, filterState.descending, filterState.startDate, filterState.endDate);
//             setSelectedRecords([]);
//         } 
//     }

//     const buttonClickHandler = (button) => {
//         const action = props.uievents.filter(a => a.Name === button.UIEvent);
//         if (action[0].Type === 'form' ) {
//             setFormConfig(button);
//         } 
//     }

//     const errorWhenRetrievingData = (response) => {
//         updateErrorStatus({visible: true, message: response});
//     }

//     const itemInvokedHandler = (item) => {
//         let viewButton = props.config.Buttons.filter(b => b.Key === 'view')[0];
//         if (viewButton) {
//             buttonClickHandler(viewButton);
//         }
//     }

//     // From DataList.
//     const selectionChangeHandler = (rxdSelectionState) => {
//         setSelectionState(rxdSelectionState);
//         updateSelectionStates(rxdSelectionState);
//     }

//     const refreshForm = () => {
//         refreshList(filterState.filters, filterState.searchText, filterState.sortedColumn, filterState.descending, filterState.startDate, filterState.endDate);
//         //setSelectedRecords([]);
//         //buttonClickHandler(button);
//     }

//     const updateSelectionStates = (selection) => {
//         if (selection !== null) {
//             setSelectedRecords(selection.getSelection());
//             setSelectedIndex(selection.getSelectedIndices());
//             setSelectionStatus(selection.getSelectedCount() > 0);
//         }
//     }

//     // const selectItem = (id) => {
//     //     let newRecordIndex = listData.findIndex(r => r.id === id);
//     //     if (newRecordIndex > 0) {
//     //         selectionState.setIndexSelected(newRecordIndex, true, true);
//     //         updateSelectionStates(selectionState);
//     //     }
//     // }

//     let unsortedVisibleButtons = GetVisibleButtons(props.config.Buttons, selectionStatus, undefined);
//     const visibleButtons = unsortedVisibleButtons.sort(ArraySorter("primaryAction"));
//     const menuButtons =  props.config.Buttons.filter((button) => { return button.OnSelect; });
//     let unsortedMenuItems = MapButtonsToContextMenu(menuButtons, buttonClickHandler);
//     const menuItems = unsortedMenuItems.sort(ArraySorter("primaryAction"));

//     const columns = props.config.GridColumns.map((column) => {
//         return { Key: column.Key,
//                  Name: column.Name,
//                  FieldName: column.FieldName,
//                  MinWidth: column.MinWidth,
//                  MaxWidth: column.MaxWidth,
//                  IsResizable: column.IsResizable,
//                  IsCollapsible: column.IsCollapsible,
//                  IsSorted: column.FieldName === filterState.sortedColumn ? true : false,
//                  IsSortedDescending: column.FieldName === filterState.sortedColumn ? filterState.descending : false,
//                  Highlight: column.Highlight
//                }
//     })

//     // Display list controls.

//     var listContentCss = "multilist-list-content";
//     listContentCss += listVisibility ? "" : " app-invisible";
// //    const listContentCss = listVisibility ? "" : "app-invisible";

//     if (props.config.Title !== undefined)  {
//         title = <div className='multilist-title'>{props.config.Title}</div>
//     }

//     if (props.config.HeaderText !== undefined) {
//         headerText = <div className='multilist-heading-text'>{props.config.HeaderText}</div>
//     }

//     if (displayFilterState && filterState.filters !== undefined && Array.isArray(filterState.filters)) {
//         filter = <CombinedFilter
//                     config={filterState}
//                     filterPresets={props.config.FilterPresets}
//                     view={props.config.Name}
//                     refresh={handleRefreshButton}
//                     filterDropDownHandler={filterDropDownHandler}
//                     onPresetClick={filterPresetClickHandler}
//                     language={props.language}
//                     onClear={clearFilter}
//                     searchText={filterState.textSearch}
//                     startDate={filterState.startDate}
//                     endDate={filterState.endDate}
//                     searchChangeHandler={searchChangeHandler}
//                     dateChangeHandler={dateChangeHandler}
//                     filterSearch={props.config.FilterSearch}
//                     dateSearch={props.config.DateSearch}>
//                  </CombinedFilter>
//     }

//     topBarMenu =    <TopbarMenu 
//                         showFilterIcon={filterState.filters !== undefined && Array.isArray(filterState.filters)}
//                         onListTypeClick={listTypeClickHandler}
//                         onFilterClick={filterClickHandler}
//                         onFullScreenClick={props.toggleFullScreen}
//                         displayGridView={buttonState.viewGrid}
//                         showCardIcon={false}
//                         buttons={visibleButtons}
//                         language={props.language}
//                         clickButton={buttonClickHandler}>
//                     </TopbarMenu>

//     displayType =
//         <ListView 
//             type={listTypeState}
//             // cardFieldNames={cardFieldNames}
//             columns={columns}
//             listData={listData}
//             menuItems={menuItems}
//             // itemSelected={itemSelected}
//             selectedRecord={selectedIndex}
//             itemInvokedHandler={itemInvokedHandler}
//             selectionChanged={selectionChangeHandler}
//             updateSortedColumn={updateSortedColumn}
//             basic={false}
//             selectionModel={selectionState}
//             multiSelect={true}
//             isDataLoaded={isDataLoaded}
//             refresh={listViewRefresh}>
//         </ListView>

//     return (
//         <div className='multilist-content'>
//             <div className={listContentCss}>
//                 <div>
//                     {!props.showFullScreen ? (
//                         <div>
//                             {title}
//                             {headerText}
//                         </div>
//                     ) : (null)}
//                     {topBarMenu}
//                     {filter}
//                     {headerDivider}
//                 </div>
//                 <div className="multilist-list">
//                     {displayType}
//                 </div>
//             </div>
//             {recordDisplay}
//             {reportDisplay}
//             <FormHandler startConfig={formStartConfig} filters={filterState.filters} refresh={refreshForm}></FormHandler>
//             <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
//         </div>
//     )
// };

// const mapStateToProps = state => {
//     return {
//         uievents: state.config.uievents,
//         forms: state.config.forms,
//         pages: state.config.pages,
//         lists: state.config.lists,
//         language: state.config.language,
//         showFullScreen: state.display.showFullScreen
//     };
// }

// const mapDispatchToProps = dispatch => {
//     return {
//         onFilterSelect: (value) => dispatch({type: actionTypes.SETFILTERPRESET, value: value})
//     }
// };

// export default connect(mapStateToProps, mapDispatchToProps)(MultiList);