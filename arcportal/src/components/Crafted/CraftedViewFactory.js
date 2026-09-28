import React from 'react';
import MenuPermissionsView from './Roles/MenuPermissions/MenuPermissionsView';
import EventPermissionsView from './Roles/EventPermissions/EventPermissionsView';


const CraftedViewFactory = (props) => {

    let pageToDisplay = (null);
    switch(props.config.Name.toLowerCase()) {
        case "menupermission":
            pageToDisplay = <MenuPermissionsView config={props.config} data={props.data}></MenuPermissionsView>
            break;
        case "eventpermission":
            pageToDisplay = <EventPermissionsView config={props.config} data={props.data}></EventPermissionsView>
            break;
        default:
            pageToDisplay = <div>No crafted view found</div>
            break;
    }

    return (
        <div>
            {pageToDisplay}
        </div>
    );
};
  
export default CraftedViewFactory;