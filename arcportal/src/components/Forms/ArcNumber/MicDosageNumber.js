import React, { useState } from 'react';
import { TextField } from '@fluentui/react';

/**
 * Fluent UI text field for MIC dosage input.
 * Accepts an optional leading comparison operator ({@code >}, {@code <=}) followed
 * by a numeric value with up to 4 integer digits (max 9999) and up to 11 decimal digits.
 * Invalid characters are silently filtered on every keystroke; the parent changeHandler is
 * only invoked on focus-out when the value has actually changed.
 *
 * @param {Object}   props                    - Component props.
 * @param {Object}   props.config             - Field configuration.
 * @param {string}   props.config.Id          - Unique field identifier used as the HTML id
 *   attribute and passed as the first argument to changeHandler.
 * @param {string}   [props.config.value]     - Controlled value supplied by the parent; the
 *   component re-syncs its local state whenever this changes.
 * @param {string}   [props.config.Label]     - Label rendered above the text field.
 * @param {boolean}  [props.config.Disabled]  - When {@code true} the field is read-only.
 * @param {number}   [props.config.TabIndex]  - Tab index forwarded to the underlying input.
 * @param {Function} props.changeHandler      - Callback invoked with {@code (id, value)} on
 *   focus-out when the value differs from the last confirmed state.
 */
const MicDosageNumber = (props) => {
    const [fieldValue, setFieldValue] = useState(
        props.config.value !== undefined ? props.config.value : ''
    );
    const [lastConfiguredState, setLastConfiguredState] = useState(
        props.config.value
    );

    if (lastConfiguredState !== props.config.value) {
        setLastConfiguredState(props.config.value);
        setFieldValue(props.config.value);
    }

    /**
     * Filters each keystroke to allow only valid MIC characters:
     * an optional leading {@code >} or {@code <=} operator; up to 4 integer digits;
     * an optional decimal point; and up to 11 decimal digits.
     * Characters that do not match these rules are silently discarded.
     *
     * @param {React.ChangeEvent<HTMLInputElement>} event - The native change event (unused).
     * @param {string} value - The full current string value of the input after the keystroke.
     */
    const valueChangeHandler = (event, value) => {
        let finalValue = '';
        let finalNumber = '';
        let finalDecimal = '';
        let decimalAdded = false;
        let characterNumber = 1;
        for (const str of value) {
            let canUse = false;

            if (!decimalAdded) {
                canUse = str >= '0' && str <= '9' && finalNumber.length < 4;
                if (canUse) {
                    finalNumber += str;
                }
                canUse =
                    canUse ||
                    ((str === '<' || str === '>') && characterNumber === 1);
                canUse =
                    canUse ||
                    (characterNumber === 2 && finalValue + str === '<=');
                if (str === '.' && !decimalAdded) {
                    canUse = true;
                    decimalAdded = true;
                }
            } else {
                canUse = str >= '0' && str <= '9' && finalDecimal.length < 11;
                if (canUse) {
                    finalDecimal += str;
                }
            }

            if (canUse) {
                finalValue += str;
            }
            characterNumber++;
        }

        setFieldValue(finalValue);
    };

    /**
     * Fires when the field loses focus. If the current local value differs from the last
     * value acknowledged by the parent, the parent changeHandler is called and the local
     * display is reset to the parent-controlled value (the parent is expected to push the
     * rounded/validated value back via {@code props.config.value}).
     */
    const focusOutEventHandler = () => {
        if (fieldValue !== lastConfiguredState) {
            props.changeHandler(props.config.Id, fieldValue);
            setFieldValue(lastConfiguredState);
        }
    };

    return (
        <TextField
            id={props.config.Id}
            label={props.config.Label ?? ''}
            autoComplete="off"
            onChange={valueChangeHandler}
            value={fieldValue}
            validateOnFocusOut={true}
            onNotifyValidationResult={focusOutEventHandler}
            disabled={props.config.Disabled === true}
            tabIndex={
                props.config.TabIndex !== undefined ? props.config.TabIndex : 0
            }
        />
    );
};

export default MicDosageNumber;
