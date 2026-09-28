import React from 'react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import ReadCraftedInputValue from '../../Admission/ReadCraftedInputValue';

/**
 * Crafted page that pretty-prints the raw JSON message stored on a queue row or instrument error.
 *
 * @param {Object} props
 * @param {Object} props.config - Crafted page config supplying the panel title and header text.
 * @param {Array<{key?: string, Key?: string, value?: *, Value?: *}>} props.data - Key/value entries from the initial query.
 */
const JsonViewer = (props) => {

    const rawMessage = ReadCraftedInputValue(props.data, 'message');
    let display = '';

    if (rawMessage !== undefined && rawMessage !== null && rawMessage !== '') {
        try {
            const jsonObject = typeof rawMessage === 'string' ? JSON.parse(rawMessage) : rawMessage;
            display = JSON.stringify(jsonObject, null, '\t');
        } catch {
            display = typeof rawMessage === 'string' ? rawMessage : JSON.stringify(rawMessage, null, '\t');
        }
    }

    return (
        <div className="app-crafted-content" id="jsonviewer-root">
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <pre id="jsonviewer-content">
                {display}
            </pre>
        </div>
    );
};
  
export default JsonViewer;
