import React, {useEffect, useState, useRef} from 'react';
import { ComboBox } from '@fluentui/react';
import { connect } from 'react-redux';
import RemoveValueFromCommaSeparatedString from '../../../Utils/General/RemoveValueFromCommaSeparatedString';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { appendOtherOptionIfNeeded } from '../../../Utils/Forms/OtherOptionConstants';

/**
 * Fluent UI ComboBox wrapper for config-driven forms. Persisted values are option keys (ids), not translated labels.
 * `Options` may be an empty string from server config until dynamic lists load; non-array values are treated as no options.
 *
 * @param {Object} props
 * @param {Object} props.config - Field config (Id, Label, Options, value, MultiSelect, …).
 * @param {function(string, *, Object=): void} props.changeHandler
 * @param {function(KeyboardEvent): void} [props.onKeyDown]
 */
const ArcCombo = (props) => {

    const [multiSelectValue, setMultiSelectValue] = useState(null);

    let component = React.createRef();

    useEffect(() => {
        if (props.config.value && multiSelectValue === null) {
            newValue = props.config.value.toString();
            setMultiSelectValue(newValue);
        }
    }, [props.config.value]);

    const rawOptions = props.config.Options;
    const fieldForOptions = { ...props.config, Options: Array.isArray(rawOptions) ? [...rawOptions] : [] };
    appendOtherOptionIfNeeded(fieldForOptions, props.otherOptionParentIds);
    const options = Array.isArray(fieldForOptions.Options) ? fieldForOptions.Options : [];

    const optionsWithCustomStyling = options.map((option) => {
        const text = typeof option.text === 'string' && option.text.startsWith('@')
            ? TranslateTag(option.text, props.language)
            : option.text;
        return {
        key: option.key,
        text,
        ParentKey: option.ParentKey,
        styles: {
          optionText: {
            fontFamily: 'Calibri, Calibri_MSFontService, sans-serif',
            overflow: 'visible',
            whiteSpace: 'normal',
            divider: true
          },
        },
    }});

    const disabled = props.config.ReadOnly === true
        || (!props.config.Disabled || props.config.Disabled == undefined ? false : props.config.Disabled);

    const singleChangeHandler = (event, value) => {
        if (value === undefined) {
            props.changeHandler(props.config.Id, undefined);
        } else {
            if (value.selected === undefined) {
                props.changeHandler(props.config.Id, value.key, { parentKey: value.ParentKey} );
            } else {
                let newValue= [...props.config.value];
                if (value.selected) {
                    newValue = [...newValue,value.key];
                } else {
                    newValue = [...props.config.value];
                    newValue = newValue.filter(key => key !== value.key);
                }
                props.changeHandler(props.config.Id, newValue, { parentKey: value.ParentKey})
            }
        }
    }

    const multiChangeHandler = (_, value) => {
        if (!value) return;
        let newValue = multiSelectValue === null || multiSelectValue === undefined ? "" : multiSelectValue;
        if (value.selected) {
            newValue += newValue === "" ? value.key : "," + value.key;
        } else {
            newValue = RemoveValueFromCommaSeparatedString(newValue, value.key, ",");
        }
        setMultiSelectValue(newValue);
        props.changeHandler(props.config.Id, newValue, { parentKey: value.ParentKey} );
    }

    let newValue = null;
    if (props.config.value) {
        newValue = props.config.value.toString();
        newValue = props.config.MultiSelect ? newValue.split(",") : newValue;
    }

    const tabIndex = props.config.noTab === undefined || !props.config.noTab ? undefined : -1;

    const onInputChange = (text) => {
        if (!props.config.MultiSelect && (text === '' || text === undefined)) {
            props.changeHandler(props.config.Id, null);
        }
    };

    /**
     * Renders an option's text inside an element carrying a stable id built from the option key,
     * e.g. `ParentId-option-427`. Fluent's own option ids are positional, so this is what lets callers
     * address an option by its list item id rather than by its translated label.
     * @param {Object} option - The option being rendered.
     * @returns {JSX.Element}
     */
    const onRenderOption = (option) => (
        <span id={`${props.config.Id}-option-${option.key}`}>{option.text}</span>
    );

    return options.length > 0  ?
         (
            <ComboBox
                onKeyDown={props.onKeyDown}
                id={props.config.Id}
                required={props.config.Required}
                placeholder={props.config.Placeholder}
                label={props.config.Label}
                allowFreeform
                autoComplete="on"
                componentRef = {component}
                useComboBoxAsMenuWidth
                multiSelect = {props.config.MultiSelect}
                selectedKey = {newValue}
                options={optionsWithCustomStyling}
                onChange={props.config.MultiSelect ? multiChangeHandler : singleChangeHandler}
                onRenderOption={onRenderOption}
                onInputChange={onInputChange}
                tabIndex={tabIndex}
                disabled={disabled}
            />
        )
        : null;   
}

const mapStateToProps = (state) => ({
    language: state.config.language,
});

export default connect(mapStateToProps)(ArcCombo);
