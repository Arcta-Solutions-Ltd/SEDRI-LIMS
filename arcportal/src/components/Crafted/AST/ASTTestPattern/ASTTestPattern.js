import React from 'react';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';

/**
 * Test pattern dropdown selectors for AST.
 * Displays main test pattern and full list dropdowns without a section header.
 *
 * @param {Object} props
 * @param {Object} props.testPattern - Config for main test pattern dropdown
 * @param {Object} props.testPatternFullList - Config for full list dropdown
 * @param {Function} props.changeHandler - (id, value) => void
 */
const ASTTestPattern = (props) => {
    const { testPattern, testPatternFullList, changeHandler } = props;
    return (
        <div className="astform-test-pattern-fields">
            {testPattern !== null && (
                <SingleLineField key={testPattern.Id} config={testPattern} changeHandler={changeHandler} />
            )}
            {testPatternFullList !== null && (
                <SingleLineField key={testPatternFullList.Id} config={testPatternFullList} changeHandler={changeHandler} />
            )}
        </div>
    );
};

export default ASTTestPattern;
