/**
 * PageRulesEditor - configures when a page is shown and which workflow state the page sets.
 *
 * The value maps to the backend PageRuleDetailModel:
 * { VisibleWhenState, StateRules: [{ Effect, Field, Rule, Value }], ...legacy flat fields }.
 *
 * "Show this page when" writes a form level rule { Outcome: 'visible', page, state }.
 * "This page sets the state" uses RulesEditor with one row per state rule. Rows sharing the same
 * Effect are ANDed at runtime; different Effect values are evaluated independently (branching).
 *
 * @param {Object} props
 * @param {Object} props.value - Current page rules including StateRules array
 * @param {Function} props.onChange - Callback with the updated page rules object
 * @param {Array<{id: string, label: string, page?: string, pageTitle?: string}>} props.stateOptions - States this page may be gated by
 * @param {Array<{id: string, label: string, type?: string, optionsName?: string}>} props.fieldOptions - Fields on this page usable in the state condition
 * @param {boolean} props.stateLocked - True when the state the page sets is system owned
 * @param {Array} [props.language] - Language array for TranslateTag
 * @param {Array<{Name: string, Options: Array<{Key: string, Text: string}>}>} [props.lists] - List definitions used to resolve condition values to ids
 */
import React, { useEffect, useState } from 'react';
import { Dropdown } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import RulesEditor from '../RulesEditor/RulesEditor';
import './PageRulesEditor.css';

const ALWAYS_KEY = '__always__';

const normalizeStateRule = (rule) => ({
    Effect: (rule?.Effect ?? rule?.effect ?? '').toLowerCase(),
    Field: rule?.Field ?? rule?.field ?? '',
    Rule: rule?.Rule ?? rule?.rule ?? '=',
    Value: rule?.Value ?? rule?.value ?? '',
});

const buildStateRulesFromValue = (pageRules) => {
    const fromArray = (pageRules?.StateRules ?? pageRules?.stateRules ?? [])
        .filter((rule) => rule != null)
        .map(normalizeStateRule)
        .filter((rule) => rule.Effect || rule.Field || rule.Rule || rule.Value);

    if (fromArray.length > 0) {
        return fromArray;
    }

    if (pageRules?.SetsState) {
        return [normalizeStateRule({
            Effect: pageRules.SetsState,
            Field: pageRules.ConditionField,
            Rule: pageRules.ConditionRule,
            Value: pageRules.ConditionValue,
        })];
    }

    return [];
};

const stateRulesToPageRules = (pageRules, rules) => {
    const stateRules = rules.map(normalizeStateRule);
    const firstRule = stateRules[0];

    return {
        ...pageRules,
        StateRules: stateRules,
        SetsState: firstRule?.Effect ?? '',
        ConditionField: firstRule?.Field ?? '',
        ConditionRule: firstRule?.Rule ?? '=',
        ConditionValue: firstRule?.Value ?? '',
    };
};

const PageRulesEditor = (props) => {
    const language = props.language || [];
    const stateOptions = props.stateOptions || [];
    const fieldOptions = props.fieldOptions || [];
    const stateLocked = props.stateLocked === true;

    const value = {
        VisibleWhenState: '',
        SetsState: '',
        ConditionField: '',
        ConditionRule: '=',
        ConditionValue: '',
        StateRules: [],
        ...(props.value || {}),
    };

    const [stateRules, setStateRules] = useState(() => buildStateRulesFromValue(value));

    useEffect(() => {
        const merged = {
            VisibleWhenState: '',
            SetsState: '',
            ConditionField: '',
            ConditionRule: '=',
            ConditionValue: '',
            StateRules: [],
            ...(props.value || {}),
        };
        const fromProps = buildStateRulesFromValue(merged);
        setStateRules((current) => {
            if (fromProps.length > 0) return fromProps;
            if (current.length > 0 && fromProps.length === 0 && !merged.SetsState) return current;
            return fromProps;
        });
    }, [props.value]);

    const translate = (tag, fallback) => TranslateTag(tag, language) || fallback;

    const update = (key, newValue) => {
        props.onChange({ ...value, [key]: newValue ?? '' });
    };

    const visibleWhenOptions = [
        { key: ALWAYS_KEY, text: translate('@ConAlw@', 'Always') },
        ...stateOptions.map((option) => ({
            key: option.id,
            text: option.pageTitle
                ? `${option.id} (${TranslateTag(option.pageTitle, language) || option.pageTitle})`
                : option.id,
        })),
    ];

    if (value.VisibleWhenState && !visibleWhenOptions.some((o) => o.key === value.VisibleWhenState)) {
        visibleWhenOptions.push({ key: value.VisibleWhenState, text: value.VisibleWhenState });
    }

    const stateRulesChangeHandler = (rules) => {
        setStateRules(rules);
        props.onChange(stateRulesToPageRules(value, rules));
    };

    return (
        <div className="pagerules-editor" id="pagerules-editor">
            <div className="pagerules-editor-section">
                <div className="pagerules-editor-header">{translate('@ConShoWhe@', 'Show this page when')}</div>
                <Dropdown
                    id="pagerules-visiblewhen"
                    options={visibleWhenOptions}
                    selectedKey={value.VisibleWhenState || ALWAYS_KEY}
                    onChange={(_, option) => update('VisibleWhenState', option?.key === ALWAYS_KEY ? '' : option?.key)}
                    styles={{ root: { maxWidth: 400 } }}
                />
            </div>

            <div className="pagerules-editor-section">
                <div className="pagerules-editor-header">{translate('@ConSetSta@', 'This page sets the state')}</div>
                <RulesEditor
                    rules={stateRules}
                    onChange={stateRulesChangeHandler}
                    fieldOptions={fieldOptions}
                    lists={props.lists || []}
                    language={language}
                    effectAsTextField
                    hideHeader
                    readOnly={stateLocked}
                    usePageRulesIds
                />
                {stateLocked && (
                    <div className="pagerules-editor-note" id="pagerules-state-locked">
                        {translate('@ConPagLocB@', 'The state this page sets is controlled by the system and cannot be changed here.')}
                    </div>
                )}
            </div>
        </div>
    );
};

export default PageRulesEditor;
