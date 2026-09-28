import React from 'react';
import { Label } from '@fluentui/react';
//import TextDisplay  from '../TextDisplay/TextDisplay';

const ArcJson = (props) => {

    const jsonObject = JSON.parse(props.value);
    const display = JSON.stringify(jsonObject, null, "\t");

    return (
        <div>
            <Label>{props.label}</Label>
            <pre>
                {display}
            </pre>
        </div>

        // <TextDisplay label={props.label} text={display} readOnly={props.readOnly}></TextDisplay>
    );
};
  
export default ArcJson;
