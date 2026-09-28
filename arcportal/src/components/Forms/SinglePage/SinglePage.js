import React from 'react';
import './SinglePage.css';
import SinglePageEntry from '../SinglePageEntry/SinglePageEntry';
import PanelBar from '../../General/PanelBar/PanelBar';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import CraftedFactory from '../../Crafted/CraftedFactory';
import { PrimaryButton, DefaultButton } from '@fluentui/react';
import TranslateTag from '../../../Utils/Local/TranslateTag';

const SinglePage = (props) => {

    const finishButtonClickHandler = (nextItem) => {
        props.save(nextItem);
    }

    let panelBar = (null);
    if (props.displayModeSwitchable) {
        panelBar = (
            <PanelBar fullScreen={props.fullScreen} fullScreenClick={props.fullScreenClick} language={props.language}></PanelBar>
        );
    }

    const onkeydown = function(e) {
        if (e.key === "Enter" && e.ctrlKey && props.saveEnabled) {
            finishButtonClickHandler(false);
        }
        const activityEvent = new Event('activity');
        document.dispatchEvent(activityEvent);
    };
    
    let pageToDisplay;
    if (props.config.Crafted) {
        pageToDisplay = <CraftedFactory
                            config={props.config}
                            data={props.data}
                            changeHandler={props.changeHandler}
                            id={props.id}
                            refresh={props.refresh}
                            reset={props.reset}
                            language={props.language}
                            records={props.records}
                            startConfig={props.startConfig}
                            fullScreen={props.fullScreen}
                            save={finishButtonClickHandler}
                            close={props.close}
                            onKeyDown={onkeydown}
                            lists={props.lists}
                            pages={props.pages}
                            embeddedPages={props.embeddedPages}
                        ></CraftedFactory>
    } else {
        pageToDisplay = <SinglePageEntry onKeyDown={onkeydown} fullScreen={props.fullScreen} config={props.config} changeHandler={props.changeHandler} fieldFocusOut={props.fieldFocusOut} data={props.data} language={props.language} id={props.id} lists={props.lists} pages={props.pages} allPages={props.allPages} uievents={props.uievents} forms={props.forms} embeddedPages={props.embeddedPages}></SinglePageEntry>
    }

    let nextButton = (null)
    if (props.config.NextButton !== "undefined" && props.config.NextButton !== "") {
        if (props.config.NextButton.Show) {
            nextButton = <PrimaryButton text={props.config.NextButton.ButtonText} onClick={() => {finishButtonClickHandler(false)}} disabled={!props.saveEnabled}/>
        }
    } else {
            nextButton = <PrimaryButton text= {TranslateTag("@GenSav@", props.language)} onClick={() => {finishButtonClickHandler(false)}} disabled={!props.saveEnabled}/>
    }

    let cancelButton = (null)
    if (props.config.CancelButton !== "undefined" && props.config.CancelButton !== "") {
        cancelButton = <DefaultButton text={props.config.CancelButton.ButtonText} onClick={props.close}/>
    } else {
        cancelButton = <DefaultButton text={TranslateTag("@GenCan@", props.language)} onClick={props.close}/>
    }

    let nextItemButton = (null)
    if (props.config.NextItemButton !== "undefined" && props.config.NextItemButton !== "") {
        if (props.config.NextItemButton.Show && props.showNextButton) {
            nextItemButton = <PrimaryButton text={props.config.NextItemButton.ButtonText} onClick={() => {finishButtonClickHandler(true)}} disabled={!props.saveEnabled}/>
        }
    }

    const singlePageContentCss = props.fullScreen ? "singlepage-content-wide" : "singlepage-content";

    return (
        <div>
            {panelBar}
            <div className={singlePageContentCss}>
                {pageToDisplay}
                <div onKeyDown={onkeydown}>
                    <br />
                    <ErrorMessage visible={props.error.visible} dismissHandler={props.errorCloseHandler} error={props.error.message}></ErrorMessage>
                    <div className='singlepage-navigation'>
                        <div className="app-button">
                            {/* {cancelButton} */}
                        </div>
                        <div className='singlepage-righthand-buttons'>
                            <div className="app-button">
                                {cancelButton}
                            </div>
                            <div className="app-button">
                                {nextButton}
                            </div>
                            <div className="app-button">
                                {nextItemButton}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
};

export default SinglePage;