import React from 'react';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import './IqcResultLine.css';
import TranslateTag from '../../../../Utils/Local/TranslateTag';

const IqcResultLine = (props) => {
    var resultConfig = {
        Id: props.resultId,
        Type: 'number',
        Min: '0',
        Max: '999',
        MaxDPs: '3',
        Required: false,
        value: props.resultValue,
    };

    const isOutOfRange = (value) => value !== null && (value < props.expectedLowerValue || value > props.expectedUpperValue) ? 'warn' : null;

    return (
        <div className="iqc-result">
            <div>{props.antibioticName}</div>
            <div><SingleLineField
                config={resultConfig}
                changeHandler={props.changeHandler}
            ></SingleLineField></div>
            <div className={isOutOfRange(props.resultValue)}>{TranslateTag("@GenExpVal@", props.language)}: {props.expectedLowerValue}-{props.expectedUpperValue}</div>
        </div>
    );
};

export default IqcResultLine;
