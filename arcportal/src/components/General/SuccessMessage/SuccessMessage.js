import React from 'react';
import { MessageBar, MessageBarType } from '@fluentui/react';

const SuccessMessage = (props) => {
    let cssContent = '';
    if (!props.visible) {
        cssContent = 'app-invisible';
    }

    return (
        <div className={cssContent}>
            <br></br>
            <MessageBar
                messageBarType={MessageBarType.success}
                isMultiline={true}
                dismissButtonAriaLabel="Close"
            >
                {props.message}
            </MessageBar>
        </div>
    );
};

export default SuccessMessage;
