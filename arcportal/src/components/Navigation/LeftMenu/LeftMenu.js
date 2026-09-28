import React, {useState} from 'react';
import Sidemenu from '../SideMenu/Sidemenu';
// import SideIconMenu from '../SideMenu/SideIconMenu';
// import SelectableSideIconMenu from '../SideMenu/SelectableSideIconMenu';

const LeftMenu = (props) => {

    const [largeMenuClass, setLargeMenuClass] = useState("");
    const [iconMenuSelectableClass, setIconMenuSelectableClass] = useState("app-invisible");

    var classContents = "";
    if (! props.visible) {
        classContents = "app-invisible";
    }

    const collapseLargeMenu = () => {
        setLargeMenuClass('app-invisible');
        setIconMenuSelectableClass('');
    }

    const expandLargeMenu = () => {
        setLargeMenuClass('');
        setIconMenuSelectableClass('app-invisible');
    }

    return (
        <div className={classContents}>
            <div className={largeMenuClass}>
                <Sidemenu currentView={props.currentView} clicked={collapseLargeMenu} menu={props.sidebarContents} sidebar={false} expanded={true} iconOnly={false} clickLink={props.clickLink}></Sidemenu>
            </div>
            <div className={iconMenuSelectableClass}>
                <Sidemenu currentView={props.currentView} clicked={expandLargeMenu} menu={props.sidebarContents} sidebar={false} expanded={false} iconOnly={false} clickLink={props.clickLink}></Sidemenu>
            </div>
            <div>
                <Sidemenu currentView={props.currentView} clicked={props.clicked} menu={props.sidebarContents} sidebar={false} expanded={false} iconOnly={true} clickLink={props.clickLink}></Sidemenu>
            </div>
        </div>
    );
}

export default LeftMenu;
