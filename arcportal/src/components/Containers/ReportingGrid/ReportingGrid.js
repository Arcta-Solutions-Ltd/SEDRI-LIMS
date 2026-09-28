import React, {useEffect, useState, useRef} from 'react';
import * as actionTypes from '../../../store/actions';
import {connect} from 'react-redux';
import ArcReportGrid from '../../General/ArcReportGrid/ArcReportGrid';
import TopBarMenu from '../../General/TopbarMenu/TopBarMenu';
import MapButtons from '../../../Utils/Forms/MapButtonsToContextMenu';
import Post from '../../../Data/Post';
import PostEvent from '../../../Data/PostEvents';
import CombinedFilter from '../../General/Filter/CombinedFilter/CombinedFilter';
import { StringArraySorter } from '../../../Utils/General/ArraySorter';
import { ChangeFilterCondition, SelectPreset } from '../ManageList/Functions/FilterState';
import AddListsIntoFilters from '../../../Utils/Forms/AddListsIntoFilters';
import TranslateTag from '../../../Utils/Local/TranslateTag';

const ReportingGrid = (props) => {

    const [items, setItems] = useState([]);
    const [columns, setColumns] = useState([]);
    const [filterState, setFilterState] = useState({});
    const [gridData, setGridData] = useState([]);
    const [filterPresets, setFilterPresets] = useState([]);
    const prevViewNameRef = useRef(null);

    let filter = (null);

    useEffect(() => {
        const viewName = props.config?.Name;
        const viewChanged = prevViewNameRef.current !== viewName;
        prevViewNameRef.current = viewName;

        if (viewChanged) {
            let state = { filters: AddListsIntoFilters(props.config.Filters, props.lists) };
            state.filters[3].values = ['1182'];
            state.filters[2].values = ['684'];
            RunGridQuery(state, "1182");
        }

        setFilterPresets(props.config.FilterPresets || []);
    }, [props.config]);

    const RunOrganismListQuery = (state, data) => {
        const susceptibilityId =  state.filters[2].values.join(",");
        const criteria = { Name: "resistantorganismquery", Parameters: [{key: 'susceptibilityId', value: susceptibilityId}]};
        Post('query/filteredget', criteria, runOrganismListSuccessful, errorWhenRetrievingData, {state: state, data: data});
    }

    const RunAntibioticListQuery = (state, data) => {
        const susceptibilityId =  state.filters[2].values.join(",");
        const criteria = { Name: "resistantantibioticquery", Parameters: [{key: 'susceptibilityId', value: susceptibilityId}]};
        Post('query/filteredget', criteria, runAntibioticListSuccessful, errorWhenRetrievingData, {state: state, data: data});
    }

    const RunGridQuery = (state) => {
        const susceptibilityId =  state.filters[2].values === undefined ? "" :state.filters[2].values.join(",");
        const organisationId =  state.filters[4].values === undefined ? "" : state.filters[4].values.join(",");
        const locationId =  state.filters[5].values === undefined ? "" : state.filters[5].values.join(",");
        const parameters = [{key: 'susceptibilityid', value: susceptibilityId}, {key: 'organisationfilterid', value: organisationId},{key: 'locationid', value: locationId},
                            {key: 'startDate', value: state.startDate}, {key: 'endDate', value: state.endDate}]
        const criteria = { Name: "antibiogramquery", Parameters: parameters};
        Post('query/filteredget', criteria, dataRetrievedSuccessfully, errorWhenRetrievingData,state);
    }

    const runOrganismListSuccessful = (data, info) => {
        info.state.filters[1].Options = data.map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}});
        RunAntibioticListQuery(info.state, info.data);
    }
    const runAntibioticListSuccessful = (data, info) => {
        info.state.filters[0].Options = data.map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}});
        
        const type =  info.state.filters[3].values.join(",");
        const antibioticList = CreateListFromFilter(info.state.filters[0].values, info.state.filters[0].Options );
        const organismList = CreateListFromFilter(info.state.filters[1].values, info.state.filters[1].Options );
        refreshGrid(info.data,type, organismList, antibioticList);
        setGridData(info.data);
       
        setFilterState(info.state);
    }

    const GetOrganismList = (data) => {
        const organismList = [];
        for (const line of data) {
            const orgSearch = organismList.filter(o => o.toLowerCase() === line.OrganismName.toLowerCase() );
            if (orgSearch.length === 0) {
                organismList.push(line.OrganismName);
            }
        }
        return StringArraySorter(organismList);
    }

    const GetAntibioticList = (data) => {
        const antibioticList = [];
        for (const line of data) {
            const antSearch = antibioticList.filter(o => o.toLowerCase() === line.AntibioticName.toLowerCase() );
            if (antSearch.length === 0) {
                antibioticList.push(line.AntibioticName);
            }
        }
        return StringArraySorter(antibioticList);
    }

    const CreateColumnsDefinition = (antibioticList) => {
        const orgName = TranslateTag("@GenOrgA@", props.language);
        const columnList = [ { key: 'organism', name: orgName, fieldName: 'organism', minWidth: 140, maxWidth: 140 } ];
        let columnCount = 1;
        for (const antibiotic of antibioticList) {
            const newColumn = { key: 'num' + columnCount, name: antibiotic, fieldName: 'num' + columnCount,minWidth: 90, maxWidth: 90 };
            columnList.push(newColumn);
            columnCount++;
        }
        return columnList;
    }

    const CreateRowDefinition = (organismList, antibioticList, data, key) => {
        const rowList = [];
        let rowCount = 1;
        for (const organism of organismList) {
            const newRow = { key: rowCount, organism: organism};
            let columnCount = 1;
            for (const antibiotic of antibioticList) {
                const matchingDataRow = data.filter(d => antibiotic.toLowerCase() === d.AntibioticName.toLowerCase() && organism.toLowerCase() === d.OrganismName.toLowerCase());
                let value = "";
                if (matchingDataRow.length > 0) {
                    switch(key){
                        case '1181':
                            value = matchingDataRow[0].Percentage;
                            break;
                        case '1182':
                            value =  matchingDataRow[0].Resistant;
                            break;
                        case '1183':
                            value = matchingDataRow[0].Resistant + "/" + matchingDataRow[0].Total;
                            break;
                        case '1184':
                            value = matchingDataRow[0].Percentage + "/" + matchingDataRow[0].Total;
                            break;
                        default:
                            value = matchingDataRow[0].Percentage;
                    }
                }
                newRow['num' + columnCount] = value;
                columnCount++; 
            }
            rowList.push(newRow);
            rowCount++;
        }
        return rowList;
    }

    const dataRetrievedSuccessfully = (data, state) => {
        RunOrganismListQuery(state, data);
    }

    const refreshGrid = (data, key, organismList, antibioticList) => {
        if (organismList === undefined || organismList.length === 0) {
            organismList = GetOrganismList(data);           
        }
        if (antibioticList === undefined || antibioticList.length === 0) {
            antibioticList = GetAntibioticList(data);
        }
        setColumns(CreateColumnsDefinition(antibioticList));
        setItems(CreateRowDefinition(organismList, antibioticList, data, key));
    }

    const errorWhenRetrievingData = (data) => {

    }

    const GenerateCsv  = () => {

        let csv = [];
        let columnCount = 0;

        if (columns.length > 0) {
            let newLine = ""
            for (const column of columns) {
                newLine += newLine === "" ? column.name : "|" + column.name;
                columnCount++;
            }
            csv.push(newLine);

            if (items.length > 0) {
                let newLine = "";
                for (const item of items) {
                    newLine = item.organism;
                    for(let x = 1; x < columnCount; x++) {
                        newLine += "|" + item['num' + x];
                    }
                    csv.push(newLine);
                }
            }
        }
        return csv;

    }

    const downloadData = () => {            
        const csvData = GenerateCsv();
        let csv = "";
        for (const row of csvData) {
            let csvRow = '\"' + row.replace(/\|/g, '","') + '\"' ;
            csv += csvRow + '\n';
        }

        var hiddenElement = document.createElement('a');
        hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(csv);
        hiddenElement.target = '_blank';
        hiddenElement.download = 'export_run.csv';
        hiddenElement.click();
        //props.save(false);
    }

    const buttonClickHandler = () => {
        downloadData();
    }

    const changeRows = (newRows) => {
        setItems(newRows);
    }

    const changeColumns = (newColumns) => {
        setColumns(newColumns);
    }

    const CreateListFromFilter = (list, filterValues) => {
        const itemList = [];
        if (list !== undefined && list.length > 0 ) {
            for (const item of list) {
                const foundItem = filterValues.filter(v => v.key === item);
                if (foundItem.length > 0) {
                    itemList.push(foundItem[0].text);
                }
            }
        }
        return itemList
    }

    const filterDropDownHandler = (event, option, key) => {
        const state = ChangeFilterCondition(option, key, filterState);
        setFilterState(state);
        if (key !== "susceptibility" && key !== "location" && key !== "organisation") {
            const type = state.filters[3].values.join(",");
            const antibioticList = CreateListFromFilter(state.filters[0].values, state.filters[0].Options );
            const organismList = CreateListFromFilter(state.filters[1].values, state.filters[1].Options );
            refreshGrid(gridData,type, organismList, antibioticList);
        }
    }

    const dateChangeHandler = (key, newValue) => {
        const newState = {...filterState};
        if (key === "StartDate") {
            newState.startDate = newValue;
        } else {
            newState.endDate = newValue;
        }
        setFilterState(newState);
    }

    const clearFilter = () => {
        const filters = filterState.filters.map((f) => {
            const returnValue = f.Key === "susceptibility" || f.Key === "type" ?  { ...f} : { ...f,  values: [] };
            return ( returnValue );
        });
        filterState.startDate = undefined;
        filterState.endDate = undefined;
        const state = {...filterState, filters: filters};

        setFilterState(state);
        return state;
    }

    const handleRefreshButton = () => {
        RunGridQuery(filterState);
    }

    const filterPresetClickHandler = (preset) => {
        const state = SelectPreset(preset, filterState);
        setFilterState(state);
        RunGridQuery(state);
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

    const buttons = MapButtons(props.config.Buttons);
    const topBarMenu = <TopBarMenu 
        showFilterIcon={false}
        onFullScreenClick={props.toggleFullScreen}
        showCardIcon={false}
        buttons={buttons}
        language={props.language}
        listType="grid"
        clickButton={buttonClickHandler}>
    </TopBarMenu>

    if (filterState.filters !== undefined && Array.isArray(filterState.filters)) {
        filter = <CombinedFilter
                    viewConfig={props.config}
                    config={filterState}
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
                    dateChangeHandler={dateChangeHandler}
                    filterSearch={props.config.FilterSearch}
                    startDate={filterState.startDate}
                    endDate={filterState.endDate}
                    dateSearch={props.config.DateSearch}>
                </CombinedFilter>
    }

    var display = <div>
                    {topBarMenu}
                    {filter}
                    <ArcReportGrid items={items} columns={columns} onChangeRows={changeRows} onChangeColumns={changeColumns}></ArcReportGrid>
                    </div>

    return (
        <div>
            {display}
        </div>
    );
}

const mapStateToProps = state => {
    return {
        language: state.config.language,
        lists: state.config.lists,
        views: state.config.views
    };
}

const mapDispatchToProps = dispatch => {
    return {
        onFilterSelect: (value) => dispatch({type: actionTypes.SETFILTERPRESET, value: value}),
        onUpdateFilterPresets: (viewName, filterPresets) =>
            dispatch({ type: actionTypes.UPDATE_VIEW_FILTER_PRESETS, viewName, filterPresets }),
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(ReportingGrid);