import React, {useState} from 'react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import { PrimaryButton } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import ErrorMessage from '../../../General/ErrorMessage/ErrorMessage';
import Post from '../../../../Data/Post';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';

const ExportConfiguration = (props) => {

    const [errorMessage, setErrorMessage] = useState(""); 
    const [toggleState, setToggleState] = useState({ Configuration: 'Yes', ListItems: 'Yes', Organisms: 'No'})

    const GenerateExport = () => {
        Post('config/export', toggleState, dataRetrievedSuccessfully, errorWhenRetrievingData);
    }

    const dataRetrievedSuccessfully = (data) => {

        let csv = "";
        for (const row of data) {
            //let csvRow = '\"' + row.replace(/\|/g, '","') + '\"' ;
            csv += row + '\n';
        }

        // Generate CSV file for download.
        var hiddenElement = document.createElement('a');
        hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(csv);
        hiddenElement.target = '_blank';
        hiddenElement.download = 'config_export.txt';
        hiddenElement.click();
    }

    const errorWhenRetrievingData = (error) => {
        setErrorMessage(error.data);
    }

    const errorCloseHandler = () => {
        setErrorMessage("");
    }

    const changeHandler = (id, value) => {
        let toggleValues = {...toggleState};
        switch (id) {
            case 'Configuration':
                toggleValues.Configuration = toggleValues.Configuration == 'Yes' ? 'No' : 'Yes';
                break;
            case 'ListItems':
                toggleValues.ListItems = toggleValues.ListItems == 'Yes' ? 'No' : 'Yes';
                break;
            case 'Coding':
                toggleValues.Coding = toggleValues.Coding == 'Yes' ? 'No' : 'Yes';
                break;
            case 'QualityControl':
                toggleValues.QualityControl = toggleValues.QualityControl == 'Yes' ? 'No' : 'Yes';
                break;
            case 'ExportProfile':
                toggleValues.ExportProfile = toggleValues.ExportProfile == 'Yes' ? 'No' : 'Yes';
                break;
            case 'Language':
                toggleValues.Language = toggleValues.Language == 'Yes' ? 'No' : 'Yes';
                break;
        }
        setToggleState(toggleValues);
    }
    
    const configurationConfig = { Id: 'Configuration', Type: 'toggle', Label: TranslateTag('@GenCon@', props.language), value: toggleState.Configuration};
    const listitemsConfig = { Id: 'ListItems', Type: 'toggle', Label: TranslateTag('@GenLis@', props.language), value: toggleState.ListItems };
    const codingConfig = { Id: 'Coding', Type: 'toggle', Label: TranslateTag('@GenCod@', props.language), value: toggleState.Coding };
    const qualityControlConfig = { Id: 'QualityControl', Type: 'toggle', Label: TranslateTag('@GenQuaA@', props.language), value: toggleState.QualityControl };
    const exportProfileConfig = { Id: 'ExportProfile', Type: 'toggle', Label: TranslateTag('@GenExpB@', props.language), value: toggleState.ExportProfile };
    const languageConfig = { Id: 'Language', Type: 'toggle', Label: TranslateTag('@GenLan@', props.language), value: toggleState.Language };

    return (
        <div className="app-crafted-content">
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <div className="export-formcolumn">
                <SingleLineField key="Configuration" config={configurationConfig} changeHandler={changeHandler}></SingleLineField>
                <SingleLineField key="ListItems" config={listitemsConfig} changeHandler={changeHandler}></SingleLineField>
                <SingleLineField key="Coding" config={codingConfig} changeHandler={changeHandler}></SingleLineField>
                <SingleLineField key="QualityControl" config={qualityControlConfig} changeHandler={changeHandler}></SingleLineField>
                <SingleLineField key="ExportProfile" config={exportProfileConfig} changeHandler={changeHandler}></SingleLineField>
                <SingleLineField key="Language" config={languageConfig} changeHandler={changeHandler}></SingleLineField>
               <br /><br />
                <div className='export-button'>
                    <PrimaryButton
                        text={TranslateTag("@ExpRet@", props.language)}
                        onClick={() => {GenerateExport()}}
                    />
                </div>
           </div>
           <div>
                <br />
                <ErrorMessage visible={errorMessage !== ""} dismissHandler={errorCloseHandler} error={errorMessage}></ErrorMessage>
            </div>
        </div>
    )
}

export default ExportConfiguration