import React, {useState, useEffect, useMemo, useRef} from 'react';
import {connect} from 'react-redux';
import './Graph.css';
import TopBarMenu from '../../General/TopbarMenu/TopBarMenu';
import MapButtons from '../../../Utils/Forms/MapButtons';
import CombinedFilter from '../../General/Filter/CombinedFilter/CombinedFilter';
import { ChangeFilterCondition, SelectPreset, SetDefaultPreset } from '../ManageList/Functions/FilterState';
import AddListsIntoFilters from '../../../Utils/Forms/AddListsIntoFilters';
import GraphFactory from './GraphFactory';
import FormatDataIntoGraph from './GraphUtils';
import { GRAPH_BACKGROUND_COLORS } from './graphBackgroundPalette';
import { chartPropsFromAnalyticsFilters } from './graphGraphtypeMapping';
import { hasListViewFilterControls } from '../../../Utils/Forms/ListViewFilterUtils';
import * as actionTypes from '../../../store/actions';
import Post from '../../../Data/Post';
import PostEvent from '../../../Data/PostEvents';
import TranslateTag from '../../../Utils/Local/TranslateTag';

/**
 * Returns filter presets for the graph matching the given view name from the Redux graph list.
 * @param {Array<{ Name?: string, FilterPresets?: Array<Object> }>|undefined} graphs - Graph configs from Redux
 * @param {string|undefined} graphName - Active graph view name (e.g. specimentypesummarygraph)
 * @returns {Array<Object>} Presets for the active graph, or empty array
 */
const getActiveGraphPresets = (graphs, graphName) => {
    if (!graphName || !Array.isArray(graphs)) return [];
    const activeGraph = graphs.find((g) => g && g.Name === graphName);
    return activeGraph?.FilterPresets || [];
};

