import React from 'react';
import {connect} from 'react-redux';
import * as actionTypes from '../../../store/actions';
import './Nav2.css';
import { CommandBarButton } from '@fluentui/react';

const Nav2 = (props) => {

  const itemStyle = {
    root: { paddingLeft: '10px', backgroundColor: 'var(--navColour)', width: '100%', display: 'flex', justifyContent: 'flex-start' }, //textAlign: 'left' },
    icon: { fontSize: '22px' }
  };

  const selectedItemStyle = {
    root: { paddingLeft: '10px', backgroundColor: 'lightgray', width: '100%', display: 'flex', justifyContent: 'flex-start' }, //textAlign: 'left' },
    icon: { fontSize: '22px' }
  };

  const submenuItemStyle = {
    root: { paddingLeft: '30px', backgroundColor: 'var(--navColour)', width: '100%', display: 'flex', justifyContent: 'flex-start' }, //textAlign: 'left' },
    icon: { fontSize: '20px' }
  };

  const selectedSubmenuItemStyle = {
    root: { paddingLeft: '30px', backgroundColor: 'lightgray', width: '100%', display: 'flex', justifyContent: 'flex-start' }, //textAlign: 'left' },
    icon: { fontSize: '20px' }
  };

  const onClick = (key) => {
    props.onLinkClick(key);
  };

  const onParentItemClick = (key) => {
    (props.expandedMenuKey === key) ? props.setExpandedMenuKey('') : props.setExpandedMenuKey(key);
  };

  return (
    <div>
        {props.groups[0].Links.map((link) => {
            return (
              <div key={link.Key}>
                <div className='nav2-menuitem'>
                  <div className={(props.currentView === link.Key) ? 'nav2-selected' : 'nav2-unselected'}></div>
                  <CommandBarButton
                    data-testid={`sidemenu-link-${link.Key}`}
                    styles={(props.currentView === link.Key) ? selectedItemStyle : itemStyle}
                    iconProps={{iconName: link.Icon}}
                    text={(props.showNames) ? link.Name : ''}
                    disabled={false}
                    checked={false}
                    onClick={(link.Links === undefined || link.Links === "") ? () => {onClick(link.Key)} : () => {onParentItemClick(link.Key)}}
                  />
                </div>
                <div className={(props.expandedMenuKey !== link.Key) ?  'app-invisible' : ''}>
                  {(link.Links !== undefined && link.Links !== "") ? link.Links.map((link) => {
                      return (
                        <div className='nav2-menuitem' key={link.Key}>
                          <div className={(props.currentView === link.Key) ? 'nav2-selected' : 'nav2-unselected'}></div>
                          <CommandBarButton
                            data-testid={`sidemenu-link-${link.Key}`}
                            styles={(props.currentView === link.Key) ? selectedSubmenuItemStyle : submenuItemStyle}
                            iconProps={{iconName: link.Icon}}
                            text={(props.showNames) ? link.Name : ''}
                            disabled={false}
                            checked={false}
                            onClick={() => {onClick(link.Key)
                            }}
                          />
                        </div>      
                      )
                    }) : null}
                </div>
              </div>
            )
          })
        }
    </div>
  );
};

const mapStateToProps = state => {
  return {
      expandedMenuKey: state.display.expandedMenuItem
  };
}

const mapDispatchToProps = dispatch => {
  return {
      setExpandedMenuKey: (value) => dispatch({type: actionTypes.SETEXPANDEDMENUITEM, value: value})
  }
};

export default connect(mapStateToProps, mapDispatchToProps)(Nav2);
