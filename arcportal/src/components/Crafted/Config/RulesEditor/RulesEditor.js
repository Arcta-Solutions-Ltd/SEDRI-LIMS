/**
 * RulesEditor - A reusable component for configuring visibility rules.
 * Rules match domain RuleConfig: { Effect, Field, Rule, Value }.
 * Can be embedded in form group edit, page edit, and fieldgrid column config.
 * Displays a "Rules" section header (via @ConRul@) above the rules grid.
 * When the selected field is a dropdown or combobox, the Value control shows the field's options for selection.
 *
 * @param {Object} props
 * @param {Array<{Effect?: string, Field?: string, Rule?: string, Value?: string}>} props.rules - Array of rule objects (PascalCase for domain compatibility)
 * @param {Function} props.onChange - Callback with updated rules array
 * @param {Array<{id: string, label: string, type?: string, optionsName?: string}>} props.fieldOptions - Options for the rule "field" dropdown. type and optionsName used to show value dropdown when field is dropdown/combobox/radio.
 * @param {Array<{id: string, label: string}>} [props.effectOptions] - Effect options for dropdown; shape { id: effectCode, label: languageKey }. Defaults to visible only. noorganism is system-only and not exposed. Provided by form queries or localFormData.
 * @param {Array<{key: string, text: string}>} [props.ruleOptions] - Rule type options
 * @param {'formgroup'|'gridcolumn'} [props.context] - Display context for layout
 * @param {Array} [props.language] - Language array for TranslateTag
 * @param {Array<{Name: string, Options: Array<{Key: string, Text: string, ParentKey?: string}>}>} [props.lists] - List definitions for resolving dropdown options when selected field has optionsName
 * @param {number} [props.maxRows] - When set, the Add button is hidden once the rules array reaches this length
 * @param {boolean} [props.effectAsTextField] - When true, Effect is a free-text state name field instead of a dropdown
 * @param {boolean} [props.hideHeader] - When true, the Rules section header is not rendered
 * @param {boolean} [props.readOnly] - When true, fields are disabled and the delete button is hidden
 * @param {boolean} [props.usePageRulesIds] - When true with effectAsTextField, uses stable pagerules-* ids for Cypress (row 0 keeps unsuffixed ids; later rows use -{index})
 */
import React from 'react';
import { Dropdown, TextField, IconButton, Stack, TooltipHost } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { getStandardTooltipProps } from '../../../../Utils/General/StandardTooltipProps';
import './RulesEditor.css';

const DEFAULT_EFFECT_OPTIONS = [{ id: 'visible', label: '@GenVis@' }];
const DEFAULT_RULE_OPTIONS = [
    { key: '=', text: '=' },
    { key: '!=', text: '!=' },
    { key: 'isempty', text: 'is empty' },
    { key: 'isnotempty', text: 'is not empty' },
];