const Graph = (props) => {

    const [currentGraph, setCurrentGraph] = useState({});
    const [sourceData, setSourceData] = useState();
    const [filterState, setFilterState] = useState({});
    const [filterPresets, setFilterPresets] = useState([]);
    const [threedimensions, setThreeDimensions] = useState();
    const [twodimensions, setTwoDimensions] = useState();

    const graphElement = useRef(null);

    const prevGraphNamesRef = useRef(null);

    useEffect(() => {
        const graph = props.graphs?.[0];
        const graphNames = (props.graphs || []).map((g) => g.Name).join(',');
        const graphsChanged = prevGraphNamesRef.current !== graphNames;
        prevGraphNamesRef.current = graphNames;

        if (graphsChanged && graph) {
            setCurrentGraph(graph);
            let filters = AddListsIntoFilters(graph.Filters, props.lists);
            setDefaultFilters(filters);
            let state = { filters };
            state = SetDefaultPreset({ ...graph, FilterPresets: graph.FilterPresets || [] }, state);
            setFilterState(state);
        }
    }, [props.config, props.graphs]);

    useEffect(() => {
        if (!currentGraph?.Name || !Array.isArray(props.graphs)) return;

        setCurrentGraph((prev) => props.graphs.find((g) => g && g.Name === prev?.Name) ?? prev);
        setFilterPresets(getActiveGraphPresets(props.graphs, currentGraph.Name));
    }, [props.graphs, currentGraph?.Name]);

    /**
     * Applies default graphtype (Bar Chart, id 478) and date interval (Month, id 480) to filter objects.
     * @param {Array<{ Key?: string, values?: string[] }>} filters - Filter definitions to mutate in place
     */
    const setDefaultFilters = (filters) => {
        for (const filter of filters) {
            if (filter.Key === "graphtype") { filter.values =  ['478']; }
            if (filter.Key === "dateinterval") { filter.values =  ['480']; }
        }
    }

    /**
     * Posts filter criteria to graph/getdata and updates chart dimensions on success.
     * @param {{ filters?: Array<{ FieldName?: string, values?: string[] }>, startDate?: string, endDate?: string }|undefined} currentFilterState - Filter state to use; defaults to component filterState
     */
    const GenerateData = (currentFilterState) => {

        currentFilterState = currentFilterState === undefined ? filterState : currentFilterState;
        const param = [];
        for (const filter of currentFilterState.filters) { 
            if (filter.values !== undefined) {
                const newQueryFilter = { Key: filter.FieldName, Value: filter.values.join()}
                param.push(newQueryFilter);               
            }
        }

        if (currentFilterState.startDate !== undefined) {
            const newQueryFilter = { Key: "StartDate", Value: currentFilterState.startDate}
            param.push(newQueryFilter);     
        }

        if (currentFilterState.endDate !== undefined) {
            const newQueryFilter = { Key: "EndDate", Value: currentFilterState.endDate}
            param.push(newQueryFilter);     
        }

        if (param.length > 0) {
            const criteria = { Name: currentGraph.Name, Parameters: param};
            Post('graph/getdata', criteria, dataRetrievedSuccessfully, errorWhenRetrievingData);
        }
    }

    const sectionContentsSort = (a, b) => {
        if (a.Value < b.Value) {
          return -1;
        }
        if (a.Value > b.Value) {
          return 1;
        }
        return 0;
      }

    const AddNewSection = (sections, newData, labelCount) => {

        newData.sort(sectionContentsSort);
        // Put in the data for existing sections
        for (const section of sections) {
             const newDataLine = newData.filter(d => d.Value === section.Value);
             if (newDataLine.length === 0) {
                section.Data.push(0);
             } else {
                section.Data.push(newDataLine[0].Number)
             }
        }

        //Put in any new sections which do not exist
        for (const newDataLine of newData) {
            const foundSection = sections.filter(d => d.Value === newDataLine.Value);
            if (foundSection.length === 0) {
                var data = [];
                for (var i = 1; i < labelCount; i++) {
                    data.push(0);
                }
                data.push(newDataLine.Number);

                const newSection = {Key: sections.length + 1, Value: newDataLine.Value, Data: data};
                sections.push(newSection);
            }
        }

        return sections;
    }

    const CreateTwoDimensionsFromThreeDimensions = (three) => {
        const labels = [];
        const sections =  [{
            Value: "",
            Data: []
        }];
        for (const section of three.Sections) {
            labels.push({Key: section.Key, Value: section.Value});
            const total = section.Data.reduce((p, a) => p + a, 0);
            sections[0].Data.push(total);
        }
        const twoDim = {Labels: labels, Sections: sections}
        setTwoDimensions(twoDim);
    }

    const ReorganiseData = (data) => {

        const returnedData = [];
        let filterResults = [];
        const intervalFilter = filterState.filters.filter(f => f.Key === "dateinterval")[0].values[0];

        for (const item of data) {
            if (intervalFilter === "479") {
                filterResults = returnedData.filter(d => d.Value === item.Value && d.Day === item.Day);
            }
            if (intervalFilter === "480") {
                filterResults = returnedData.filter(d => d.Value === item.Value && d.Month === item.Month);
            }
            if (intervalFilter === "481") {
                filterResults = returnedData.filter(d => d.Value === item.Value && d.Year === item.Year);
            }

            if (filterResults.length === 0) {
                returnedData.push(item);
            } else {
                filterResults[0].Number += item.Number;
            }
        }
        return returnedData;
    }

    const GenerateThreeDimensions = (data) => {
        let currentLabel = "";
        let labelKeyCount = 0;
        let sections = []
        const labels = [];
        let sectionDataForThisColumn = [];
        const newData = ReorganiseData(data);

        const dateInterval = filterState.filters.filter(t => t.Key === "dateinterval");
        for (const line of newData) { 
            const newLabel = dateInterval[0].values[0] === "480" ? line.MonthName.substring(0,3) : dateInterval[0].values[0] === "481" ? line.Year : line.Day;
            if (newLabel !== currentLabel) {
                if (sectionDataForThisColumn.length > 0) {
                    sections = AddNewSection(sections, sectionDataForThisColumn, labelKeyCount);
                }
                labelKeyCount++;
                labels.push({Key: labelKeyCount, Value: newLabel});
                currentLabel = newLabel;
                sectionDataForThisColumn = [];
            }
            sectionDataForThisColumn.push({Value: line.Value, Number: line.Number})
        }

        if (sectionDataForThisColumn.length > 0) {
            sections = AddNewSection(sections, sectionDataForThisColumn, labelKeyCount);
        }
        setThreeDimensions({Labels: labels, Sections: sections});
        return {Labels: labels, Sections: sections};
    }

    const dataRetrievedSuccessfully = (data) => {
        const three = GenerateThreeDimensions(data);
        CreateTwoDimensionsFromThreeDimensions(three);
        setSourceData(data);
    }

    const errorWhenRetrievingData = (data) => {
        let x = 1;
    }

    /**
     * Re-fetches graph data using the current filter state.
     */
    const handleRefreshButton = () => {
        GenerateData();
    }

    const filterPresetClickHandler = (preset) => {
        const state = SelectPreset(preset, filterState);
        setFilterState(state);
        GenerateData(state);
    }

    /**
     * Saves a new filter preset to the database and updates the Redux config store.
     * @param {Object} newPreset - The preset to add (Key, Name, Default, Fields)
     */
    const saveFilterPresetHandler = (newPreset) => {
        const existing = getActiveGraphPresets(props.graphs, currentGraph.Name);
        const newPresets = [...existing, newPreset];
        setFilterPresets(newPresets);
        const payload = { Event: 'savefilterpresetsevent', Id: '0', ViewName: currentGraph.Name, FilterPresets: newPresets };
        PostEvent(payload, () => props.onUpdateFilterPresets(currentGraph.Name, newPresets), errorWhenRetrievingData);
    }

    /**
     * Removes a filter preset from the database and updates the Redux config store.
     * @param {Object} presetToRemove - The preset to remove
     */
    const removeFilterPresetHandler = (presetToRemove) => {
        const existing = getActiveGraphPresets(props.graphs, currentGraph.Name);
        const newPresets = existing.filter((p) => p.Key !== presetToRemove.Key);
        setFilterPresets(newPresets);
        const payload = { Event: 'savefilterpresetsevent', Id: '0', ViewName: currentGraph.Name, FilterPresets: newPresets };
        PostEvent(payload, () => props.onUpdateFilterPresets(currentGraph.Name, newPresets), errorWhenRetrievingData);
    }

    let filter = (null);

    const filterClickHandler = () => {
        GenerateData();
    } 

    /**
     * Handles top-bar graph menu actions: switch graph, export data, or export chart image.
     * Switching graphs resets filter values (including graphtype default); chart type is derived from filters at render time.
     * @param {{ key?: string, UIEvent?: string }} button - Menu button configuration
     */
    const menuClickHandler = (button) => {
        if (button.key === "exportdata") {
            if (sourceData !== undefined) {
                downloadData();
            }
        } else {
            if (button.key === "exportgraphimage") {
                if (sourceData !== undefined) {
                    exportGraphImage();
                }
            } else {
                const action = props.uievents.filter(a => a.Name === button.UIEvent);
                var newGraph = props.graphs.filter(a => a.Name === action[0].Action);
                const graphToSwitch = newGraph[0];
                setCurrentGraph(graphToSwitch);
                setFilterPresets(graphToSwitch.FilterPresets || []);
                let filters = AddListsIntoFilters(graphToSwitch.Filters, props.lists);
                filters = filters.map((f) => {
                    return ( { ...f, values: [] });
                });;
                setDefaultFilters(filters);
                let state = { filters };
                state = SetDefaultPreset({ ...graphToSwitch, FilterPresets: graphToSwitch.FilterPresets || [] }, state);
                setFilterState(state);
                setThreeDimensions(undefined);
                setTwoDimensions(undefined);
            }
        }
    } 

    const clearFilter = () => {
        const filters = filterState.filters.map((f) => {
            const returnValue = f.Key === "graphtype" || f.Key === "dateinterval" ?  { ...f} : { ...f,  values: [] };
            return ( returnValue );
        });;
        filterState.startDate = undefined;
        filterState.endDate = undefined;
        setFilterState({...filterState, filters: filters});
    }

    function fillCanvasBackgroundWithColor(canvas, color) {
        const context = canvas.getContext('2d');
        context.save();
        context.globalCompositeOperation = 'destination-over';
        context.fillStyle = color;
        context.fillRect(0, 0, canvas.width, canvas.height);
        context.restore();
      }

    const exportGraphImage = () => {
        const graph = graphElement.current;
        if (graph) {
            const canvasElement = graph.canvas;
            fillCanvasBackgroundWithColor(canvasElement, 'white')
            let canvas = canvasElement.toDataURL('image/jpeg');
            var hiddenElement = document.createElement('a');
            hiddenElement.href = canvas;
            hiddenElement.download = 'graphimage.jpg';
            document.body.appendChild(hiddenElement);
            hiddenElement.click();
        }
    }

    const dateChangeHandler = (key, newValue) => {
        let newState;
        if (key === "StartDate") {
            newState = {...filterState, startDate: newValue};
        } else {
            newState = {...filterState, endDate: newValue};
        }

        setFilterState(newState);
    }

    /**
     * Updates filter state when a dropdown value changes. Chart visualization follows filter graphtype id on render.
     * @param {object} event - Fluent UI dropdown event
     * @param {{ key?: string }} option - Selected list option (key is list item id)
     * @param {string} key - Filter key (e.g. graphtype)
     */
    const filterDropDownHandler = (event, option, key) => {
        const state = ChangeFilterCondition(option, key, filterState);
        setFilterState(state);
    }

    const OrganiseSourceDataForExport = (data) => {
        const dataForExport = [];
        const intervalFilter = filterState.filters.filter(f => f.Key === "dateinterval")[0].values[0];
        for (const row of sourceData) {
            let newItem = {Year: row.Year, Number: row.Number, Value: row.Value};
            if (intervalFilter === "480" || intervalFilter === "479") {
                newItem.Month = row.Month;
                newItem.MonthName = row.MonthName;
            }
            if (intervalFilter === "479") {
                newItem.Day = row.Day;
            }
            dataForExport.push(newItem);
        }        
        return dataForExport;
    }

    const GenerateCsv  = () => {

        const dataToExport = OrganiseSourceDataForExport(sourceData);
        let header = "";
        let csv = [];
        let firstRecord = true;
        if (Array.isArray(dataToExport)) {
            for (const row of dataToExport) {
                let line = "";
                for(const key in row) {
                    if (firstRecord) {
                        header += header === "" ?  key : "|" + key;
                    }
                    line += line === "" ?  row[key].toString().trim() : "|" + row[key].toString().trim();
                }
                if (firstRecord) {
                    csv.push(header);
                    firstRecord = false;
                }
                csv.push(line);
            }
            firstRecord = false;
        };
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
    }

    const buttons = MapButtons(props.config.Buttons);
    const topBarMenu = <TopBarMenu 
                            onFilterClick={filterClickHandler}
                            onFullScreenClick={props.toggleFullScreen}
                            buttons={buttons}
                            clickButton={menuClickHandler}>
                        </TopBarMenu>

    if (hasListViewFilterControls(currentGraph) && filterState.filters !== undefined && Array.isArray(filterState.filters)) {
        filter = <CombinedFilter
                    viewConfig={currentGraph}
                    config={filterState}
                    filterPresets={filterPresets}
                    view={currentGraph.Name}
                    filterDropDownHandler={filterDropDownHandler}
                    onPresetClick={filterPresetClickHandler}
                    onSavePreset={saveFilterPresetHandler}
                    onRemovePreset={removeFilterPresetHandler}
                    language={props.language}
                    refresh={handleRefreshButton}
                    onClear={clearFilter}
                    searchText={filterState.textSearch}
                    dateChangeHandler={dateChangeHandler}
                    filterSearch={false}
                    startDate={filterState.startDate}
                    endDate={filterState.endDate}
                    dateSearch={currentGraph.DateSearch ?? currentGraph.dateSearch}>
                </CombinedFilter>
    }

    const { type: vizType, stack: vizStack } = useMemo(
        () => chartPropsFromAnalyticsFilters(filterState, currentGraph),
        [filterState, currentGraph]
    );

    const graphData = useMemo(() => {
        if (!threedimensions || !twodimensions) return null;
        return FormatDataIntoGraph(threedimensions, twodimensions, GRAPH_BACKGROUND_COLORS, vizType);
    }, [threedimensions, twodimensions, vizType]);

    let graphToDisplay = (null);
    if (threedimensions !== undefined && twodimensions !== undefined) {
        if (sourceData !== undefined && sourceData.length > 0 && graphData) {
            graphToDisplay  = (<div className="graph-graph" data-chart-type={vizType}><GraphFactory ref={graphElement} data={graphData} text={currentGraph.Title} type={vizType} stack={vizStack}></GraphFactory></div>);
        } else {
            const noDataMessage = TranslateTag("@GraNo@", props.language);
            graphToDisplay = (<div class="app-crafted-fullscreencontent"><div class="graph-title">{noDataMessage}</div><div class="graph-message">{currentGraph.Message}</div></div>)
        }
    } else {
        graphToDisplay = (<div class="app-crafted-fullscreencontent"><div class="graph-title">{currentGraph.Title}</div><div class="graph-message">{currentGraph.Message}</div></div>)
    }

    return (
        <div>
            {!props.showFullScreen ? (
                <div>
                    <div className='managelist-title'>{props.config.Title}</div>
                    <div className='managelist-heading-text'>{props.config.HeaderText}</div>
                </div>
            ) : (null)}
            {topBarMenu}
            {filter}
            <div className="graph-container">
                {graphToDisplay}
            </div>
        </div>
    )
};

const mapStateToProps = state => {
    return {
        showFullScreen: state.display.showFullScreen,
        lists: state.config.lists,
        graphs: state.config.graphs,
        uievents: state.config.uievents,
        language: state.config.language
    };
}

const mapDispatchToProps = dispatch => {
    return {
        onUpdateFilterPresets: (viewName, filterPresets) =>
            dispatch({ type: actionTypes.UPDATE_VIEW_FILTER_PRESETS, viewName, filterPresets }),
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Graph);