import React, {useEffect, useState} from 'react';
import './PatientSearchResult.css';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import Post from '../../../../Data/Post';
import { Icon } from '@fluentui/react/lib/Icon';
import SimpleCard from '../../../General/SimpleCard/SimpleCard';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import TransformDatesInJson from '../../../../Utils/Local/TransformDatesInJson';
import buildPatientSearchQueryParameters from './buildPatientSearchQueryParameters';


const PatientSearchResult = (props) => {

    const [searchResults, setSearchResults] = useState();

    const addIcon = <Icon iconName="Add" />;

    useEffect(() => {
        const criteria = { Name: props.config.QueryName, Parameters: buildPatientSearchQueryParameters(props.data) };
        Post('query/filteredget', criteria, patientSearchResultsHandler, errorWhenRetrievingData);
    }, [props.config, props.data]);

    const patientSearchResultsHandler = (data) => {
        TransformDatesInJson(data);
        setSearchResults(data);
    }

    const errorWhenRetrievingData = () => {

    }

    const setPageValue = (target, source, fieldChanges)  => {
        const record = props.data.filter((f) => f.key !== undefined && f.key.toLowerCase() === source); 
        const newValue = record.length > 0 ? record[0].value : "";
        fieldChanges.push({key: target, value: { key: target, value: newValue}});
    }

    const buttonClickHandler = (button) => {
        const fieldChanges = [];
        if (button.id === -1) {
            fieldChanges.push({key: "PatientId", value: { Key: "PatientId", value: 0 }});
            setPageValue("PatientRef", "patientrefsearch", fieldChanges);
            setPageValue("FirstName", "values", fieldChanges);
            setPageValue("Surname", "surnamesearch", fieldChanges);
            setPageValue("LocationId", "locationsearch", fieldChanges);
            setPageValue("DateOfBirth", "dateofbirthsearch", fieldChanges);
            props.rightButtonClick(fieldChanges, "custom");
        } else {
            fieldChanges.push({key: "PatientId", value: { Key: "PatientId", value: button.id}});
            fieldChanges.push({key: "PatientRef", value: { Key: "PatientRef", value: button.patientref}});
            fieldChanges.push({key: "FirstName", value: { Key: "FirstName", value: button.firstname}});
            fieldChanges.push({key: "Surname", value: { Key: "Surname", value: button.surname}});
            const dob = button.dateofbirth ?? button.DateOfBirth;
            if (dob !== undefined && dob !== null && dob !== '') {
                fieldChanges.push({key: "DateOfBirth", value: { Key: "DateOfBirth", value: dob }});
            }
            props.rightButtonClick(fieldChanges);
        }
    }

    let dataToDisplay = [];
    if (searchResults !== undefined) {
        dataToDisplay = searchResults.map((item) => {

            const keys = Object.keys(item);
            const returnValue = {name: item.firstname + " " + item.surname, patientref: item.patientref};
            returnValue.location = item.fullyqualifiedname !== null && item.fullyqualifiedname !== undefined && item.fullyqualifiedname !== "" ? item.fullyqualifiedname : "";
            for (const key of keys) {
                const currentKey = key.toLowerCase();
                if (currentKey !== "firstname" && currentKey !== "surname" && currentKey !== "fullyqualifiedname" && currentKey !== "patientref") {
                    returnValue[key] = item[key];
                }
            }

            return returnValue;
        });
    }

    if (props.allowNewPatient) {
        const addNewPatientButton = {
            id: -1,
            UIEvent: "createpatientuievent",
            space1: "",
            icon: addIcon,
            space2: "",
            text: TranslateTag("@PatCreA@", props.language),
            colour: "blue",
            centre: true
        }
    
        dataToDisplay.push(addNewPatientButton);
    }

    return (
        <div className="patientsearchresult-content">
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <SimpleCard data={dataToDisplay} onClick={buttonClickHandler} onDoubleClick={buttonClickHandler}  onKeySelect={buttonClickHandler}></SimpleCard>
        </div>
    );
};
  
export default PatientSearchResult;