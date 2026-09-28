import React from 'react';
import {connect} from 'react-redux';
import './BarcodePage.css';
import BarcodeRow from './BarcodeRow';


const BarcodePage = (props) => {

    const printConfig = props.barcodeprintconfigs.filter((config) => {
        return config.Name === props.config.type;
    })[0];

    let display = [];
    for (var i = 0; i < printConfig.NumberOfRows; i++) {
        display.push(i);
    }

    return (
        <div>
            {display.map((index) => {
                return <BarcodeRow  key={index} config={props.config} record={props.record}></BarcodeRow>
            })}
        </div>
    );
};

const mapStateToProps = state => {
    return {
        barcodeprintconfigs: state.config.barcodeprintconfigs
    };
}

export default connect(mapStateToProps)(BarcodePage);
