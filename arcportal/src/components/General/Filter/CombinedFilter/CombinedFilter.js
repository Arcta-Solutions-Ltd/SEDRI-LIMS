import React from 'react';
import Filter from '../Filter';
import FilterPresets from '../FilterPresets/FilterPresets';
import { hasListViewFilterControls } from '../../../../Utils/Forms/ListViewFilterUtils';

/**
 * Wraps the manage-list filter bar and optional preset bar for a list view.
 * Renders nothing when the view config defines no filter controls and no applicable presets.
 */
const CombinedFilter = (props) => {

    let filter = (null);
    let filterPresets = (null);

    const viewConfig = props.viewConfig;
    const showFilterControls = hasListViewFilterControls(viewConfig);
    const presets = Array.isArray(props.filterPresets) ? props.filterPresets : [];

    if (showFilterControls && props.config.filters !== undefined && Array.isArray(props.config.filters)) {
        filter = <Filter 
                    filters={props.config.filters} 
                    clear={props.onClear} 
                    refresh={props.refresh} 
                    click={props.filterDropDownHandler} 
                    language={props.language}
                    searchText={props.searchText}
                    startDate={props.startDate}
                    endDate={props.endDate}
                    searchChangeHandler={props.searchChangeHandler}
                    dateChangeHandler={props.dateChangeHandler}
                    filterChangeHandler={props.filterChangeHandler}
                    filterSearch={props.filterSearch}
                    numberRanges={props.numberRanges}
                    dateSearch={props.dateSearch}
                    dateSearchLabel={props.dateSearchLabel}
                    showRefresh={props.showRefresh}
                    showClear={props.showClear}
                    >
                </Filter>
    }

    if (showFilterControls && (presets.length > 0 || props.onSavePreset != null)) {
        filterPresets = <FilterPresets
            filterPresets={presets}
            selectedPreset={props.config.selectedPreset}
            click={props.onPresetClick}
            language={props.language}
            onSavePreset={props.onSavePreset}
            onRemovePreset={props.onRemovePreset}
            viewName={props.view}
            filterState={props.config}
        />
    }

    if (!filter && !filterPresets) {
        return null;
    }

    return (
        <div>
            {filter}
            {filterPresets}
        </div>
    );
}

export default CombinedFilter;
