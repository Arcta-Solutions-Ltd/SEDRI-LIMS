import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import GetFieldsForForm from '../../../../Utils/Forms/GetFieldsForForm';
import Post from '../../../../Data/Post';

const LabelFields = (props) => {

    const [labelFieldOptions, setLabelFieldOptions] = useState([]);

    useEffect(() => {

        const dataRetrievedSuccessfully = (data) => {

            if (data === undefined || data === null || data === "") {
                return;
            } else {
                var listOfKeys = Object.keys(data);
                var availableFields = listOfKeys.map((k) => { return ( { key: k, text: k } ) });
                availableFields = availableFields.filter( f => f.key !== "id" );
                const fullSpecimenFieldOptions = GetFieldsForForm(props.config.Parameter2, props.forms, props.pages);
                var availableSpecimenFieldOptions = fullSpecimenFieldOptions.filter(o => listOfKeys.findIndex(k => k.toLowerCase() === o.id.toLowerCase() ) !== -1 );
                var fieldOptions = [];
                if (listOfKeys.includes('AccessionNumber')) { // Hacked for now as accession number is not a form field.
                    fieldOptions.push( { key: 'AccessionNumber', text: TranslateTag("@SpeAcc@", props.language) } )
                }
                availableSpecimenFieldOptions.forEach(o => {
                    if (fieldOptions.find(f => f.key === o.id) === undefined) {
                        fieldOptions.push( { key: o.id, text: o.field } )
                    }
                });
                setLabelFieldOptions(fieldOptions);
            }
        }
        const criteria = { Name: props.config.Parameter1, Parameters: [{ Key: 'id', Value: 1 }]};
        Post('query/filteredget', criteria, dataRetrievedSuccessfully, errorWhenRetrievingData);
    }, [])

    const errorWhenRetrievingData = () => {
    }

    const changeHandler = (event, value) => {
        props.changeHandler(undefined, value);
    }

    const labelFieldsConfig = { Id: "LabelFields", Type: 'dropdown', MultiSelect: true, Label: TranslateTag("@CfgLab1@", props.language), Options: labelFieldOptions, value: props.config.value, Resettable: true };

    return (
        <SingleLineField key="LabelFields" config={labelFieldsConfig} changeHandler={changeHandler}></SingleLineField>
    )
};

const mapStateToProps = state => {
    return {
        forms: state.config.forms,
        pages: state.config.pages,
        lists: state.config.lists,
        language: state.config.language,
    };
}

export default connect(mapStateToProps)(LabelFields);
