import React, { useEffect, useState } from 'react';
import MenuPermissions from '../Roles/MenuPermissions/MenuPermissions';
import Post from '../../../Data/Post';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import TextDisplay from '../../Forms/TextDisplay/TextDisplay';

const SelectQcOrganismsPage = (props) => {
    const getValueFromKeyValuePair = (data, key, upperKey = false) => {
        if (!data) return data;
        const keyValuePair = upperKey
            ? data.find((o) => o.Key === key)
            : data.find((o) => o.key === key);
        return keyValuePair ? keyValuePair.value : undefined;
    };

    const newTestProfileId = getValueFromKeyValuePair(
        props.inputData,
        'TestProfileId'
    );
    const currentTestProfileId = getValueFromKeyValuePair(
        props.data,
        'profileid',
        true
    );
    const qcOrganisms = getValueFromKeyValuePair(
        props.data,
        'qcorganisms',
        true
    );

    useEffect(() => {
        if (currentTestProfileId == newTestProfileId) {
            return;
        }

        const Parameters = [
            { Key: 'iqctestprofileid', Value: newTestProfileId },
        ];
        const criteria = {
            Name: props.config.QueryName,
            Parameters,
        };
        Post(
            'query/filteredget',
            criteria,
            dataRetrievedSuccessfully,
            errorWhenRetrievingData
        );
    }, [newTestProfileId]);

    const dataRetrievedSuccessfully = (data) => {
        const changes = [
            {
                key: 'qcorganisms',
                value: {
                    Key: 'qcorganisms',
                    value: JSON.parse(data.Crafted[0].Contents),
                },
            },
            {
                key: 'profileid',
                value: { Key: 'profileid', value: newTestProfileId },
            },
        ];

        props.changeHandler('multiplechanges', changes, { rootCopy: true });
    };

    const changeHandler = (id, value) => {
        const changes = [
            {
                key: 'qcorganisms',
                value: {
                    Key: 'qcorganisms',
                    value: qcOrganisms.map((item) =>
                        item.Key === id ? value : item
                    ),
                },
            },
            {
                key: 'profileid',
                value: { Key: 'profileid', value: newTestProfileId },
            },
        ];
        props.changeHandler('multiplechanges', changes, { rootCopy: true });
    };

    const errorWhenRetrievingData = () => {};
    
    return qcOrganisms && qcOrganisms.length > 0 ? (
        <MenuPermissions
            config={props.config}
            data={qcOrganisms}
            changeHandler={changeHandler}
            language={props.language}
        />
    ) : (
        <div className="app-crafted-content">
            <div className="app-crafted-title">{props.config.PageTitle}</div>
            <div className="app-crafted-headertext">
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <div
                style={{
                    fontWeight: 'bold',
                    display: 'flex',
                    alignContent: 'center',
                    justifyContent: 'center',
                }}
            >
                <div>{TranslateTag('@QuaIqcProNot@', props.language)}.</div>
            </div>
        </div>
    );
};

export default SelectQcOrganismsPage;
