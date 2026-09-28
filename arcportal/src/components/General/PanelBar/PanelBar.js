import React from 'react';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { Icon } from '@fluentui/react/lib/Icon';
import './PanelBar.css';
import ArcBreadcrumb from '../../Forms/ArcBreadcrumb/ArcBreadcrumb';

const PanelBar = (props) => {

    const iconProps = props.fullScreenClick === undefined ? undefined : { iconName: (props.fullScreen) ? 'BackToWindow' : 'FullScreen'};

    const farItems = [
        {
            id: 'fullscreen',
            key: 'fullscreen',
            ariaLabel: TranslateTag("@GenFul@", props.language),
            iconOnly: true,
            iconProps: iconProps,
            onClick: () => props.fullScreenClick()
        }
    ];

    return (
        <div className="panelbar-content">
            <div className="panelbar-item panelbar-breadcrumb">
                <ArcBreadcrumb items={props.pages}></ArcBreadcrumb>
            </div>

            {/* <div className="panelbar-item">
                { iconProps !== undefined && <Icon iconName={'Help'} onClick={() => props.showHelp()} />  }      
            </div> */}
            <div className="panelbar-item">
                { iconProps !== undefined && <Icon iconName={iconProps.iconName} onClick={() => props.fullScreenClick()} />  }      
            </div>
        </div>

    );
};

export default PanelBar;