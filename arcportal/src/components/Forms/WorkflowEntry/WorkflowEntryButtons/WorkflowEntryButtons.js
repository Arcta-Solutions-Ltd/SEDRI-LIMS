import React from 'react';
import './WorkflowEntryButtons.css';
import { PrimaryButton, DefaultButton } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';

/**
 * Renders Cancel, Back, Next, and Finish buttons for multi-page workflow forms.
 * @param {Object} props
 * @param {Object} props.config - Button visibility and labels (displayCancelButton, displayLeftButton, etc.)
 * @param {boolean} [props.saveEnabled=true] - When false, disables the Finish button during save to prevent double-clicks.
 */
const WorkflowEntryButtons = (props) => {

    const finishStyle = {
        root: [{
            background: 'forestgreen'
          }
        ],
        rootHovered: {
            backgroundColor: 'darkgreen'
        }
    };

    let cancelButton = (null);
    let leftButton = (null);
    let rightButton = (null);
    let lowerRightButton = (null);

    if (props.config.displayCancelButton) {
        cancelButton = (
            <div className="app-button">
                <DefaultButton text={TranslateTag("@GenCan@", props.language)} onClick={props.cancelButtonClick} />
            </div>
        );
    }

    if (props.config.displayLeftButton) {
        leftButton = (
            <div className="app-button">
                <PrimaryButton text={TranslateTag("@GenBac@", props.language)} onClick={props.leftButtonClick}/>
            </div>
        );
    }

    if (props.config.displayRightButton) {
        rightButton = (
            <div className="app-button">
                <PrimaryButton
                    id="workflowentry-next-button"
                    data-testid="workflowentry-next-button"
                    text={props.config.rightButtonName}
                    onClick={props.rightButtonClick}
                />
            </div>
        );
    }

    if (props.config.displayFinishButton) {
        if (rightButton !== null) {
            lowerRightButton = (
                <div className="app-button">
                    <PrimaryButton
                        id="workflowentry-finish-button"
                        data-testid="workflowentry-finish-button"
                        text={TranslateTag("@GenFin@", props.language)} onClick={props.finishButtonClick}
                        styles={finishStyle}
                        disabled={!props.saveEnabled}
                    />
                </div>
            );
        } else {
            rightButton = (
                <div className="app-button">
                    <PrimaryButton
                        id="workflowentry-finish-button"
                        data-testid="workflowentry-finish-button"
                        text={TranslateTag("@GenFin@", props.language)} onClick={props.finishButtonClick}
                        styles={finishStyle}
                        disabled={!props.saveEnabled}
                    />
                </div>
            );
        }
    }

    // Experimental hot-keys.
    const keyDown = function(e) {
        if (e.key === "ArrowLeft" && e.ctrlKey && props.config.displayLeftButton) {
            props.leftButtonClick();
        } else if (e.key === "ArrowRight" && e.ctrlKey && props.config.displayRightButton) {
            props.rightButtonClick();
        } else if (e.key === "Enter" && e.ctrlKey && props.config.displayFinishButton && props.saveEnabled) {
            props.finishButtonClick();
        }
    };

    const labelsClass = props.fullScreen ? "workflowentrybuttons-labels" : "workflowentrybuttons-labels";
    const buttonsClass = props.fullScreen ? "workflowentrybuttons-buttons" : "workflowentrybuttons-buttons";

    return (
        <div className='workflowentrybuttons-navigation' onKeyDown={keyDown}>
            <div className={labelsClass}>
                <div>
                    {props.config.leftButtonText}
                </div>
                <div>
                    {props.config.rightButtonText}
                </div>
            </div>
            <div className={buttonsClass}>
                {cancelButton}
                {leftButton}
                {rightButton}
            </div>
            <div className="workflowentrybuttons-buttons">
                <div className="workflowentry-button workflowentrybuttons-invisible">
                    <PrimaryButton text="Dummy" disabled/>
                </div>
                {lowerRightButton}
            </div>
        </div>
    )
};
  
export default WorkflowEntryButtons;