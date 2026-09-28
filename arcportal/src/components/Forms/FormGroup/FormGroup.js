import React from 'react';
import './FormGroup.css';
import SingleLineField from'../SingleLineField/SingleLineField';
import {
    appendOtherOptionIfNeeded,
    collectParentFieldIdsWithOtherCompanion,
    getFormDataValue,
    isOtherDetailsFieldVisible,
} from '../../../Utils/Forms/OtherOptionConstants';

const FormGroup = (props) => {

    let fields = [];
    let first;

    if (typeof props.config.Fields[Symbol.iterator] === 'function') {
        for (const item of props.config.Fields) {
            fields.push(item);
        }
    }

    const parentIdsWithCompanion = collectParentFieldIdsWithOtherCompanion(
        props.allPages || props.pages || [{ Columns: [{ FormGroups: [{ Fields: fields }] }] }],
    );
    for (const field of fields) {
        const fieldType = (field.Type ?? field.type ?? '').toLowerCase();
        if (fieldType === 'dropdown' || fieldType === 'combobox' || fieldType === 'radio') {
            appendOtherOptionIfNeeded(field, parentIdsWithCompanion);
        }
    }

    for (const config of fields) {
        const otherDetailsFor = config.OtherDetailsFor ?? config.otherDetailsFor;
        if (!otherDetailsFor) {
            continue;
        }
        if (!isOtherDetailsFieldVisible(config, props.data, fields)) {
            const currentValue = getFormDataValue(props.data, config.Id ?? config.id);
            if (currentValue !== undefined && currentValue !== null && currentValue !== '') {
                props.changeHandler(config.Id ?? config.id, null);
            }
        }
    }

    return (
        <React.Fragment>
            {fields.map((config) => {
                if (!isOtherDetailsFieldVisible(config, props.data, fields)) {
                    return null;
                }
                if (first === undefined) {
                    first = config.Type === "text" ? undefined : true;
                } else {
                    first = false;
                }
                return (
                 <SingleLineField
                    key={config.Id}
                    config={config}
                    otherOptionParentIds={parentIdsWithCompanion}
                    changeHandler={props.changeHandler}
                    onKeyDown={props.onKeyDown}
                    focusOut={props.fieldFocusOut}
                    first={first}
                    language={props.language}
                    id={props.id}
                    lists={props.lists}
                    pages={props.pages}
                    allPages={props.allPages}
                    uievents={props.uievents}
                    forms={props.forms}
                    embeddedPages={props.embeddedPages}
                    data={props.data}
                ></SingleLineField>
             )}
             )}
        </React.Fragment>
    )
};
  
export default FormGroup;
