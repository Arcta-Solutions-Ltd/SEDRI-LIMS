import React from 'react';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import { IconButton } from '@fluentui/react';
import ExpertTriggerIcon from '../ExpertTriggerIcon/ExpertTriggerIcon';
import ManualSusceptibilityIcon from '../ManualSusceptibilityIcon/ManualSusceptibilityIcon';
import {
    buildManualSusceptibilityDomIds,
    isRowManuallySetSusceptibility,
} from '../astSusceptibilityOverrideUtils';

/**
 * Embedded special-consideration row under a parent Disk or MIC AST line.
 */
const SpecialConsiderationRows = (props) => {

    const index = props.index;

    const specialConsiderationId = props.value.SpecialConsiderationId ?? props.name ?? 0;
    const rowDomPrefix = `ast-${props.type}-special-${specialConsiderationId}-parent-${props.index}-`;
    const hasExpertTriggers = Array.isArray(props.expertTriggers) && props.expertTriggers.length > 0;
    const includeOnReportValue = props.value?.IncludeOnReport ?? 'No';

    const newEntry = [
        { Id: rowDomPrefix + 'TestResult', FieldKey: 'TestResult', Type: 'combobox', Label: '', Options: [], Resettable: true, noTab: true, value: props.value.TestResult },
        { Id: rowDomPrefix + 'IncludeOnReport', FieldKey: 'IncludeOnReport', Type: 'toggle', Label: '', TabIndex: -1, value: includeOnReportValue },
    ];

    const optionIndex = props.lists.findIndex(l => l.Name === 'testresult');
    if (optionIndex !== -1) {
        const mappedOptions = props.lists[optionIndex].Options.map(option => ({ key: option.Key, text: option.Text }));
        newEntry.find(i => i.FieldKey === 'TestResult').Options.push(...mappedOptions);
    }

    if (props.parentSuppressedByExpert) {
        newEntry[0].Disabled = true;
        newEntry[1].Disabled = true;
    }

    const changeHandler = (id, value) => {
        const row = props.index;
        const specialRow = props.specialIndex;
        const type = props.type;
        const entry = newEntry.find((f) => f.Id === id);
        const fieldKey = entry?.FieldKey ?? id;
        const newValue = { ...props.value, [fieldKey]: value };
        props.changeHandler(type, row, specialRow, newValue, fieldKey);
    };

    const manualOverrideActive = isRowManuallySetSusceptibility(props.value);
    const { iconId, trashId } = buildManualSusceptibilityDomIds(props.type, props.index, props.specialIndex);
    const susceptibilityCellClass = `astform-manual-susceptibility${manualOverrideActive ? ' astform-susceptibility-manual' : ''}`;

    return (
        <div
            id={rowDomPrefix + 'row'}
            className={`astform-special-consider-row${props.parentSuppressedByExpert ? ' astform-manual-row-suppressed-by-expert' : ''}`}
        >
            <div className='astform-special-consider-padding'>
            </div>
            <div className={`astform-manual-label-special-consider${hasExpertTriggers ? ' astform-special-consider-label-with-trigger' : ''}`}>
                <span>{props.value.SpecialConsideration}</span>
                {hasExpertTriggers && (
                    <ExpertTriggerIcon
                        triggers={props.expertTriggers}
                        id={rowDomPrefix + 'expert-trigger'}
                        language={props.language}
                    />
                )}
            </div>
            <div className={susceptibilityCellClass}>
                <div className="astform-manual-susceptibility-inner">
                    <SingleLineField
                        key="susceptibility"
                        config={newEntry[0]}
                        changeHandler={changeHandler}
                    />
                    <ManualSusceptibilityIcon
                        id={iconId}
                        trashId={trashId}
                        override={props.value.SusceptibilityOverride}
                        cannedOptions={props.overrideCannedOptions}
                        language={props.language}
                        resolveSusceptibilityLabel={props.resolveSusceptibilityLabel}
                        onClick={() => props.onManualOverrideIconClick && props.onManualOverrideIconClick(props.type, props.index, props.specialIndex)}
                        onRevert={() => props.onManualOverrideRevert && props.onManualOverrideRevert(props.type, props.index, props.specialIndex)}
                    />
                </div>
            </div>
            <div className='astform-manual-include-in-report'>
                <div className='astform-toggle'>
                    <SingleLineField
                        key="includeonreport"
                        config={newEntry[1]}
                        changeHandler={changeHandler}
                    />
                </div>
            </div>
            <div>
                <IconButton
                    tabIndex={-1}
                    iconProps={{ iconName: 'Cancel' }}
                    disabled={props.parentSuppressedByExpert}
                    onClick={() => props.deleteHandler(props.index, props.specialIndex, props.type)}
                />
            </div>
        </div>
    );
};

export default SpecialConsiderationRows;
