import React, { useState } from 'react';
import IqcResultLine from './IqcResultLine/IqcResultLine';
import TextDisplay from '../../Forms/TextDisplay/TextDisplay';
import './InputIqcResultsPage.css';

const InputIqcResultsPage = (props) => {
    const localData = [...props.data];
    const organisms = localData.find(
        (item) => item.Key === 'QcOrganismsWithIqcResults'
    );

    const changeHandler = (id, value) => {
        organisms.value.forEach((organism) => {
            organism.IqcResults.forEach((result) => {
                if (result.IqcResultId === id) {
                    result.ResultValue = value;
                }
            });
        });

        props.changeHandler('QcOrganismsWithIqcResults', organisms);
    };

    return (
        <div className="app-crafted-content">
            <div className="app-crafted-title">{props.config.PageTitle}</div>
            <div className="headertext">
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            {organisms.value.map((organism) => (
                <div key={organism.QcOrganismId}>
                    <div className="organism-title">
                        {organism.QcOrganismName} ({organism.StandardsBody}{' '}
                        {organism.PrimaryStrain})
                    </div>
                    <div className="iqc-results">
                        {organism.IqcResults.map((result) => (
                            <div className="result" key={result.IqcResultId}>
                                <IqcResultLine
                                    resultId={result.IqcResultId}
                                    antibioticName={result.AntibioticName}
                                    resultValue={result.ResultValue}
                                    expectedLowerValue={
                                        result.ExpectedLowerValue
                                    }
                                    expectedUpperValue={
                                        result.ExpectedUpperValue
                                    }
                                    changeHandler={changeHandler}
                                    language={props.language}
                                ></IqcResultLine>
                            </div>
                        ))}
                    </div>
                </div>
            ))}
        </div>
    );
};

export default InputIqcResultsPage;
