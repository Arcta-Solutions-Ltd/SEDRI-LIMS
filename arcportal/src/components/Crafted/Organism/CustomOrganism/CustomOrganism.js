import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Separator } from '@fluentui/react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import AddValuesIntoValueList from '../../../../Utils/Forms/AddValuesIntoValueList';
import { runOrderListQuery, runFamilyListQuery, runSpeciesListQuery, runGenusListQuery } from './CustomOrganismQueries';
import './CustomOrganism.css';

const CustomOrganism = (props) => {

    const [orderOptions, setOrderOptions] = useState();
    const [familyOptions, setFamilyOptions] = useState();
    const [genusOptions, setGenusOptions] = useState();
    const [speciesOptions, setSpeciesOptions] = useState();
    const [filterData, setFilterData] = useState({ description: "", code: "", orderid: 0, familyid: 0, genusid: 0, speciesid: 0, subspeciesid: 0, serotypeid: 0, additionalid: "0" });

    useEffect(() => {

        const orderRetrieved = (data) => {
            const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
            setOrderOptions(newData);
        }

        runOrderListQuery(orderRetrieved, errorWhenRetrievingData);
    }, [])

    useEffect(() => {
        
        const GetFieldContents = (data, fieldName) => {
            if (data !== undefined && data != null) {
                var element = data[fieldName];
                return element !== undefined && element !== null ? element.toString() : element;
            }
        }

        const existingOrderRetrieved = (data) => {
            const newData = data.map((r) => { return {key: r.Key, text: r.Text}})
            setOrderOptions(newData);
        }

        var data = [];
        if (Array.isArray(props.data)) {
            var foo = [...props.data];

            foo.forEach(element => {
                element.key = element.Key;
            });

            data = AddValuesIntoValueList(data, foo);
        } else {
            data = {...props.data};
        }

        const newFilterData = {
            description: GetFieldContents(data, "Description"),
            code: GetFieldContents(data, "Code"),
            orderid: GetFieldContents(data, "OrderId"),
            familyid: GetFieldContents(data, "FamilyId"),
            genusid: GetFieldContents(data, "GenusId"),
            speciesid: GetFieldContents(data, "SpeciesId"),
            additionalid: GetFieldContents(data, "AdditionalId")
        }

        setFilterData(newFilterData);

        if (newFilterData.orderid !== undefined && newFilterData.orderid !== "0") {
            runOrderListQuery(existingOrderRetrieved, errorWhenRetrievingData);
            runFamilyListQuery(newFilterData.orderid, familyRetrieved, errorWhenRetrievingData);
        }

        if (newFilterData.familyid !== undefined && newFilterData.familyid !== "0") {
            runGenusListQuery(newFilterData.familyid, genusRetrieved, errorWhenRetrievingData);
        }

        if (newFilterData.genusid !== undefined && newFilterData.genusid !== "0") {
            runSpeciesListQuery(newFilterData.genusid, speciesRetrieved, errorWhenRetrievingData);
        }

    }, [props.data])

    const errorWhenRetrievingData = () => {
    }

    const orderChangeHandler = (event, value) => {
        if (value !== undefined)  {
            runFamilyListQuery(value, familyRetrieved, errorWhenRetrievingData);
        } else {
            setFamilyOptions([]);
            setGenusOptions([]);
            setSpeciesOptions([]);
        }
        updateFieldChanges({...filterData, orderid: value, familyid: undefined, genusid: undefined, speciesid: undefined});
    }

    const familyChangeHandler = (event, value) => {
        if (value !== undefined)  {
            runGenusListQuery(value, genusRetrieved, errorWhenRetrievingData);
        } else {
            setGenusOptions([]);
            setSpeciesOptions([]);
        }
        updateFieldChanges({...filterData, familyid: value, genusid: undefined, speciesid: undefined});
    }

    const genusChangeHandler = (event, value) => {
        if (value !== undefined)  {
            runSpeciesListQuery(value, speciesRetrieved, errorWhenRetrievingData);
        } else {
            setSpeciesOptions([]);
        }
        updateFieldChanges({...filterData, genusid: value, speciesid: undefined});
    }

    const updateFieldChanges = (data) => {

        const changes = [{ key: "OrderId", value: { Key: "OrderId", value: data.orderid }},
            { key: "FamilyId", value: { Key: "FamilyId", value: data.familyid }},
            { key: "GenusId", value: { Key: "GenusId", value: data.genusid }},
            { key: "SpeciesId", value: { Key: "SpeciesId", value: data.speciesid }},
            { key: "Description", value: { Key: "Description", value: data.description }},
            { key: "Code", value: { Key: "Code", value: data.code }},
            { key: "AddtionalId", value: { Key: "AdditionalId", value: data.additionalid }}
        ];
        setFilterData(data);
        props.changeHandler("multiplechanges", changes, { rootCopy: true });
    }

    const speciesChangeHandler = (event, value) => {
        updateFieldChanges({...filterData, speciesid: value, serotypeid: undefined, subspeciesid: undefined});
    }

    const descriptionChangeHandler = (event, value) => {
        updateFieldChanges({...filterData, description: value});
    }

    const codeChangeHandler = (event, value) => {
        updateFieldChanges({...filterData, code: value});
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

    const customDesc = { Id: "Description", Type: 'singleline', Label: TranslateTag("@GenEntB@", props.language), value: filterData.description, Required: true, Placeholder: TranslateTag("@GenEntB@", props.language) };
    const standardDesc = { Id: "Description", Type: 'text', Label: TranslateTag("@GenEntB@", props.language), value: filterData.description, Placeholder: TranslateTag("@GenEntB@", props.language) };
    const code = { Id: "Code", Type: 'singleline', Label: TranslateTag("@GenCodA@", props.language), value: filterData.code, Placeholder: TranslateTag("@GenCodA@", props.language) };
    const orderConfig = { Id: "OrderId", Type: 'combobox', Label: TranslateTag("@GenOrd@", props.language), MinFilterLength: 2, Options: orderOptions, value: filterData.orderid, Placeholder: TranslateTag("@GenEntC@", props.language) };
    const familyConfig = { Id: "FamilyId", Type: 'combobox', Label: TranslateTag("@GenFam@", props.language), Options: familyOptions, value: filterData.familyid, Resettable: true };
    const genusConfig = { Id: "GenusId", Type: 'combobox', Label: TranslateTag("@GenGen@", props.language), Options: genusOptions, value: filterData.genusid, Resettable: true };
    const speciesConfig = { Id: "SpeciesId", Type: 'combobox', Label: TranslateTag("@GenSpeB@", props.language), Options: speciesOptions, value: filterData.speciesid, Resettable: true };

    var display = (null);
    if (props.config.Name === "addcustompage") { display = (
        <div>
            <SingleLineField key="Description" config={customDesc} changeHandler={descriptionChangeHandler}></SingleLineField>
            <SingleLineField key="Code" config={code} changeHandler={codeChangeHandler}></SingleLineField>
            <br />
            <Separator></Separator>
            <SingleLineField key="OrderId" config={orderConfig} changeHandler={orderChangeHandler}></SingleLineField>
            <SingleLineField key="FamilyId" config={familyConfig} changeHandler={familyChangeHandler}></SingleLineField>
            <SingleLineField key="GenusId" config={genusConfig} changeHandler={genusChangeHandler}></SingleLineField>
            <SingleLineField key="SpeciesId" config={speciesConfig} changeHandler={speciesChangeHandler}></SingleLineField>
        </div>
    )} else {
        display = (
            <div>
                <SingleLineField key="Description" config={standardDesc} changeHandler={descriptionChangeHandler}></SingleLineField>
                <SingleLineField key="Code" config={code} changeHandler={codeChangeHandler}></SingleLineField>
            </div>
        )
    };

    const pageCss = props.fullScreen ? "app-crafted-fullscreencontent" : "app-crafted-content";

    return (
        <div className={pageCss}>
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <div className="customorganism-formcolumn">
                {display}
            </div>
        </div>
    )
};

const mapStateToProps = state => {
    return {
        lists: state.config.lists
    };
}

export default connect(mapStateToProps)(CustomOrganism);
