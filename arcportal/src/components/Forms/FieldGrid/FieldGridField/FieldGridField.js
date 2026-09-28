import React from 'react';
import ArcDropdown from '../../ArcDropdown/ArcDropdown';
import ArcTextField from '../../ArcTextField/ArcTextField';
import ArcCombo from '../../ArcCombo/ArcCombo';
import ArcFilteredCombo from '../../ArcFilteredCombo/ArcFilteredCombo';
import ArcHierarchyPicker from '../../ArcHierarchyPicker/ArcHierarchyPicker';
import ArcNumber2 from '../../ArcNumber/ArcNumber2';
import { isGridColumnVisible, mapGridWidthToClass } from '../FieldGridLayoutUtils';
import './FieldGridField.css';
import ArcToggle from '../../ArcToggle/ArcToggle';
import { TextField } from '@fluentui/react';

const FieldGridField = (props) => {

    const mapTypeToField = (type) => {
        switch (type) {
            case 'dropdown':
                return (
                    <ArcDropdown
                        onKeyDown={props.onKeyDown}
                        config={props.config}
                        valueChangeHandler={fieldChangeHandler}
                    ></ArcDropdown>
                );
            case 'combobox':
                return (
                    <ArcCombo
                        onKeyDown={props.onKeyDown}
                        config={props.config}
                        changeHandler={fieldChangeHandler}
                    ></ArcCombo>
                );
            case 'filteredcombo':
                return (
                    <ArcFilteredCombo
                        config={props.config}
                        changeHandler={comboBoxChangeHandler}
                    ></ArcFilteredCombo>
                );
            case 'hierarchicalpicker':
                return (
                    <ArcHierarchyPicker
                        onKeyDown={props.onKeyDown}
                        config={props.config}
                        valueChangeHandler={(id, value) => props.changeHandler(id, value)}
                        uievents={props.uievents}
                        forms={props.forms}
                        language={props.language}
                        recordId={props.recordId}
                    ></ArcHierarchyPicker>
                );
            case 'number':
                return (
                    <ArcNumber2
                        onKeyDown={props.onKeyDown}
                        config={props.config}
                        changeHandler={fieldChangeHandler}
                    ></ArcNumber2>
                );
            case 'toggle':
                return (
                    <ArcToggle
                        onKeyDown={props.onKeyDown}
                        config={props.config}
                        valueChangeHandler={fieldChangeHandler}
                        showText={true}
                    ></ArcToggle>
                );
            case 'text':
                return (
                    <TextField
                        onKeyDown={props.onKeyDown}
                        id={props.config.Id}
                        label={props.config.Label}
                        disabled
                        value={props.config.value}
                        onChange={fieldChangeHandler}
                        readOnly={true}
                    ></TextField>
                );

            case 'colourswatch':
                const hexValue = props.config.value;
                const bgColor = (hexValue && /^#[0-9A-Fa-f]{6}$/.test(hexValue)) ? hexValue : '#cccccc';
                return (
                    <div
                        className="fieldgridfield-colourswatch"
                        style={{
                            backgroundColor: bgColor,
                            width: 24,
                            height: 24,
                            borderRadius: 4,
                            border: '1px solid #d1d1d1',
                        }}
                        title={hexValue || ''}
                    />
                );

            case 'hidden':
                break;
            default:
                return (
                    <ArcTextField
                        onKeyDown={props.onKeyDown}
                        config={props.config}
                        changeHandler={fieldChangeHandler}
                    ></ArcTextField>
                );
        }
    };

    const fieldChangeHandler = (_, value) => {
        props.changeHandler(props.config.FieldKey ?? props.config.Id, value);
    };

    const comboBoxChangeHandler = (_, value) => {
        if (value) props.changeHandler(props.config.FieldKey ?? props.config.Id, value.key);
    };

    const visible = isGridColumnVisible(props.config, props.data);

    return visible ? (
        <span className={mapGridWidthToClass(props.config.Width)}>
            {mapTypeToField(props.config.Type)}
        </span>
    ) : null;
};

export default FieldGridField;
