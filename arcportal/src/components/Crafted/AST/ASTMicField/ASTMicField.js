import React, { useCallback } from 'react';

import MicDosageNumber from '../../../Forms/ArcNumber/MicDosageNumber';

import { roundToValidMIC } from './micRounding';

import { parseMicMeasurement } from './micMeasurementUtils';



/**

 * MIC input field with CLSI/EUCAST round-up on blur; non-standard guidelines pass through unchanged.

 */

const ASTMicField = (props) => {

    const { config, guidelines, changeHandler, disabled } = props;

    const configWithDisabled =

        disabled === true ? { ...config, Disabled: true } : config;



    /**

     * Handles blur from MicDosageNumber. Clears legacy lone {@code <}; forwards operator-only

     * {@code >} and {@code <=} so server save validation can return {@code @AstMicMea@}.

     */

    const handleChange = useCallback(

        (id, value) => {

            if (value === '' || value === '<') {

                changeHandler(id, '');

                return;

            }

            const parsed = parseMicMeasurement(value);

            if (parsed.isBlank) {

                changeHandler(id, '');

                return;

            }

            if (parsed.operatorOnly) {

                changeHandler(id, value);

                return;

            }

            if (!parsed.hasNumericValue) {

                changeHandler(id, value);

                return;

            }

            const rounded = roundToValidMIC(parsed.numeric, guidelines, parsed.numericString);

            const result = parsed.operator ? parsed.operator + rounded : rounded;

            changeHandler(id, result);

        },

        [guidelines, changeHandler]

    );



    return (

        <div className="singlelinefield-item">

            <div className="singlelinefield-data">

                <MicDosageNumber config={configWithDisabled} changeHandler={handleChange} />

            </div>

        </div>

    );

};



export default ASTMicField;

