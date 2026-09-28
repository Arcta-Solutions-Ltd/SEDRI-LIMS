import React, { useState, useEffect } from 'react';
import { TextField, Label } from '@fluentui/react';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import './ArcAge.css';

/**
 * ArcAge component for entering age in years, months, days, and hours.
 * Used for specimen age at creation time. Outputs AgeYears, AgeMonths, AgeDays, AgeHours to the form data.
 * @param {Object} props.config - Field config with Id (used as base for output keys), Label, Required, Placeholder
 * @param {Function} props.changeHandler - Called with 'multiplechanges' and array of {key, value} for the four sub-fields
 * @param {Function} props.focusOut - Optional focus-out callback
 * @param {Function} props.onKeyDown - Optional key-down handler
 * @param {Object} props.data - Form data containing AgeYears, AgeMonths, AgeDays, AgeHours for initial values
 * @param {Object} props.language - Language/locale for translating y/m/d/h labels
 */
const ArcAge = (props) => {
    const baseId = props.config.Id || 'Age';
    const yearsKey = baseId + 'Years';
    const monthsKey = baseId + 'Months';
    const daysKey = baseId + 'Days';
    const hoursKey = baseId + 'Hours';

    const getVal = (data, key) => {
        if (!data) return '';
        const val = data[key] ?? data[key.toLowerCase()];
        return val !== undefined && val !== null && val !== '' ? Number(val) : '';
    };

    const [years, setYears] = useState(() => getVal(props.data, yearsKey));
    const [months, setMonths] = useState(() => getVal(props.data, monthsKey));
    const [days, setDays] = useState(() => getVal(props.data, daysKey));
    const [hours, setHours] = useState(() => getVal(props.data, hoursKey));

    useEffect(() => {
        const data = props.data || {};
        const y = getVal(data, yearsKey);
        const m = getVal(data, monthsKey);
        const d = getVal(data, daysKey);
        const h = getVal(data, hoursKey);
        if (y !== '' || m !== '' || d !== '' || h !== '') {
            setYears(y);
            setMonths(m);
            setDays(d);
            setHours(h);
        }
    }, [props.data]);

    const notifyChanges = (y, m, d, h) => {
        if (props.changeHandler) {
            props.changeHandler('multiplechanges', [
                { key: yearsKey, value: y },
                { key: monthsKey, value: m },
                { key: daysKey, value: d },
                { key: hoursKey, value: h }
            ]);
        }
    };

    const clamp = (val, min, max) => {
        const n = parseInt(val, 10);
        if (isNaN(n)) return '';
        if (min !== undefined && n < min) return min;
        if (max !== undefined && n > max) return max;
        return n;
    };

    const createChangeHandler = (setter, key, min, max) => (event, value) => {
        const newVal = value === '' ? '' : clamp(value, min, max);
        setter(newVal);
        const vals = key === yearsKey ? [newVal, months, days, hours] :
            key === monthsKey ? [years, newVal, days, hours] :
            key === daysKey ? [years, months, newVal, hours] : [years, months, days, newVal];
        notifyChanges(vals[0], vals[1], vals[2], vals[3]);
    };

    const createFocusOutHandler = (key) => () => {
        if (props.focusOut) {
            props.focusOut(key, key === yearsKey ? years : key === monthsKey ? months : key === daysKey ? days : hours);
        }
    };

    const label = props.config.Label;
    const required = props.config.Required ? ' *' : '';

    const textFieldStyles = {
        root: { maxWidth: 80 },
        field: { textAlign: 'center' }
    };

    return (
        <div className="arcage-container">
            <Label>{label}{required}</Label>
            <div className="arcage-fields">
                <div className="arcage-field-with-label">
                    <TextField
                        id={yearsKey}
                        placeholder={TranslateTag("@AgeY@", props.language)}
                        type="number"
                        min={0}
                        max={130}
                        value={years}
                        onChange={(e, v) => createChangeHandler(setYears, yearsKey, 0, 130)(e, v)}
                        onBlur={createFocusOutHandler(yearsKey)}
                        onKeyDown={props.onKeyDown}
                        styles={textFieldStyles}
                    />
                    <span className="arcage-unit-label">{TranslateTag("@AgeY@", props.language)}</span>
                </div>
                <div className="arcage-field-with-label">
                    <TextField
                        id={monthsKey}
                        placeholder={TranslateTag("@AgeM@", props.language)}
                        type="number"
                        min={0}
                        max={11}
                        value={months}
                        onChange={(e, v) => createChangeHandler(setMonths, monthsKey, 0, 11)(e, v)}
                        onBlur={createFocusOutHandler(monthsKey)}
                        onKeyDown={props.onKeyDown}
                        styles={textFieldStyles}
                    />
                    <span className="arcage-unit-label">{TranslateTag("@AgeM@", props.language)}</span>
                </div>
                <div className="arcage-field-with-label">
                    <TextField
                        id={daysKey}
                        placeholder={TranslateTag("@AgeD@", props.language)}
                        type="number"
                        min={0}
                        max={31}
                        value={days}
                        onChange={(e, v) => createChangeHandler(setDays, daysKey, 0, 31)(e, v)}
                        onBlur={createFocusOutHandler(daysKey)}
                        onKeyDown={props.onKeyDown}
                        styles={textFieldStyles}
                    />
                    <span className="arcage-unit-label">{TranslateTag("@AgeD@", props.language)}</span>
                </div>
                <div className="arcage-field-with-label">
                    <TextField
                        id={hoursKey}
                        placeholder={TranslateTag("@AgeH@", props.language)}
                        type="number"
                        min={0}
                        max={23}
                        value={hours}
                        onChange={(e, v) => createChangeHandler(setHours, hoursKey, 0, 23)(e, v)}
                        onBlur={createFocusOutHandler(hoursKey)}
                        onKeyDown={props.onKeyDown}
                        styles={textFieldStyles}
                    />
                    <span className="arcage-unit-label">{TranslateTag("@AgeH@", props.language)}</span>
                </div>
            </div>
        </div>
    );
};

export default ArcAge;
