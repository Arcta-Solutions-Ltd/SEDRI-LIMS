import React, {useEffect, useState} from 'react';
import {connect} from 'react-redux';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import SingleLineField from '../SingleLineField/SingleLineField';
import { Separator } from '@fluentui/react';

const ArcOrganismList = (props) => {

    const [organismList, setOrganismList] = useState(); 
    const [organismCodeList, setOrganismCodeList] = useState(); 

    useEffect(() => {
        let list = props.lists.filter(l => l.Name.toLowerCase() === "specimenorganism");
        setOrganismList(list[0].Options.map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}}));
        list = props.lists.filter(l => l.Name.toLowerCase() === "specimenorganismcode");
        setOrganismCodeList(list[0].Options.map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}}));
    },[]);

    const orgListConfig = { Id: "OrganismId", Type: 'combobox', Label: TranslateTag("@OrgNam@", props.language), Placeholder:  TranslateTag("@GenSelP@", props.language), OptionsName: "specimenorganism", Options: organismList, value: props.value };
    const orgCodesConfig = { Id: "OrganismCodeId", Type: 'combobox', Label: TranslateTag("@OrgCod@", props.language), Placeholder:  TranslateTag("@GenSelP@", props.language), OptionsName: "specimenorganismcode", Options: organismCodeList, value: props.value };

    const orgNameChangeHandler = (id, value) => {
        props.changeHandler(props.config.Id, value);
    };

    const orgCodeChangeHandler = (id, value) => {
        props.changeHandler(props.config.Id, value);
    }

    return (
        <React.Fragment>
            <SingleLineField key="OrganismId" config={orgListConfig} changeHandler={orgNameChangeHandler} onKeyDown={props.onKeyDown}></SingleLineField>
            <SingleLineField key="OrganismCodeId" config={orgCodesConfig} changeHandler={orgCodeChangeHandler} onKeyDown={props.onKeyDown}></SingleLineField>

            {orgListConfig.Options !== undefined && orgListConfig.Options !== null && orgListConfig.Options.length > 0 && props.includeOrText ? (
                    <React.Fragment>
                        <br />
                        <Separator>OR</Separator>
                    </React.Fragment>
            ) : (null) }
        </React.Fragment>
    )
}

const mapStateToProps = state => {
    return {
        lists: state.config.lists
    };
}

export default connect(mapStateToProps)(ArcOrganismList);