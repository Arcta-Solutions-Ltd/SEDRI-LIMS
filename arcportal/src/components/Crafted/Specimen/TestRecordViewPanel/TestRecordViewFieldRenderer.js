import React from 'react';
import { Separator } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import GetTextForListItemsInGrid from '../../../../Utils/Forms/GetTextForListItemsInGrid';
import FileThumbnailGallery from '../../../General/FileThumbnailGallery/FileThumbnailGallery';
import './TestRecordViewPanel.css';

/**
 * Extracts field names from a FieldFormat string (e.g. "@AntibioticId@" -> ["AntibioticId"]).
 * @param {string} format - FieldFormat string
 * @returns {string[]} Array of field names
 */
const getFieldFormat = (format) => {
    if (!format || typeof format !== 'string') return [];
    const matches = format.match(/@([^@]+)@/g) || [];
    return matches.map((m) => m.slice(1, -1));
};

/**
 * Resolves display value for a grid field, using FieldFormat or raw value.
 * @param {Object} field - Grid field config
 * @param {Object} lineValues - Row values
 * @param {Array} displayData - Resolved {key, value} from GetTextForListItemsInGrid
 * @returns {string|undefined}
 */
const getDisplayValueForGridField = (field, lineValues, displayData) => {
    if (field?.FieldFormat) {
        let updated = field.FieldFormat;
        getFieldFormat(field.FieldFormat).forEach((el) => {
            const val = displayData.find((d) => d.key.toLowerCase() === el.toLowerCase())?.value ?? lineValues[el];
            updated = updated.replace(`@${el}@`, val !== undefined && val !== null ? String(val) : '');
        });
        return updated;
    }
    const id = field.Id || field.id;
    const resolved = displayData.find((d) => (d.key || '').toLowerCase() === (id || '').toLowerCase())?.value;
    if (resolved !== undefined && resolved !== null) return String(resolved);
    const val = lineValues[id];
    return val !== undefined && val !== null ? String(val) : undefined;
};

/**
 * Renders a single field in read-only mode based on field type.
 *
 * @param {Object} props - Component props
 * @param {Object} props.field - Field config with Id, Type, Label, value, GridFields (for fieldgrid), etc.
 * @param {Array} props.lists - Option lists for resolving dropdown/combobox values
 * @param {Array} props.pages - Page definitions (for grid field list resolution)
 * @param {string} props.language - Language for translations
 */
const TestRecordViewFieldRenderer = (props) => {
    const { field, lists, pages, language } = props;
    const type = (field.Type || field.type || '').toLowerCase();
    const label = field.Label || field.label || '';
    const value = field.value;

    const toggleTranslator = (v) => {
        if (v === 'Yes') return TranslateTag("@GenYesA@", language);
        if (v === 'No') return TranslateTag("@GenNo@", language);
        return v;
    };

    const resolveOptionText = (optValue) => {
        if (optValue === undefined || optValue === null || optValue === '') return '';
        const optStr = String(optValue);
        const optionName = field.OptionsName || field.optionsName;
        if (!optionName) return optStr;
        const list = (lists || []).find(
            (l) => String(l.Name || l.name || '').toLowerCase() === String(optionName || '').toLowerCase()
        );
        const opt = (list?.Options ?? list?.options ?? []).find(
            (o) => String(o.Key ?? o.key ?? '') === optStr
        );
        return opt ? (opt.Text ?? opt.text ?? optStr) : optStr;
    };

    if (type === 'space' || type === 'separator') {
        return type === 'separator' && label ? <Separator key={field.Id}>{label}</Separator> : null;
    }

    const displayValue = () => {
        if (value === undefined || value === null) return null;
        switch (type) {
            case 'toggle':
                return toggleTranslator(value);
            case 'dropdown':
            case 'combobox':
            case 'filteredcombo':
            case 'picker':
                return resolveOptionText(value) || value;
            case 'date':
                if (value instanceof Date) {
                    return value.toLocaleDateString();
                }
                if (typeof value === 'string') {
                    try {
                        const d = new Date(value);
                        return isNaN(d.getTime()) ? value : d.toLocaleDateString();
                    } catch {
                        return value;
                    }
                }
                return value;
            case 'time':
                return typeof value === 'string' ? value : String(value);
            case 'number':
                return String(value);
            default:
                return String(value);
        }
    };

    if (type === 'fieldgrid') {
        const gridValue = Array.isArray(value) ? value : [];
        const gridFields = field.GridFields || field.gridfields || [];
        if (gridValue.length === 0 && gridFields.length === 0) return null;

        const gridLabel = label && (typeof label === 'string' && label.startsWith('@')) ? TranslateTag(label, language) : label;
        const hasHeader = gridFields.some((gf) => {
            const t = gf.GridTitle ?? gf.gridtitle ?? gf.Label ?? gf.label;
            return t !== undefined && t !== null && String(t).trim() !== '';
        });
        return (
            <div key={field.Id || field.id} className="testrecordview-field testrecordview-grid">
                {gridLabel ? <div className="testrecordview-grid-title">{gridLabel}</div> : null}
                <div className="testrecordview-grid-table">
                    {gridFields.length > 0 && (
                        <>
                            {hasHeader && (
                                <div className="testrecordview-grid-header">
                                    {gridFields.map((gf) => {
                                        const colLabel = gf.GridTitle ?? gf.gridtitle ?? gf.Label ?? gf.label ?? '';
                                        const colDisplay = (typeof colLabel === 'string' && colLabel.startsWith('@')) ? TranslateTag(colLabel, language) : colLabel;
                                        return (
                                            <div key={gf.Id || gf.id} className="testrecordview-grid-cell testrecordview-grid-header-cell">
                                                {colDisplay}
                                            </div>
                                        );
                                    })}
                                </div>
                            )}
                            {gridValue.map((row, idx) => {
                                const savedArray = Object.entries(row).map(([k, v]) => ({
                                    key: k,
                                    value: v !== undefined && v !== null ? String(v) : ''
                                }));
                                GetTextForListItemsInGrid(savedArray, lists || [], pages || []);
                                const lineValues = { ...row };
                                return (
                                    <div key={idx} className="testrecordview-grid-row">
                                        {gridFields.map((gf) => {
                                            const displayVal = getDisplayValueForGridField(gf, lineValues, savedArray);
                                            return (
                                                <div key={gf.Id || gf.id} className="testrecordview-grid-cell">
                                                    {displayVal ?? ''}
                                                </div>
                                            );
                                        })}
                                    </div>
                                );
                            })}
                        </>
                    )}
                </div>
            </div>
        );
    }

    if (type === 'upload') {
        if (value === undefined || value === null || value === '') return null;
        return (
            <div key={field.Id || field.id} className="testrecordview-field testrecordview-upload">
                {label ? <div className="testrecordview-label">{label}:</div> : null}
                <div className="testrecordview-value">
                    <FileThumbnailGallery fileIds={value} language={language} readOnly />
                </div>
            </div>
        );
    }

    if (type === 'crafted') {
        return null;
    }

    const val = displayValue();
    if (val === null && type !== 'upload') return null;
    if (val === '' && type !== 'number') return null;

    return (
        <div key={field.Id || field.id} className="testrecordview-field">
            <div className="testrecordview-label">{label}:</div>
            <div className="testrecordview-value">{val}</div>
        </div>
    );
};

export default TestRecordViewFieldRenderer;
