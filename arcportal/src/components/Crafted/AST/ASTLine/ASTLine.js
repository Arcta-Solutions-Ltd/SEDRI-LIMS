import React from 'react';
import { TooltipHost } from '@fluentui/react';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import { Icon } from '@fluentui/react/lib/Icon';
import { IconButton } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import ASTMicField from '../ASTMicField/ASTMicField';
import { formatMicDisplay } from '../ASTMicField/micMeasurementUtils';
import ExpertTriggerIcon from '../ExpertTriggerIcon/ExpertTriggerIcon';
import ManualSusceptibilityIcon from '../ManualSusceptibilityIcon/ManualSusceptibilityIcon';
import {
    buildManualSusceptibilityDomIds,
    isRowManuallySetSusceptibility,
} from '../astSusceptibilityOverrideUtils';

const categoryColours = ['black', 'red', 'blue', 'green'];

/** Resolve dropdown/combobox display text from Fluent-style { key, text } options. */
function resolveListOptionText(options, rawKey) {
    if (rawKey === undefined || rawKey === null || rawKey === '') {
        return '';
    }
    const keyStr = String(rawKey);
    const found = options.find((o) => String(o.key) === keyStr);
    return found ? found.text : '';
}

/**
 * Shared row component for Disk, Mic, and Expert Rule AST lines.
 * Ensures column alignment across all three variants.
 *
 * @param {Object} props
 * @param {'disk'|'mic'|'expertRule'} props.variant - Row type: disk (dosage + zone diameter), mic (mic field), expertRule (static text + Include on report)
 * @param {Object} props.value - Row data. For expertRule rows, an unset Include on report (`IncludeOnReport` null/empty, or `IncludeOnReportEditable === true`) renders an editable toggle defaulting to Yes; an explicit Yes/No renders read-only text.
 * @param {number} props.index - Row index
 * @param {number} [props.ruleId] - Expert rule id, used to build deterministic element ids for expert action rows (Include on report toggle/text).
 * @param {number} [props.specialIndex] - Embedded row index when applicable
 * @param {'disk'|'mic'} props.type - Data type for change/delete handlers
 * @param {Function} props.changeHandler - (type, row, specialRow, value) => void
 * @param {Function} props.deleteHandler - (index, specialIndex, type) => void
 * @param {Array} props.lists - List options for dropdowns
 * @param {boolean} [props.isHighlighted] - Whether row is highlighted (e.g. on expert rule hover)
 * @param {Function} [props.applyRuleHandler] - For expert rule embedded rows
 * @param {boolean} [props.hideApplyToggle] - Hide Apply Rule toggle (e.g. when in group header)
 * @param {boolean} [props.hideIcon] - Hide expert rule icon
 * @param {Function} [props.onIconHover] - Icon hover handler
 * @param {Function} [props.onIconLeave] - Icon leave handler
 * @param {boolean} [props.manualRowSuppressedByExpertRule] - Disk/Mic: manual row is read-only while an applied expert rule with the same antibiotic is applied.
 * @param {boolean} [props.includeOnReportLockedByExpertRule] - Disk/Mic: Include on report is read-only (print-only expert rule) when the row is not fully suppressed.
 * @param {Array<{ruleId:number, ruleName:string, ruleText:string, colour:string, hasActions:boolean}>} [props.expertTriggers] - Disk/Mic: expert rules this line triggered (inline trigger icon + colour-coded hover).
 * @param {Function} [props.onManualOverrideIconClick] - Opens override audit panel for this row.
 * @param {Array<{key:string,text:string}>} [props.overrideCannedOptions] - Canned override reason options.
 * @param {boolean} [props.recordSusceptibilityChangeAudit] - Lab requires audit reason when manually setting susceptibility.
 * @param {Function} [props.resolveSusceptibilityLabel] - Maps susceptibility id to display text.
 * @param {string} [props.language] - Current language
 */
