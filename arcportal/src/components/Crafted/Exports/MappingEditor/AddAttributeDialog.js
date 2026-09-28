import React, { useState, useEffect, useMemo } from 'react';
import { Panel, PanelType, PrimaryButton, DefaultButton, TextField, Dropdown, Toggle } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import {
    resolveAttributeTableBucket,
    isUniqueReferenceBlocked,
    describeUniqueReferenceBucket
} from './useMappingConstraints';

/**
 * Side panel used to add an attribute (leaf) under an object or array node.
 *
 * When the parent is inside a grid array we restrict the field picker to the
 * grid's prepopulated sub-field list. Otherwise the picker shows the full set
 * of (non-grid) profile fields.
 *
 * When props.isEdit is true the panel binds/renames an existing (e.g. imported)
 * attribute instead of adding a new one; initial values seed the inputs.
 *
 * @param {object} props
 * @param {boolean} props.visible
 * @param {boolean} [props.isEdit] - true when editing an existing attribute (bind/rename).
 * @param {string}  [props.initialName] - initial attribute name (edit mode).
 * @param {string}  [props.initialFieldKey] - initial bound field key (edit mode).
 * @param {string}  [props.initialGridSubFieldId] - initial grid sub-field id (edit mode).
 * @param {string}  [props.initialUniqueReference] - initial unique reference flag (edit mode).
 * @param {object|null} [props.structure] - current mapping tree for unique-reference checks.
 * @param {string}  [props.excludeNodeId] - node id to exclude when editing.
 * @param {string}  [props.gridFieldKey] - parent grid field key when inside a grid array.
 * @param {Array}   props.allFieldOptions - full profile field options for table resolution.
 * @param {Array}   props.fieldOptions - profile field options shown in the picker.
 * @param {Array}   [props.gridSubFields] - prepopulated grid sub-fields when inside a grid array.
 * @param {Array}   props.language
 * @param {function(name:string, fieldKey:string|undefined, gridSubFieldId:string|undefined, uniqueReference:string):void} props.onConfirm
 * @param {function():void} props.onDismiss
 */
