import React from 'react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';

/**
 * Shared column header row for Disk and Mic AST result sections.
 * Displays all column headers (Antibiotic, Dosage, Guidelines, measurement column, Susceptibility, Include on report)
 * so that Disk and Mic rows align. The measurement column label depends on variant: "Zone Diameter (mm)" for disk,
 * "MIC (ug/mL)" for mic.
 *
 * @param {Object} props
 * @param {string} props.language - Current language for translations
 * @param {'disk'|'mic'} props.variant - Section type: disk shows Zone Diameter (mm), mic shows MIC (ug/mL)
 */
const ASTLineTitle = (props) => {
    const measurementLabel =
        props.variant === 'disk'
            ? `${TranslateTag("@ZonDia@", props.language)}\u00A0(mm)`
            : `${TranslateTag("@AstMicA@", props.language)}\u00A0(ug/mL)`;

    const dosageLabel =
        props.variant === 'disk'
            ? `${TranslateTag('@GenDos@', props.language)}\u00A0(ug)`
            : ``;

    return (
        <div className="astform-test-pattern-title-bar">
            <div className="astform-antibiotic-level" />
            <div className="astform-manual-title-antibiotic">
                {TranslateTag("@GenAnt@", props.language)}
            </div>
            <div className="astform-manual-title-dosage">{dosageLabel}</div>
            <div className="astform-manual-title-guidelines">
                {TranslateTag("@TesGui@", props.language)}
            </div>
            <div className="astform-manual-title-measurement">
                {measurementLabel}
            </div>
            <div className="astform-manual-title-susceptibility">
                {TranslateTag("@GenSus@", props.language)}
            </div>
            <div className="astform-manual-title-include-in-report">
                {TranslateTag("@GenInc@", props.language)}?
            </div>
        </div>
    );
};

export default ASTLineTitle;
