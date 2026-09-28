import React from 'react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import ASTExpertRuleGroup from '../ASTExpertRuleGroup/ASTExpertRuleGroup';
import { getExpertRuleColour } from '../expertRuleColours';

/**
 * Expert Rules section: an "Expert Rules" heading and column headers matching the Disk and MIC sections,
 * then one colour-coded group per expert rule (rule name, apply toggle, and action rows).
 * Guidance (no-action) rules are not shown here; they appear in the inline trigger hover next to the AST
 * lines that triggered them. Returns null when there are no action rule groups.
 *
 * @param {Object} props
 * @param {Object} props.groupedRules - { groupKey: { ruleName, ruleText, ruleId, entries } } keyed by rule id so
 *   two rules with the same display name remain separate groups.
 * @param {Object} props.astData - { DiskResults, MicResults }
 * @param {Function} props.changeHandler - (type, row, specialRow, value) => void
 * @param {Function} props.deleteHandler - (index, specialIndex, type) => void
 * @param {Function} props.onGroupApplyRuleChange - (ruleName, value, ruleGroup) => void
 * @param {Function} props.setHighlightedParentIds - (Set) => void
 * @param {Array} props.lists - List options
 * @param {string} props.language - Current language
 * @param {Object<string,string>} props.expertRuleColourMap - Rule id -> colour map.
 */
const ASTExpertRules = (props) => {
    const {
        groupedRules,
        astData,
        changeHandler,
        deleteHandler,
        onGroupApplyRuleChange,
        setHighlightedParentIds,
        lists,
        language,
        expertRuleColourMap
    } = props;

    const ruleGroups = Object.values(groupedRules || {});
    if (ruleGroups.length === 0) {
        return null;
    }

    return (
        <div className="astform-manual-section" id="ast-expert-rules-section">
            <div className="astform-add-row-top">
                <div className="astform-antibiotic-level" />
                <span className="astform-section-label" id="ast-expert-rules-heading">
                    {TranslateTag('@AstExpRul@', language)}
                </span>
                <span className="astform-expert-finding-header-spacer" aria-hidden="true" />
            </div>
            <div className="astform-test-pattern-title-bar">
                <div className="astform-antibiotic-level" />
                <div className="astform-expert-rule-title-antibiotic">{TranslateTag('@GenAnt@', language)}</div>
                <div className="astform-manual-title-dosage" />
                <div className="astform-expert-rule-title-guidelines">{TranslateTag('@TesGui@', language)}</div>
                <div className="astform-manual-title-measurement" />
                <div className="astform-expert-rule-title-susceptibility" id="ast-expert-rules-header-susceptibility">
                    {TranslateTag('@GenSus@', language)}
                </div>
                <div className="astform-expert-rule-title-include-in-report" id="ast-expert-rules-header-include-on-report">
                    {TranslateTag('@GenInc@', language)}?
                </div>
            </div>
            {ruleGroups.map((ruleGroup, groupIndex) => (
                <ASTExpertRuleGroup
                    key={ruleGroup.ruleId !== undefined && ruleGroup.ruleId !== null ? `rule-${ruleGroup.ruleId}` : `rule-name-${ruleGroup.ruleName}-${groupIndex}`}
                    ruleName={ruleGroup.ruleName}
                    ruleGroup={ruleGroup}
                    ruleColour={getExpertRuleColour(expertRuleColourMap, ruleGroup.ruleId)}
                    astData={astData}
                    changeHandler={changeHandler}
                    deleteHandler={deleteHandler}
                    onGroupApplyRuleChange={onGroupApplyRuleChange}
                    setHighlightedParentIds={setHighlightedParentIds}
                    lists={lists}
                    language={language}
                />
            ))}
        </div>
    );
};

export default ASTExpertRules;
