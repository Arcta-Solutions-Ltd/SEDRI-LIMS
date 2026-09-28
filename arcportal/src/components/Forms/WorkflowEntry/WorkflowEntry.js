import React, { useEffect } from 'react';
import './WorkflowEntry.css';
import SinglePageEntry from '../SinglePageEntry/SinglePageEntry';
import { EvaluateFormStateRules, WillThisBeTheLastPage, CalculateStateFromFormStateRules } from '../../../Utils/State/ApplyFormStateRules';
import PanelBar from '../../General/PanelBar/PanelBar';
import IsThisTheFirstPage from '../../../Utils/PageStructure/IsThisTheFirstPage';
import IsThisTheLastPage from '../../../Utils/PageStructure/IsThisTheLastPage';
import GetPreviousPage from '../../../Utils/PageStructure/GetPreviousPage';
import GetNextPage from '../../../Utils/PageStructure/GetNextPage';
import WorkflowEntryButtons from './WorkflowEntryButtons/WorkflowEntryButtons';
import CraftedFactory from '../../Crafted/CraftedFactory';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import TranslateTag from '../../../Utils/Local/TranslateTag';

const WorkflowEntry = (props) => {

    const currentPage = props.currentPage;
    let pagesToDisplay = props.config.Pages.filter(p => p.Visible);
    let completedPages = [];

    let foundPage = false;
    for (const page of pagesToDisplay) {
        if (! foundPage) {
            completedPages.push({text: page.PageTitle, key: page.Name });
        }
        if (page.Name === currentPage.Name) {
            foundPage = true;
        }
    }

    const leftButtonClickHandler = () => {
        props.previous(currentPage);
    }

    const rightButtonClickHandler = (fieldChangesToUse, sourceOfClick) => {
        props.next(currentPage, fieldChangesToUse, sourceOfClick);
    }

    const finishButtonClickhandler = () => {
        props.save(currentPage);
    }

    const isFirstPage = IsThisTheFirstPage(currentPage, pagesToDisplay);
    const isLastPage = IsThisTheLastPage(currentPage, pagesToDisplay);
    const buttonConfig = {
        displayCancelButton : isFirstPage,
        displayLeftButton: ! isFirstPage,
        leftButtonText: isFirstPage ? "" : '<- ' + GetPreviousPage(currentPage, pagesToDisplay).PageTitle,
        displayRightButton: ! isLastPage,
        rightButtonText: isLastPage ? "" : GetNextPage(currentPage, pagesToDisplay).PageTitle + ' ->',
        displayFinishButton: isLastPage || currentPage.optionalFinish,
        rightButtonName: TranslateTag("@GenNex@", props.language) 
    }

    if (currentPage.NextButton !== "undefined" && currentPage.NextButton !== "") {
        buttonConfig.rightButtonName = currentPage.NextButton.ButtonText;
        buttonConfig.rightButtonText = "";
        buttonConfig.displayRightButton = buttonConfig.displayRightButton && currentPage.NextButton.Show;
    }

    if (currentPage.CancelButton !== "undefined" && currentPage.CancelButton !== "") {
        buttonConfig.displayCancelButton = buttonConfig.displayCancelButton && currentPage.CancelButton.Show;
    }

    if (currentPage.PrevButton !== "undefined" && currentPage.PrevButton !== "") {
        buttonConfig.leftButtonText = "";
        buttonConfig.displayLeftButton = buttonConfig.displayLeftButton && currentPage.PrevButton.Show;
    }

    if (! EvaluateFormStateRules(currentPage.NextButton, props.data)) {

        const newState = CalculateStateFromFormStateRules(currentPage, props.state, "page");

        buttonConfig.displayFinishButton = WillThisBeTheLastPage(newState, props.config.Rules, pagesToDisplay, currentPage.Name);

        //buttonConfig.displayFinishButton = WillThisBeTheLastPage(currentPage.NextButton.OnClickState.State, props.config.Rules, pagesToDisplay, currentPage.Name);

        buttonConfig.displayRightButton = ! buttonConfig.displayFinishButton;
    }

    const onkeydown = function(e, crafted) {
        if (e.key === "ArrowLeft" && e.ctrlKey && (buttonConfig.displayLeftButton || crafted)) {
            leftButtonClickHandler();
        } else if (e.key === "ArrowRight" && e.ctrlKey && (buttonConfig.displayRightButton || crafted)) {
            rightButtonClickHandler(e);
        } else if (e.key === "Enter" && e.ctrlKey && (buttonConfig.displayFinishButton || crafted) && props.saveEnabled) {
            finishButtonClickhandler();
        }
        const activityEvent = new Event('activity');
        document.dispatchEvent(activityEvent);
    };

    useEffect(() => {
        if (!buttonConfig.displayFinishButton || !props.saveEnabled) {
            return undefined;
        }
        const handler = (e) => {
            if (e.key === "Enter" && e.ctrlKey) {
                props.save(currentPage);
                const activityEvent = new Event('activity');
                document.dispatchEvent(activityEvent);
            }
        };
        document.addEventListener('keydown', handler);
        return () => document.removeEventListener('keydown', handler);
    }, [buttonConfig.displayFinishButton, props.saveEnabled, currentPage, props.save]);

    let panelbar = (null);
    if (props.displayModeSwitchable) {
        panelbar = (
            <PanelBar showHelp={props.showHelp} fullScreen={props.fullScreen} pages={completedPages} fullScreenClick={props.fullScreenClick} language={props.language}></PanelBar>
        );
    }

    let pageToDisplay;
    if (currentPage.Crafted) {
        pageToDisplay = <CraftedFactory 
                            config={currentPage}
                            data={props.config.craftedPageData}
                            inputData={props.config.inputData}
                            changeHandler={props.changeHandler}
                            rightButtonClick={rightButtonClickHandler}
                            leftButtonClick={leftButtonClickHandler}
                            fullScreen={props.fullScreen}
                            language={props.language}
                            onKeyDown={onkeydown}
                            close={props.close}
                            embeddedPages={props.embeddedPages}>                       
                        </CraftedFactory>
    } else {
        pageToDisplay = <SinglePageEntry onKeyDown={onkeydown} fullScreen={props.fullScreen} formTitle={props.config.Title} config={currentPage} changeHandler={props.changeHandler} fieldFocusOut={props.fieldFocusOut} data={props.data} language={props.language} id={props.id} lists={props.lists} pages={props.pages} allPages={props.allPages} uievents={props.uievents} forms={props.forms} embeddedPages={props.embeddedPages}></SinglePageEntry>
    }

    const workflowEntryFieldsCss = "workflowentry-fields-wide";

    return (
        <div className='workflowentry-content'>
            <div className='workflowentry-panel'>
                {panelbar}
            </div>
            <div className="workflowentry-bottom">
                <div className={workflowEntryFieldsCss}>
                    <div>
                        {pageToDisplay}
                        <br />
                    </div>
                    <ErrorMessage visible={props.error.visible} dismissHandler={props.errorCloseHandler} error={props.error.message}></ErrorMessage>
                    <WorkflowEntryButtons 
                        config={buttonConfig} 
                        cancelButtonClick={props.close}
                        leftButtonClick={leftButtonClickHandler}
                        rightButtonClick={rightButtonClickHandler}
                        finishButtonClick={finishButtonClickhandler}
                        language={props.language}
                        fullScreen={props.fullScreen}
                        saveEnabled={props.saveEnabled}
                    >
                    </WorkflowEntryButtons>
                </div>
            </div>
        </div>
    )
};

export default WorkflowEntry;