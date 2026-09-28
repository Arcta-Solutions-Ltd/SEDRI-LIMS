import React from 'react';
import { SpinButton } from '@fluentui/react/lib/SpinButton';

const ArcNumber = (props) => {
    
    const getNumericPart = (value) => {
        const valueRegex = /^(\d+(\.\d+)?).*/;
        if (valueRegex.test(value)) {
        const numericValue = Number(value.replace(valueRegex, '$1'));
        return isNaN(numericValue) ? undefined : numericValue;
        }
        return undefined;
    };
  
    const onIncrement = (value) => {
        const numericValue = getNumericPart(value.toString()); // Convert to string to prevent crash when value is a number.
        if (numericValue !== undefined) {
            const value = String(Math.min(numericValue + Number(props.config.Step), props.config.Max));
            props.valueChangeHandler(null, value);
            if (props.config.Suffix === undefined) {
                return value;
            } else {
                return value + " " + props.config.Suffix;
            }
        }
    };
  
    const onDecrement = (value) => {
        const numericValue = getNumericPart(value);
        if (numericValue !== undefined) {
            const value = String(Math.max(numericValue - Number(props.config.Step), props.config.Min));
            props.valueChangeHandler(null, value);
            if (props.config.Suffix === undefined) {
                return value;
            } else {
                return value + " " + props.config.Suffix;
            }
        }
    };
  
    const onValidate = (value) => {
        let numericValue = getNumericPart(value);
        if (numericValue !== undefined) {
            numericValue = roundUp(numericValue);
            numericValue = Math.min(numericValue, props.config.Max);
            numericValue = Math.max(numericValue, props.config.Min);
            if (props.config.Suffix === undefined) {
                return String(numericValue);
            } else {
                return String(numericValue) + " " + props.config.Suffix;
            }
        }
    };

    const focusOutEventHandler = () => {
        const element = document.getElementById(props.config.Id);
        const value = onValidate(element.children[0].value);
        if (value !== undefined && value !== null) {
            let numericValue = getNumericPart(value);
            props.valueChangeHandler(null, numericValue);
        }
    }

    function roundUp(num) {
        const decimalPos = props.config.Step.indexOf(".");
        let precision = decimalPos === -1 ? 0 : props.config.Step.length - props.config.Step.indexOf(".") - 1;
        precision = Math.pow(10, precision)
        return Math.ceil(num * precision) / precision
    }

    const newValue = props.config.value === undefined || props.config.value === null ? "0" : props.config.value.toString(); // Control seems to need string values.
    const defaultValue = props.config.Suffix === undefined || props.config.Suffix === "" || props.config.Suffix === null ? newValue : newValue + " " + props.config.Suffix;


    return (
        <SpinButton
            id={props.config.Id}
            label={props.config.Label}
            labelPosition={0}
            onValidate={onValidate}
            onIncrement={onIncrement}
            onDecrement={onDecrement}
            min={props.config.Min}
            max={props.config.Max}
            step={props.config.Step}
            value={defaultValue}
            precision={0}
            onBlur={focusOutEventHandler}
        />
    )
}

export default ArcNumber;

