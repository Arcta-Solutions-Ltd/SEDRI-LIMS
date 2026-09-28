import React, { useState, useEffect, useMemo } from 'react';
import { Panel, PanelType, PrimaryButton, DefaultButton, TextField, Dropdown } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import {
    getAllowedArrayTypes,
    getAllowedArrayTypeKeysForParent,
    getGridFieldOptions,
    getAstFieldOptions
} from './useMappingConstraints';

/**
 * Side panel used to add an array node. Array types ('specimens'|'cultures'|'grid'|'ast')
 * are gated based on the fields present on the export profile and on the
 * parent node where the array is being inserted. Nested arrays are only
 * allowed inside isolate/culture arrays and only as AST arrays - i.e. arrays
 * that group fields from the AST (ast) table such as antibiotic susceptibility
 * results. Selecting a grid type also requires choosing which grid field the
 * array represents.
 *
 * When props.isEdit is true the panel classifies an existing (e.g. imported)
 * untyped array instead of adding a new one; initial values seed the inputs and
 * the array's existing children are preserved for non-grid types.
 *
 * @param {object} props
 * @param {boolean} props.visible
 * @param {boolean} [props.isEdit] - true when classifying an existing array.
 * @param {string}  [props.initialName] - initial array name (edit mode).
 * @param {string}  [props.initialArrayType] - initial array type (edit mode).
 * @param {string}  [props.initialGridFieldKey] - initial grid field key (edit mode).
 * @param {object}  [props.existingChildren] - existing children preserved for non-grid types (edit mode).
 * @param {Array}   props.fieldOptions
 * @param {boolean} props.hasPatient
 * @param {boolean} props.hasCulture
 * @param {boolean} props.hasAst - true when the profile has at least one ast-table field.
 * @param {object}  [props.parentNode] - the node the new array will be inserted under.
 * @param {Array}   props.language
 * @param {function(node:object):void} props.onConfirm
 * @param {function():void} props.onDismiss
 */
const AddArrayDialog = (props) => {
    const [name, setName] = useState('');
    const [arrayType, setArrayType] = useState('');
    const [gridFieldKey, setGridFieldKey] = useState('');
    const [error, setError] = useState('');

    const gridFieldOptions = useMemo(() => getGridFieldOptions(props.fieldOptions), [props.fieldOptions]);
    const astFieldOptions = useMemo(() => getAstFieldOptions(props.fieldOptions), [props.fieldOptions]);
    const hasGrid = gridFieldOptions.length > 0;
    const hasAst = !!props.hasAst || astFieldOptions.length > 0;

    const parentAllowedKeys = useMemo(
        () => getAllowedArrayTypeKeysForParent(props.parentNode, { hasAst }),
        [props.parentNode, hasAst]
    );

    const allowedTypes = useMemo(() => {
        const baseTypes = getAllowedArrayTypes({
            hasPatient: !!props.hasPatient,
            hasCulture: !!props.hasCulture,
            hasGrid: hasGrid,
            hasAst: hasAst
        }, props.language);
        return baseTypes.filter((t) => parentAllowedKeys.includes(t.key));
    }, [props.hasPatient, props.hasCulture, hasGrid, hasAst, props.language, parentAllowedKeys]);

    useEffect(() => {
        if (props.visible) {
            setName(props.initialName ?? '');
            setGridFieldKey(props.initialGridFieldKey ?? '');
            setError('');
            if (props.isEdit && props.initialArrayType) {
                setArrayType(props.initialArrayType);
            } else {
                const enabled = allowedTypes.filter((t) => t.enabled);
                setArrayType(enabled.length === 1 ? enabled[0].key : '');
            }
        }
    }, [props.visible, props.isEdit, props.initialName, props.initialArrayType, props.initialGridFieldKey]);

    const dropdownOptions = useMemo(() =>
        allowedTypes.map((t) => ({ key: t.key, text: t.label, disabled: !t.enabled, title: t.reason || '' })),
        [allowedTypes]);

    const gridOptions = useMemo(() =>
        gridFieldOptions.map((o) => ({ key: o.Key || o.key, text: o.Text || o.text || o.FieldId || o.fieldId })),
        [gridFieldOptions]);

    const handleConfirm = () => {
        if (!arrayType) {
            setError(TranslateTag('@ExpProMapFieldErr@', props.language) || 'A field selection is required.');
            return;
        }
        if (arrayType === 'grid') {
            if (!gridFieldKey) {
                setError(TranslateTag('@ExpProMapFieldErr@', props.language) || 'A field selection is required.');
                return;
            }
            const opt = gridFieldOptions.find((o) => (o.Key || o.key) === gridFieldKey);
            const subFields = opt?.GridSubFields || opt?.gridSubFields || [];
            const node = {
                kind: 'array',
                name: (name && name.trim().length > 0) ? name.trim() : (opt?.FieldId || opt?.fieldId || 'GridArray'),
                arrayType: 'grid',
                gridFieldKey: gridFieldKey,
                children: subFields.map((sf) => ({
                    kind: 'attribute',
                    name: sf.Id || sf.id || sf.Label || sf.label || 'attribute',
                    gridSubFieldId: sf.Id || sf.id || ''
                }))
            };
            props.onConfirm(node);
            return;
        }
        const fallbackName = arrayType === 'specimens' ? 'Specimens'
            : arrayType === 'ast' ? 'AST'
            : 'Cultures';
        const node = {
            kind: 'array',
            name: (name && name.trim().length > 0) ? name.trim() : fallbackName,
            arrayType: arrayType,
            // Preserve the imported/existing children when classifying an existing array.
            children: Array.isArray(props.existingChildren) ? props.existingChildren : []
        };
        props.onConfirm(node);
    };

    const headerText = props.isEdit
        ? (TranslateTag('@ExpProMapClassifyArr@', props.language) || 'Classify array')
        : (TranslateTag('@ExpProMapAddArr@', props.language) || 'Add array');

    return (
        <Panel
            isOpen={props.visible}
            onDismiss={props.onDismiss}
            type={PanelType.medium}
            headerText={headerText}
            closeButtonAriaLabel="Close"
        >
            <Dropdown
                label={TranslateTag('@ExpProMapArrType@', props.language) || 'Array type'}
                options={dropdownOptions}
                selectedKey={arrayType}
                onChange={(_, opt) => setArrayType(opt?.key ?? '')}
            />
            {arrayType === 'grid' && (
                <Dropdown
                    label={TranslateTag('@ExpProMapField@', props.language) || 'Field'}
                    options={gridOptions}
                    selectedKey={gridFieldKey}
                    onChange={(_, opt) => setGridFieldKey(opt?.key ?? '')}
                />
            )}
            <TextField
                label={TranslateTag('@ExpProMapArrName@', props.language) || 'Array name'}
                value={name}
                onChange={(_, v) => setName(v ?? '')}
            />
            {error && <div style={{ color: '#a4262c', marginTop: 6 }}>{error}</div>}
            <div className="mappingeditor-panel-footer">
                <PrimaryButton onClick={handleConfirm} text={props.isEdit
                    ? (TranslateTag('@GenSaveB@', props.language) || 'Save')
                    : (TranslateTag('@GenAddB@', props.language) || 'Add')} />
                <DefaultButton onClick={props.onDismiss} text={TranslateTag('@GenCanA@', props.language) || 'Cancel'} />
            </div>
        </Panel>
    );
};

export default AddArrayDialog;