const AddAttributeDialog = (props) => {
    const insideGrid = Array.isArray(props.gridSubFields) && props.gridSubFields.length > 0;
    const [name, setName] = useState('');
    const [fieldKey, setFieldKey] = useState('');
    const [gridSubFieldId, setGridSubFieldId] = useState('');
    const [uniqueReference, setUniqueReference] = useState('No');
    const [error, setError] = useState('');

    const selectedBucket = useMemo(() => {
        if (insideGrid) {
            return resolveAttributeTableBucket(
                undefined,
                props.structure,
                props.allFieldOptions,
                props.gridFieldKey,
                gridSubFieldId
            );
        }
        return resolveAttributeTableBucket(fieldKey, props.structure, props.allFieldOptions);
    }, [insideGrid, fieldKey, gridSubFieldId, props.structure, props.allFieldOptions, props.gridFieldKey]);

    const uniqueReferenceBlocked = useMemo(() => (
        !!selectedBucket && isUniqueReferenceBlocked(
            props.structure,
            selectedBucket,
            props.excludeNodeId,
            props.allFieldOptions
        )
    ), [props.structure, selectedBucket, props.excludeNodeId, props.allFieldOptions]);

    const uniqueRefErrorText = useMemo(() => {
        const fallback = selectedBucket
            ? `Only one attribute may be marked as the unique reference for ${describeUniqueReferenceBucket(selectedBucket)} fields.`
            : 'Only one attribute per table may be marked as the unique reference.';
        const tag = TranslateTag('@ExpProMapUniqueRefErr@', props.language) || fallback;
        return tag.replace('{table}', describeUniqueReferenceBucket(selectedBucket));
    }, [props.language, selectedBucket]);

    const uniqueRefNoFieldText = useMemo(
        () => TranslateTag('@ExpProMapUniqueRefNoField@', props.language)
            || 'Select a field before marking Unique Reference.',
        [props.language]
    );

    const validateUniqueReference = (next) => {
        if (next !== 'Yes') return '';
        if (!selectedBucket) return uniqueRefNoFieldText;
        if (uniqueReferenceBlocked) return uniqueRefErrorText;
        return '';
    };

    useEffect(() => {
        if (props.visible) {
            setName(props.initialName ?? '');
            setFieldKey(props.initialFieldKey ?? '');
            setGridSubFieldId(props.initialGridSubFieldId ?? '');
            setUniqueReference(props.initialUniqueReference === 'Yes' ? 'Yes' : 'No');
            setError('');
        }
    }, [props.visible, props.initialName, props.initialFieldKey, props.initialGridSubFieldId, props.initialUniqueReference]);

    useEffect(() => {
        if (uniqueReference !== 'Yes') return;
        const uniqueRefError = validateUniqueReference('Yes');
        setError(uniqueRefError);
    }, [selectedBucket, uniqueReferenceBlocked, uniqueReference]);

    const fieldOptions = useMemo(() => {
        const list = Array.isArray(props.fieldOptions) ? props.fieldOptions : [];
        return list.map((o) => ({ key: o.Key || o.key, text: o.Text || o.text || o.FieldId || o.fieldId }));
    }, [props.fieldOptions]);

    const gridOptions = useMemo(() => {
        if (!insideGrid) return [];
        return props.gridSubFields.map((sf) => ({ key: sf.Id || sf.id, text: sf.Label || sf.label || sf.Id || sf.id }));
    }, [insideGrid, props.gridSubFields]);

    const handleUniqueReferenceChange = (_, checked) => {
        const next = checked ? 'Yes' : 'No';
        setUniqueReference(next);
        const uniqueRefError = validateUniqueReference(next);
        setError(uniqueRefError);
    };

    const handleConfirm = () => {
        if (!name || name.trim().length === 0) {
            setError(TranslateTag('@ExpProMapAttrNameErr@', props.language) || 'Attribute name is required.');
            return;
        }
        const uniqueRefError = validateUniqueReference(uniqueReference);
        if (uniqueRefError) {
            setError(uniqueRefError);
            return;
        }
        if (insideGrid) {
            if (!gridSubFieldId) {
                setError(TranslateTag('@ExpProMapFieldErr@', props.language) || 'A field selection is required.');
                return;
            }
            props.onConfirm(name.trim(), undefined, gridSubFieldId, uniqueReference);
            return;
        }
        if (!fieldKey) {
            setError(TranslateTag('@ExpProMapFieldErr@', props.language) || 'A field selection is required.');
            return;
        }
        props.onConfirm(name.trim(), fieldKey, undefined, uniqueReference);
    };

    const headerText = props.isEdit
        ? (TranslateTag('@ExpProMapEditAttr@', props.language) || 'Edit attribute')
        : (TranslateTag('@ExpProMapAddAttr@', props.language) || 'Add attribute');

    return (
        <Panel
            isOpen={props.visible}
            onDismiss={props.onDismiss}
            type={PanelType.medium}
            headerText={headerText}
            closeButtonAriaLabel="Close"
        >
            {insideGrid ? (
                <Dropdown
                    id="mapping-attr-field"
                    label={TranslateTag('@ExpProMapField@', props.language) || 'Field'}
                    options={gridOptions}
                    selectedKey={gridSubFieldId}
                    onChange={(_, opt) => setGridSubFieldId(opt?.key ?? '')}
                />
            ) : (
                <Dropdown
                    id="mapping-attr-field"
                    label={TranslateTag('@ExpProMapField@', props.language) || 'Field'}
                    options={fieldOptions}
                    selectedKey={fieldKey}
                    onChange={(_, opt) => setFieldKey(opt?.key ?? '')}
                />
            )}
            <TextField
                id="mapping-attr-name"
                label={TranslateTag('@ExpProMapAttrName@', props.language) || 'Attribute name'}
                value={name}
                onChange={(_, v) => setName(v ?? '')}
            />
            <Toggle
                id="mapping-attr-unique-ref"
                label={TranslateTag('@ExpProMapUniqueRef@', props.language) || 'Unique Reference'}
                checked={uniqueReference === 'Yes'}
                onText={TranslateTag('@GenYesA@', props.language) || 'Yes'}
                offText={TranslateTag('@GenNo@', props.language) || 'No'}
                onChange={handleUniqueReferenceChange}
            />
            {error && <div style={{ color: '#a4262c', marginTop: 6 }}>{error}</div>}
            <div className="mappingeditor-panel-footer">
                <PrimaryButton id="mapping-attr-add" onClick={handleConfirm} text={props.isEdit
                    ? (TranslateTag('@GenSaveB@', props.language) || 'Save')
                    : (TranslateTag('@GenAddB@', props.language) || 'Add')} />
                <DefaultButton id="mapping-attr-cancel" onClick={props.onDismiss} text={TranslateTag('@GenCanA@', props.language) || 'Cancel'} />
            </div>
        </Panel>
    );
};

export default AddAttributeDialog;