const ASTLine = (props) => {
    const { variant, value, index, specialIndex, type, changeHandler, deleteHandler, lists, isHighlighted, language } = props;

    /** Expert rows: muted text only when Apply Rule is off; applied rows use full contrast. Values are plain text, not ComboBox/Dropdown. */
    const expertGreyedOut = variant === 'expertRule' && value.ApplyRule !== 'Yes';

    /** Manual disk/MIC row suppressed because an applied expert rule targets the same antibiotic. */
    const manualSuppressed = (variant === 'disk' || variant === 'mic') && !!props.manualRowSuppressedByExpertRule;

    /** Print-only expert rule: only the Include on report toggle is locked. */
    const includeOnReportLockedByExpert =
        (variant === 'disk' || variant === 'mic') &&
        !manualSuppressed &&
        !!props.includeOnReportLockedByExpertRule;

    const newEntry = [
        { Id: 'Drugcategory', Type: '', Label: '', value: '', Options: [], Resettable: true, noTab: true, value: value.DrugCategory },
        { Id: 'Antibiotic', Type: 'combobox', Label: '', value: '', Options: [], Resettable: true, noTab: true, value: value.Antibiotic },
        { Id: 'Dosage', Type: 'number', Label: '', value: '', Min: '0', Max: '999', MaxDPs: '0', TabIndex: -1, Key: 'dosage', value: value.Dosage },
        { Id: 'Guidelines', Type: 'dropdown', Label: '', value: '', Options: [], Resettable: true, noTab: true, value: value.Guidelines },
        { Id: 'ZoneDiameter', Type: 'number', Label: '', value: '', Min: '6', Max: '50', MaxDPs: '0', Key: 'zonediameter', value: value.ZoneDiameter },
        { Id: 'TestResult', Type: 'combobox', Label: '', value: '', Options: [], Resettable: true, noTab: true, value: value.TestResult },
        { Id: 'IncludeOnReport', Type: 'toggle', Label: '', value: 'No', TabIndex: -1, value: value.IncludeOnReport },
        { Id: 'Mic', Type: 'micdosage', Label: '', value: '', Min: '0', Max: '9999', MaxDPs: '3', value: formatMicDisplay(value) },
        { Id: 'ApplyRule', Type: 'toggle', Label: '', value: 'No', TabIndex: -1, value: value.ApplyRule || 'No', Disabled: false }
    ];

    const rowIdPrefix =
        variant === 'disk' || variant === 'mic' ? `ast-${type}-row-${index}-` : null;

    if (rowIdPrefix) {
        newEntry.forEach((field) => {
            field.FieldKey = field.Id;
            field.Id = rowIdPrefix + field.Id;
        });
    }

    if (manualSuppressed) {
        for (const fld of newEntry) {
            if (['Antibiotic', 'Dosage', 'Guidelines', 'ZoneDiameter', 'TestResult', 'IncludeOnReport', 'Mic'].includes(fld.FieldKey ?? fld.Id)) {
                fld.Disabled = true;
            }
        }
    } else if (includeOnReportLockedByExpert) {
        const por = newEntry.find((fld) => (fld.FieldKey ?? fld.Id) === 'IncludeOnReport');
        if (por) {
            por.Disabled = true;
        }
    }

    const categories = ['drugcategory', 'antibiotic', 'guidelines', 'testresult'];
    categories.forEach((category) => {
        const listIndex = lists.findIndex((l) => l.Name === category);
        if (listIndex !== -1) {
            const mappedOptions = lists[listIndex].Options.map((option) => ({ key: option.Key, text: option.Text }));
            const entry = newEntry.find((i) => (i.FieldKey ?? i.Id).toLowerCase() === category);
            if (entry) entry.Options.push(...mappedOptions);
        }
    });

    const resolveFieldKey = (configId) => {
        const entry = newEntry.find((f) => f.Id === configId);
        return entry?.FieldKey ?? configId;
    };

    const manualOverrideActive = isRowManuallySetSusceptibility(value);
    const { iconId, trashId } = buildManualSusceptibilityDomIds(type, index, specialIndex);

    const susceptibilityCellClass = `astform-manual-susceptibility${manualOverrideActive ? ' astform-susceptibility-manual' : ''}`;

    const handleChange = (id, val) => {
        const fieldKey = resolveFieldKey(id);
        if (variant === 'expertRule') {
            if (fieldKey === 'ApplyRule' && index !== undefined && props.applyRuleHandler) {
                props.applyRuleHandler(type, index, specialIndex, val, value);
                return;
            }
            if (fieldKey === 'IncludeOnReport' || String(id).endsWith('IncludeOnReport')) {
                changeHandler(type, index, specialIndex, { ...value, IncludeOnReport: val }, 'IncludeOnReport');
            }
            return;
        }
        if (fieldKey === 'ApplyRule' && index !== undefined && props.applyRuleHandler) {
            props.applyRuleHandler(type, index, specialIndex, val, value);
            return;
        }
        const newValue = { ...value, [fieldKey]: val };
        changeHandler(type, index, specialIndex, newValue, fieldKey);
    };

    const showDrugCategory = variant !== 'expertRule';
    const showDosage = variant === 'disk';
    const showApplyRule = variant === 'expertRule' && index !== undefined && !props.hideApplyToggle;

    const expertActionIdPrefix =
        variant === 'expertRule' && props.ruleId !== undefined && props.ruleId !== null
            ? `ast-expert-action-${props.ruleId}-${specialIndex}`
            : null;

    /**
     * Expert action Include on report is editable (toggle) when the rule action left Print On Report unset.
     * The flag is carried on the row so the control stays a toggle after the user makes a choice, rather than
     * reverting to read-only text once a value is present.
     */
    const expertIncludeOnReportEditable =
        value.IncludeOnReportEditable === true ||
        value.IncludeOnReport === null ||
        value.IncludeOnReport === undefined ||
        value.IncludeOnReport === '';

    const includeOnReportDisplay = variant === 'expertRule' ? (
        expertIncludeOnReportEditable ? (
            <div className="astform-manual-include-in-report">
                <div className="astform-toggle">
                    <SingleLineField
                        key="expertincludeinreport"
                        config={{
                            Id: expertActionIdPrefix ? `${expertActionIdPrefix}-IncludeOnReport` : undefined,
                            FieldKey: 'IncludeOnReport',
                            Type: 'toggle',
                            Label: '',
                            ShowText: true,
                            value: value.IncludeOnReport === 'No' ? 'No' : 'Yes',
                            TabIndex: -1,
                            Disabled: expertGreyedOut
                        }}
                        changeHandler={handleChange}
                    />
                </div>
            </div>
        ) : (
            <div
                className="astform-manual-include-in-report astform-display-only"
                id={expertActionIdPrefix ? `${expertActionIdPrefix}-IncludeOnReport-text` : undefined}
            >
                {value.IncludeOnReport === 'Yes' ? TranslateTag("@GenYesA@", props.language) : TranslateTag("@GenNo@", props.language)}
            </div>
        )
    ) : (
        <div className="astform-manual-include-in-report">
            <div className="astform-toggle">
                <SingleLineField key="includeinreport" config={newEntry[6]} changeHandler={handleChange} />
            </div>
        </div>
    );

    const firstColumn = variant === 'expertRule' ? (
        <div className="astform-expert-rule-level">
            {!props.hideIcon && (
                <TooltipHost
                    content={
                        value.ExpertRuleLine ? (
                            <span>
                                <strong>{value.ExpertRuleName + ':'}</strong> {value.ExpertRuleText}
                            </span>
                        ) : (
                            ''
                        )
                    }
                    id={100}
                >
                    <div onMouseEnter={props.onIconHover} onMouseLeave={props.onIconLeave}>
                        <div className={value.ExpertRuleLine ? 'astform-expert-rule-item' : 'white'}>
                            <div>{value.ExpertRuleLine ? <Icon iconName="DecisionSolid" /> : ''}</div>
                        </div>
                    </div>
                </TooltipHost>
            )}
        </div>
    ) : (
        <div className="astform-antibiotic-level">
            <TooltipHost content={newEntry[0].Options[newEntry[0].value > 1219 ? newEntry[0].value - 1220 : 0]?.text} id={100}>
                <div className={'astform-antibiotic-level-item-' + (newEntry[0].value > 1219 ? categoryColours[newEntry[0].value - 1220] : 'white')}>
                    <div>{newEntry[0].value > 1219 ? newEntry[0].value - 1219 : ''}</div>
                </div>
            </TooltipHost>
        </div>
    );

    const expertRuleRowClass =
        variant === 'expertRule'
            ? value.ApplyRule === 'Yes'
                ? ' astform-expert-rule-line-applied'
                : ' astform-expert-rule-line-not-applied'
            : '';

    const manualSuppressedClass = manualSuppressed ? ' astform-manual-row-suppressed-by-expert' : '';

    const hasExpertTriggers =
        (variant === 'disk' || variant === 'mic') &&
        Array.isArray(props.expertTriggers) &&
        props.expertTriggers.length > 0;

    const expertStaticClass = `astform-display-only astform-expert-rule-static-text${expertGreyedOut ? ' astform-expert-rule-static-muted' : ''}`;
    const antibioticDisplayText = resolveListOptionText(newEntry[1].Options, value.Antibiotic);
    const guidelinesDisplayText = resolveListOptionText(newEntry[3].Options, value.Guidelines);
    const testResultDisplayText = resolveListOptionText(newEntry[5].Options, value.TestResult);

    const rowInner = (
        <>
            {firstColumn}
            <div
                className={`astform-manual-drug ${isHighlighted ? 'astform-row-highlight' : ''}${
                    hasExpertTriggers ? ' astform-manual-drug-with-trigger' : ''
                }`}
            >
                {variant === 'expertRule' ? (
                    <div className={expertStaticClass}>{antibioticDisplayText}</div>
                ) : (
                    <SingleLineField key="antibiotic" config={newEntry[1]} changeHandler={handleChange} />
                )}
                {hasExpertTriggers && (
                    <ExpertTriggerIcon
                        triggers={props.expertTriggers}
                        id={rowIdPrefix ? `${rowIdPrefix}expert-trigger` : undefined}
                        language={language}
                    />
                )}
            </div>
            <div className={showDosage ? 'astform-manual-dosage' : 'astform-manual-dosage hidden'}>
                <SingleLineField key="dosage" config={newEntry[2]} changeHandler={handleChange} />
            </div>
            <div className="astform-manual-guidelines">
                {variant === 'expertRule' ? (
                    <div className={expertStaticClass}>{guidelinesDisplayText}</div>
                ) : (
                    <SingleLineField key="guidelines" config={newEntry[3]} changeHandler={handleChange} />
                )}
            </div>
            <div className="astform-measurement-column">
                {variant === 'disk' && (
                    <SingleLineField key="zonediameter" config={newEntry[4]} changeHandler={handleChange} />
                )}
                {variant === 'mic' && (
                    <ASTMicField
                        key="mic"
                        config={newEntry[7]}
                        guidelines={Number(value.Guidelines) || 0}
                        changeHandler={handleChange}
                        disabled={manualSuppressed}
                    />
                )}
            </div>
            <div className={susceptibilityCellClass}>
                <div className={`astform-manual-susceptibility-inner${isHighlighted ? ' astform-row-highlight' : ''}`}>
                    {variant === 'expertRule' ? (
                        <div className={expertStaticClass}>{testResultDisplayText}</div>
                    ) : (
                        <>
                            <SingleLineField key="susceptibility" config={newEntry[5]} changeHandler={handleChange} />
                            <ManualSusceptibilityIcon
                                id={iconId}
                                trashId={trashId}
                                override={value.SusceptibilityOverride}
                                cannedOptions={props.overrideCannedOptions}
                                language={language}
                                resolveSusceptibilityLabel={props.resolveSusceptibilityLabel}
                                onClick={() => props.onManualOverrideIconClick && props.onManualOverrideIconClick(type, index, specialIndex)}
                                onRevert={() => props.onManualOverrideRevert && props.onManualOverrideRevert(type, index, specialIndex)}
                            />
                        </>
                    )}
                </div>
            </div>
            {includeOnReportDisplay}
            {showApplyRule && (
                <div className="astform-manual-apply-rule">
                    <div className="astform-toggle">
                        <SingleLineField key="applyrule" config={newEntry[8]} changeHandler={handleChange} />
                    </div>
                </div>
            )}
            {variant !== 'expertRule' && (
                <div>
                    <IconButton
                        id={rowIdPrefix ? `${rowIdPrefix}delete` : undefined}
                        tabIndex={-1}
                        iconProps={{ iconName: 'Cancel' }}
                        disabled={manualSuppressed}
                        onClick={() => deleteHandler(index, specialIndex, type)}
                    />
                </div>
            )}
        </>
    );

    return (
        <div
            key={index}
            className={`astform-test-pattern-row${expertRuleRowClass}${manualSuppressedClass}`}
            {...(manualSuppressed
                ? { 'aria-label': TranslateTag('@AstExpOvr@', language) }
                : {})}
        >
            {rowInner}
        </div>
    );
};

export default ASTLine;
