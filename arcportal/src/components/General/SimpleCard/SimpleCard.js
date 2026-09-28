import React from 'react';
import './SimpleCard.css';
import SimpleCardItem from './SimpleCardItem/SimpleCardItem';

const SimpleCard = (props) => {

    return (
        <div className={props.data.length > 1 ? "simplecard-content" : "simplecard-content-button-only"}>
            {props.data !== undefined && props.data.map((item, index) => (
                <SimpleCardItem key={index} data={item} onClick={props.onClick} onDoubleClick={props.onDoubleClick} onKeySelect={props.onKeySelect} onDelete={props.onDelete} deleteDisabled={props.deleteDisabled}></SimpleCardItem>
            ))}
        </div>
    );
};

export default SimpleCard;