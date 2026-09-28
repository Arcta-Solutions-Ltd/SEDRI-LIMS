import React from 'react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';

const ASTRowDiskTitle = (props) => {

    return (
        <div className='astform-test-pattern-title-bar'>
            <div className='astform-antibiotic-level'>
            </div>
            <div className='astform-manual-title-antibiotic'>
                {TranslateTag("@GenAnt@", props.language)}
            </div>
            <div className='astform-manual-title-dosage'>
                {TranslateTag("@GenDos@", props.language)}&nbsp;(ug)
            </div>
            <div className='astform-manual-title-guidelines'>
                {TranslateTag("@TesGui@", props.language)}
            </div>
            <div className='astform-manual-title-zone-diameter'>
                {TranslateTag("@ZonDia@", props.language)}&nbsp;(mm)
            </div>
            <div className='astform-manual-title-susceptibility'>
                {TranslateTag("@GenSus@", props.language)}
            </div>
            <div className='astform-manual-title-include-in-report'>
                {TranslateTag("@GenInc@", props.language)}?
            </div>
        </div>
    );
};
  
export default ASTRowDiskTitle;