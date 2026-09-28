import React from 'react';
import './RecordReportField.css';

const RecordReportField = (props) => {

    let value = props.config.Value;
    if (value === undefined || value === null || value === '') {
        value = <div>&nbsp;</div>;
    }

    let label = (null);
    if (props.config.NoLabel === undefined || props.config.NoLabel === false) {
        label = (
            <div className="recordreportfield-label">
                {props.config.Label}
            </div>
        );
    }

    let valueClass = "";

    if (props.config.WideValue !== undefined && props.config.WideValue === true) {
        valueClass = "recordreportfield-value-wide";
    } else {
        valueClass = "recordreportfield-value";
    }

    if (props.config.NoBox === undefined || props.config.NoBox === false) {
        valueClass += " recordreportfield-box";
    }

    if (props.config.Style !== undefined && props.config.Style !== '') {
        switch (props.config.Style) {
            case "heading1":
                valueClass += " recordreportfield-heading1";
                break;
            case "heading2":
                valueClass += " recordreportfield-heading2";
                break;
            case "heading3":
                valueClass += " recordreportfield-heading3";
                break;
            case "emphasis":
                valueClass += " recordreportfield-emphasis";
                break;
            case "emphasis-centre":
                valueClass += " recordreportfield-emphasis-centre";
                break;
            default:
                break;
        }
    }

    return (
        <div className="recordreportfield-content">
            {label}
            <div className={valueClass}>
                {value}
            </div>
        </div>
    )
};

export default RecordReportField;
