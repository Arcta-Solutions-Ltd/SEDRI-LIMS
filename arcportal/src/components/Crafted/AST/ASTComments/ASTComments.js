import React from 'react';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import TranslateTag from '../../../../Utils/Local/TranslateTag';

/**
 * AST comments section. No header.
 * Contains ASTCommentOne, ASTCommentTwo, ASTAdditionalNotes.
 *
 * @param {Object} props
 * @param {Object} props.commentOne - Config for first comment
 * @param {Object} props.commentTwo - Config for second comment
 * @param {Object} props.additionalNotes - Config for additional notes
 * @param {Function} props.changeHandler - (id, value) => void
 */
const ASTComments = (props) => {
    const { commentOne, commentTwo, additionalNotes, changeHandler } = props;

    return (
        <div>
            <div className="astform-section-label-alt">
                {TranslateTag('@GenComE@', props.language)}
            </div>

            <div className="astform-comments-section">
                <div className="astform-first-row">
                    <div className="astform-antibiotic-level" />
                    <div className="astform-comments">
                        {commentOne !== null && (
                            <SingleLineField
                                key={commentOne.Id}
                                config={commentOne}
                                changeHandler={changeHandler}
                            />
                        )}
                    </div>
                </div>
                <div className="astform-first-row">
                    <div className="astform-antibiotic-level" />
                    <div className="astform-comments">
                        {commentTwo !== null && (
                            <SingleLineField
                                key={commentTwo.Id}
                                config={commentTwo}
                                changeHandler={changeHandler}
                            />
                        )}
                    </div>
                </div>
                <div className="astform-first-row">
                    <div className="astform-antibiotic-level" />
                    <div className="astform-comments">
                        {additionalNotes !== null && (
                            <SingleLineField
                                key={additionalNotes.Id}
                                config={additionalNotes}
                                changeHandler={changeHandler}
                            />
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ASTComments;
