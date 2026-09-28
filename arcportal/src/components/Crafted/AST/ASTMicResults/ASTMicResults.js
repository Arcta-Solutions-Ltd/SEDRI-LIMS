import React from 'react';
import { IconButton } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import ASTLineTitle from '../ASTLineTitle/ASTLineTitle';
import ASTLine from '../ASTLine/ASTLine';
import SpecialConsiderationRows from '../SpecialConsiderationRows/SpecialConsiderationRows';

/**
 * Mic AST results section. No "Mic Tests" header.
 * Uses shared ASTLine with variant="mic" (includes ASTMicField with rounding) and + button at top.
 *
 * @param {Object} props
 * @param {Array} props.results - MicResults array from astData
 * @param {Function} props.changeHandler - (type, row, specialRow, value) => void
 * @param {Function} props.deleteHandler - (index, specialIndex, type) => void
 * @param {Function} props.addHandler - () => void
 * @param {Array} props.lists - List options
 * @param {Set} [props.highlightedParentIds] - Set of "mic-{index}" for hover highlight
 * @param {boolean[]} [props.manualRowSuppressedFlags] - When true, manual row is overridden by an applied expert rule (same antibiotic).
 * @param {boolean[]} [props.includeOnReportLockedByExpertFlags] - When true, only Include on report is locked (print-only expert rule).
 * @param {Object<string,Array>} [props.expertTriggerMap] - AST line identity -> expert rules it triggered (inline trigger icon).
 * @param {string} props.language - Current language
 */
const MIC_TEST_METHOD = 680;

/** Stable React list key from id-based line match (680 + antibiotic + guidelines; MIC ignores dosage). */
const micRowReactKey = (entry, index) => {
    const antibioticId = Number(entry.Antibiotic) || 0;
    const guidelinesId = Number(entry.Guidelines) || 0;
    if (antibioticId !== 0 && guidelinesId !== 0) {
        return `${MIC_TEST_METHOD}|${antibioticId}|${guidelinesId}|0`;
    }
    return `mic-row-${index}`;
};

const ASTMicResults = (props) => {
    const {
        results,
        changeHandler,
        deleteHandler,
        addHandler,
        lists,
        highlightedParentIds,
        language,
        manualRowSuppressedFlags,
        includeOnReportLockedByExpertFlags,
        expertTriggerMap,
        recordSusceptibilityChangeAudit,
        overrideCannedOptions,
        onManualOverrideIconClick,
        onManualOverrideRevert,
        resolveSusceptibilityLabel,
    } = props;

    return (
        <div className="astform-manual-section">
            <div className="astform-add-row-top">
                <div className="astform-antibiotic-level" />
                <span className="astform-section-label">{TranslateTag("@AstMic@", language)}
                    <IconButton
                        tabIndex={-1}
                        iconProps={{ iconName: 'Add' }}
                        title="Add"
                        onClick={addHandler}
                    />
                </span>
            </div>
            {results.length > 0 && <ASTLineTitle language={language} variant="mic" />}
            {results.map((entry, index) => {
                if (entry.ExpertRuleLine) return null;
                const antibioticId = Number(entry.Antibiotic) || 0;
                const lineTriggers = expertTriggerMap?.[`${MIC_TEST_METHOD}:${antibioticId}:0`];
                return (
                    <div key={micRowReactKey(entry, index)}>
                        <ASTLine
                            variant="mic"
                            value={entry}
                            index={index}
                            type="mic"
                            changeHandler={changeHandler}
                            deleteHandler={deleteHandler}
                            lists={lists}
                            isHighlighted={highlightedParentIds?.has(`mic-${index}`)}
                            manualRowSuppressedByExpertRule={!!manualRowSuppressedFlags?.[index]}
                            includeOnReportLockedByExpertRule={!!includeOnReportLockedByExpertFlags?.[index]}
                            expertTriggers={lineTriggers}
                            language={language}
                            recordSusceptibilityChangeAudit={recordSusceptibilityChangeAudit}
                            overrideCannedOptions={overrideCannedOptions}
                            onManualOverrideIconClick={onManualOverrideIconClick}
                            onManualOverrideRevert={onManualOverrideRevert}
                            resolveSusceptibilityLabel={resolveSusceptibilityLabel}
                        />
                        {entry.EmbeddedASTRows?.map((row, specialIndex) => {
                            if (row.SpecialConsiderationId == null || row.SpecialConsiderationId === 0 || row.SpecialConsiderationId === 973) {
                                return null;
                            }
                            const scTriggers =
                                expertTriggerMap?.[`${MIC_TEST_METHOD}:${antibioticId}:${Number(row.SpecialConsiderationId) || 0}`];
                            return (
                                <SpecialConsiderationRows
                                    key={specialIndex}
                                    specialIndex={specialIndex}
                                    index={index}
                                    type="mic"
                                    changeHandler={changeHandler}
                                    deleteHandler={deleteHandler}
                                    lists={lists}
                                    value={row}
                                    name={row.SpecialConsiderationId}
                                    parentSuppressedByExpert={!!manualRowSuppressedFlags?.[index]}
                                    expertTriggers={scTriggers}
                                    language={language}
                                    recordSusceptibilityChangeAudit={recordSusceptibilityChangeAudit}
                                    overrideCannedOptions={overrideCannedOptions}
                                    onManualOverrideIconClick={onManualOverrideIconClick}
                                    onManualOverrideRevert={onManualOverrideRevert}
                                    resolveSusceptibilityLabel={resolveSusceptibilityLabel}
                                />
                            );
                        })}
                    </div>
                );
            })}
        </div>
    );
};

export default ASTMicResults;
