import React, { useState, useEffect } from 'react';
import TextDisplay from '../../Forms/TextDisplay/TextDisplay';
import SingleLineField from '../../Forms/SingleLineField/SingleLineField';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import {
    FormatDatesToLocaleForExport,
    TransformDatesInJsonDeep,
    TransformDatesInXml
} from '../../../Utils/Local/TransformDatesInJson';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import { PrimaryButton } from '@fluentui/react';
import { connect } from 'react-redux';
import Post from '../../../Data/Post';
import { useRunOnce } from '../../../Utils/General/UseRunOnce';
import './Export.css';

const Export = (props) => {

    const [errorMessage, setErrorMessage] = useState(""); 
    const [specimenTypeList, setSpecimenTypeList] = useState(); 
    const [specimenStateList, setSpecimenStateList] = useState(); 
    const [tagList, setTagList] = useState(); 
    const [organisationList, setOrganisationList] = useState(); 
    const [locationList, setLocationList] = useState(); 
    const [testList, setTestList] = useState(); 

    const changeHandler = (id, value) => {
        props.changeHandler(id, value);
    }

    const GetFieldInfo = (fields, field) => {
        const fieldInfo = fields.filter(f => f.Id == field);
        return fieldInfo.length > 0 ? fieldInfo[0].value : undefined; 
    }

    const fields = props.config.Columns[0].FormGroups[0].Fields;
    const organismId = GetFieldInfo(fields,"OrganismId");
    const specimenTypeId = GetFieldInfo(fields,"SpecimenTypeId");
    const specimenStateId = GetFieldInfo(fields,"SpecimenStateId");
    const tagId = GetFieldInfo(fields,"TagId");
    const organisationId = GetFieldInfo(fields,"OrganisationId");
    const locationId = GetFieldInfo(fields,"LocationId");
    const testId = GetFieldInfo(fields,"TestId");
    const astExclusive = GetFieldInfo(fields,"ASTExclusive");

    useRunOnce(() => {
        let list = props.lists.filter(l => l.Name.toLowerCase() === "specimentype");
        setSpecimenTypeList(list[0].Options.map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}}));
        list = props.lists.filter(l => l.Name.toLowerCase() === "statelist");
        setSpecimenStateList(list[0].Options.map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}}));
        list = props.lists.filter(l => l.Name.toLowerCase() === "tag");
        setTagList(list[0].Options.map((option) => ({ key: option.Key, text: option.Text, ParentKey: option.ParentKey })));
        list = props.lists.filter(l => l.Name.toLowerCase() === "organisationlist");
        setOrganisationList(list[0].Options.map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}}));
        list = props.lists.filter(l => l.Name.toLowerCase() === "locationlist");
        setLocationList(list[0].Options.map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}}));
        list = props.lists.filter(l => l.Name.toLowerCase() === "directtestconfiglist");
        setTestList(list[0].Options.map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}}));
    });

    const closeExportForm = () => {
        if (typeof props.close === 'function') {
            props.close();
        }
    };

    const GenerateExport = () => {
        const parameters = [ 
            { key: "exportprofileid", value: props.id },
            { key: "organismId", value: organismId },
            { key: "specimenTypeId", value: specimenTypeId },
            { key: "stateId", value: specimenStateId },
            { key: "tagId", value: tagId },
            { key: "organisationFilterId", value: organisationId },
            { key: "locationId", value: locationId },
            { key: "testId", value: testId },
            { key: "startDate", value: GetFieldInfo(fields,"StartDate") },
            { key: "endDate", value: GetFieldInfo(fields,"EndDate") },          
            { key: "astExclusive", value: astExclusive }          
        ]
        const criteria = { Name: 'runexport', Parameters: parameters};
        Post('export/run', criteria, dataRetrievedSuccessfully, errorWhenRetrievingData);
    }

    const dataRetrievedSuccessfully = (result) => {

        // The export endpoint returns an envelope describing the resolved format. CSV keeps the
        // existing client-side date localisation and pipe-to-CSV conversion. JSON and XML also
        // localise ISO date strings in the downloaded file before the browser saves it.
        const format = ((result && (result.Format || result.format)) || 'csv').toString().toLowerCase();

        if (format === 'json' || format === 'xml') {
            const rawContent = (result && (result.Content || result.content)) || '';
            let content = typeof rawContent === 'string'
                ? rawContent
                : JSON.stringify(rawContent, null, 2);
            if (format === 'json') {
                const parsed = typeof rawContent === 'string' ? JSON.parse(rawContent) : rawContent;
                content = JSON.stringify(TransformDatesInJsonDeep(parsed), null, 2);
            } else {
                content = TransformDatesInXml(content);
            }
            const fileName = (result && (result.FileName || result.fileName)) || ('export_run.' + format);
            const mime = format === 'xml' ? 'application/xml' : 'application/json';
            var jsonElement = document.createElement('a');
            jsonElement.href = 'data:' + mime + ';charset=utf-8,' + encodeURIComponent(content);
            jsonElement.target = '_blank';
            jsonElement.download = fileName;
            jsonElement.click();
            closeExportForm();
            return;
        }

        let rows = (result && (result.Rows || result.rows)) || [];
        rows = FormatDatesToLocaleForExport(rows);

        let csv = "";
        for (const row of rows) {
            let csvRow = '\"' + row.replace(/\|/g, '","') + '\"' ;
            csv += csvRow + '\n';
        }

        var hiddenElement = document.createElement('a');
        hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(csv);
        hiddenElement.target = '_blank';
        hiddenElement.download = 'export_run.csv';
        hiddenElement.click();
        closeExportForm();
    }

    const errorWhenRetrievingData = (error) => {
        setErrorMessage(error.data);
    }

    const errorCloseHandler = () => {
        setErrorMessage("");
    }

    const startDateConfig = { Id: 'StartDate', Type: 'date', Label: TranslateTag("@GenStaB@", props.language), Required: true, Placeholder: TranslateTag("@GenSel@", props.language) };
    const endDateConfig = { Id: 'EndDate', Type: 'date', Label: TranslateTag("@GenEnd@", props.language), Placeholder: TranslateTag("@GenSelA@", props.language), Required: true };
    const organismListConfig = { Id: "OrganismId", Type: 'organismlist', value: organismId, includeOrText: false, value: organismId, Placeholder: TranslateTag("@GenSel@", props.language) };
    const specimenTypeConfig = { Id: 'SpecimenTypeId', Type: 'combobox', Label: TranslateTag('@SpeSpeB@', props.language), MultiSelect: true, OptionsName: 'SpecimenType', Options: specimenTypeList, value: specimenTypeId, Placeholder: TranslateTag("@SpeSelCA@", props.language)};
    const specimenStateConfig = { Id: 'SpecimenStateId', Type: 'combobox', Label: TranslateTag('@SpeSpeQ@', props.language), MultiSelect: true, OptionsName: 'statelist', Options: specimenStateList, value: specimenStateId, Placeholder: TranslateTag("@SpeSelQ@", props.language)};
    const tagConfig = { Id: 'TagId', Type: 'hierarchicalpicker', Label: TranslateTag('@GenTagK@', props.language), MultiSelect: true, OptionsName: 'tag', Options: tagList, value: tagId, Placeholder: TranslateTag("@GenTagE@", props.language), SearchPlaceholder: TranslateTag("@GenTagC@", props.language) };
    const organisationConfig = { Id: 'OrganisationId', Type: 'hierarchicalpicker', Label: TranslateTag('@GenOrg@', props.language), MultiSelect: true, OptionsName: 'OrganisationList', Options: organisationList, value: organisationId, DropDownWidth: 300, Placeholder: TranslateTag("@ExpSelOrg@", props.language), SearchPlaceholder: TranslateTag("@ExpFilOrg@", props.language), AllowAdd: true };
    const locationConfig = { Id: 'LocationId', Type: 'hierarchicalpicker', Label: TranslateTag('@PatPatJ@', props.language), MultiSelect: true, OptionsName: 'LocationList', Options: locationList, value: locationId, DropDownWidth: 300, Placeholder: TranslateTag("@ExpSelLoc@", props.language), SearchPlaceholder: TranslateTag("@ExpFilLoc@", props.language), AllowAdd: true };
    const testConfig = { Id: 'TestId', Type: 'combobox', Label: TranslateTag('@ConDirA@', props.language), MultiSelect: true, OptionsName: 'TestConfigList', Options: testList, value: testId, Placeholder: TranslateTag("@CulSelA@", props.language)};
    const astExclusiveConfig = { Id: 'ASTExclusive', Type: 'toggle', Label: TranslateTag('@ExpASTExc@', props.language), value: astExclusive};
    
    return (
        <div className="app-crafted-content">
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <div className="export-formcolumn">
                <SingleLineField key="StartDate" config={startDateConfig} changeHandler={changeHandler} language={props.language} uievents={props.uievents} forms={props.forms}></SingleLineField>                    
                <SingleLineField key="EndDate" config={endDateConfig} changeHandler={changeHandler} language={props.language} uievents={props.uievents} forms={props.forms}></SingleLineField>
                <SingleLineField key="Location" config={locationConfig} changeHandler={changeHandler} language={props.language} uievents={props.uievents} forms={props.forms}></SingleLineField>
                <SingleLineField key="Organisation" config={organisationConfig} changeHandler={changeHandler} language={props.language} uievents={props.uievents} forms={props.forms}></SingleLineField>
                <SingleLineField key="Tag" config={tagConfig} changeHandler={changeHandler} language={props.language} uievents={props.uievents} forms={props.forms}></SingleLineField>
                <SingleLineField key="SpecimenType" config={specimenTypeConfig} changeHandler={changeHandler}></SingleLineField>
                <SingleLineField key="SpecimenState" config={specimenStateConfig} changeHandler={changeHandler}></SingleLineField>
                <SingleLineField key="Test" config={testConfig} changeHandler={changeHandler}></SingleLineField>
                <SingleLineField key="OrganismList" config={organismListConfig} changeHandler={changeHandler} language={props.language}></SingleLineField>
                <SingleLineField key="ASTExclusive" config={astExclusiveConfig} changeHandler={changeHandler}></SingleLineField>
                <br /><br />
                <div className='export-button'>
                    <PrimaryButton
                        id="retrieveexport"
                        text={TranslateTag("@ExpRet@", props.language)}
                        onClick={() => {GenerateExport()}}
                    />
                </div>
                <div>
                    <br />
                    <ErrorMessage visible={errorMessage !== ""} dismissHandler={errorCloseHandler} error={errorMessage}></ErrorMessage>
                </div>
            </div>
        </div>
    )
};

const mapStateToProps = state => {
    return {
        lists: state.config.lists,
        uievents: state.config.uievents,
        forms: state.config.forms,
        language: state.config.language,
    };
}

export default connect(mapStateToProps)(Export);
