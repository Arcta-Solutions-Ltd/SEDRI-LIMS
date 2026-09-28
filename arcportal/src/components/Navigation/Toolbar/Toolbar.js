import { ContextualMenu, Dropdown, IconButton, TooltipHost } from '@fluentui/react';
import React, { useEffect, useRef, useState } from 'react';
import { connect } from 'react-redux';
import TokenInfo from '../../../Classes/Security/TokenInfo';
import Post from '../../../Data/Post';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import FormHandler from '../../Containers/FormHandler/FormHandler';
import Logo from '../../Layout/Logo/Logo';
import NavIcon from '../NavIcon/NavIcon';
import UserFace from '../UserFace/Userface';
import './Toolbar.css';

const Toolbar = (props) => {
    const [showContextualMenu, setShowContextualMenu] = useState(false);
    const [formStartConfig, setFormStartConfig] = useState({});
    const [LaboratoryId, setLaboratoryId] = useState(1);
    const [visibility, setVisibility] = useState(true);
    const [LabOptions, setLabOptions] = useState([]);

    const linkRef = useRef(null);
    const dropdownRef = React.useRef < Dropdown > (null);

    const dropdownStyles = { dropdown: { width: 200 } };

    let navVisible = props.navVisible;
    let navIcon = null;
    const labListConfig = { Id: 'lablist', Type: 'dropdown', Label: 'Test', Options: [], Placeholder: '', Configurable: 'No', Options: LabOptions };

    useEffect(() => {
        const allOptions = props.lists.find((l) => l.Name === "completelaborglist")?.Options.map((option) => { return { key: option.Key, text: option.Text } });

        const token = new TokenInfo("arctoken");
        setLabOptions(OnlyIncludeAllowedLaboratoriesAndOrganisations(allOptions, token.AllowedLaboratories, token.AllowedOrganisations));
        setLaboratoryId(Number(token.LaboratoryId) > 0 ? "L" + token.LaboratoryId : "O" + token.OrganisationId);
    }, []);

    const onShowContextualMenu = (ev) => {
        ev.preventDefault();
        setShowContextualMenu(true);
    };

    const onHideContextualMenu = () => setShowContextualMenu(false);

    const onChangePassword = () => {
        setFormConfig('mypassworduievent');
    };

    const onChangePreferences = () => {
        setFormConfig('preferenceuievent');
    };

    const setFormConfig = (uievent) => {
        setFormStartConfig({
            button: { UIEvent: uievent },
            containerVisibility: setVisibility,
        });
    };

    const labChangeHandler = (event, value) => {
        const fieldValues = value.key.startsWith("L") ? { labid: value.key.slice(1) } : { orgid: value.key.slice(1) };
        Post('auth/switch', fieldValues, loggedInSuccessfully, errorWhenLoggingIn, value.key)
    }

    const loggedInSuccessfully = (token, extraInfo) => {
        localStorage.setItem("arctoken", token.token);
        setLaboratoryId(extraInfo);
        props.labSelectClicked(extraInfo);
    }

    const errorWhenLoggingIn = () => {
        const x = 1;
    }

    const OnlyIncludeAllowedLaboratoriesAndOrganisations = (options, laboratoryList, organisationList) => {
        const laboratories = new Set(laboratoryList.split(",").map(item => item.trim()));
        const organisations = new Set(organisationList.split(",").map(item => item.trim()));

        return options.filter(option => {
            const key = option.key.slice(1);
            return option.key.startsWith("L") ? laboratories.has(key) : organisations.has(key);
        });
    };

    const menuItems = [];
    if (props.uievents !== undefined) {
        if (
            props.uievents.find(
                (e) => e.Name.toLowerCase() === 'mypassworduievent'
            )
        ) {
            menuItems.push({
                key: 'changePassword',
                text: TranslateTag('@UseChaC@', props.language),
                onClick: onChangePassword,
            });
        }
        if (
            props.uievents.find(
                (e) => e.Name.toLowerCase() === 'preferenceuievent'
            )
        ) {
            menuItems.push({
                key: 'preferences',
                text: TranslateTag('@UsePre@', props.language),
                onClick: onChangePreferences,
            });
        }
    }

    let userIcon = (
        <div
            className="toolbar-cursor"
            onClick={onShowContextualMenu}
            ref={linkRef}
        >
            <UserFace username={props.username} givenName={props.givenName} surname={props.surname}></UserFace>
            <ContextualMenu
                items={menuItems}
                hidden={!showContextualMenu}
                target={linkRef}
                onItemClick={onHideContextualMenu}
                onDismiss={onHideContextualMenu}
            />
        </div>
    );

    let logOff = (
        <TooltipHost
            content={TranslateTag('@GenLog@', props.language)}
            id={999}
        >
            <IconButton
                iconProps={{ iconName: 'SignOut' }}
                onClick={props.logOut}
                styles={{
                    root: { height: '40px', verticalAlign: 'middle' },
                }}
            ></IconButton>
        </TooltipHost>
    );
    if (props.loginVisible) {
        navVisible = false;
        userIcon = null;
        logOff = null;
    }

    let labSelect = (null);
    if (LabOptions.length > 1) {
        labSelect = <Dropdown
            id="lablist"
            componentRef={dropdownRef}
            multiSelect={false}
            options={labListConfig?.Options ?? []}
            onChange={labChangeHandler}
            styles={dropdownStyles}
            selectedKey={LaboratoryId}
        />
    } else {
        labSelect = LabOptions.length > 0 ? LabOptions[0].text : "";
    }

    navIcon = <NavIcon clicked={props.clicked} visible={navVisible}></NavIcon>;

    return (
        <header className="toolbar-header">
            <div className="toolbar-content">
                <div className="toolbar-left">
                    {navIcon}
                    <Logo></Logo>
                    <div className="toolbar-name2">
                        {labSelect}
                        {process.env.REACT_APP_IS_EVALUATION_SYSTEM &&
                            process.env.REACT_APP_IS_EVALUATION_SYSTEM.toUpperCase() ==
                            'TRUE' && (
                                <>
                                    <div className="toolbar-disclaimer large-screen">
                                        {TranslateTag("@FrontendToolbarEvaluationDisclaimer@", props.language)}
                                    </div>
                                    <div className="toolbar-disclaimer small-screen">
                                        {TranslateTag("@FrontendToolbarEvaluationDisclaimerShort@", props.language)}
                                    </div>
                                </>
                            )}
                    </div>
                </div>
                <div className="toolbar-right">
                    {userIcon}
                    {logOff}
                </div>
            </div>
            <FormHandler startConfig={formStartConfig}></FormHandler>
        </header>
    );
};

const mapStateToProps = (state) => {
    return {
        language: state.config.language,
        uievents: state.config.uievents,
        laboratory: state.config.laboratory,
        lists: state.config.lists
    };
};

export default connect(mapStateToProps)(Toolbar);
