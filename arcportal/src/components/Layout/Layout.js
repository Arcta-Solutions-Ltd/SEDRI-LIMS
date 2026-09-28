import React, {useState, useEffect} from 'react';
import {connect} from 'react-redux';
import * as actionTypes from '../../store/actions';
import './Layout.css';
import Toolbar from '../Navigation/Toolbar/Toolbar';
import Sidebar from '../Navigation/Sidebar/Sidebar';
import RightSidebar from '../Navigation/RightSideBar/RightSideBar';
import LeftMenu from '../Navigation/LeftMenu/LeftMenu';
import Router from './Router/Router';
import Inactivity from '../Security/Inactivity/Inactivity';
import { authService } from '../Security/Login/authService'
import { jwtDecode } from 'jwt-decode';
import {
    resolvePatientRecordViewName,
    resolveSpecimenRecordViewName,
    resolveCultureRecordViewName,
    resolveTestRecordViewName,
} from './homeNavigationUtils';

const Layout = (props) => {

    const [layoutState, setLayoutState] = useState({viewSidebar: false, viewRightSidebar: false});
    const [currentView, setCurrentView] = useState("login");
    /** Deep-link context when opening a record/list from Home Recently Used (return navigates to Home). */
    const [homeNavigation, setHomeNavigation] = useState(null);
    // const [newClick, setNewClick] = useState(0);
    const [displayLogin, setDisplayLogin] = useState(true);
    const [username, setUsername] = useState('');
    const [givenName, setGivenName] = useState('');
    const [surname, setSurname] = useState('');
    const [laboratorySelected, setLaboratorySelected] = useState("");

    const logOut = async () => {
        setUsername("")
        await authService.logout();
        setCurrentView("login");
        setDisplayLogin(true);
    }

    const navIconClickedHandler = () => {
        if (currentView !== "login") {
            setLayoutState({viewSidebar: true, viewRightSidebar: false});
        }
    }

    const closeSidebar = () => {
        setLayoutState({viewSidebar: false, viewRightSidebar: false});
    }

    const rightMenuClickedHandler = () => {
        setLayoutState({viewSidebar: false, viewRightSidebar: true});
    }

    const closeRightSidebar = () => {
        setLayoutState({viewSidebar: false, viewRightSidebar: false});
    }

    const linkClickHandler = (name) => {
        setCurrentView(name);
        closeSidebar();
        setHomeNavigation(null);
    }

    const homeClickHandler = () => {
        setCurrentView("specimens");
    }

    const returnToHomeFromRecent = () => {
        setHomeNavigation(null);
        setCurrentView('home');
    };

    /**
     * @param {{ kind: string, patientId?: number|string, specimenId?: number|string, testId?: number|string, cultureId?: number|string }} payload
     */
    const onRecentlyUsedNavigate = (payload) => {
        const kind = (payload?.kind || '').toLowerCase();
        if (kind === 'culture') {
            const viewName = resolveCultureRecordViewName(props.recordviews);
            const cid = payload.cultureId ?? payload.CultureId;
            if (!viewName || cid == null || cid === '') {
                return;
            }
            setHomeNavigation({ mode: 'record', viewName, itemId: cid });
            setCurrentView(viewName);
            return;
        }
        if (kind === 'specimen') {
            const viewName = resolveSpecimenRecordViewName(props.recordviews);
            const sid = payload.specimenId ?? payload.SpecimenId;
            if (!viewName || sid == null || sid === '') {
                return;
            }
            setHomeNavigation({ mode: 'record', viewName, itemId: sid });
            setCurrentView(viewName);
            return;
        }
        if (kind === 'patient') {
            const viewName = resolvePatientRecordViewName(props.recordviews);
            const pid = payload.patientId ?? payload.PatientId;
            if (!viewName || pid == null || pid === '') {
                return;
            }
            setHomeNavigation({ mode: 'record', viewName, itemId: pid });
            setCurrentView(viewName);
            return;
        }
        if (kind === 'test') {
            const viewName = resolveTestRecordViewName(props.recordviews);
            const tid = payload.testId ?? payload.TestId;
            const testName = payload.testName ?? payload.TestName ?? '';
            if (!viewName || tid == null || tid === '') {
                return;
            }
            setHomeNavigation({
                mode: 'record',
                viewName,
                itemId: tid,
                recordInfo: { TestName: testName, Source: 'direct' },
            });
            setCurrentView(viewName);
        }
    };

    const loggedInSuccessfully = (configData, userData) => {
        props.updateConfig(configData);
        const token = localStorage.getItem('arctoken');
        const decodedToken = jwtDecode(token);
        setUsername(decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata']);
        setGivenName(decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname']);
        setSurname(decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname']);
        setCurrentView("home");
        setDisplayLogin(false);
    }

    const labClickHandler = (value) => {
        setCurrentView("home");
        setLaboratorySelected(value);
        setHomeNavigation(null);
    }

    // const savePreferences = (data) => {
    //     props.updatePreferences(data);
    // }

    let isLeftMenuVisible = true;
    if (props.showFullScreen !== undefined) {
        isLeftMenuVisible =  ! props.showFullScreen;
    }

    if (displayLogin) {
        isLeftMenuVisible =  false;
    }

    let menu = null;
    if (isLeftMenuVisible) {
        menu = (
            <div className="layout-menu">
                <LeftMenu currentView={currentView} clicked={navIconClickedHandler} sidebarContents = {props.sidebar} visible={true} clickLink={linkClickHandler}></LeftMenu>
            </div>
        );
    }

    let toolbar = null;
    if (!displayLogin) {
        toolbar = (
            <Toolbar clicked={navIconClickedHandler} rightclicked={rightMenuClickedHandler} labSelectClicked={labClickHandler} logOut={logOut} navVisible={! isLeftMenuVisible} loginVisible={displayLogin} username={username} givenName={givenName} surname={surname}/>
        )
    }

    return (
        <React.Fragment>
            {toolbar}
            <main className="layout-main">
                {menu}
                <div className="layout-content">
                    <Router
                        currentView={currentView}
                        successfulLogin={loggedInSuccessfully}
                        homeClick={homeClickHandler}
                        laboratorySelected={laboratorySelected}
                        homeNavigation={homeNavigation}
                        onRecentlyUsedNavigate={onRecentlyUsedNavigate}
                        onReturnToHome={returnToHomeFromRecent}
                    />
                </div>
                <Sidebar currentView={currentView} sidebarContents = {props.sidebar} visible={layoutState.viewSidebar} closed={closeSidebar} clickLink={linkClickHandler}/>
                <RightSidebar sidebarContents={props.rightSidebar} visible={layoutState.viewRightSidebar} closed={closeRightSidebar}/>
            </main>
            <Inactivity enabled={!displayLogin} logOut={logOut} language={props.language}></Inactivity>
        </React.Fragment>
    )
};

const mapStateToProps = state => {
    return {
        sidebar: state.config.sidebar,
        rightSidebar: state.config.rightSidebar,
        showFullScreen: state.display.showFullScreen,
        language: state.config.language,
        views: state.config.views,
        recordviews: state.config.recordviews,
    };
}

const mapDispatchToProps = dispatch => {
    return {
        updateConfig: (value) => dispatch({type: actionTypes.UPDATECONFIG, value: value})
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(Layout);
