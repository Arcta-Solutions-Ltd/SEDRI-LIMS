import React from 'react';
import { connect } from 'react-redux';
import './SideMenu.css';
import { IconButton, TooltipHost } from '@fluentui/react';
import Nav2 from '../../Navigation/Nav2/Nav2';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { useId } from 'react';

const Sidemenu = (props) => {

  const buttonId = useId();
  const calloutProps = { gapSpace: 0 };
  const hostStyles = { root: { display: 'inline-block' } };
  
  let contentClass = props.sidebar ? 'sidemenu-content-sidebar' : 'sidemenu-content';
  let content = TranslateTag("@GenCo1@", props.language);
  let iconClass = "collapse-menu";
  let icon = {iconName: 'DoubleChevronLeft'};

  if (! props.expanded) {
    contentClass = props.iconOnly ? 'sidemenu-content-icons_only' : 'sidemenu-icons-selectable';
    content = TranslateTag("@GenExpA@", props.language);
    iconClass = "expand-menu";
    icon = {iconName: 'DoubleChevronRight'};
  };

  return (
    <div className={contentClass}>
      <TooltipHost
        content={content}
        id={buttonId}
        calloutProps={calloutProps}
        styles={hostStyles}
        >
          <IconButton
            className={iconClass}
            iconProps={icon}
            disabled={false} checked={false}
            onClick={props.clicked}
          />
        </TooltipHost>
        <Nav2
          groups={props.menu}
          currentView={props.currentView} 
          onLinkClick={props.clickLink}
          showNames={props.expanded}
        />
      </div>
  );
};

const mapStateToProps = state => {
  return {
      language: state.config.language
  };
}

export default connect(mapStateToProps)(Sidemenu);
