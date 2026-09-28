import React, { useEffect, useRef, useState } from 'react';
import ParseDate from './ParseDate';
import ParseLimit from './ParseLimit';
import { DatePicker, Label } from '@fluentui/react';
import languageService from '../../../services/LanguageService';
import parseLocalDate from '../../../Utils/General/ParseLocalDate';

const ArcDate = (props) => {

    const [fieldValue, setFieldValue] = useState(props.config.value);
    const [minValue] = useState(ParseLimit(props.config.Min));
    const [maxValue] = useState(ParseLimit(props.config.Max));
    const lastCommittedIsoRef = useRef(undefined);

    useEffect(() => {
        const externalValue = props.config.value;
        if (externalValue === undefined || externalValue === null || externalValue === '') {
            setFieldValue(externalValue);
            return;
        }
        const parsed = parseLocalDate(externalValue);
        if (parsed !== undefined && !isNaN(parsed.getTime())) {
            setFieldValue(parsed);
        }
    }, [props.config.value]);

    const formatAsISO = (d) => {
        const z = (n) => {return (n<10?'0':'')+n}

        if (d !== null && d !== undefined) {
            return d.getFullYear() + '-' + z(d.getMonth()+1) + '-' +
            z(d.getDate());
        } else {
            return d;
        }
    }

    /**
     * Commits a parsed date to form state and triggers dependent default resolution.
     * Skips when the ISO value matches the last committed value to avoid duplicate updates.
     * @param {Date|null|undefined} value
     */
    const commitDateValue = (value) => {
        const valueAsDate = formatAsISO(value);
        if (valueAsDate === lastCommittedIsoRef.current) {
            return;
        }
        lastCommittedIsoRef.current = valueAsDate;
        setFieldValue(value);
        props.changeHandler(props.config.Id, valueAsDate);
        focusOutEventHandler(value);
    }

    const valueChangeHandler = (value) => {
        commitDateValue(value);
    }

    const onParseDateFromString = React.useCallback(
        (newValue) => {
            let parsedDate = ParseDate(newValue);
            if ((maxValue !== null && parsedDate > maxValue) || (minValue !== null && parsedDate < minValue)) {
                return "";
            }
            return parsedDate;
        },
        [maxValue, minValue]
    );

    const focusOutEventHandler = (value) => {
        if (props.focusOut !== undefined) {
            const valueAsDate = formatAsISO(value)
            props.focusOut(props.config.Id, valueAsDate);
        }
    }

    /**
     * Commits keyboard-entered text when the DatePicker input loses focus.
     * @param {React.FocusEvent<HTMLInputElement>} event
     */
    const blurHandler = (event) => {
        const inputValue = event?.target?.value;
        if (inputValue === undefined || inputValue === null || inputValue.trim() === '') {
            return;
        }
        const parsedDate = onParseDateFromString(inputValue);
        if (parsedDate === '' || parsedDate === undefined || parsedDate === null || isNaN(parsedDate.getTime?.())) {
            return;
        }
        commitDateValue(parsedDate);
    }

    let min = (minValue === undefined) ? ParseLimit(props.config.Min) : minValue;
    let max = (maxValue === undefined) ? ParseLimit(props.config.Max) : maxValue;
    const parsedDisplayDate = fieldValue === undefined || fieldValue === null || fieldValue === ''
        ? undefined
        : parseLocalDate(fieldValue);
    const dateToDisplay = parsedDisplayDate !== undefined && !isNaN(parsedDisplayDate.getTime())
        ? parsedDisplayDate
        : undefined;
    const fieldLabel = props.config.Label ?? '';
    
    // Get localized strings for the DatePicker
    const dayPickerStrings = languageService.getDayPickerStrings();

    return (
        <>
            {fieldLabel !== '' && (
                <Label
                    id={`${props.config.Id}-label`}
                    required={props.config.Required}
                    htmlFor={props.config.Id}
                >
                    {fieldLabel}
                </Label>
            )}
            <DatePicker
                id={props.config.Id}
                ariaLabel={fieldLabel}
                onKeyDown={props.onKeyDown}
            placeholder={props.config.Placeholder}
            allowTextInput={true}
            onSelectDate={valueChangeHandler}
            value={dateToDisplay}
            parseDateFromString={onParseDateFromString}
            minDate={min}
            maxDate={max}
            strings={dayPickerStrings}
            disabled={props.config.ReadOnly === true}
            textField={{ onBlur: blurHandler }}
        />
        </>
    )
}

export default ArcDate;
