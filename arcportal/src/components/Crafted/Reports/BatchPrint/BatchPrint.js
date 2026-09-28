import React, {useState} from 'react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import { CompoundButton, Icon } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import Post from '../../../../Data/Post';
import ErrorMessage from '../../../General/ErrorMessage/ErrorMessage';
import TransformDatesInJson from '../../../../Utils/Local/TransformDatesInJson';
import { WriteMultipleReports } from '../../../Reports/ReportWriter';
import './BatchPrint.css';

const BatchPrint = (props) => {

    const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});

    const printIcon = <Icon iconName="Print" />;

    const recordCount = props.records.length;

    const printClickHandler = () => {
        const criteria = { ReportName: 'DefaultSpecimenReport' };
        Post('config/getreport', criteria, configDataRetrieved, errorWhenRetrievingData, {recordCount: 0});
    }

    const configDataRetrieved = (data, state) => {
        state.config = data;
        const recordsToPrint = props.records.map((r) => { return r.Id });
        const parameters = { 'reportid': 868, 'itemstoprint': recordsToPrint, 'type': 'print', 'history': props.history };
        Post('batch/print', parameters, reportDataRetrieved, errorWhenRetrievingData, state);
    }

    const reportDataRetrieved = async (data, state) => {
        let dataToPrint = [];
        let newData = JSON.parse(data);
        const config = JSON.parse(state.config);
        for (const row of newData) {
            let formattedData = JSON.parse(row);
            TransformDatesInJson(formattedData.Standard);
            dataToPrint.push(formattedData);
        }
        await WriteMultipleReports(dataToPrint, config, props.language);
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
                <CompoundButton primary onClick={printClickHandler} >
                    <div className="batchprint-button">
                        {printIcon}
                        <br></br>
                        {TranslateTag("@GenPriC@", props.language) + " " + recordCount + " " + TranslateTag("@GenSelL@", props.language)}
                    </div>
                </CompoundButton> 
            </div>

            <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
        </div>
    )
};

export default BatchPrint;