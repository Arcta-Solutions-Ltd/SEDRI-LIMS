import React, {useState, useEffect} from 'react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import FormColumn from '../../../Forms/FormColumn/FormColumn';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import {PostList} from '../../../../Data/Post';

/**
 * Interprets the Fixed flag, which arrives as "Yes"/"No" from the form and "True"/"False" from the database.
 * Fixed entries are system rows: they cannot be deleted and they do not take a parent.
 * @param {boolean|string|null|undefined} fixedValue
 * @returns {boolean} True when the entry is a system row.
 */
const isFixedListItem = (fixedValue) => {
    if (fixedValue === true) return true;
    if (fixedValue == null) return false;
    const normalized = String(fixedValue).trim().toLowerCase();
    return normalized === 'yes' || normalized === 'true';
};

/**
 * Reads a value out of crafted contents held as a list of Key/value pairs, matching the key case insensitively.
 * @param {Array<{Key?: string, key?: string, value?: *, Value?: *}>} items
 * @param {string} key
 * @returns {*} The value held against the key, or undefined.
 */
const getEntryField = (items, key) => {
    const item = items.find(x => (x.Key ?? x.key)?.toLowerCase() === key.toLowerCase());
    if (!item) return undefined;
    return item.value ?? item.Value;
};

/**
 * Reads the table entry the form is working on out of whichever shape the crafted page was given.
 * The backend supplies a list of Key/value pairs; the object branch is kept so a form opened from a
 * record copy still populates rather than silently rendering an empty entry.
 * @param {Array|Object} data - Crafted page contents.
 * @returns {Object} The entry's Value, Enabled, ParentId, OptionName, InternalHierarchyParentOptionName and Fixed.
 */
const normalizeTableEntryData = (data) => {
    if (Array.isArray(data)) {
        return {
            Value: getEntryField(data, 'Value'),
            Enabled: getEntryField(data, 'Enabled') ?? 'Yes',
            ParentId: getEntryField(data, 'ParentId') ?? 0,
            OptionName: getEntryField(data, 'OptionName'),
            InternalHierarchyParentOptionName: getEntryField(data, 'InternalHierarchyParentOptionName'),
            Fixed: getEntryField(data, 'Fixed'),
        };
    }
    return {
        Value: data?.Value ?? data?.value,
        Enabled: data?.Enabled ?? data?.enabled ?? 'Yes',
        ParentId: data?.ParentId ?? data?.parentId ?? 0,
        OptionName: data?.OptionName ?? data?.optionName,
        InternalHierarchyParentOptionName:
            data?.InternalHierarchyParentOptionName ?? data?.internalHierarchyParentOptionName,
        Fixed: data?.Fixed ?? data?.fixed,
    };
};

/**
 * Add and Edit form for a single entry of a reference table (Configuration > Tables).
 * Shows Value and Enabled always, and appends a Parent picker when the table has parents to offer:
 * from another table when OptionName is set, or from its own fixed rows when the table parents itself
 * (InternalHierarchyParentOptionName). Fixed system rows are never given a Parent picker.
 * @param {Object} props
 * @param {Array|Object} props.data - Crafted page contents describing the entry.
 * @param {function(string, *, Object=): void} props.changeHandler
 */
const TableEntry = (props) => {

    const [column, setColumn] = useState();
    const [entryData, setEntryData] = useState({ value: 0, enabled: 0, parentid: 0});

    useEffect(() => {

        const newEntryData = normalizeTableEntryData(props.data);

        if (newEntryData.Enabled !== undefined) {
            const fields = [
                {
                    Id: "Value",
                    Label: TranslateTag("@GenVal@", props.language),
                    Type: "singleline",
                    Placeholder: TranslateTag("@PatEntH@", props.language),
                    value: newEntryData.Value
                },
                {
                    Id: "Enabled",
                    Label: TranslateTag("@GenEna@", props.language),
                    Type: "toggle",
                    value: newEntryData.Enabled
                },
            ];
    
            updateFieldChanges(newEntryData);
            setEntryData(newEntryData);

            const optionName = newEntryData.OptionName;
            const internalHierarchyOptionName = newEntryData.InternalHierarchyParentOptionName;
            const isFixedParent = isFixedListItem(newEntryData.Fixed);

            if (optionName !== undefined && optionName !== null) {
                const param = [{ name: optionName, includeFixed: true }];
                PostList(param, listRetrieved, errorHandler, { fields: fields, data: newEntryData, multiSelect: true });
            } else if (internalHierarchyOptionName && !isFixedParent) {
                const param = [{
                    name: internalHierarchyOptionName,
                    includeFixed: true,
                    parentNodesOnly: true
                }];
                PostList(param, listRetrieved, errorHandler, { fields: fields, data: newEntryData, multiSelect: false });
            } else {
                setColumnValue(fields);
            }
        }
    }, []);

    const updateFieldChanges = (data) => {

        const changes = [{ key: "Value", value: { Key: "Value", value: data.Value }},
            { key: "Enabled", value: { Key: "Enabled", value: data.Enabled }},
            { key: "ParentId", value: { Key: "ParentId", value: data.ParentId }},
            { key: "OptionName", value: { Key: "OptionName", value: data.OptionName ?? data.optionName }},
            { key: "InternalHierarchyParentOptionName", value: { Key: "InternalHierarchyParentOptionName", value: data.InternalHierarchyParentOptionName ?? data.internalHierarchyParentOptionName }},
            { key: "Fixed", value: { Key: "Fixed", value: data.Fixed ?? data.fixed }}
        ];
        setEntryData(data);
        props.changeHandler("multiplechanges", changes, { rootCopy: true });
    }

    /**
     * Records a field edit against the displayed field and lifts the whole entry back to the form.
     * Fields are matched by Id rather than by position, because the Parent field is only appended for
     * tables that have parents and so its index is not fixed.
     * @param {string} id - The field's Id.
     * @param {*} value - The new value.
     */
    const changeHandler = (id, value) => {

        const fields = column.FormGroups[0].Fields;
        const fieldToChange = fields.find(field => field.Id === id);
        if (fieldToChange !== undefined) { fieldToChange.value = value; }

        const changes = entryData;
        changes[id] = value;
        updateFieldChanges(changes);
    }

    const listRetrieved = (data, extraInfo) => {
        const newField = {
            Id: "ParentId",
            Label: TranslateTag("@GenPar@", props.language),
            Type: "combobox",
            Options: data[0].options,
            MultiSelect: extraInfo.multiSelect !== false,
            Placeholder: TranslateTag("@GenSelG@", props.language),
            value: extraInfo.data.ParentId
        }
        extraInfo.fields.push(newField);
        setColumnValue(extraInfo.fields);
    }

    const errorHandler = (error) => {
        var x = 1;
    }

    const setColumnValue = (fields) => {
        const metadata = { 
            FieldWidth: "wide",
            ItemWidth: "wide", 
            Key: "col1", 
            FormGroups: [{
                Key: "fg1",
                Column: 0,
                Fields: fields
            }]
        }

        setColumn(metadata);
    }

    let display = null;
    if (column !== undefined) {
        display = <FormColumn key={column.Key} config={column} changeHandler={changeHandler}></FormColumn>
    }

    const contentCss = props.fullScreen ? "app-crafted-fullscreencontent" : "app-crafted-content";

    return (
        <div className={contentCss}>
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <div>
                {display}
            </div>
        </div>
    );
};
  
export default TableEntry;
