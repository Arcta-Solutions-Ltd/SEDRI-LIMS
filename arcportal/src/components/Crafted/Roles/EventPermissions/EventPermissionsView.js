import React from 'react';
import './EventPermissions.css';

const EventPermissionsView = (props) => {

    if (props.data === undefined || props.data === null || props.data.length === 0) {
        return (
            <div>No event permissions data!!</div>
        )
    }

    let topicList = [], orderedList = [];
    props.data.Crafted[1].Contents.forEach((item) => {
        if (!topicList.includes(item.Topic)) {
            topicList.push(item.Topic);
        }
    });

    topicList.forEach((item) => {
        let topicGroup = { topicName: item, items: [] };
        orderedList.push(topicGroup);
    });

    props.data.Crafted[1].Contents.forEach((item) => {
        let index = orderedList.findIndex((topic) => topic.topicName === item.Topic);
        orderedList[index].items.push({ description: item.Description, allowed: item.Allowed, Key: item.Key});
    });

    return (
        <div>
            {orderedList.map((topic) => { return (
                <div key={topic.topicName}>
                    <div className='eventpermissions-view-topic'>
                        {topic.topicName + ':'}
                    </div>
                    <div className='eventpermissions-view-details'>
                        {topic.items.map((menuItem) => { return (
                            <div className='eventpermissions-view-field' key={menuItem.Key}>
                                <div className='eventpermissions-view-itemname-wide'>
                                    {menuItem.description}
                                </div>
                                <div className='eventpermissions-view-itemvalue'>
                                    {menuItem.allowed === "Yes" ? 'Allowed' : 'Disallowed'}
                                </div>
                            </div>
                        )})}
                    </div>
                </div>
            )})}
        </div>
    );
};
  
export default EventPermissionsView;

