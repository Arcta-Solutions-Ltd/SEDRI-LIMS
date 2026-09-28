import React from 'react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import './FormattedJsonViewer.css';
import FormattedJsonDetails from '../FormattedJsonDetails/FormattedJsonDetails';

/**
 * Panel that shows the stored contents of a queue entry, used by the diary and the monitoring screens.
 *
 * @param {Object} props
 * @param {Object} props.config - Crafted page config supplying the panel title and header text.
 * @param {Array} props.data - Rendered items returned by the backend for the selected queue entry.
 */
const FormattedJsonViewer = (props) => {

    return (
        <div className="app-crafted-content" id="formattedjson-root">
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <FormattedJsonDetails data={props.data}></FormattedJsonDetails>
        </div>
    );
};
  
export default FormattedJsonViewer;