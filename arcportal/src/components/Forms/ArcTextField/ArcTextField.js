import React from 'react';
import { TextField, MaskedTextField } from '@fluentui/react';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { translateEmbeddedLanguageTags } from '../../../Utils/General/FormatAgeDisplay';

const resolveTaggedText = (text, language) => {
    if (typeof text === 'string' && text.startsWith('@')) {
        return TranslateTag(text, language);
    }
    return text;
};

const resolveDisplayValue = (value, language) => {
    if (typeof value !== 'string' || !value.includes('@')) {
        return value;
    }
    return translateEmbeddedLanguageTags(value, language);
};

const ArcTextField = (props) => {

    const valueChangeHandler = (event, value) => {

        const maxValue = isNaN(parseInt(props.config.Max)) ? 100000 : Number(props.config.Max);
        value = value.length > maxValue ? value.substring(0,maxValue) : value;
        value = value.replace(/[|'"]/g, '');
        value = value.replace(/\\/g, '');
        props.changeHandler(props.config.Id, value);
    }

    const focusOutEventHandler = () => {

        let value = props.config.value;
        if  ( props.config.Mask !== undefined && props.config.Mask !== "") {
            if (value !== undefined) {
                let lastChar = value[value.length -1];
                while (value.length > 0 && lastChar === "_") {
                    value = value.slice(0,-1);
                    lastChar = value[value.length -1];
                }
            }
        }
        props.changeHandler(props.config.Id, props.config.value);

        if (props.focusOut !== undefined) {
            props.focusOut(props.config.Id, props.config.value);
        }
    }

    const label = resolveTaggedText(props.config.Label, props.language);
    const placeholder = resolveTaggedText(props.config.Placeholder, props.language);
    const displayValue = resolveDisplayValue(props.config.value, props.language);

    let textField = "";
    if (props.config.Mask === undefined || props.config.Mask === "") {
        textField = <TextField
                        onKeyDown={props.onKeyDown}
                        id={props.config.Id || props.config.id}
                        label={label}
                        autoComplete="off"
                        required={props.config.Required}
                        placeholder={placeholder}
                        onChange={valueChangeHandler}
                        value={displayValue}
                        onNotifyValidationResult={focusOutEventHandler}
                        validateOnFocusOut={true}
                        disabled={props.config.ReadOnly === true}
                        tabIndex={props.config.TabIndex !== undefined ? props.config.TabIndex : 0}
                    />
    } else {
        const currentValue = displayValue === undefined ? displayValue : displayValue.toString();
        textField = <MaskedTextField
                        id={props.config.Id}
                        onKeyDown={props.onKeyDown}
                        label={label}
                        autoComplete="off"
                        required={props.config.Required}
                        placeholder={placeholder}
                        onChange={valueChangeHandler}
                        value={currentValue}
                        onNotifyValidationResult={focusOutEventHandler}
                        validateOnFocusOut={true}
                        mask={props.config.Mask}
                        maskChar=" "
                        disabled={props.config.ReadOnly === true}
                        tabIndex={props.config.TabIndex !== undefined ? props.config.TabIndex : 0}
                    />        
    }

    return (
        <React.Fragment>
            {textField}
        </React.Fragment>
    )
}

export default ArcTextField;
