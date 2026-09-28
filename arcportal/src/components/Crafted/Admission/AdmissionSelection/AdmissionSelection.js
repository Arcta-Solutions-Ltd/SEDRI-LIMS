import React, { useEffect, useState } from 'react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import Post from '../../../../Data/Post';
import { Icon } from '@fluentui/react/lib/Icon';
import SimpleCard from '../../../General/SimpleCard/SimpleCard';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import TransformDatesInJson from '../../../../Utils/Local/TransformDatesInJson';
import ReadCraftedInputValue from '../ReadCraftedInputValue';

/** Reads a field from a special-query row whether the API used lowercase or PascalCase keys. */
const rowField = (item, name) => {
    if (item == null) {
        return undefined;
    }
    if (item[name] !== undefined && item[name] !== null) {
        return item[name];
    }
    const lower = name.toLowerCase();
    return item[lower];
};

/**
 * Lets the user attach the request being raised to one of the patient's existing admissions, or start a new
 * one. Selecting the new admission card sets AdmissionId to zero, which is what the backend reads as
 * "create one from the admission fields in this payload", and raises the newadmission form state so the
 * admission data entry page becomes visible.
 */
const AdmissionSelection = (props) => {

    const [admissions, setAdmissions] = useState();

    const patientId = ReadCraftedInputValue(props.data, "patientid");

    useEffect(() => {
        if (patientId === undefined || patientId === null || patientId === "" || Number(patientId) === 0) {
            setAdmissions([]);
            return;
        }

        const criteria = { Name: props.config.QueryName, Parameters: [{ key: "patientid", value: patientId }] };
        Post('query/filteredget', criteria, admissionsRetrieved, errorWhenRetrievingData);
    }, [props.config, patientId]);

    const admissionsRetrieved = (data) => {
        TransformDatesInJson(data);
        setAdmissions(data);
    }

    const errorWhenRetrievingData = () => {
        setAdmissions([]);
    }

    const buttonClickHandler = (button) => {
        const fieldChanges = [{ key: "AdmissionId", value: { Key: "AdmissionId", value: button.id === -1 ? 0 : button.id } }];
        props.rightButtonClick(fieldChanges, button.id === -1 ? "custom" : undefined);
    }

    let dataToDisplay = [];
    if (admissions !== undefined) {
        dataToDisplay = admissions.map((item) => ({
            id: rowField(item, 'Id'),
            description: `${rowField(item, 'AdmissionDate') ?? ""} ${rowField(item, 'AdmissionTime') ?? ""}`.trim(),
            requests: `${TranslateTag("@NeoReq@", props.language)}: ${rowField(item, 'RequestCount') ?? 0}`,
            specimens: `${TranslateTag("@NeoSpe@", props.language)}: ${rowField(item, 'SpecimenCount') ?? 0}`
        }));
    }

    dataToDisplay.push({
        id: -1,
        icon: <Icon iconName="Add" />,
        text: TranslateTag("@NeoAdmCre@", props.language),
        colour: "blue",
        centre: true
    });

    return (
        <div className="patientsearchresult-content">
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <SimpleCard data={dataToDisplay} onClick={buttonClickHandler} onDoubleClick={buttonClickHandler} onKeySelect={buttonClickHandler}></SimpleCard>
        </div>
    );
};

export default AdmissionSelection;
