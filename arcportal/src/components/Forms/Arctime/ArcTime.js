import React, { useState } from 'react';
import { MaskedTextField } from '@fluentui/react';

const ArcTime = (props) => {
    const [fieldValue, setFieldValue] = useState(
        props.config.value === null ? '' : props.config.value
    );
    const [lastConfiguredState, setLastConfiguredState] = useState(
        props.config.value === null ? '' : props.config.value
    );

    // Ensure that if the provisioned value changes, the field value is updated (props.config.value is often undefined on first render).
    let configuredValue = props.config.value === null ? '' : props.config.value;
    if (configuredValue !== lastConfiguredState) {
        setLastConfiguredState(props.config.value);
        setFieldValue(props.config.value);
    }

    // if (lastConfiguredState !== props.config.value) {
    //     setLastConfiguredState(props.config.value);
    //     setFieldValue(props.config.value);
    // }

    const valueChangeHandler = (event, value) => {
        setFieldValue(value);
    };

    const checkValue = (value) => {
        let save = NumberCheck(value.slice(0, 1), 0, 2);
        if (save) {
            save = NumberCheck(value.slice(0, 2), 0, 23);
        }
        if (save) {
            save = NumberCheck(value.slice(3, 1), 0, 5);
        }
        if (save) {
            save = NumberCheck(value.slice(3, 2), 0, 59);
        }

        return save;
    };

    const NumberCheck = (num, min, max) => {
        let retval = true;
        if (!isNaN(num)) {
            if (Number(num) < min || Number(num) > max) {
                retval = false;
            }
        }
        return retval;
    };

    const focusOutEventHandler = () => {
        if (fieldValue !== undefined && checkValue(fieldValue)) {
            props.changeHandler(props.config.Id, fieldValue);
        } else {
            setFieldValue(undefined);
        }
    };

    return (
        <MaskedTextField
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
            mask="99:99"
            disabled={props.config.ReadOnly === true}
        />
    );
};

export default ArcTime;
