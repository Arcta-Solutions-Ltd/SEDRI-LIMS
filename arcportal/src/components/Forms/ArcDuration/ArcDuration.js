import React, { useState, useEffect } from 'react';
import { TextField, Label } from '@fluentui/react';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import './ArcDuration.css';

/**
 * ArcDuration component for entering duration in days, hours, and minutes.
 * Used for TAT ranges. Outputs {baseId}Days, {baseId}Hours, {baseId}Minutes to the form data.
 * @param {Object} props.config - Field config with Id (used as base for output keys), Label, Required, Placeholder
 * @param {Function} props.changeHandler - Called with 'multiplechanges' and array of {key, value} for the three sub-fields
 * @param {Function} props.focusOut - Optional focus-out callback
 * @param {Function} props.onKeyDown - Optional key-down handler
 * @param {Object} props.data - Form data containing {baseId}Days, {baseId}Hours, {baseId}Minutes for initial values
 * @param {Object} props.language - Language/locale for translating d/h/m labels
 */
const ArcDuration = (props) => {
    const baseId = props.config.Id || 'Duration';
    const daysKey = baseId + 'Days';
    const hoursKey = baseId + 'Hours';
    const minutesKey = baseId + 'Minutes';

    const getVal = (data, key) => {
        if (!data) return '';
        const val = data[key] ?? data[key.toLowerCase()];
        return val !== undefined && val !== null && val !== '' ? Number(val) : '';
    };

    const [days, setDays] = useState(() => getVal(props.data, daysKey));
    const [hours, setHours] = useState(() => getVal(props.data, hoursKey));
    const [minutes, setMinutes] = useState(() => getVal(props.data, minutesKey));

    useEffect(() => {
        const data = props.data || {};
        const d = getVal(data, daysKey);
        const h = getVal(data, hoursKey);
        const m = getVal(data, minutesKey);
        if (d !== '' || h !== '' || m !== '') {
            setDays(d);
            setHours(h);
            setMinutes(m);
        }
    }, [props.data]);

    const notifyChanges = (d, h, m) => {
        if (props.changeHandler) {
            props.changeHandler('multiplechanges', [
                { key: daysKey, value: d },
                { key: hoursKey, value: h },
                { key: minutesKey, value: m }
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
        const vals = key === daysKey ? [newVal, hours, minutes] :
            key === hoursKey ? [days, newVal, minutes] : [days, hours, newVal];
        notifyChanges(vals[0], vals[1], vals[2]);
    };

    const createFocusOutHandler = (key) => () => {
        if (props.focusOut) {
            props.focusOut(key, key === daysKey ? days : key === hoursKey ? hours : minutes);
        }
    };

    const label = props.config.Label;
    const required = props.config.Required ? ' *' : '';

    const textFieldStyles = {
        root: { maxWidth: 80 },
        field: { textAlign: 'center' }
    };

    return (
        <div className="arcduration-container">
            <Label>{label}{required}</Label>
            <div className="arcduration-fields">
                <div className="arcduration-field-with-label">
                    <TextField
                        id={daysKey}
                        placeholder={TranslateTag("@AgeD@", props.language)}
                        type="number"
                        min={0}
                        max={999}
                        value={days}
                        onChange={(e, v) => createChangeHandler(setDays, daysKey, 0, 999)(e, v)}
                        onBlur={createFocusOutHandler(daysKey)}
                        onKeyDown={props.onKeyDown}
                        styles={textFieldStyles}
                    />
                    <span className="arcduration-unit-label">{TranslateTag("@AgeD@", props.language)}</span>
                </div>
                <div className="arcduration-field-with-label">
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
                    <span className="arcduration-unit-label">{TranslateTag("@AgeH@", props.language)}</span>
                </div>
                <div className="arcduration-field-with-label">
                    <TextField
                        id={minutesKey}
                        placeholder={TranslateTag("@AgeM@", props.language)}
                        type="number"
                        min={0}
                        max={59}
                        value={minutes}
                        onChange={(e, v) => createChangeHandler(setMinutes, minutesKey, 0, 59)(e, v)}
                        onBlur={createFocusOutHandler(minutesKey)}
                        onKeyDown={props.onKeyDown}
                        styles={textFieldStyles}
                    />
                    <span className="arcduration-unit-label">{TranslateTag("@AgeM@", props.language)}</span>
                </div>
            </div>
        </div>
    );
};

export default ArcDuration;
