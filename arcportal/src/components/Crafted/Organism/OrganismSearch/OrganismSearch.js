import React, { useEffect, useState } from 'react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { runSerotypeListQuery, runSubSpeciesListQuery, runSpeciesListQuery, runGenusListQuery } from './OrganismSearchQueries';
import { Separator, IconButton, TooltipHost } from '@fluentui/react';
import './OrganismSearch.css';

const OrganismSearch = (props) => {

    const [genusOptions, setGenusOptions] = useState();
    const [speciesOptions, setSpeciesOptions] = useState();
    const [subspeciesOptions, setSubSpeciesOptions] = useState();
    const [serotypeOptions, setSerotypeOptions] = useState();
    const [filterData, setFilterData] = useState({ search: "", genusid: 0, speciesid: 0, subspeciesid: 0, serotypeid: 0});

    // const iconStyles = {
    //     root: { fontSize: '24px', height: '24px', width: '24px', }
    //   };

    useEffect(() => {

        const genusRetrieved = (data) => {
            const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
            setGenusOptions(newData);
        }

        runGenusListQuery(genusRetrieved, errorWhenRetrievingData);
    }, [])

    useEffect(() => {
        
        const GetFieldContents = (data, fieldName) => {
            if (data !== undefined) {
                let element = props.data.filter((r) => r.Key === fieldName);
                if (Array.isArray(element) && element.length > 0) {
                    return element[0].value;
                }
            }
        }

        const existingGenusRetrieved = (data) => {
            const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
            setGenusOptions(newData);
        }

        const newFilterData = { 
            search: GetFieldContents(props.data, "search"), 
            genusid: GetFieldContents(props.data, "genusid"), 
            speciesid: GetFieldContents(props.data, "speciesid"), 
            subspeciesid: GetFieldContents(props.data, "subspeciesid"), 
            serotypeid: GetFieldContents(props.data, "serotypeid")
        }

        setFilterData(newFilterData);

        if (newFilterData.genusid !== undefined) {
            runGenusListQuery(existingGenusRetrieved, errorWhenRetrievingData);
            runSpeciesListQuery(newFilterData.genusid, speciesRetrieved, errorWhenRetrievingData);
        }

        if (newFilterData.speciesid !== undefined) {
            runSubSpeciesListQuery(newFilterData.speciesid, subspeciesRetrieved, errorWhenRetrievingData);
            runSerotypeListQuery(newFilterData.speciesid, serotypeRetrieved, errorWhenRetrievingData);
        }

    }, [props.data])

    const errorWhenRetrievingData = () => {
    }

    const searchChangeHandler = (event, value) => {
        const changes = [{ key: "search", value: { Key: "search", value: value}}];
        props.changeHandler("multiplechanges", changes);   
        setFilterData({...filterData, search: value});
    }

    const genusChangeHandler = (event, value) => {
        if (event === "GenusIdText") {
            const changes = [{ key: event, value: { Key: event, value: value}}, {key: "genusid", value: {Key: "genusid", value: undefined } }, {key: "serotypeid", value: {Key: "serotypeid", value: undefined } }, {key: "speciesid", value: {Key: "speciesid", value: undefined } }, {key: "subspeciesid", value: {Key: "subspeciesid", value: undefined } }];
            props.changeHandler("multiplechanges", changes);
            setFilterData({...filterData, speciesid: undefined, serotypeid: undefined, genusid: undefined});
        } else {
            if (value !== undefined)  {
                runSpeciesListQuery(value, speciesRetrieved, errorWhenRetrievingData);
            }
            setFilterData({...filterData, genusid: value, speciesid: undefined, subspeciesid: undefined, serotypeid: undefined});
            const changes = [{ key: "genusid", value: { Key: "genusid", value: value}}, {key: "serotypeid", value: {Key: "serotypeid", value: undefined } }, {key: "speciesid", value: {Key: "speciesid", value: undefined } }, {key: "subspeciesid", value: {Key: "subspeciesid", value: undefined } }];
            props.changeHandler("multiplechanges", changes);   
        }
    }

    const speciesRetrieved = (data) => {
        const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
        setSpeciesOptions(newData);
   }

    const speciesChangeHandler = (event, value) => {
        runSubSpeciesListQuery(value, subspeciesRetrieved, errorWhenRetrievingData);
        runSerotypeListQuery(value, serotypeRetrieved, errorWhenRetrievingData);
        const changes = [{key: "serotypeid", value: {Key: "serotypeid", value: undefined } }, {key: "speciesid", value: {Key: "speciesid", value: value } }, {key: "subspeciesid", value: {Key: "subspeciesid", value: undefined } }]
        props.changeHandler("multiplechanges", changes);  
        setFilterData({...filterData, speciesid: value, serotypeid: undefined, subspeciesid: undefined});
    }
    
    const subspeciesRetrieved = (data) => {
        const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
        setSubSpeciesOptions(newData);
    }

    const serotypeRetrieved = (data) => {
        const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
        setSerotypeOptions(newData);
    }

    const subspeciesChangeHandler = (event, value) => {
        const changes = [{key: "subspeciesid", value: {Key: "subspeciesid", value: value }}, {key: "serotypeid", value: {Key: "serotypeid", value: undefined }}]
        props.changeHandler("multiplechanges", changes);
        setFilterData({...filterData, subspeciesid: value, serotypeid: undefined});


        props.changeHandler("subspeciesid", {Key: "subspeciesid", value: value});
        setFilterData({...filterData, subspeciesid: value, serotypeid: undefined});
    }

    const serotypeChangeHandler = (event, value) => {
        const changes = [{key: "serotypeid", value: {Key: "serotypeid", value: value }}, {key: "subspeciesid", value: {Key: "subspeciesid", value: undefined }}]
        props.changeHandler("multiplechanges", changes);
        setFilterData({...filterData, serotypeid: value, subspeciesid: undefined});
    }

    const searchConfig = { Id: "Search", Type: 'singleline', Label: TranslateTag("@GenSea@", props.language), value: filterData.search};
    const genusConfig = { Id: "GenusId", Type: 'filteredcombo', Label: TranslateTag("@GenGen@", props.language), MinFilterLength: 2, Options: genusOptions, value: filterData.genusid, Placeholder: TranslateTag("@GenEntC@", props.language) };
    const speciesConfig = { Id: "SpeciesId", Type: 'combobox', Label: TranslateTag("@GenSpeB@", props.language), Options: speciesOptions, value: filterData.speciesid, Resettable: true };
    const subspeciesConfig = { Id: "SubSpeciesId", Type: 'combobox', Label: TranslateTag("@GenSubA@", props.language), Options: subspeciesOptions, value: filterData.subspeciesid, Resettable: true };
    const serotypeConfig = { Id: "SerotypeId", Type: 'combobox', Label: TranslateTag("@GenSer@", props.language), Options: serotypeOptions, value: filterData.serotypeid, Resettable: true }

    const pageCss = props.fullScreen ? "app-crafted-fullscreencontent" : "app-crafted-content";

    return (
        <div className={pageCss}>
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>

            <div className="organismsearch-formcolumn">
                <SingleLineField key="Search" config={searchConfig} changeHandler={searchChangeHandler}></SingleLineField>
                <br></br>
                <Separator>
                    <TooltipHost
                        content={TranslateTag("@GenSea@", props.language)}
                        id={100}
                    >
                        <IconButton
                            iconProps={{iconName: 'Zoom'}}
                            onClick={props.rightButtonClick}
                        />
                    </TooltipHost>
                </Separator>
                <SingleLineField key="GenusId" config={genusConfig} changeHandler={genusChangeHandler}></SingleLineField>                    
                <SingleLineField key="SpeciesId" config={speciesConfig} changeHandler={speciesChangeHandler}></SingleLineField>
                <SingleLineField key="SubSpeciesId" config={subspeciesConfig} changeHandler={subspeciesChangeHandler}></SingleLineField>
                <SingleLineField key="SerotypeId" config={serotypeConfig} changeHandler={serotypeChangeHandler}></SingleLineField>
            </div>
        </div>
    )
};

export default OrganismSearch;