import React, {useState} from 'react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import '../BatchPrint/BatchPrint.css';
import { CompoundButton, Icon } from '@fluentui/react';
import ErrorMessage from '../../../General/ErrorMessage/ErrorMessage';
import SuccessMessage from '../../../General/SuccessMessage/SuccessMessage';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import Post from '../../../../Data/Post';

const BatchPublish = (props) => {

    const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
    const [successStatus, setSuccessStatus] = useState({visible: false, message: ''});

    const printIcon = <Icon iconName="WebPublish" />;

    const recordCount = props.records.length;

    const publishClickHandler = () => {
        const criteria = { ReportName: 'DefaultSpecimenReport' };
        Post('config/getreport', criteria, configDataRetrieved, errorWhenRetrievingData, {recordCount: 0});
    }

    const configDataRetrieved = (data, state) => {
        state.config = data;
        const recordsToPrint = props.records.map((r) => { return r.id });
        const parameters = { 'reportid': 868, 'itemstoprint': recordsToPrint, 'type': 'publish' };
        Post('batch/print', parameters, reportDataRetrieved, errorWhenRetrievingData, state);
    }

    const reportDataRetrieved = () => {
        setSuccessStatus({visible: true, message: TranslateTag('@BatRec@', props.language)});
        props.refresh();
    }

    const errorWhenRetrievingData = (response) => {
        updateErrorStatus({visible: true, message: response.data});
    }

    const errorCloseHandler = () => {
        updateErrorStatus({visible: false, message: ''});
    }

    return (
        <div className="app-crafted-content">
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>

            <div className="batchprint-contents">
                <CompoundButton primary onClick={publishClickHandler} >
                    <div className="batchprint-button">
                        {printIcon}
                        <br></br>
                        {TranslateTag("@GenPubA@", props.language) + " " + recordCount + " " + TranslateTag("@GenSelL@", props.language)}
                    </div>
                </CompoundButton> 
            </div>

            <br></br>
            <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
            <SuccessMessage visible={successStatus.visible} message={successStatus.message}></SuccessMessage>
        </div>
    )
};

export default BatchPublish;