import React from 'react';
import './HomeCardItem.css';

const HomeCardItem = (props) => {

    const cardClicked = () => {
        props.onClick({id: props.id, type: props.type} );
    }

    const cardDoubleClicked = () => {
        props.onClick({id: props.id, type: props.type});
    }

    const fieldSizeCalc = (fieldLabel, fieldValue) => {

        var widthRequired = 0;
        if (fieldValue === null || fieldValue === undefined) {
            widthRequired = fieldLabel.length;
        } else {
            widthRequired = Math.max(fieldLabel.length, fieldValue.length);
        }

        if (Math.trunc(widthRequired / 25) === 0) {
            return '';
        } else {
            return 'xtra-long';
        }
    }

    return (
        <div key={props.id} onClick={cardClicked} onDoubleClick={cardDoubleClicked} className={'homecarditem-field ' + fieldSizeCalc(props.value, props.number)}>
            <div  className='homecarditem-itemname'>
                {props.value}
            </div>
            <div className='homecarditem-itemvalue'>
                {props.number}
            </div>
        </div>
    )
};

export default HomeCardItem;