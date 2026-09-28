import React from 'react';
import { TooltipHost } from '@fluentui/react';
import { Icon } from '@fluentui/react/lib/Icon';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import ASTLine from '../ASTLine/ASTLine';
import SpecialConsiderationRows from '../SpecialConsiderationRows/SpecialConsiderationRows';

/**
 * Single expert rule group: header with a colour swatch, rule name, tooltip, Apply Rule toggle, and AST action lines.
 * The rule colour (from the 15-colour palette) is applied to the rule icon and the swatch so the group can be linked
 * visually to the inline trigger icons next to the AST lines that triggered it.
 *
 * @param {Object} props
 * @param {string} props.ruleName - Expert rule name
 * @param {Object} props.ruleGroup - { ruleName, ruleText, ruleId, entries }
 * @param {string} props.ruleColour - Hex colour assigned to this rule.
 * @param {Object} props.astData - { DiskResults, MicResults }
 * @param {Function} props.changeHandler - (type, row, specialRow, value) => void
 * @param {Function} props.deleteHandler - (index, specialIndex, type) => void
 * @param {Function} props.onGroupApplyRuleChange - (ruleName, value, ruleGroup) => void
 * @param {Function} props.setHighlightedParentIds - (Set) => void
 * @param {Array} props.lists - List options
 * @param {string} props.language - Current language
 */
const ASTExpertRuleGroup = (props) => {
    const { ruleName, ruleGroup, ruleColour, astData, changeHandler, deleteHandler, onGroupApplyRuleChange, setHighlightedParentIds, lists, language } = props;
    const ruleId = ruleGroup?.ruleId;

    const allApplied = ruleGroup.entries.every(({ entry, isTopLevel, type, index, specialIndex }) => {
        if (isTopLevel) {
            return entry.ApplyRule === 'Yes';
        }
        const parentArray = type === 'mic' ? astData.MicResults : astData.DiskResults;
        return (
            parentArray[index] &&
            parentArray[index].EmbeddedASTRows &&
            parentArray[index].EmbeddedASTRows[specialIndex] &&
            parentArray[index].EmbeddedASTRows[specialIndex].ApplyRule === 'Yes'
        );
    });

    return (
        <div id={ruleId !== undefined && ruleId !== null ? `ast-expert-rule-group-${ruleId}` : undefined}>
            <div className="astform-test-pattern-row">
                <div className="astform-expert-rule-level">
                    <TooltipHost content={ruleGroup.ruleText}>
                        <div
                            onMouseEnter={() =>
                                setHighlightedParentIds(
                                    new Set(
                                        ruleGroup.entries.map(({ type, index, expertGroupIndex }) =>
                                            type === 'expert' ? `expert-${expertGroupIndex}` : `${type}-${index}`
                                        )
                                    )
                                )
                            }
                            onMouseLeave={() => setHighlightedParentIds(null)}
                        >
                            <div
                                className="astform-expert-rule-item"
                                id={ruleId !== undefined && ruleId !== null ? `ast-expert-rule-icon-${ruleId}` : undefined}
                                style={{ backgroundColor: ruleColour }}
                            >
                                <Icon iconName="DecisionSolid" />
                            </div>
                        </div>
                    </TooltipHost>
                </div>
                <div className="astform-expert-rule-header-drug">
                    <div
                        className="astform-expert-rule-title-text"
                        id={ruleId !== undefined && ruleId !== null ? `ast-expert-rule-name-${ruleId}` : undefined}
                    >
                        {ruleName}
                    </div>
                    <div className="">
                        <SingleLineField
                            config={{
                                Id: ruleId !== undefined && ruleId !== null ? `groupApplyRule-${ruleId}` : `groupApplyRule-${ruleName}`,
                                Type: 'toggle',
                                Label: '',
                                ShowText: false,
                                value: allApplied ? 'Yes' : 'No',
                                TabIndex: -1
                            }}
                            changeHandler={(id, value) => onGroupApplyRuleChange(ruleName, value, ruleGroup)}
                        />
                    </div>
                </div>
                <div className="astform-manual-dosage hidden" />
                <div className="astform-manual-guidelines" />
                <div className="astform-measurement-column" aria-hidden="true" />
                <div className="astform-manual-susceptibility" />
                <div className="astform-manual-include-in-report" />
                <div className="astform-manual-apply-rule hidden" aria-hidden="true" />
            </div>

            {ruleGroup.entries.map(({ entry, type, index, specialIndex, isTopLevel, expertGroupIndex }, actionIndex) => (
                <div
                    key={`${ruleName}-action-${actionIndex}`}
                    id={
                        ruleId !== undefined && ruleId !== null
                            ? `ast-expert-action-${ruleId}-${type === 'expert' ? actionIndex : specialIndex ?? actionIndex}`
                            : undefined
                    }
                >
                    <ASTLine
                        variant="expertRule"
                        value={entry}
                        index={type === 'expert' ? expertGroupIndex : isTopLevel ? undefined : index}
                        specialIndex={type === 'expert' ? actionIndex : isTopLevel ? undefined : specialIndex}
                        type={type}
                        ruleId={ruleId}
                        changeHandler={changeHandler}
                        deleteHandler={deleteHandler}
                        lists={lists}
                        hideApplyToggle={true}
                        hideIcon={true}
                        language={language}
                    />
                    {Array.isArray(entry.EmbeddedASTRows) &&
                        entry.EmbeddedASTRows.map((row, specialIdx) => {
                            const validId =
                                row.SpecialConsiderationId &&
                                row.SpecialConsiderationId !== 0 &&
                                row.SpecialConsiderationId !== 973;
                            return (
                                validId && (
                                    <SpecialConsiderationRows
                                        key={`special-${index}-${specialIdx}`}
                                        specialIndex={specialIdx}
                                        index={index}
                                        type={type}
                                        changeHandler={changeHandler}
                                        deleteHandler={deleteHandler}
                                        lists={lists}
                                        value={row}
                                        name={row.SpecialConsiderationId}
                                    />
                                )
                            );
                        })}
                </div>
            ))}
        </div>
    );
};

export default ASTExpertRuleGroup;
