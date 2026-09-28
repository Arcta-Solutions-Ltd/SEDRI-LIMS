import React from 'react';
import './RecordReportSubsection.css';
import RecordReportRow from '../RecordReportRow/RecordReportRow';

const RecordReportSubsection = (props) => {

    let padding = (null);
    if (props.config.Padding !== undefined && props.config.Padding === true) {
        padding = <div><br /><br /></div>
    }

    return (
        <div>
            <div className="recordreportsubsection-title">
                {props.config.Title}
            </div>
            {props.config.Rows !== undefined && props.config.Rows !== null ? (
                <div>
                    {props.config.Rows.map((row) => {
                        return <RecordReportRow key={row.Id} config={row}></RecordReportRow>
                    })}
                </div>
            ) : (null)}
            {props.config.Subsections !== undefined && props.config.Subsections !== null ? (
                <div>
                    {props.config.Subsections.map((subsection) => {
                        return <RecordReportSubsection key={subsection.Id} config={subsection}></RecordReportSubsection>
                    })}
                </div>
            ) : (null)}
            {padding}
        </div>
    )
};

export default RecordReportSubsection;
