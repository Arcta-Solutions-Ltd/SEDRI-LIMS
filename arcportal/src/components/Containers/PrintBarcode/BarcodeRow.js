import React from 'react';
import {connect} from 'react-redux';
import './BarcodeRow.css';
import BarcodeItem from './BarcodeItem';


const BarcodeRow = (props) => {

    const printConfig = props.barcodeprintconfigs.filter((config) => {
        return config.Name === props.config.type;
    })[0];

    let display = [];
    for (var i = 0; i < printConfig.ItemsPerRow; i++) {
        display.push(i);
    }

    return (
        <div className='barcoderow-content'>
            {display.map((index) => {
                return <BarcodeItem  key={index} config={props.config} record={props.record}></BarcodeItem>
            })}
        </div>
    );
};

const mapStateToProps = state => {
    return {
        barcodeprintconfigs: state.config.barcodeprintconfigs
    };
}

export default connect(mapStateToProps)(BarcodeRow);
