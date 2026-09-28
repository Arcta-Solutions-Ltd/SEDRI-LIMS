import React from 'react';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import TranslateTag from '../../../../Utils/Local/TranslateTag';

/**
 * Date and time selection fields for AST finalisation.
 * Labels: "Recorded Date" and "Recorded Time". No section header.
 * Fields are aligned horizontally.
 *
 * @param {Object} props
 * @param {Object} props.completedDate - Config for date field
 * @param {Object} props.completedTime - Config for time field
 * @param {Function} props.changeHandler - (id, value) => void
 * @param {string} props.language - Current language for translations
 */
const ASTDate = (props) => {
    const { completedDate, completedTime, changeHandler, language } = props;

    const dateConfig = completedDate
        ? { ...completedDate, Label: TranslateTag("@GenDatE@", language) }
        : null;
    const timeConfig = completedTime
        ? { ...completedTime, Label: TranslateTag("@GenTim@", language) }
        : null;

    return (
        <div className="astform-date-time-group">
            {dateConfig !== null && (
                <SingleLineField key={dateConfig.Id} config={dateConfig} changeHandler={changeHandler} />
            )}
            {timeConfig !== null && (
                <SingleLineField key={timeConfig.Id} config={timeConfig} changeHandler={changeHandler} />
            )}
        </div>
    );
};

export default ASTDate;
