import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Separator, Icon } from '@fluentui/react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { CompoundButton } from '@fluentui/react';
import { runSerotypeListQuery, runSubSpeciesListQuery, runSpeciesListQuery, runGenusListQuery, runFamilyListQuery, runOrderListQuery, runOrderAndFamilyFromGenusIdQuery } from './OrganismScopeQueries';
import './OrganismScope.css';

const OrganismScope = (props) => {

    const [orderOptions, setOrderOptions] = useState();
    const [familyOptions, setFamilyOptions] = useState();
    const [genusOptions, setGenusOptions] = useState();
    const [speciesOptions, setSpeciesOptions] = useState();
    const [subspeciesOptions, setSubSpeciesOptions] = useState();
    const [serotypeOptions, setSeroTypeOptions] = useState();
    const [orgGroupOptions, setOrgGroupOptions] = useState([]);
    const [onlyGenusOptions, setOnlyGenusOptions] = useState();
    const [onlySpeciesOptions, setOnlySpeciesOptions] = useState();
    const [onlySubspeciesOptions, setOnlySubSpeciesOptions] = useState();
    const [onlySerotypeOptions, setOnlySeroTypeOptions] = useState();

    const searchIcon = <Icon iconName="Search" />;

    const GetFieldContents = (data, fieldName) => {
        if (data !== undefined) {
            let element = props.data.filter((r) => r.Key.toLowerCase() === fieldName);
            if (Array.isArray(element) && element.length > 0) {
                return element[0].value;
            }
        }
    }

    const [filterData, setFilterData] = useState({ xorderid: GetFieldContents(props.data, "orderid"), xfamilyid: GetFieldContents(props.data, "familyid"), xgenusid: GetFieldContents(props.data, "genusid"),
                                                    xspeciesid: GetFieldContents(props.data, "speciesid"), xsubspeciesid: GetFieldContents(props.data, "subspeciesid"), xserotypeid: GetFieldContents(props.data, "serotypeid")});

    useEffect(() => {

        const genusRetrieved = (data) => {
            const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
            setOnlyGenusOptions(newData);
        }

        updateFieldChanges({...filterData});
        runGenusListQuery(undefined, genusRetrieved, errorWhenRetrievingData);
    }, [])

    useEffect(() => {

        const orderRetrieved = (data) => {
            const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
            setOrderOptions(newData);
        }

        runOrderListQuery(orderRetrieved, errorWhenRetrievingData);

        if (orgGroupOptions.length === 0) {
            let index = props.lists.findIndex(l => l.Name.toLowerCase() === 'coding');
            if (index !== -1) {
                const options = props.lists[index].Options.filter(f => f.Key !== '676').map(o => { return {key: o.Key, text: o.Text}});
                setOrgGroupOptions(options);
            }
        }
    }, [])

    useEffect(() => {

        const existingOrderRetrieved = (data) => {
            const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
            setOrderOptions(newData);
        }

        const onlyGenusId = GetFieldContents(props.data, "onlygenusid");
        
        const newFilterData = { 
            orderid: GetFieldContents(props.data, "orderid"), 
            familyid: GetFieldContents(props.data, "familyid"), 
            genusid: GetFieldContents(props.data, "genusid"), 
            speciesid: GetFieldContents(props.data, "speciesid"),
            subspeciesid: GetFieldContents(props.data, "subspeciesid"), 
            serotypeid: GetFieldContents(props.data, "serotypeid"),
            onlyorderid: GetFieldContents(props.data, "onlyorderid"), 
            onlyfamilyid: GetFieldContents(props.data, "onlyfamilyid"), 
            onlygenusid: GetFieldContents(props.data, "onlygenusid"), 
            onlyspeciesid: GetFieldContents(props.data, "onlyspeciesid"),
            onlysubspeciesid: GetFieldContents(props.data, "onlysubspeciesid"), 
            onlyserotypeid: GetFieldContents(props.data, "onlyserotypeid"),
            orggroupcodingid: GetFieldContents(props.data, "orggroupcodingid")
        }

        if (onlyGenusId === undefined) {
            newFilterData.xorderid = GetFieldContents(props.data, "orderid");
            newFilterData.xfamilyid = GetFieldContents(props.data, "familyid"); 
            newFilterData.xgenusid = GetFieldContents(props.data, "genusid"); 
            newFilterData.xspeciesid = GetFieldContents(props.data, "speciesid");
            newFilterData.xsubspeciesid = GetFieldContents(props.data, "subspeciesid"); 
            newFilterData.xserotypeid = GetFieldContents(props.data, "serotypeid");
        } else {
            newFilterData.xorderid = undefined;
            newFilterData.xfamilyid = undefined; 
            newFilterData.xgenusid = undefined; 
            newFilterData.xspeciesid = undefined;
            newFilterData.xsubspeciesid = undefined; 
            newFilterData.xserotypeid = undefined;
        } 

        setFilterData(newFilterData);

        if (newFilterData.xorderid !== undefined && orderOptions === undefined) {
            runOrderListQuery(existingOrderRetrieved, errorWhenRetrievingData);
            runFamilyListQuery(newFilterData.xorderid, familyRetrieved, errorWhenRetrievingData);
        }

        if (newFilterData.xfamilyid !== undefined && familyOptions === undefined) {
            runGenusListQuery(newFilterData.xfamilyid, genusRetrieved, errorWhenRetrievingData);
        }

        if (newFilterData.xgenusid !== undefined && genusOptions === undefined) {
            runSpeciesListQuery(newFilterData.xgenusid, speciesRetrieved, errorWhenRetrievingData);
        }

        if (newFilterData.xspeciesid !== undefined && speciesOptions === undefined) {
            runSubSpeciesListQuery(newFilterData.xspeciesid, subspeciesRetrieved, errorWhenRetrievingData);
            runSerotypeListQuery(newFilterData.xspeciesid, serotypeRetrieved, errorWhenRetrievingData);
        }

        if (newFilterData.onlygenusid !== undefined && onlyGenusOptions === undefined) {
            runSpeciesListQuery(newFilterData.onlygenusid, onlySpeciesRetrieved, errorWhenRetrievingData);
        }

        if (newFilterData.onlyspeciesid !== undefined && onlySpeciesOptions === undefined) {
            runSubSpeciesListQuery(newFilterData.onlyspeciesid, onlySubspeciesRetrieved, errorWhenRetrievingData);
            runSerotypeListQuery(newFilterData.onlyspeciesid, onlySerotypeRetrieved, errorWhenRetrievingData);
        }

    }, [])

    const errorWhenRetrievingData = (error) => {
        let x = 1;
    }

    const updateFieldChanges = (data) => {
        const newOrderId = data.xorderid !== undefined ? data.xorderid : data.onlyorderid;;
        const newFamilyId = data.xfamilyid !== undefined ? data.xfamilyid : data.onlyfamilyid;
        const newGenusId = data.xgenusid !== undefined ? data.xgenusid : data.onlygenusid;
        const newSpeciesId = data.xspeciesid !== undefined ? data.xspeciesid : data.onlyspeciesid;
        const newSubSpeciesId = data.xsubspeciesid !== undefined ? data.xsubspeciesid : data.onlysubspeciesid;
        const newSerotypeId = data.xserotypeid !== undefined ? data.xserotypeid : data.onlyserotypeid;

        const changes = [{ key: "orderid", value: { Key: "orderid", value: newOrderId }},
            { key: "familyid", value: { Key: "familyid", value: newFamilyId }},
            { key: "genusid", value: { Key: "genusid", value: newGenusId }},
            { key: "speciesid", value: { Key: "speciesid", value: newSpeciesId }},
            { key: "subspeciesid", value: { Key: "subspeciesid", value: newSubSpeciesId }},
            { key: "serotypeid", value: { Key: "serotypeid", value: newSerotypeId }},
            { key: "xorderid", value: { Key: "xorderid", value: data.xorderid }},
            { key: "xfamilyid", value: { Key: "xfamilyid", value: data.xfamilyid }},
            { key: "xgenusid", value: { Key: "xgenusid", value: data.xgenusid }},
            { key: "xspeciesid", value: { Key: "xspeciesid", value: data.xspeciesid }},
            { key: "xsubspeciesid", value: { Key: "xsubspeciesid", value: data.xsubspeciesid }},
            { key: "xserotypeid", value: { Key: "xserotypeid", value: data.xserotypeid }},
            { key: "orggroupcodingid", value: { Key: "orggroupcodingid", value: data.orggroupcodingid }},
            { key: "onlygenusid", value: { Key: "onlygenusid", value: data.onlygenusid }},
            { key: "onlyspeciesid", value: { Key: "onlyspeciesid", value: data.onlyspeciesid }},
            { key: "onlysubspeciesid", value: { Key: "onlysubspeciesid", value: data.onlysubspeciesid }},
            { key: "onlyserotypeid", value: { Key: "onlyserotypeid", value: data.onlyserotypeid }}
        ];
        setFilterData(data);
        props.changeHandler("multiplechanges", changes, { rootCopy: true }, true);
    }

    const clearData = () => {
        const data = { xorderid: undefined, xfamilyid: undefined, xgenusid: undefined, xspeciesid: undefined, xsubspeciesid: undefined,
            xserotypeid: undefined, orggroupcodingid: undefined, onlygenusid: undefined, onlyspeciesid: undefined, 
            onlysubspeciesid: undefined, onlyserotypeid: undefined };
        updateFieldChanges(data);    
    }

    const resetFields = (family, genus, species, subSpecies, seroType, onlySpecies, onlySubSpecies, onlySerotype) => {
        if (family) { setFamilyOptions([]); }
        if (genus) { setGenusOptions([]); }
        if (species) { setSpeciesOptions([]); }
        if (subSpecies) { setSubSpeciesOptions([]); }
        if (seroType) { setSeroTypeOptions([]); }
        if (onlySpecies) { setOnlySpeciesOptions([]); }
        if (onlySubSpecies) { setOnlySubSpeciesOptions([]); }
        if (onlySerotype) { setOnlySeroTypeOptions([]); }
    }

    const orderChangeHandler = (event, value) => {
        if (value !== undefined)  {
            runFamilyListQuery(value, familyRetrieved, errorWhenRetrievingData);
        }             
        resetFields(true, true, true, true, true, true, true, true)
        updateFieldChanges({...filterData, xorderid: value, xfamilyid: undefined, xgenusid: undefined, xspeciesid: undefined, xsubspeciesid: undefined, xserotypeid: undefined, orggroupcodingid: undefined, onlyorderid: undefined, onlyfamilyid: undefined, onlygenusid: undefined, onlyspeciesid: undefined, onlysubspeciesid: undefined, onlyserotypeid: undefined});
    }

    const familyChangeHandler = (event, value) => {
        if (value !== undefined)  {
            runGenusListQuery(value, genusRetrieved, errorWhenRetrievingData);
        }  else {
            setGenusOptions([]);
            setSubSpeciesOptions([]);
            setSeroTypeOptions([]);
        }
        setSpeciesOptions([]);
        updateFieldChanges({...filterData, xfamilyid: value, xgenusid: undefined, xspeciesid: undefined, xsubspeciesid: undefined, xserotypeid: undefined});
    }

    const orderAndFamilyRetrieved = (data, currentFilter) => {
        updateFieldChanges({...currentFilter, onlyfamilyid: data.FamilyId, onlyorderid: data.OrderId});
    }

    const genusChangeHandler = (event, value) => {
        if (value !== undefined)  {
            runSpeciesListQuery(value, speciesRetrieved, errorWhenRetrievingData);
        } else {
            setSpeciesOptions([]);
            setSubSpeciesOptions([]);
            setSeroTypeOptions([]);
        }
        updateFieldChanges({...filterData, xgenusid: value, xspeciesid: undefined, xsubspeciesid: undefined, xserotypeid: undefined});
    }

    const onlyGenusChangeHandler = (event, value) => {
        const currentFilter = {...filterData, xfamilyid: undefined, xorderid: undefined, xgenusid: undefined, onlygenusid: value, xspeciesid: undefined, xsubspeciesid: undefined, xserotypeid: undefined, onlyspeciesid: undefined, onlysubspeciesid: undefined, onlyserotypeid: undefined, orggroupcodingid: undefined };
        if (value !== undefined && event !== "OnlyGenusIdText")  {
            runSpeciesListQuery(value, onlySpeciesRetrieved, errorWhenRetrievingData);
            runOrderAndFamilyFromGenusIdQuery(value, orderAndFamilyRetrieved, errorWhenRetrievingData, currentFilter);
        } else {
            updateFieldChanges(currentFilter);
        }
        resetFields(true, true, true, true, true, true, true, true);
    }

    const speciesChangeHandler = (event, value) => {
        if (value !== undefined && props.allLevels)  {
            runSubSpeciesListQuery(value, subspeciesRetrieved, errorWhenRetrievingData);
            runSerotypeListQuery(value, serotypeRetrieved, errorWhenRetrievingData);
        } else {
            setSubSpeciesOptions([]);
            setSeroTypeOptions([]);
        }

        updateFieldChanges({...filterData, xspeciesid: value, xsubspeciesid: undefined, xserotypeid: undefined});
    }

    const onlySpeciesChangeHandler = (event, value) => {
        if (value !== undefined && props.allLevels)  {
            runSubSpeciesListQuery(value, onlySubspeciesRetrieved, errorWhenRetrievingData);
            runSerotypeListQuery(value, onlySerotypeRetrieved, errorWhenRetrievingData);
        } else {
            setOnlySubSpeciesOptions([]);
            setOnlySeroTypeOptions([]);
        }

        updateFieldChanges({...filterData, onlyspeciesid: value, onlysubspeciesid: undefined, onlyserotypeid: undefined});
    }

    const subspeciesChangeHandler = (event, value) => {
        updateFieldChanges({...filterData, xsubspeciesid: value, xserotypeid: undefined});
    }

    const serotypeChangeHandler = (event, value) => {
        updateFieldChanges({...filterData, xsubspeciesid: undefined, xserotypeid: value});
    }

    const onlySubspeciesChangeHandler = (event, value) => {
        updateFieldChanges({...filterData, onlysubspeciesid: value, onlyserotypeid: undefined});
    }

    const onlySerotypeChangeHandler = (event, value) => {
        updateFieldChanges({...filterData, onlysubspeciesid: undefined, onlyserotypeid: value});
    }

    const orgGroupChangeHandler = (event, value) => {
        updateFieldChanges({...filterData, orggroupcodingid: value, xorderid: undefined, xfamilyid: undefined, xgenusid: undefined, xspeciesid: undefined, xsubspeciesid: undefined, xserotypeid: undefined, onlygenusid: undefined, onlyspeciesid: undefined, onlysubspeciesid: undefined, onlyserotypeid: undefined});
        resetFields(true, true, true, true, true, true, true, true);
    }

    const familyRetrieved = (data) => {
        const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
        setFamilyOptions(newData);
    }

    const genusRetrieved = (data) => {
        const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
        setGenusOptions(newData);
    }

    const speciesRetrieved = (data) => {
        const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
        setSpeciesOptions(newData);
    }

    const onlySpeciesRetrieved = (data) => {
        const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
        setOnlySpeciesOptions(newData);
    }

    const subspeciesRetrieved = (data) => {
        const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
        setSubSpeciesOptions(newData);
    }

    const onlySubspeciesRetrieved = (data) => {
        const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
        setOnlySubSpeciesOptions(newData);
    }

    const serotypeRetrieved = (data) => {
        const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
        setSeroTypeOptions(newData);
    }

    const onlySerotypeRetrieved = (data) => {
        const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
        setOnlySeroTypeOptions(newData);
    }

    const organismSearchClickHandler = () => {
        clearData();
        props.rightButtonClick([], "customnovalidation");
    };

    const orderConfig = { Id: "OrderId", Type: 'combobox', Label: TranslateTag("@GenOrd@", props.language), MinFilterLength: 2, Options: orderOptions, value: filterData.xorderid, Placeholder: TranslateTag("@GenEntC@", props.language)};
    const familyConfig = { Id: "FamilyId", Type: 'combobox', Label: TranslateTag("@GenFam@", props.language), Options: familyOptions, value: filterData.xfamilyid, Resettable: true };
    const genusConfig = { Id: "GenusId", Type: 'combobox', Label: TranslateTag("@GenGen@", props.language), Options: genusOptions, value: filterData.xgenusid, Resettable: true };
    const speciesConfig = { Id: "SpeciesId", Type: 'combobox', Label: TranslateTag("@GenSpeB@", props.language), Options: speciesOptions, value: filterData.xspeciesid, Resettable: true };
    const orgGroupConfig = { Id: "OrgGroupCodingId", Type: 'combobox', Label: TranslateTag("@GenOrgE@", props.language), Options: orgGroupOptions, value: filterData.orggroupcodingid, Resettable: true, RemoveFixed: true };
    const subspeciesConfig = { Id: "SubSpeciesId", Type: 'combobox', Label: TranslateTag("@GenSubA@", props.language), Options: subspeciesOptions, value: filterData.xsubspeciesid, Resettable: true };
    const serotypeConfig = { Id: "SerotypeId", Type: 'combobox', Label: TranslateTag("@GenSer@", props.language), Options: serotypeOptions, value: filterData.xserotypeid, Resettable: true }
    const onlyGenusConfig = { Id: "OnlyGenusId", Type: 'filteredcombo', Label: TranslateTag("@GenGen@", props.language), MinFilterLength: 2, Options: onlyGenusOptions, value: filterData.onlygenusid, Placeholder: TranslateTag("@GenEntC@", props.language), Resettable: true };
    const onlySpeciesConfig = { Id: "OnlySpeciesId", Type: 'combobox', Label: TranslateTag("@GenSpeB@", props.language), Options: onlySpeciesOptions, value: filterData.onlyspeciesid, Resettable: true };
    const onlySubspeciesConfig = { Id: "OnlySubSpeciesId", Type: 'combobox', Label: TranslateTag("@GenSubA@", props.language), Options: onlySubspeciesOptions, value: filterData.onlysubspeciesid, Resettable: true };
    const onlySerotypeConfig = { Id: "OnlySerotypeId", Type: 'combobox', Label: TranslateTag("@GenSer@", props.language), Options: onlySerotypeOptions, value: filterData.onlyserotypeid, Resettable: true }

    const pageCss = props.fullScreen ? "app-crafted-fullscreencontent" : "app-crafted-content";

    return (
        <div className={pageCss}>
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <div className="organismscope-formcolumn">
                <SingleLineField key="OrderId" config={orderConfig} changeHandler={orderChangeHandler}></SingleLineField>                    
                <SingleLineField key="FamilyId" config={familyConfig} changeHandler={familyChangeHandler}></SingleLineField>
                <SingleLineField key="GenusId" config={genusConfig} changeHandler={genusChangeHandler}></SingleLineField>                    
                <SingleLineField key="SpeciesId" config={speciesConfig} changeHandler={speciesChangeHandler}></SingleLineField>
                <SingleLineField key="SubSpeciesId" config={subspeciesConfig} changeHandler={subspeciesChangeHandler}></SingleLineField>
                <SingleLineField key="SerotypeId" config={serotypeConfig} changeHandler={serotypeChangeHandler}></SingleLineField>
                &nbsp;
                <Separator>{TranslateTag("@GenOr@", props.language)}</Separator>
                <SingleLineField key="OnlyGenusId" config={onlyGenusConfig} changeHandler={onlyGenusChangeHandler}></SingleLineField>
                <SingleLineField key="OnlySpeciesId" config={onlySpeciesConfig} changeHandler={onlySpeciesChangeHandler}></SingleLineField>
                <SingleLineField key="OnlySubSpeciesId" config={onlySubspeciesConfig} changeHandler={onlySubspeciesChangeHandler}></SingleLineField>
                <SingleLineField key="OnlySerotypeId" config={onlySerotypeConfig} changeHandler={onlySerotypeChangeHandler}></SingleLineField>
                &nbsp;
                <Separator>{TranslateTag("@GenOr@", props.language)}</Separator>
                <SingleLineField key="OrgGroupCodingId" config={orgGroupConfig} changeHandler={orgGroupChangeHandler}></SingleLineField>
                &nbsp;
                <Separator>{TranslateTag("@GenOr@", props.language)}</Separator>
                <div className="cultureorganismselection-buttons">
                    <CompoundButton primary onClick={() => organismSearchClickHandler()} >
                        <div className="printpublish-button">
                            {searchIcon}
                            <br />
                            {TranslateTag("@SpeFul@", props.language)}
                        </div>
                    </CompoundButton>
                </div>
            </div>
        </div>
    )
};

const mapStateToProps = state => {
    return {
        lists: state.config.lists
    };
}

export default connect(mapStateToProps)(OrganismScope);