import React, { useEffect, useState } from 'react';
import './Login.css';
import {
    PrimaryButton,
    Stack,
    TextField,
    Spinner,
    SpinnerSize,
} from '@fluentui/react';
import GetWithNoParams from '../../../Data/GetWithNoParams';
import InitialSetUp from './InitialSetUp/InitialSetUp';
import Post from '../../../Data/Post';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import { connect } from 'react-redux';
import * as actionTypes from '../../../store/actions';
import sedrilims_logo from '../../../assets/images/SEDRILIMS Logo.png';
import { Dropdown } from '@fluentui/react';
import { authService } from './authService';
import { useRunOnce }from '../../../Utils/General/UseRunOnce';
import languageService from '../../../services/LanguageService';

const userNameProp = { iconName: 'Contact' };
const passwordProp = { iconName: 'Lock' };
const columnProps = {
    tokens: { childrenGap: 15 },
    styles: { root: { minWidth: 300, horizontalAlign: 'center' } },
};

const spinnerProps = {
    circle: {
        width: '100px',
        height: '100px',
        borderWidth: '10px',
    },
};

const Login = (props) => {
    const loginStrings = languageService.getLoginStrings();
    const [displaySetUpWindowState, setDisplaySetUpWindowState] =
        useState('No');
    const [errorStatus, updateErrorStatus] = useState({
        visible: false,
        message: '',
    });
    const [fieldValues, updateFieldValues] = useState({
        username: '',
        password: '',
    });
    const [loginDisplay, setLoginDisplay] = useState('pending-submission');
    const [displayText, setDisplayText] = useState({
        usernameText: loginStrings.username,
        passwordText: loginStrings.password,
        establishingText: 'Establishing',
        authenticatingText: 'Authenticating',
        loginText: 'Login',
    });
    const [loginOptions, setLoginOptions] = useState({
        LocalLoginEnabled: process.env.REACT_APP_LOCAL_LOGIN === 'TRUE',
        AzureLoginEnabled: process.env.REACT_APP_AZURE_LOGIN === 'TRUE',
    });
    const [startupCheckComplete, setStartupCheckComplete] = useState(false);
    const [isLoggingIn, setIsLoggingIn] = useState(false);

    useRunOnce(() => {
        const startUpResponseReceived = (startUp) => {
            setDisplaySetUpWindowState(startUp.ConfigureSystem);
            setDisplayText((prevDisplayText) => {
                let newDisplayText = { ...prevDisplayText };
                newDisplayText.usernameText = startUp.UsernameText;
                newDisplayText.passwordText = startUp.PasswordText;
                newDisplayText.establishingText = startUp.EstablishingText;
                newDisplayText.authenticatingText = startUp.AuthenticatingText;
                newDisplayText.loginText = startUp.LoginText;
                newDisplayText.LabList = startUp.LabList;
                newDisplayText.labText = startUp.LabText;
                newDisplayText.warningTextLines = startUp.WarningTextLines;
                return newDisplayText;
            });
            props.setExpandedMenuKey('');

            if (Array.isArray(startUp.LabList)) {
                updateFieldValues((prevFieldValues) => ({
                    ...prevFieldValues,
                    labid: startUp.LabList[0].Options[0].Key,
                }));
                props.headingText(startUp.LabList[0].Options[0].Text);
            }

            setLoginOptions({
                LocalLoginEnabled: startUp.LocalLoginEnabled,
                AzureLoginEnabled: startUp.AzureAdLoginEnabled,
            });
            setStartupCheckComplete(true);
        };

        const startUpErrorHandler = (error) => {
            // Even if startup check fails, show the login form
            console.error('Startup check failed:', error);
            setStartupCheckComplete(true);
        };

        GetWithNoParams('/setup/startupcheck', startUpResponseReceived, startUpErrorHandler);
    }, []);

    const dropdownRef = React.useRef < Dropdown > null;

    const closeSetupWindowHandler = () => {
        setDisplaySetUpWindowState('No');
    };

    const loginButtonClickHandler = async () => {
        if (isLoggingIn) return;
        
        setIsLoggingIn(true);
        errorCloseHandler();

        try {
            const response = await authService.loginWithCredentials(
                fieldValues.username,
                fieldValues.password
            );
            setLoginDisplay('pending-authentication');
            loggedInSuccessfully(response.token);
        } catch (error) {
            setIsLoggingIn(false);
            if (error?.response?.data) {
                errorWhenLoggingIn(error.response.data);
            } else if (error?.code) {
                errorWhenLoggingIn(error.message);
            }
        }
    };

    // Experimental hot-keys.
    document.onkeydown = function(e) {
        switch (e.key) {
            case "Enter":
                loginButtonClickHandler();
                break;
            default:
                break;
        }
    };

    const azureLoginButtonClickHandler = async () => {
        if (isLoggingIn) return;
        
        setIsLoggingIn(true);
        errorCloseHandler();
        try {
            const azureResponse = await authService.loginToAzure();
            const token = await authService.loginWithAzureAccessToken(
                azureResponse.accessToken
            );
            setLoginDisplay('pending-authentication');
            loggedInSuccessfully(token);
        } catch (error) {
            setIsLoggingIn(false);
            if (error?.response?.data) {
                errorWhenLoggingIn(error.response.data);
            } else if (error?.errorCode) {
                errorWhenLoggingIn(error.errorMessage);
            } else if (error?.code) {
                errorWhenLoggingIn(error.message);
            }
        }
    };

    const loggedInSuccessfully = () => {
        const token = localStorage.getItem('arctoken');
        GetWithNoParams(
            '/config/get',
            configRetrievedSuccessfully,
            errorWhenLoggingIn,
            token.token
        );
        setLoginDisplay('pending-configuration');
    };

    const configRetrievedSuccessfully = (configData) => {
        props.successfulLogin(configData, fieldValues);
        setLoginDisplay('finished');
    };

    const errorWhenLoggingIn = (response) => {
        setIsLoggingIn(false);
        localStorage.setItem('arctoken', '');
        
        // Extract error message from various response formats
        let errorMessage = 'An error occurred';
        
        if (typeof response === 'string') {
            errorMessage = response;
        } else if (response?.Message) {
            // Backend ErrorDetails format: {StatusCode, Message}
            errorMessage = response.Message;
        } else if (response?.data) {
            if (typeof response.data === 'string') {
                errorMessage = response.data;
            } else if (response.data?.Message) {
                errorMessage = response.data.Message;
            }
        }
        
        updateErrorStatus({
            visible: true,
            message: errorMessage,
        });
        setLoginDisplay('pending-submission');
    };

    const errorCloseHandler = () => {
        updateErrorStatus({ visible: false, message: '' });
    };

    const handleChange = (field) => (event) =>
        updateFieldValues({ ...fieldValues, [field]: event.target.value });

    const storeUsername = handleChange('username');
    const storePassword = handleChange('password');

    let credentials = null;
    if (
        loginDisplay === 'pending-submission' ||
        loginDisplay === 'pending-authentication'
    ) {
        credentials = (
            <div>
                <TextField
                    id="txtUsername"
                    data-test="username"
                    label={displayText.usernameText}
                    required
                    iconProps={userNameProp}
                    onChange={storeUsername}
                    styles={{
                        subComponentStyles: {
                            label: { root: { color: 'purple' } },
                        },
                    }}
                />
                <TextField
                    id="txtPassword"
                    data-test="password"
                    label={displayText.passwordText}
                    type="password"
                    required
                    iconProps={passwordProp}
                    onChange={storePassword}
                    styles={{
                        subComponentStyles: {
                            label: { root: { color: 'purple' } },
                        },
                    }}
                />
            </div>
        );
    }

    let progressMessage = null;
    if (!errorStatus.visible) {
        if (loginDisplay === 'pending-configuration') {
            progressMessage = (
                <div className="login-spinner-text">
                    {displayText.establishingText}...
                </div>
            );
        } else if (loginDisplay === 'pending-authentication') {
            progressMessage = (
                <div className="login-spinner-text">
                    {displayText.authenticatingText}...
                </div>
            );
        }
    }

    let submitButton = null;
    if (loginDisplay === 'pending-submission') {
        submitButton = (
            <PrimaryButton
                data-test="login"
                text={displayText.loginText}
                onClick={loginButtonClickHandler}
                disabled={isLoggingIn}
            />
        );
    }

    let spinner = null;
    if (
        !errorStatus.visible &&
        (loginDisplay === 'pending-authentication' ||
            loginDisplay === 'pending-configuration')
    ) {
        spinner = <Spinner styles={spinnerProps} />;
    }

    const windowsIcon = { iconName: 'WindowsLogo' };

    const AzureLogin = () => {
        if (
            loginOptions.AzureLoginEnabled &&
            (loginDisplay === 'pending-submission' ||
                loginDisplay === 'pending-authentication')
        ) {
            return (
                <>
                    <PrimaryButton
                        data-test="microsoft-login"
                        text="Microsoft Login"
                        iconProps={windowsIcon}
                        onClick={azureLoginButtonClickHandler}
                        disabled={isLoggingIn}
                    />
                </>
            );
        }
    };

    const OrComponent = () => {
        if (
            loginOptions.LocalLoginEnabled &&
            loginOptions.AzureLoginEnabled &&
            (loginDisplay === 'pending-submission' ||
                loginDisplay === 'pending-authentication')
        ) {
            return (
                <div className="split">
                    <div></div>
                    <div>or</div>
                    <div></div>
                </div>
            );
        }
    };

    if (displaySetUpWindowState === 'Yes') {
        return (
            <InitialSetUp
                closeWindowHandler={closeSetupWindowHandler}
            ></InitialSetUp>
        );
    } else {
        if (!startupCheckComplete) {
            return (
                <div className="login-content">
                    <div className="login-logo">
                        <img src={sedrilims_logo} alt="Sedric Logo" height="100%" />
                    </div>
                    <div className="login-main">
                        <div className="login-form">
                            <Stack {...columnProps}>
                                <Spinner styles={spinnerProps} />
                            </Stack>
                        </div>
                    </div>
                </div>
            );
        }
        return (
            <div className="login-content">
                <div className="login-logo">
                    <img src={sedrilims_logo} alt="Sedric Logo" height="100%" />
                </div>
                <div className="login-main">
                    <div className="login-form">
                        <Stack {...columnProps}>
                            {credentials}
                            {progressMessage}
                            <ErrorMessage
                                visible={errorStatus.visible}
                                dismissHandler={errorCloseHandler}
                                error={errorStatus.message}
                            ></ErrorMessage>
                            {submitButton}
                            <OrComponent />
                            <AzureLogin />
                            {spinner}
                        </Stack>
                    </div>
                    {process.env.REACT_APP_IS_EVALUATION_SYSTEM &&
                        process.env.REACT_APP_IS_EVALUATION_SYSTEM.toUpperCase() ==
                            'TRUE' && (
                            <div className="login-disclaimer">
                                {Array.isArray(displayText.warningTextLines) &&
                                    displayText.warningTextLines.map(
                                        (line, idx) => <p key={idx}>{line}</p>
                                    )}
                            </div>
                        )}
                </div>
                <div className="login-footer">
                    {process.env.REACT_APP_CODE_VERSION && (
                        <div className="login-version">
                            {loginStrings.version}{' '}
                            {process.env.REACT_APP_CODE_VERSION}
                        </div>
                    )}
                </div>
            </div>
        );
    }
};

const mapStateToProps = (state) => {
    return {
        expandedMenuKey: state.display.expandedMenuItem,
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        setExpandedMenuKey: (value) =>
            dispatch({ type: actionTypes.SETEXPANDEDMENUITEM, value: value }),
        headingText: (value) =>
            dispatch({ type: actionTypes.HEADINGTEXT, value: value }),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Login);
