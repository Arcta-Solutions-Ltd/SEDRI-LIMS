import React, {useState, useEffect} from 'react';
import { Dropdown } from '@fluentui/react';
import { connect } from 'react-redux';
import RemoveValueFromCommaSeparatedString from "../../../Utils/General/RemoveValueFromCommaSeparatedString";
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { appendOtherOptionIfNeeded } from '../../../Utils/Forms/OtherOptionConstants';

/**
 * Fluent UI Dropdown wrapper for config-driven forms. Persisted values are list item keys (ids), not translated labels.
 * Fields with `parentList` can be cleared to `null` when the parent changes (`PopulateDynamicLists`); this component
 * treats null/undefined like an empty selection, consistent with `AddValuesIntoPageStructure`.
 *
 * Inline option text that is a language tag (e.g. '@GenNon@') is translated against the active language so
 * fixed option sets (such as the expert-rule print-on-report Not set/Yes/No dropdown) localise correctly.
 * List-driven options carry already-translated data text and are left untouched.
 *
 * @param {Object} props
 * @param {Object} props.config - Field config (Id, Label, Options, value, MultiSelect, Resettable, …).
 * @param {Array} [props.language] - Active language tag/value pairs from redux config state.
 * @param {function(string, *, Object=): void} props.valueChangeHandler
 * @param {function(KeyboardEvent): void} [props.onKeyDown]
 */
const ArcDropdown = (props) => {

    const [multiSelectValue, setMultiSelectValue] = useState(null);

    const dropdownStyles = { dropdown: { width: 350 } };

    useEffect(() => {
        if (props.config.MultiSelect) {
            const newValue = props.config.value == null ? "" : props.config.value;
            setMultiSelectValue(newValue);
        }
    }, [props.config]);

    const dropdownChangeHandler = (event, value) => {
        if (event.type !== 'focus') {
            if (value.selected === undefined) {
                props.valueChangeHandler(props.config.Id, value.key, { parentKey: value.ParentKey} );
            } else {
                const base = props.config.value == null || !Array.isArray(props.config.value) ? [] : props.config.value;
                let newValue = [...base];
                if (value.selected) {
                    newValue = [...newValue,value.key];
                } else {
                    newValue = newValue.filter(key => key !== value.key);
                }
                props.valueChangeHandler(props.config.Id, newValue, { parentKey: value.ParentKey})
            }            
        }
    }

    const multiSelectChangeHandler = (event, value) => {
        let newValue = multiSelectValue === null || multiSelectValue === undefined ? "" : multiSelectValue;
        if (value.selected) {
            newValue += newValue === "" ? value.key : "," + value.key;
        } else {
            newValue = RemoveValueFromCommaSeparatedString(newValue, value.key, ",");
        }
        setMultiSelectValue(newValue);
        props.valueChangeHandler(props.config.Id, newValue, { parentKey: value.ParentKey} );
    }

    const label = props.config.Label;
    const disabled = props.config.ReadOnly === true
        || (!props.config.Disabled || props.config.Disabled == undefined ? false : props.config.Disabled);

    const raw = props.config.value;
    let newValue = null;
    if (props.config.MultiSelect) {
        if (raw == null || raw === '') {
            newValue = [];
        } else {
            const stringValue = String(raw);
            newValue = stringValue.split(",").filter((k) => k !== '');
        }
    } else if (raw == null) {
        newValue = '';
    } else {
        newValue = String(raw);
    }

    const tabIndex = props.config.noTab === undefined || !props.config.noTab ? 0 : -1;

    const fieldForOptions = { ...props.config, Options: Array.isArray(props.config.Options) ? [...props.config.Options] : [] };
    appendOtherOptionIfNeeded(fieldForOptions, props.otherOptionParentIds);

    const options = (fieldForOptions.Options || []).map((option) => {
        if (typeof option.text === 'string' && option.text.startsWith('@')) {
            return { ...option, text: TranslateTag(option.text, props.language) };
        }
        return option;
    });

    let fieldToDisplay = (null);
    if (options.length > 0) {

        if (props.config.MultiSelect) {
            fieldToDisplay = <Dropdown
                                onKeyDown={props.onKeyDown}
                                tabIndex={tabIndex}
                                id={props.config.Id}
                                required={props.config.Required}
                                placeholder={props.config.Placeholder}
                                label={label}
                                multiSelect={props.config.MultiSelect}
                                options={options}
                                onChange={multiSelectChangeHandler}
                                selectedKeys={newValue}
                                styles={dropdownStyles}
                                disabled={disabled}
                            />;
        } else if (props.config.Resettable) {
            fieldToDisplay = <Dropdown
                                onKeyDown={props.onKeyDown}
                                tabIndex={tabIndex}  
                                id={props.config.Id}
                                required={props.config.Required}
                                placeholder={props.config.Placeholder} 
                                label={label} 
                                multiSelect={props.config.MultiSelect}
                                options={options}
                                onChange={dropdownChangeHandler}
                                selectedKey={newValue}
                                disabled={disabled}
                            />;
        } else {
            fieldToDisplay = <Dropdown
                                onKeyDown={props.onKeyDown}
                                tabIndex={tabIndex}  
                                id={props.config.Id}
                                required={props.config.Required}
                                placeholder={props.config.Placeholder} 
                                label={label} 
                                multiSelect={props.config.MultiSelect}
                                options={options}
                                onChange={dropdownChangeHandler}
                                selectedKey={newValue}
                                disabled={disabled}
                            />;
        }
    }

    return (
        <React.Fragment>
            {fieldToDisplay}
        </React.Fragment>
    )
}

const mapStateToProps = (state) => {
    return {
        language: state.config.language,
    };
};

export default connect(mapStateToProps)(ArcDropdown)
