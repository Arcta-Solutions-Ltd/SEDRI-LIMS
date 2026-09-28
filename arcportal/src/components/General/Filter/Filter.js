import React from 'react';
import { connect } from 'react-redux';
import './Filter.css'
import FilterByKeyword from '../FilterByKeyword/FilterByKeyword';
import FilterDropdown from './FilterDropdown/FilterDropdown';
import FilterByDate from './FilterByDate/FilterByDate';
import { IconButton, TooltipHost } from '@fluentui/react';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import FilterPicker from './FilterPicker/FilterPicker';
import FilterHierarchyPicker from './FilterHierarchyPicker/FilterHierarchyPicker';
import FilterNumberRanges from './FilterNumber/FilterNumberRanges';

/**
 * Manage-list filter bar. Renders keyword search, date range, dropdown/picker filters,
 * and refresh/clear actions only when enabled by view config props (FilterSearch, DateSearch,
 * Filters array, NumberRanges). Returns null when no filter controls are configured.
 */
const Filter = (props) => {

    const hasDropdownFilters = Array.isArray(props.filters) && props.filters.length > 0;
    const showSearch = props.filterSearch === true;
    const showDate = props.dateSearch != null && String(props.dateSearch).trim() !== '';
    const showNumberRange = props.numberRanges !== undefined && props.numberRanges !== "";
    const showRefresh = props.showRefresh !== undefined ? props.showRefresh : true;
    const showClear = props.showClear !== undefined ? props.showClear : true;

    const hasAnyControl = hasDropdownFilters || showSearch || showDate || showNumberRange;
    if (!hasAnyControl) {
        return null;
    }

    // Experimental hot-keys.
    document.onkeydown = function (e) {
        if (e.ctrlKey && e.key === 'Backspace' && showRefresh && props.refresh) {
            props.refresh();
        }
    };

    return (
        <div id="manageListFilterBar" className="filter-content">
            <div className="filter-search">
                <div className="filter-search-item">
                    {showSearch && <div className="filter-child">
                        <FilterByKeyword searchChangeHandler={props.searchChangeHandler} language={props.language} value={props.searchText}></FilterByKeyword>
                    </div>}
                    {showDate && <div className="filter-child">
                        <FilterByDate dateChangeHandler={props.dateChangeHandler} startDate={props.startDate} endDate={props.endDate} language={props.language} dateSearchLabel={props.dateSearchLabel}></FilterByDate>
                    </div>}
                </div>
                <div className="filter-search-item">
                    {showNumberRange && <div className="filter-child">
                        <FilterNumberRanges numberRanges={props.numberRanges} language={props.language} changeHandler={props.filterChangeHandler} filters={props.filters} ></FilterNumberRanges>
                    </div>}
                </div>
            </div>
            <div className="filter-dropdowns">
                {props.filters.map((filter) => {
                        const filterType = (filter.Type || filter.type || '').toLowerCase();
                        const isTagFilter = (filter.OptionsName || filter.optionsName || '').toLowerCase() === 'tag';
                        const useHierarchicalPicker = filterType === 'hierarchicalpicker' || (isTagFilter && filterType !== 'picker');
                        const filterItemKey = filter.Key || filter.key;
                        if (useHierarchicalPicker) {
                            return (
                                <div className="filter-item" key={filterItemKey}>
                                    <FilterHierarchyPicker filter={filter} click={props.click} language={props.language}></FilterHierarchyPicker>
                                </div>
                            )
                        } else if (filterType === "picker") {
                            return (
                                <div className="filter-item" key={filterItemKey}>
                                    <FilterPicker filter={filter} click={props.click} language={props.language}></FilterPicker>
                                </div>
                            )
                        } else {
                            return (
                                <div className="filter-item" key={filterItemKey}>
                                    <FilterDropdown filter={filter} click={props.click}></FilterDropdown>
                                </div>
                            )
                        }
                })}
                {showRefresh && (
                    <TooltipHost
                        content={TranslateTag("@GenRef@", props.language)}
                        id={901}
                    >
                        <IconButton
                            id="refreshFilter"
                            iconProps={{ iconName: 'Refresh' }}
                            onClick={props.refresh}
                        />
                    </TooltipHost>
                )}
                {showClear && (
                    <TooltipHost
                        content={TranslateTag("@GenCle@", props.language)}
                        id={902}
                    >
                        <IconButton
                            id="clearFilter"
                            iconProps={{ iconName: 'Cancel' }}
                            onClick={props.clear}
                        />
                    </TooltipHost>
                )}
            </div>
        </div>
    );
};

const mapStateToProps = state => {
    return {
        language: state.config.language
    };
}

export default connect(mapStateToProps)(Filter);
