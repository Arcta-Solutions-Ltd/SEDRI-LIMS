import React from 'react';
import './TextDisplay.css';

const TextDisplay = (props) => {

    return (
        <div className="textdisplay-item">
            <div className="textdisplay-data">
                {props.text}
            </div>
        </div>
    )
};
  
export default TextDisplay;