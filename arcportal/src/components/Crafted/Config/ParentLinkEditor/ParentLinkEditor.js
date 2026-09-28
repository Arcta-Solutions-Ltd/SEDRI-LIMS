/**
 * ParentLinkEditor - configures a child list field to filter by a parent field on the same form.
 *
 * @param {Object} props
 * @param {string} props.value - Parent field id (FieldConfig.ParentList)
 * @param {Function} props.onChange - Callback with parent field id or empty string
 * @param {Array<{ListId?: number, listId?: number, ParentListId?: number, parentListId?: number, ParentListName?: string, parentListName?: string}>} props.childLists
 * @param {Array<{FieldId?: string, fieldId?: string, Label?: string, label?: string, ListId?: number, listId?: number}>} props.pageListFields
 * @param {number|string} props.selectedListId - Currently selected list id from the List field
 * @param {Array} [props.language] - Language array for TranslateTag
 */
import React, { useEffect, useMemo, useState } from 'react';
import { Dropdown, Toggle } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import './ParentLinkEditor.css';

const readListId = (row) => row?.ListId ?? row?.listId ?? 0;
const readParentListId = (row) => row?.ParentListId ?? row?.parentListId ?? 0;
const readParentListName = (row) => row?.ParentListName ?? row?.parentListName ?? '';
const readFieldId = (row) => row?.FieldId ?? row?.fieldId ?? '';
const readLabel = (row) => row?.Label ?? row?.label ?? '';

const ParentLinkEditor = (props) => {
    const language = props.language || [];
    const childLists = props.childLists || [];
    const pageListFields = props.pageListFields || [];
    const selectedListId = parseInt(props.selectedListId, 10) || 0;
    const value = props.value || '';

    const childList = useMemo(
        () => childLists.find((row) => readListId(row) === selectedListId),
        [childLists, selectedListId]
    );

    const candidates = useMemo(() => {
        if (!childList) {
            return [];
        }
        const parentListId = readParentListId(childList);
        return pageListFields
            .filter((row) => readListId(row) === parentListId)
            .map((row) => ({
                key: readFieldId(row),
                text: TranslateTag(readLabel(row), language) || readLabel(row) || readFieldId(row),
            }))
            .filter((opt) => opt.key);
    }, [childList, pageListFields, language]);

    const [linkEnabled, setLinkEnabled] = useState(!!value);

    useEffect(() => {
        setLinkEnabled(!!value);
    }, [value]);

    useEffect(() => {
        if (!childList && value) {
            props.onChange('');
        }
    }, [childList, value, props.onChange]);

    useEffect(() => {
        if (!childList || !linkEnabled) {
            return;
        }
        if (candidates.length === 1 && !value) {
            props.onChange(candidates[0].key);
        }
    }, [childList, linkEnabled, candidates, value, props.onChange]);

    if (!childList) {
        return null;
    }

    const translate = (tag, fallback) => TranslateTag(tag, language) || fallback;

    const onToggleChange = (_event, checked) => {
        const enabled = checked === true;
        setLinkEnabled(enabled);
        if (!enabled) {
            props.onChange('');
            return;
        }
        if (candidates.length === 1) {
            props.onChange(candidates[0].key);
        }
    };

    const onParentFieldChange = (_event, option) => {
        props.onChange(option?.key ?? '');
    };

    const missingMessage = translate('@ConParMis@', 'Add a {0} field to this form before linking.')
        .replace('{0}', readParentListName(childList));

    return (
        <div className="parentlink-editor" id="fieldparentlink-editor">
            <Toggle
                id="fieldparentlink-enable"
                label={translate('@ConParEna@', 'Filter options by parent field')}
                checked={linkEnabled}
                onChange={onToggleChange}
            />
            {linkEnabled && candidates.length > 0 && (
                <Dropdown
                    id="fieldparentlink-parentfield"
                    label={translate('@ConParSel@', 'Parent field')}
                    placeholder={translate('@ConParSel@', 'Parent field')}
                    options={candidates}
                    selectedKey={value || undefined}
                    onChange={onParentFieldChange}
                />
            )}
            {linkEnabled && candidates.length === 0 && (
                <div className="parentlink-editor-note" id="fieldparentlink-missing">
                    {missingMessage}
                </div>
            )}
        </div>
    );
};

export default ParentLinkEditor;
