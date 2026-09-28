import React from 'react';
import { IconButton } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import ASTLineTitle from '../ASTLineTitle/ASTLineTitle';
import ASTLine from '../ASTLine/ASTLine';
import SpecialConsiderationRows from '../SpecialConsiderationRows/SpecialConsiderationRows';

/**
 * Disk AST results section. No "Disk Test" header.
 * Uses shared ASTLine with variant="disk" and + button at top to add rows.
 *
 * @param {Object} props
 * @param {Array} props.results - DiskResults array from astData
 * @param {Function} props.changeHandler - (type, row, specialRow, value) => void
 * @param {Function} props.deleteHandler - (index, specialIndex, type) => void
 * @param {Function} props.addHandler - () => void
 * @param {Array} props.lists - List options
 * @param {Set} [props.highlightedParentIds] - Set of "disk-{index}" for hover highlight
 * @param {boolean[]} [props.manualRowSuppressedFlags] - When true, manual row is overridden by an applied expert rule (same antibiotic).
 * @param {boolean[]} [props.includeOnReportLockedByExpertFlags] - When true, only Include on report is locked (print-only expert rule).
 * @param {Object<string,Array>} [props.expertTriggerMap] - AST line identity -> expert rules it triggered (inline trigger icon).
 * @param {string} props.language - Current language
 */
const DISK_TEST_METHOD = 681;

/** Stable React list key from id-based line match (681 + antibiotic + guidelines + dosage). */
const diskRowReactKey = (entry, index) => {
    const antibioticId = Number(entry.Antibiotic) || 0;
    const guidelinesId = Number(entry.Guidelines) || 0;
    const dosage = Number(entry.Dosage) || 0;
    if (antibioticId !== 0 && guidelinesId !== 0) {
        return `${DISK_TEST_METHOD}|${antibioticId}|${guidelinesId}|${dosage}`;
    }
    return `disk-row-${index}`;
};

const ASTDiskResults = (props) => {
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
                <span className="astform-section-label">{TranslateTag("@AstDis@", language)}
                    <IconButton
                        tabIndex={-1}
                        iconProps={{ iconName: 'Add' }}
                        title="Add"
                        onClick={addHandler}
                    />
                </span>
            </div>
            {results.length > 0 && <ASTLineTitle language={language} variant="disk" />}
            {results.map((entry, index) => {
                if (entry.ExpertRuleLine) return null;
                const antibioticId = Number(entry.Antibiotic) || 0;
                const lineTriggers = expertTriggerMap?.[`${DISK_TEST_METHOD}:${antibioticId}:0`];
                return (
                    <div key={diskRowReactKey(entry, index)}>
                        <ASTLine
                            variant="disk"
                            value={entry}
                            index={index}
                            type="disk"
                            changeHandler={changeHandler}
                            deleteHandler={deleteHandler}
                            lists={lists}
                            isHighlighted={highlightedParentIds?.has(`disk-${index}`)}
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
                                expertTriggerMap?.[`${DISK_TEST_METHOD}:${antibioticId}:${Number(row.SpecialConsiderationId) || 0}`];
                            return (
                                <SpecialConsiderationRows
                                    key={specialIndex}
                                    specialIndex={specialIndex}
                                    index={index}
                                    type="disk"
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

export default ASTDiskResults;