const RulesEditor = (props) => {
    const rules = props.rules || [];
    const fieldOptions = props.fieldOptions || [];
    const effectOptions = props.effectOptions || DEFAULT_EFFECT_OPTIONS;
    const ruleOptions = props.ruleOptions || DEFAULT_RULE_OPTIONS;
    const language = props.language || [];
    const lists = props.lists || [];
    const effectAsTextField = props.effectAsTextField === true;
    const readOnly = props.readOnly === true;
    const usePageRulesIds = props.usePageRulesIds === true;
    const maxRows = props.maxRows;
    const canAdd = maxRows == null || rules.length < maxRows;

    const fieldDropdownOptions = fieldOptions.map((opt) => {
        const id = opt.id ?? opt.Id;
        const label = opt.label ?? opt.Label ?? '';
        return {
            key: id,
            text: TranslateTag(label, language) || label || id,
        };
    });

    const effectDropdownOptions = effectOptions.map((opt) => ({
        key: opt.id,
        text: TranslateTag(opt.label, language) || opt.label || opt.id,
    }));

    const normalizeRule = (rule) => ({
        Effect: rule.Effect ?? rule.effect ?? (effectAsTextField ? '' : 'visible'),
        Field: rule.Field ?? rule.field ?? '',
        Rule: rule.Rule ?? rule.rule ?? '=',
        Value: rule.Value ?? rule.value ?? '',
    });

    const updateRule = (index, field, value) => {
        const newRules = [...rules].map((r, i) =>
            i === index ? { ...normalizeRule(r), [field]: value } : normalizeRule(r)
        );
        props.onChange(newRules);
    };

    const addRule = () => {
        const newRule = effectAsTextField
            ? { Effect: '', Field: '', Rule: '=', Value: '' }
            : { Effect: 'visible', Field: '', Rule: '=', Value: '' };
        props.onChange([...rules.map(normalizeRule), newRule]);
    };

    const removeRule = (index) => {
        const newRules = rules.filter((_, i) => i !== index).map(normalizeRule);
        props.onChange(newRules);
    };

    const needsValue = (rule) => {
        const r = (rule.Rule ?? rule.rule ?? '').toLowerCase();
        return r === '=' || r === '!=';
    };

    const getValueOptionsForField = (fieldId) => {
        const fieldOpt = fieldOptions.find((f) => (f.id || f.Id)?.toLowerCase() === (fieldId || '').toLowerCase());
        if (!fieldOpt) return null;
        const type = (fieldOpt.type || fieldOpt.Type || '').toLowerCase();
        const optionsName = fieldOpt.optionsName || fieldOpt.OptionsName;
        if (!['dropdown', 'combobox', 'radio'].includes(type) || !optionsName) return null;
        const list = lists.find((l) => (l.Name || l.name)?.toLowerCase() === optionsName.toLowerCase());
        if (!list?.Options) return null;
        return list.Options.map((opt) => ({
            key: opt.Key ?? opt.key,
            text: opt.Text ?? opt.text ?? opt.Key ?? opt.key,
        }));
    };

    const effectId = (index) => {
        if (!usePageRulesIds) return `rule-effect-${index}`;
        return index === 0 ? 'pagerules-setsstate' : `pagerules-setsstate-${index}`;
    };
    const fieldId = (index) => {
        if (!usePageRulesIds) return `rule-field-${index}`;
        return index === 0 ? 'pagerules-conditionfield' : `pagerules-conditionfield-${index}`;
    };
    const ruleId = (index) => {
        if (!usePageRulesIds) return `rule-rule-${index}`;
        return index === 0 ? 'pagerules-conditionrule' : `pagerules-conditionrule-${index}`;
    };
    const valueId = (index) => {
        if (!usePageRulesIds) return `rule-value-${index}`;
        return index === 0 ? 'pagerules-conditionvalue' : `pagerules-conditionvalue-${index}`;
    };
    const deleteId = (index) => {
        if (!usePageRulesIds) return undefined;
        return index === 0 ? 'pagerules-setsstate-delete' : `pagerules-setsstate-delete-${index}`;
    };
    const addId = usePageRulesIds ? 'pagerules-setsstate-add' : undefined;

    const effectLabel = (index) => {
        if (index !== 0) return '';
        if (effectAsTextField) return TranslateTag('@ConStaNam@', language) || 'State name';
        return TranslateTag('@GenEff@', language) || 'Effect';
    };

    const fieldLabel = (index) => {
        if (index !== 0) return '';
        if (effectAsTextField) return TranslateTag('@ConStaWhe@', language) || 'when';
        return TranslateTag('@GenFie@', language) || 'Field';
    };

    const ruleLabel = (index) => {
        if (index !== 0) return '';
        if (effectAsTextField) return TranslateTag('@ConCom@', language) || 'Comparison';
        return TranslateTag('@GenRul@', language) || 'Rule';
    };

    const renderEffectControl = (norm, index) => {
        if (effectAsTextField) {
            return (
                <TextField
                    id={effectId(index)}
                    label={effectLabel(index)}
                    value={norm.Effect}
                    disabled={readOnly}
                    onChange={(_, val) => updateRule(index, 'Effect', (val ?? '').toLowerCase())}
                    placeholder={TranslateTag('@ConStaNamA@', language) || ''}
                    styles={{ root: { minWidth: 220 } }}
                />
            );
        }

        return (
            <Dropdown
                id={effectId(index)}
                label={effectLabel(index)}
                options={effectDropdownOptions}
                selectedKey={norm.Effect}
                disabled={readOnly}
                onChange={(_, opt) => updateRule(index, 'Effect', opt?.key)}
                styles={{ root: { minWidth: 120 } }}
            />
        );
    };

    const renderValueControl = (norm, index) => {
        const valueOptions = getValueOptionsForField(norm.Field);
        if (valueOptions && valueOptions.length > 0) {
            return (
                <Dropdown
                    id={valueId(index)}
                    label={index === 0 ? TranslateTag('@GenVal@', language) || 'Value' : ''}
                    options={valueOptions}
                    selectedKey={norm.Value || undefined}
                    disabled={readOnly}
                    onChange={(_, opt) => updateRule(index, 'Value', opt?.key ?? '')}
                    placeholder={TranslateTag('@ConSelA@', language) || 'Select...'}
                    styles={{ root: { minWidth: 150 } }}
                />
            );
        }
        return (
            <TextField
                id={valueId(index)}
                label={index === 0 ? TranslateTag('@GenVal@', language) || 'Value' : ''}
                value={norm.Value}
                disabled={readOnly}
                onChange={(_, val) => updateRule(index, 'Value', val ?? '')}
                placeholder=""
                styles={{ root: { minWidth: 150 } }}
            />
        );
    };

    const addRuleTooltip = TranslateTag('@GenAddI@', language) || 'Add rule';
    const deleteRuleTooltip = TranslateTag('@GenDel@', language) || 'Remove';
    const tooltipProps = getStandardTooltipProps();
    const calloutProps = { gapSpace: 10 };

    const addButton = canAdd && !readOnly ? (
        <TooltipHost content={addRuleTooltip} tooltipProps={tooltipProps} calloutProps={calloutProps}>
            <IconButton
                data-test-id={addId || 'rules-editor-add'}
                iconProps={{ iconName: 'Add' }}
                text={addRuleTooltip}
                onClick={addRule}
                ariaLabel={addRuleTooltip}
            />
        </TooltipHost>
    ) : null;

    return (
        <div className="rules-editor">
            {!props.hideHeader && (
                <div className="rules-editor-header">
                    <span>{TranslateTag('@ConRul@', language) || 'Rules'}</span>
                    {addButton}
                </div>
            )}
            <Stack tokens={{ childrenGap: 8 }}>
                {props.hideHeader && addButton}
                {rules.map((rule, index) => {
                    const norm = normalizeRule(rule);
                    return (
                        <Stack
                            key={index}
                            horizontal
                            tokens={{ childrenGap: 12 }}
                            verticalAlign="end"
                            className="rules-editor-row"
                        >
                            {renderEffectControl(norm, index)}
                            <Dropdown
                                id={fieldId(index)}
                                label={fieldLabel(index)}
                                options={fieldDropdownOptions}
                                selectedKey={norm.Field || undefined}
                                disabled={readOnly}
                                onChange={(_, opt) => updateRule(index, 'Field', opt?.key ?? '')}
                                placeholder={TranslateTag('@ConSelA@', language) || 'Select...'}
                                styles={{ root: { minWidth: 150 } }}
                            />
                            <Dropdown
                                id={ruleId(index)}
                                label={ruleLabel(index)}
                                options={ruleOptions}
                                selectedKey={norm.Rule}
                                disabled={readOnly}
                                onChange={(_, opt) => updateRule(index, 'Rule', opt?.key ?? '=')}
                                styles={{ root: { minWidth: 140 } }}
                            />
                            {needsValue(norm) && renderValueControl(norm, index)}
                            {!readOnly && (
                                <TooltipHost content={deleteRuleTooltip} tooltipProps={tooltipProps} calloutProps={calloutProps}>
                                    <IconButton
                                        data-test-id={deleteId(index) || `rule-delete-${index}`}
                                        iconProps={{ iconName: 'Delete' }}
                                        ariaLabel={deleteRuleTooltip}
                                        onClick={() => removeRule(index)}
                                        className="rules-editor-remove"
                                    />
                                </TooltipHost>
                            )}
                        </Stack>
                    );
                })}
            </Stack>
        </div>
    );
};

export default RulesEditor;
