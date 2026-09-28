import React from 'react';
import './RecordReportRow.css';
import RecordReportField from '../RecordReportField/RecordReportField'

const RecordReportRow = (props) => {

    return (
        <div>
            {props.config.Fields !== undefined && props.config.Fields !== null ? (
                <div className="recordreportrow-content">
                    {props.config.Fields.map((field) => {
                        return <RecordReportField key={field.Id} config={field}></RecordReportField>
                    })}
                </div>
            ) : (null)}
        </div>
    )
};

export default RecordReportRow;
