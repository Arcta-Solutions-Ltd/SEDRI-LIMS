import React from 'react';
import './MenuPermissions.css';
import { isMandatorySidebarKey } from './menuPermissionKeys';

const MenuPermissionsView = (props) => {

    if (props.data === undefined || props.data === null || props.data.length === 0) {
        return (
            <div>No menu permissions data!!</div>
        )
    }

    const permissions = [...props.data.Crafted[0].Contents];

    return (
        <div className='menupermissions-view-details'>
            {permissions.map((menuItem)=> { return (
                <div className='menupermissions-view-field' key={menuItem.Key}>
                    <div className='menupermissions-view-itemname'>
                        {menuItem.Name}
                    </div>
                    <div className='menupermissions-view-itemvalue'>
                        {isMandatorySidebarKey(menuItem.Key) || menuItem.Allowed === "Yes" ? 'Allowed' : 'Disallowed'}
                    </div>
                </div>
            )})}
        </div>
    );
};

export default MenuPermissionsView;