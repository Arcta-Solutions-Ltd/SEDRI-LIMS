import React, { useRef, useEffect } from 'react';
import { connect } from 'react-redux';
import ArcHierarchyPicker from '../../../Forms/ArcHierarchyPicker/ArcHierarchyPicker';

/**
 * Wraps ArcHierarchyPicker for use in Filter component.
 * Adapts valueChangeHandler to the filter click API (key, selected) per item.
 * No label is shown for filter dropdowns.
 * In-place add is disabled by default (AllowAdd defaults to false).
 */
const FilterHierarchyPicker = (props) => {
    const prevValueRef = useRef(null);

    const filter = props.filter || {};
    const filterKey = filter.Key || filter.key;
    const values = filter.values || filter.Values;
    const valueStr = Array.isArray(values) ? values.join(',') : (values || '');

    const config = {
        Id: filter.Id || filter.id || filterKey,
        Label: '',
        Options: filter.Options || filter.options || [],
        OptionsName: filter.OptionsName || filter.optionsName,
        value: valueStr,
        MultiSelect: filter.MultiSelect === true || filter.multiSelect === true,
        Placeholder: filter.PlaceHolder || filter.placeholder,
        Required: filter.Required || filter.required,
        Width: filter.Width ?? filter.width,
        DropDownWidth: filter.DropDownWidth ?? filter.dropdownwidth,
        AllowAdd: filter.AllowAdd ?? filter.allowAdd ?? false,
        AddFormUIEvent: filter.AddFormUIEvent || filter.addFormUIEvent,
    };

    const valueChangeHandler = (id, newValue) => {
        const prevStr = prevValueRef.current ?? '';
        const prevSet = new Set(prevStr ? prevStr.split(',').map(s => s.trim()).filter(Boolean) : []);
        const newSet = new Set(newValue ? String(newValue).split(',').map(s => s.trim()).filter(Boolean) : []);

        const added = [...newSet].filter(k => !prevSet.has(k));
        const removed = [...prevSet].filter(k => !newSet.has(k));

        added.forEach(key => props.click(null, { key, selected: true }, filterKey));
        removed.forEach(key => props.click(null, { key, selected: false }, filterKey));

        prevValueRef.current = newValue || '';
    };

    useEffect(() => {
        prevValueRef.current = valueStr;
    }, [valueStr]);

    return (
        <div id={config.Id} className="filter-hierarchy-picker">
            <ArcHierarchyPicker
                config={config}
                valueChangeHandler={valueChangeHandler}
                uievents={props.uievents}
                forms={props.forms}
                language={props.language}
                view="filter"
                onOptionsRefreshed={props.onOptionsRefreshed}
            />
        </div>
    );
};

const mapStateToProps = (state) => ({
    uievents: state.config.uievents,
    forms: state.config.forms,
    language: state.config.language,
});

export default connect(mapStateToProps)(FilterHierarchyPicker);
