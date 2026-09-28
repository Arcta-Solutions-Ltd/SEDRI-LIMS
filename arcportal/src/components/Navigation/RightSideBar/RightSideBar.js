import React from 'react';
import { Panel, PanelType, Nav } from '@fluentui/react';
import propTypes from 'prop-types';

const RightSidebar = (props) => {
    return (
        <div className="rightsidebar-content">
            <Panel
                isLightDismiss
                isOpen={props.visible}
                onDismiss={props.closed}
                type={PanelType.smallFixedFar}
            >
                <Nav groups={props.sidebarContents} />
            </Panel>
        </div>
    );
};

RightSidebar.propTypes = {
    visible: propTypes.bool.isRequired,
    closed: propTypes.func.isRequired,
};

export default RightSidebar;
