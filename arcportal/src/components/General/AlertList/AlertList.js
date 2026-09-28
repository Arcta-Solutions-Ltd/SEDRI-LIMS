import React from 'react';
import { MessageBar, MessageBarType } from '@fluentui/react';

const AlertList = (props) => {
    const positionId = props.position === 'top' ? 998 : 997;
    const messages = props.messages.filter((m) => m.positionId === positionId);

    return (
        <div>
            {messages.map((message) => {
                // If (red*0.299 + green*0.587 + blue*0.114) > 186 use #000000 else use #ffffff.
                let red = parseInt(message.colour.substring(1, 3), 16);
                let green = parseInt(message.colour.substring(3, 5), 16);
                let blue = parseInt(message.colour.substring(5, 7), 16);
                let combined = red * 0.299 + green * 0.587 + blue * 0.114;
                const styles = {
                    root: {
                        color: combined > 186 ? '#000000' : '#ffffff',
                        backgroundColor: message.colour,
                        marginBottom: '5px',
                    },
                };
                const type =
                    message.alertTypeId === 989
                        ? MessageBarType.error
                        : message.alertTypeId === 990
                        ? MessageBarType.warning
                        : MessageBarType.info;
                return (
                    <MessageBar
                        messageBarType={type}
                        isMultiline={true}
                        styles={styles}
                    >
                        {message.message}
                    </MessageBar>
                );
            })}
        </div>
    );
};

export default AlertList;
