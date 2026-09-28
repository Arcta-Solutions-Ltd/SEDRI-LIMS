import React from 'react';
import './Sidebar.css';
import { Panel, PanelType } from '@fluentui/react';
import Sidemenu from '../SideMenu/Sidemenu';
import propTypes from 'prop-types';

const Sidebar = (props) => {
    const panelStyles = {
        content: {
            padding: '0px',
        },
        commands: {
            margin: '0px',
        },
        contentInner: {
            backgroundColor: 'var(--navColour)',
        },
        main: {
            width: '200px',
        },
    };

    return (
        <div>
            <Panel
                className={'sidebar-content'}
                styles={panelStyles}
                isLightDismiss={true}
                isOpen={props.visible}
                onDismiss={props.closed}
                hasCloseButton={false}
                type={PanelType.smallFixedNear}
            >
                <Sidemenu
                    currentView={props.currentView}
                    clicked={props.closed}
                    menu={props.sidebarContents}
                    sidebar={true}
                    expanded={true}
                    iconOnly={false}
                    clickLink={props.clickLink}
                ></Sidemenu>
            </Panel>
        </div>
    );
};

Sidebar.propTypes = {
    visible: propTypes.bool.isRequired,
    closed: propTypes.func.isRequired,
};

export default Sidebar;
