import React from 'react';
import './SinglePagePanel.css';
import { Panel, PanelType, Spinner } from '@fluentui/react';
import SinglePage from '../SinglePage/SinglePage';
import WorkflowEntry from '../WorkflowEntry/WorkflowEntry';
import ViewRecord from '../../Containers/ViewRecord/ViewRecord';
import FormContentLoading from '../FormContentLoading/FormContentLoading';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import NormalizeCraftedPageContents from '../../../Utils/Crafted/NormalizeCraftedPageContents';
// import RightHelp from '../../General/RightHelp/RightHelp';

/**
 * Panel wrapper for single-page and multi-page workflow forms.
 * @param {Object} props
 * @param {boolean} [props.saveEnabled=true] - When false, disables the Save/Finish button. Defaults to true when not provided.
 * @param {boolean} [props.isSaving=false] - When true, shows a blocking overlay while save is in flight.
 * @param {Function} [props.onDismissed] - Called after the panel exit animation completes.
 */
const SinglePagePanel = (props) => {

    // const [helpVisible, setHelpVisible] = useState(false);

    // const openHelpView = () => {
    //     setHelpVisible(true);
    // }

    let formtodisplay;

    let type = PanelType.medium;
    let width = undefined;
    const p = props.currentPage || {};
    const isExtraWide = p.ExtraWide || p.extraWide;
    const isWider = p.Wider || p.wider;
    const isWide = p.Wide || p.wide;
    if (isExtraWide) {
        type = PanelType.extraLarge;
    } else if (isWider) {
        type = PanelType.custom;
        width = '1000px';
    } else if (isWide) {
        type = PanelType.large;
    }

    const pages = props.config?.Pages;
    const pageCount = Array.isArray(pages) ? pages.length : 0;

    if (props.loadFailed && props.error?.visible) {
        formtodisplay = (
            <div className="singlepagepanel-load-error">
                <ErrorMessage
                    visible={true}
                    dismissHandler={props.loadErrorCloseHandler ?? props.errorCloseHandler}
                    error={props.error.message}
                />
            </div>
        );
    } else if (props.shellLoading) {
        formtodisplay = (
            <FormContentLoading active={true} delayMs={props.shellLoadingDelayMs} />
        );
    } else if (pageCount === 1) {
        let data = props.config.data;
        if (props.config.Pages[0].Crafted) {
            if (props.config.data !== undefined && props.config.data.Crafted !== undefined) {
                const contents = props.config.data.Crafted[0].Contents;
                data = NormalizeCraftedPageContents(contents);
                if (data.length === 0) {data.id = props.id; }
            }
        }
        formtodisplay = <SinglePage
                            config={props.config.Pages[0]}
                            fullScreen={props.fullScreen}
                            displayModeSwitchable={props.displayModeSwitchable}
                            fullScreenClick={props.fullScreenClick}
                            close={props.close}
                            data={data}
                            id={props.id}
                            currentPage={props.currentPage}
                            save={props.save}
                            error={props.error}
                            changeHandler={props.changeHandler}
                            errorCloseHandler={props.errorCloseHandler}
                            refresh={props.refresh}
                            language={props.language}
                            saveEnabled={props.saveEnabled ?? true}
                            fieldFocusOut={props.fieldFocusOut}
                            records={props.records}
                            startConfig={props.startConfig}
                            showNextButton={props.showNextButton}
                            lists={props.lists}
                            pages={props.config.Pages}
                            allPages={props.allPages}
                            uievents={props.uievents}
                            forms={props.forms}
                            embeddedPages={props.embeddedPages}
                        >
                        </SinglePage>
    } else if (pageCount > 1) {
        formtodisplay = <WorkflowEntry
                            currentPage={props.currentPage}
                            config={props.config}
                            fullScreen={props.fullScreen}
                            displayModeSwitchable={props.displayModeSwitchable}
                            fullScreenClick={props.fullScreenClick}
                            close={props.close}
                            data={props.config.data}
                            inputData={props.config.inputData}
                            save={props.save}
                            id={props.id}
                            next={props.next}
                            previous={props.previous}
                            error={props.error}
                            changeHandler={props.changeHandler}
                            fieldFocusOut={props.fieldFocusOut}
                            errorCloseHandler={props.errorCloseHandler}
                            refresh={props.refresh}
                            language={props.language}
                            saveEnabled={props.saveEnabled ?? true}
                            state={props.state}
                            lists={props.lists}
                            pages={props.config.Pages}
                            allPages={props.allPages}
                            uievents={props.uievents}
                            forms={props.forms}
                            embeddedPages={props.embeddedPages}
                            //showHelp={openHelpView}
                        >
                        </WorkflowEntry>
    } else {
        formtodisplay = null;
    }

    // Experimental hot-key.
    document.onkeydown = function(e) {
        if (e.key === "Escape") {
            props.close();
        }
    };

    const onClick = function(e) {
        const activityEvent = new Event('activity');
        document.dispatchEvent(activityEvent);
    };

    let display;
    if (props.fullScreen) {
        if (!props.config.SuppressRecordView) {
            display =
                <div className='singlepagepanel-columns'>
                    <div className='singlepagepanel-column-left'>
                        <ViewRecord
                            itemId={props.id}
                            type={props.recordType}
                            reportConfiguration={props.reportConfiguration}
                        >
                        </ViewRecord>
                    </div>
                    <div className='singlepagepanel-column-right'>
                        {formtodisplay}
                    </div>
                </div>
        } else {
            display =
                <div>
                    {formtodisplay}
                </div>
        }
    } else {
        display = <Panel
                    onClick={onClick}
                    isOpen={props.visible}
                    onDismiss={props.suppressDismiss ? () => {} : props.close}
                    onDismissed={props.onDismissed}
                    type={type}
                    customWidth={width}
                    hasCloseButton={false}
                  >
                    {formtodisplay}
                  </Panel>
    }

    return (
        <div className="singlepagepanel-wrapper">
            {props.isSaving && (
                <div className="singlepagepanel-saving-overlay" role="status" aria-live="polite">
                    <Spinner styles={{ circle: { width: '100px', height: '100px', borderWidth: '10px' } }} />
                </div>
            )}
            {display}
        </div>
    )
};

export default SinglePagePanel;