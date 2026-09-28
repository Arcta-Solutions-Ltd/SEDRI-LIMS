import React from 'react';
import './NavIcon.css';
import { Icon } from '@fluentui/react/lib/Icon';

const navIcon = (props) => {

    // 'navicon-content' changes with screen-size.
    let navClass = "navicon-content";

    // Forces screen-size independent visibility (e.g. for full-screen mode).
    if (props.visible) {
        navClass = "navicon-content-always-visible"
    }

    return (
        <div className={navClass} onClick={props.clicked} data-cy='navicon'>
            <Icon iconName='CollapseMenu' />
        </div>
    )
};

export default navIcon;
