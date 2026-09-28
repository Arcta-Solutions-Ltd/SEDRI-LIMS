import React, { useState, useEffect, useRef } from 'react';
import {
    Dialog,
    DialogType,
    DialogFooter,
    PrimaryButton,
    DefaultButton,
} from '@fluentui/react';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { connect } from 'react-redux';

const Inactivity = (props) => {
    const modalPropsStyles = { main: { maxWidth: 450 } };
    const dialogContentProps = {
        type: DialogType.normal,
        title: TranslateTag('@GenLogA@', props.language),
        subText: TranslateTag('@GenLogB@', props.language),
    };

    const inactivityTimeoutMinutes = Number(
        props?.settings?.find((setting) => setting.name === 'timeout').value
    );

    const [inactivity, _setInactivity] = useState(true);
    const [inactivityState, _setInactivityState] = useState('tracking');
    const [inactiveMinutes, _setInactiveMinutes] = useState(0);
    const [showDialog, _setShowDialog] = useState(false);
    const [timerHandle, _setTimerHandle] = useState(null);

    const inactivityRef = useRef(inactivity);
    const inactivityStateRef = useRef(inactivityState);
    const inactiveMinutesRef = useRef(inactiveMinutes);
    const showDialogRef = useRef(showDialog);
    const timerHandleRef = useRef(timerHandle);

    const setInactivity = (data) => {
        inactivityRef.current = data;
        _setInactivity(data);
    };

    const setInactivityState = (data) => {
        inactivityStateRef.current = data;
        _setInactivityState(data);
    };

    const setInactiveMinutes = (data) => {
        inactiveMinutesRef.current = data;
        _setInactiveMinutes(data);
    };

    const setShowDialog = (data) => {
        showDialogRef.current = data;
        _setShowDialog(data);
    };

    const setTimerHandle = (data) => {
        timerHandleRef.current = data;
        _setTimerHandle(data);
    };

    const active = () => {
        setInactivity(false);
    };

    const kickOffTimer = () => {
        if (props.enabled) {
            var timer = setTimeout(() => {
                timerTick();
            }, 60000);
            setTimerHandle(timer);
        }
    };

    const timerTick = () => {
        if (inactivityStateRef.current === 'warning') {
            props.logOut();
        } else {
            let tempInactiveMinutes = inactiveMinutesRef.current;
            if (inactivityRef.current === true) {
                tempInactiveMinutes = inactiveMinutesRef.current + 1;
            } else {
                tempInactiveMinutes = 0;
            }
            setInactivity(true);
            setInactiveMinutes(tempInactiveMinutes);
            if (tempInactiveMinutes >= inactivityTimeoutMinutes) {
                setInactivityState('warning');
                setShowDialog(true);
            }
            kickOffTimer();
        }
    };

    const resetInactivityTracking = () => {
        setShowDialog(false);
        setInactivity(true);
        setInactiveMinutes(0);
        setInactivityState('tracking');
        if (timerHandleRef.current !== null) {
            clearTimeout(timerHandleRef.current);
        }
    };

    const continueSession = () => {
        resetInactivityTracking();
        kickOffTimer();
    };

    const exitSession = () => {
        resetInactivityTracking();
        props.logOut();
    };

    useEffect(() => {
        document.onmousemove = active;
        document.addEventListener('keydown', active);
        document.addEventListener('activity', active);
        resetInactivityTracking();
        kickOffTimer();
    }, [props]);

    return (
        <div>
            <Dialog
                hidden={!showDialog}
                dialogContentProps={dialogContentProps}
                modalProps={modalPropsStyles}
            >
                <DialogFooter>
                    <PrimaryButton
                        onClick={continueSession}
                        text={TranslateTag('@GenYesA@', props.language)}
                    />
                    <DefaultButton
                        onClick={exitSession}
                        text={TranslateTag('@GenNo@', props.language)}
                    />
                </DialogFooter>
            </Dialog>
        </div>
    );
};

const mapStateToProps = (state) => {
    return {
        settings: state.config.settings,
    };
};

export default connect(mapStateToProps)(Inactivity);
