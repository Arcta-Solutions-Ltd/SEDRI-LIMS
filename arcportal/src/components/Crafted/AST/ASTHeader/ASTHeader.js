import React from 'react';
import ASTTestPattern from '../ASTTestPattern/ASTTestPattern';
import ASTDate from '../ASTDate/ASTDate';

/**
 * AST header containing Test Pattern and Date/Time components.
 * No "Test Pattern Selection:" header. Labels: "Recorded Date" and "Recorded Time".
 *
 * @param {Object} props
 * @param {Object} props.testPattern - Config for main test pattern dropdown
 * @param {Object} props.testPatternFullList - Config for full list dropdown
 * @param {Object} props.completedDate - Config for date field
 * @param {Object} props.completedTime - Config for time field
 * @param {Function} props.changeHandler - (id, value) => void
 * @param {string} props.language - Current language for translations
 */
const ASTHeader = (props) => {
    const { testPattern, testPatternFullList, completedDate, completedTime, changeHandler, language } = props;

    return (
        <div className="astform-header">
            <div className="astform-first-row">
                <div className="astform-antibiotic-level" />
                <ASTTestPattern
                    testPattern={testPattern}
                    testPatternFullList={testPatternFullList}
                    changeHandler={changeHandler}
                />
                <ASTDate
                    completedDate={completedDate}
                    completedTime={completedTime}
                    changeHandler={changeHandler}
                    language={language}
                />
            </div>
        </div>
    );
};

export default ASTHeader;
