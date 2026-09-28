import React, {useState} from 'react';
import './EventPermissions.css';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import { Dropdown } from '@fluentui/react';
import ArcToggle from '../../../Forms/ArcToggle/ArcToggle';
import TranslateTag from '../../../../Utils/Local/TranslateTag';

const EventPermissions = (props) => {

    const [eventList, setEventList] = useState([]);
    const [data] = useState(props.data);

    var tempList = props.data.map((item) => item.TranslatedTopic);
    
    let optionList = [];
    tempList.forEach((item) => {
        if (!optionList.includes(item)) {
            optionList.push(item);
        }
    });
    optionList = optionList.map((item) => { return {key: item, text: item}})
    optionList = optionList.sort(function (a, b) {
        return ('' + a.text).localeCompare(b.text);
    })

    const valueChangeHandler = (event,selection) => {
        const filtered = data.filter((item) => item.TranslatedTopic === selection.key);
        const sorted = filtered.sort((a, b) => ('' + a.Description).localeCompare(b.Description));
        setEventList(sorted);
}

    const toggleChangeHandler = (id,value, extraInfo) => {
        const itemToChange = data.filter((item) => item.Key === id)[0];
        itemToChange.Allowed = value;
        
        const returnValue = {Key: id, Allowed: value, Event: id, Description: extraInfo.Description, Topic: extraInfo.Topic};
        props.changeHandler(id, returnValue)
    }

    return (
        <div className="app-crafted-content">
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <div className="eventpermissions-data">
                <Dropdown 
                    required={true}
                    placeholder={TranslateTag("@GenTopA@", props.language)}
                    label={TranslateTag("@GenTop@", props.language)} 
                    multiSelect={false}
                    options={optionList}
                    onChange={valueChangeHandler}
                    id="eventPermissionsDropDown"
                />
            </div>
            <div className="eventpermissions-items">
                {eventList !== undefined && eventList.map((item) => {
                    const config = {Label: item.Description, value: item.Allowed, id: item.Event, extraInfo: { Topic: item.Topic, Description: item.Description}}
                    return (
                        <div>
                            <ArcToggle config={config} valueChangeHandler={toggleChangeHandler} showText={false} ></ArcToggle>
                        </div>
                    )
                })}
            </div>
        </div>
    );
};
  
export default EventPermissions;

