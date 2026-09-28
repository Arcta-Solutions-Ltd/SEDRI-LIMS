import React from 'react';
import { Panel, PanelType } from '@fluentui/react';

const RightHelp = (props) => {
    const closePanel = () => {};

    return (
        <div>
            <Panel
                isLightDismiss
                isOpen={props.visible}
                onDismiss={props.close}
                type={PanelType.smallFixedFar}
                headerText={props.title + ' Help'}
            >
                {props.helpText !== undefined &&
                props.helpText !== null &&
                props.helpText !== '' ? (
                    <div>
                        <br />
                        {props.helpText.map((string, index) => {
                            return (
                                <div>
                                    {string + '.'}
                                    <br />
                                    <br />
                                </div>
                            );
                        })}
                    </div>
                ) : null}
            </Panel>
        </div>
    );
};

export default RightHelp;
