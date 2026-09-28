import React from 'react';
import './RecordReportSection.css';
import RecordReportRow from '../RecordReportRow/RecordReportRow';
import RecordReportSubsection from '../RecordReportSubsection/RecordReportSubsection';

const RecordReportSection = (props) => {
    let separator = null;

    if (props.config.Type === 'Header') {
        separator = <div className="recordreportsection-separator"></div>;
    }

    return (
        <div className="">
            <div className="recordreportsection-title">
                {props.config.Title}
            </div>
            {props.config.Rows !== undefined && props.config.Rows !== null ? (
                <div>
                    {props.config.Rows.map((row) => {
                        return (
                            <RecordReportRow
                                key={row.Id}
                                config={row}
                            ></RecordReportRow>
                        );
                    })}
                </div>
            ) : null}
            {props.config.Subsections !== undefined &&
            props.config.Subsections !== null ? (
                <div>
                    {props.config.Subsections.map((subsection) => {
                        return (
                            <RecordReportSubsection
                                key={subsection.Id}
                                config={subsection}
                            ></RecordReportSubsection>
                        );
                    })}
                </div>
            ) : null}
            {separator}
            <br />
        </div>
    );
};

export default RecordReportSection;
