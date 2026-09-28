import React from 'react';
import { MessageBar, MessageBarType } from '@fluentui/react';

/**
 * Converts various error formats (string, object, axios response) to a displayable string.
 * React cannot render plain objects as children; this ensures we always render a string.
 * @param {string|object} error - The error from an API callback or exception
 * @returns {string}
 */
const getDisplayableError = (error) => {
    if (error == null) return '';
    if (typeof error === 'string') return error;
    if (typeof error !== 'object') return String(error);
    const data = error.data !== undefined ? error.data : error;
    if (typeof data === 'string') return data;
    if (data && typeof data === 'object') {
        if (data.title) return data.title;
        if (data.Message) return data.Message;
        if (data.message) return data.message;
        if (data.detail) return data.detail;
    }
    return 'An error occurred';
};

const ErrorMessage = (props) => {
    let cssContent = '';
    if (!props.visible) {
        cssContent = 'app-invisible';
    }

    return (
        <div className={cssContent}>
            <br></br>
            <MessageBar
                messageBarType={MessageBarType.error}
                onDismiss={props.dismissHandler}
                isMultiline={true}
                dismissButtonAriaLabel="Close Error"
            >
                {getDisplayableError(props.error)}
            </MessageBar>
        </div>
    );
};

export default ErrorMessage;
