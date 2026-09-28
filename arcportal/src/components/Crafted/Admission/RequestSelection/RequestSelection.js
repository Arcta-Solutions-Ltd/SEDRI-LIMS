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
 * Lets the user attach the specimen being registered to one of the existing requests, or start a new one.
 * Whether the existing requests are those of the patient or only those of the chosen admission is set by
 * the RequestScope property on the page config, so a form can be configured either way.
 */
const RequestSelection = (props) => {

    const [requests, setRequests] = useState();

    const scopeToAdmission = (props.config.RequestScope ?? "patient").toLowerCase() === "admission";
    const patientId = ReadCraftedInputValue(props.data, "patientid");
    const admissionId = ReadCraftedInputValue(props.data, "admissionid");
    const scopeId = scopeToAdmission ? admissionId : patientId;

    useEffect(() => {
        if (scopeId === undefined || scopeId === null || scopeId === "" || Number(scopeId) === 0) {
            setRequests([]);
            return;
        }

        const criteria = {
            Name: props.config.QueryName,
            Parameters: [{ key: scopeToAdmission ? "admissionid" : "patientid", value: scopeId }]
        };
        Post('query/filteredget', criteria, requestsRetrieved, errorWhenRetrievingData);
    }, [props.config, scopeId, scopeToAdmission]);

    const requestsRetrieved = (data) => {
        TransformDatesInJson(data);
        setRequests(data);
    }

    const errorWhenRetrievingData = () => {
        setRequests([]);
    }

    const buttonClickHandler = (button) => {
        const fieldChanges = [{ key: "RequestId", value: { Key: "RequestId", value: button.id === -1 ? 0 : button.id } }];
        props.rightButtonClick(fieldChanges, button.id === -1 ? "custom" : undefined);
    }

    let dataToDisplay = [];
    if (requests !== undefined) {
        dataToDisplay = requests.map((item) => ({
            id: rowField(item, 'Id'),
            description: rowField(item, 'RequestId') ?? "",
            raised: `${rowField(item, 'RequestDate') ?? ""} ${rowField(item, 'RequestTime') ?? ""}`.trim(),
            ward: rowField(item, 'Ward') ?? "",
            indication: rowField(item, 'Indication') ?? "",
            urgency: rowField(item, 'Urgency') ?? "",
            specimens: `${TranslateTag("@NeoSpe@", props.language)}: ${rowField(item, 'SpecimenCount') ?? 0}`
        }));
    }

    dataToDisplay.push({
        id: -1,
        icon: <Icon iconName="Add" />,
        text: TranslateTag("@NeoReqCre@", props.language),
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

export default RequestSelection;
