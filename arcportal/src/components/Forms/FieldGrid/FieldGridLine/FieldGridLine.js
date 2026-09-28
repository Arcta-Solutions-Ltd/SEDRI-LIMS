import React from 'react';
import FieldGridField from '../FieldGridField/FieldGridField';

const FieldGridLine = (props) => {
    return props.config.map(
        (config) =>
            (config.Visible === undefined || config.Visible) && (
                <FieldGridField
                    onKeyDown={props.onKeyDown}
                    key={config.Id}
                    config={config}
                    changeHandler={(field, value) =>
                        props.changeHandler(props.lineNumber, field, value)
                    }
                    data={props.data[props.lineNumber - 1]}
                    uievents={props.uievents}
                    forms={props.forms}
                    lists={props.lists}
                    language={props.language}
                    recordId={props.recordId}
                ></FieldGridField>
            )
    );
};

export default FieldGridLine;
