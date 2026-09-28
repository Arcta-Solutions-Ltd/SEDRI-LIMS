import React, { useState, useEffect } from 'react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import { CompoundButton } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { NotNullOrUndefined } from '../../../../Utils/General/Undefined';

const ChangeOrganismSelector = (props) => {

    const [currentOrganismScope, setCurrentOrganismScope] = useState(true);
    const [startOrganismConfigured, setStartOrganismConfigured] = useState();

    const orderConfig = { Id: "OrderName", Type: 'text', Label: TranslateTag("@GenOrd@", props.language) };
    const familyConfig = { Id: "FamilyName", Type: 'text', Label: TranslateTag("@GenFam@", props.language) };
    const organismConfig = { Id: "OrganismName", Type: 'text', Label: TranslateTag("@GenOrgA@", props.language) };
    const orgGroupConfig = { Id: "OrganismName", Type: 'text', Label: TranslateTag("@GenCodB@", props.language) };

    useEffect(() => {
        setCurrentOrganismScope(props.data);
        setStartOrganismConfigured(hasOrganismBeenConfigured());
    }, []);

    const hasOrganismBeenConfigured = () => {
        let organismConfigured = "No";
        if (Array.isArray(props.data)) {
            const foundElements = props.data.filter((f) => f.Key.toLowerCase() === "organismid" || f.Key.toLowerCase() === "orderid"
             || f.Key.toLowerCase() === "orggroupcodingid" || f.Key.toLowerCase() === "familyid"
            );
            const doElementsContainData = foundElements.filter((f) => f.value !== undefined && f.value !== "" && f.value !== "0" );
            organismConfigured = doElementsContainData.length > 0 ? "Yes" : "No";
        }

        return organismConfigured;
    }

    const orderVal = props.data.filter(f => f.Key ==="order");
    if (orderVal.length > 0) {
        orderConfig.value = orderVal[0].value;
    }
    const familyVal = props.data.filter(f => f.Key === "family");
    if (familyVal.length > 0) {
        familyConfig.value = familyVal[0].value;
    }
    const organismVal = props.data.filter(f => f.Key === "organism");
    if (organismVal.length > 0) {
        organismConfig.value = organismVal[0].value;
    }
    const orgGroupVal = props.data.filter(f => f.Key === "orggroup");
    if (orgGroupVal.length > 0) {
        orgGroupConfig.value = orgGroupVal[0].value;
    }

    const pageCss = props.fullScreen ? "app-crafted-fullscreencontent" : "app-crafted-content";

    const newOrganismHandler = () => {

        const changes = [{ key: "OrganismConfigured", value: { Key: "OrganismConfigured", value: "Yes"}}];

        props.changeHandler("multiplechanges", changes, { rootCopy: true }, true);
        props.rightButtonClick([], "custom");
    }

    const nextPageHandler = () => {

        const order = currentOrganismScope.filter(k => k.Key === "orderid");
        const family = currentOrganismScope.filter(k => k.Key === "familyid");
        const genus = currentOrganismScope.filter(k => k.Key === "genusid");
        const species = currentOrganismScope.filter(k => k.Key === "speciesid");
        const orggroupcoding = currentOrganismScope.filter(k => k.Key === "orggroupcodingid");
        const organism = currentOrganismScope.filter(k => k.Key === "organismid");

        const changes = [{ key: "orderid", value: order.length > 0 ? order[0] : { Key: "orderid", value: "0" }},
            { key: "familyid", value: family.length > 0 ? family[0] : { Key: "familyid", value: "0" }},
            { key: "genusid", value: genus.length > 0 ? genus[0] : { Key: "genusid", value: "0"}},
            { key: "speciesid", value: species.length > 0 ? species[0] : { Key: "speciesid", value: "0"}},
            { key: "organismid", value: organism.length > 0 ? organism[0] : { Key: "organismid", value: "0"}},
            { key: "orggroupcodingid", value: orggroupcoding.length > 0 ? orggroupcoding[0] : { Key: "orggroupcodingid", value: "0" }},
            { key: "OrganismConfigured", value: { Key: "OrganismConfigured", value: startOrganismConfigured}}
        ];

        props.changeHandler("multiplechanges", changes, { rootCopy: true }, true);
        props.rightButtonClick([], "");
    }

    let fieldToDisplay = (null);
    let buttonText = TranslateTag("@OrgDon@", props.language);
    if (NotNullOrUndefined(orderConfig.value) || NotNullOrUndefined(familyConfig.value) || NotNullOrUndefined(organismConfig.value) || NotNullOrUndefined(orgGroupConfig.value)) {
        fieldToDisplay = <div className="organismsearch-formcolumn">
            <SingleLineField key="Order" config={orderConfig} ></SingleLineField>
            <SingleLineField key="Family" config={familyConfig} ></SingleLineField>
            <SingleLineField key="Organism" config={organismConfig} ></SingleLineField>
            <SingleLineField key="OrgGroup" config={orgGroupConfig} ></SingleLineField>
        </div>;
        buttonText = TranslateTag("@OrgKee@", props.language);
    }

    return (
        <div className={pageCss}>
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>

            {fieldToDisplay}

            <div className="cultureorganismselection-buttons">
                    <CompoundButton primary onClick={newOrganismHandler} >
                        <div className="printpublish-button">
                            {TranslateTag("@OrgSelA@", props.language)}
                        </div>
                    </CompoundButton>
                    <br></br>
                    <br></br>
                    <CompoundButton primary onClick={nextPageHandler} >
                        <div className="printpublish-button">
                            {buttonText}
                        </div>
                    </CompoundButton>
            </div>
        </div>
    )

}

export default ChangeOrganismSelector