import React from 'react';

const ArcPlainText = (props) => {
    switch (props.config.Markup) {
        case 'heading':
            return (<h3 id={props.config.id}>{props.config.Label}</h3>);
        default:
            return (<span id={props.config.id}>{props.config.Label}</span>);
    }
};

export default ArcPlainText;
