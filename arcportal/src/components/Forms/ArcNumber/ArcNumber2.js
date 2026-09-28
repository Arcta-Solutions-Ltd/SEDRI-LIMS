import React, { useState } from 'react';
import { TextField } from '@fluentui/react';

const ArcNumber2 = (props) => {
    const [fieldValue, setFieldValue] = useState(
        props.config.value !== undefined ? props.config.value : ''
    );
    const [ignoreValidationEvent, setIgnoreValidationEvent] = useState(true);
    const [lastConfiguredState, setLastConfiguredState] = useState(
        props.config.value
    );

    if (lastConfiguredState !== props.config.value) {
        setLastConfiguredState(props.config.value);
        setFieldValue(props.config.value);
    }

    const valueChangeHandler = (event, value) => {
        if (!isNaN(value)) {
            setFieldValue(value);
        }
    };

    const focusOutEventHandler = (parm1, parm2) => {
        if (ignoreValidationEvent) {
            setIgnoreValidationEvent(false);
            return;
        }

        if (
            fieldValue !== undefined &&
            fieldValue !== null &&
            fieldValue !== ''
        ) {
            let number = parseFloat(fieldValue);
            let numberString = number.toString();
            let max = parseInt(props.config.Max);
            let min = parseInt(props.config.Min);
            let maxDPs = parseInt(props.config.MaxDPs);
            let components = numberString.split('.');
            let decimalPlaces =
                components.length < 2 ? 0 : components[1].length;
            if (decimalPlaces > maxDPs) {
                numberString = numberString.slice(
                    0,
                    numberString.length - (decimalPlaces - maxDPs)
                );
                number = parseFloat(numberString);
            }
            if (number > max) {
                number = max;
            } else if (number < min) {
                number = min;
            }
            setFieldValue(number);
            props.changeHandler(props.config.Id, number);

            if (props.focusOut !== undefined) {
                props.focusOut(props.config.Id, number);
            }
        } else {
            props.changeHandler(props.config.Id, '');
        }
    };

    return (
        <TextField
            id={props.config.Id}
            onKeyDown={props.onKeyDown}
            label={props.config.Label}
            autoComplete="off"
            required={props.config.Required}
            placeholder={props.config.Placeholder}
            onChange={valueChangeHandler}
            value={fieldValue}
            onNotifyValidationResult={focusOutEventHandler}
            validateOnFocusOut={true}
            tabIndex={
                props.config.TabIndex !== undefined ? props.config.TabIndex : 0
            }
            disabled={props.config.Disabled === true || props.config.ReadOnly === true}
            styles={props.config.styles !== undefined && props.config.styles !== null ? props.config.styles : {}}
        />
    );
};

export default ArcNumber2;
