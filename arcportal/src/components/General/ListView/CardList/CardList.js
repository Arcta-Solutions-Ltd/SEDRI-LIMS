import React, { useState } from 'react';
import './CardList.css';
import CardItem from './CardItem/CardItem';

const CardList = (props) => {

    const [selectedItem, setSelectedItem] = useState(0);

    var cards = [];

    if (typeof props.data[Symbol.iterator] === 'function') {
        for(let item of props.data){
            cards.push(item);
        }
    }

    var keys = [];
    if (cards.length > 0) {
        for (var k in cards[0]) keys.push(k);
    }

    const itemSelected = (index, data, selected) => {
        if (selected) {
            setSelectedItem(index);
        }
        props.itemSelected(data, selected);
    }

    return (
        <div className="cardlist-content">
            {cards.map((card, index) => (
                <CardItem
                    key={index}
                    keys={keys}
                    fieldNames={props.fieldNames}
                    data={card}
                    menuItems={props.menuItems}
                    itemSelected={itemSelected}
                    selectedItem={selectedItem}
                    itemInvokedHandler={props.itemInvokedHandler}
                    cardId={index}
                    laboratoryConfig={props.laboratoryConfig}
                    language={props.language}
                    turnaroundTimeFieldName={props.turnaroundTimeFieldName}
                    onMenuButtonClick={props.onMenuButtonClick}
                    forms={props.forms}
                    viewName={props.viewName}>
                </CardItem>
            ))}
        </div>
    );
};
  
export default CardList;
