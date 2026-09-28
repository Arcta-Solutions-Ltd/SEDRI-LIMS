/**
 * ExistingFieldSelector - picks fields that already exist on other forms so they can be referenced
 * onto the current page. Selecting a field places a copy of its definition on the target page while
 * keeping the original field id, so both forms read and write the same stored data.
 *
 * Candidates are supplied by the backend, which has already applied the entity scope rules (a
 * specimen page only offers specimen fields, a patient page only patient fields, and so on).
 *
 * @param {Object} props
 * @param {string} props.value - Comma separated reference keys, each `{form}|{page}|{fieldId}`.
 * @param {Function} props.onChange - Callback with the new comma separated reference key string.
 * @param {Array<{Key: string, Text: string, FieldId: string, FieldType: string, TableName: string, SourcePageTitle: string}>} props.options - Reuse candidates.
 * @param {string} [props.targetTable] - Resolved entity table of the target page, shown as context.
 * @param {Array} [props.language] - Language array for TranslateTag.
 */
import React, { useMemo, useState } from 'react';
import { Checkbox, SearchBox } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import './ExistingFieldSelector.css';

const readKey = (row) => row?.Key ?? row?.key ?? '';
const readText = (row) => row?.Text ?? row?.text ?? '';
const readFieldId = (row) => row?.FieldId ?? row?.fieldId ?? '';
const readFieldType = (row) => row?.FieldType ?? row?.fieldType ?? '';
const readPageTitle = (row) => row?.SourcePageTitle ?? row?.sourcePageTitle ?? '';

/**
 * Splits the stored comma separated reference key string into an array, dropping blanks.
 * @param {string} value - Comma separated reference keys.
 * @returns {Array<string>} The individual reference keys.
 */
const splitKeys = (value) => {
    if (typeof value !== 'string' || value.trim() === '') {
        return [];
    }
    return value.split(',').map((key) => key.trim()).filter((key) => key !== '');
};

const ExistingFieldSelector = (props) => {
    const language = props.language || [];
    const options = Array.isArray(props.options) ? props.options : [];
    const [search, setSearch] = useState('');

    const translate = (tag, fallback) => TranslateTag(tag, language) || fallback;

    const selectedKeys = useMemo(() => splitKeys(props.value), [props.value]);

    const groups = useMemo(() => {
        const term = search.trim().toLowerCase();
        const byPage = new Map();

        for (const option of options) {
            const key = readKey(option);
            if (key === '') {
                continue;
            }

            const label = readText(option);
            const fieldId = readFieldId(option);
            if (term !== ''
                && !label.toLowerCase().includes(term)
                && !fieldId.toLowerCase().includes(term)) {
                continue;
            }

            const pageTitle = readPageTitle(option) || translate('@ConOthP@', 'Other pages');
            if (!byPage.has(pageTitle)) {
                byPage.set(pageTitle, []);
            }
            byPage.get(pageTitle).push({
                key,
                label: label || fieldId,
                fieldId,
                fieldType: readFieldType(option)
            });
        }

        return Array.from(byPage.entries()).map(([title, fields]) => ({ title, fields }));
    }, [options, search, language]);

    const toggleKey = (key, checked) => {
        const next = checked
            ? [...selectedKeys.filter((existing) => existing !== key), key]
            : selectedKeys.filter((existing) => existing !== key);

        props.onChange(next.join(','));
    };

    const totalCount = options.length;
    const selectedCount = selectedKeys.length;

    // The entity table is carried on the container rather than shown, because it is an internal
    // routing concept. The backend has already applied it when choosing the candidates.
    const scope = typeof props.targetTable === 'string' ? props.targetTable : '';

    if (totalCount === 0) {
        return (
            <div className="existingfieldselector" id="existingfieldselector" data-scope={scope}>
                <div className="existingfieldselector-empty" id="existingfieldselector-empty">
                    {translate('@ConExiFE@', 'There are no other fields available to add to this page.')}
                </div>
            </div>
        );
    }

    return (
        <div className="existingfieldselector" id="existingfieldselector" data-scope={scope}>
            <div className="existingfieldselector-head">
                <SearchBox
                    id="existingfieldselector-search"
                    className="existingfieldselector-search"
                    placeholder={translate('@GenSea@', 'Search')}
                    value={search}
                    onChange={(_event, newValue) => setSearch(newValue ?? '')}
                    onClear={() => setSearch('')}
                    styles={{ root: { width: '100%' } }}
                />
                <div className="existingfieldselector-count" id="existingfieldselector-count">
                    {`${selectedCount} / ${totalCount}`}
                </div>
            </div>
            <div className="existingfieldselector-list" id="existingfieldselector-list">
                {groups.length === 0 && (
                    <div className="existingfieldselector-empty" id="existingfieldselector-nomatch">
                        {translate('@GenNoRes@', 'No matching fields.')}
                    </div>
                )}
                {groups.map((group) => (
                    <div className="existingfieldselector-group" key={group.title}>
                        <div className="existingfieldselector-grouptitle">{group.title}</div>
                        {group.fields.map((field) => (
                            <div className="existingfieldselector-row" key={field.key}>
                                <Checkbox
                                    id={`existingfieldselector-option-${field.fieldId}`}
                                    label={field.label}
                                    checked={selectedKeys.includes(field.key)}
                                    onChange={(_event, checked) => toggleKey(field.key, checked === true)}
                                />
                                <span className="existingfieldselector-type">{field.fieldType}</span>
                            </div>
                        ))}
                    </div>
                ))}
            </div>
        </div>
    );
};

export default ExistingFieldSelector;
