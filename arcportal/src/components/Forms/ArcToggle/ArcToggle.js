import React, { useState, useEffect } from 'react';
import { Toggle } from '@fluentui/react';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { connect } from 'react-redux';

const ArcToggle = (props) => {
    const checkValue = props.config.value === 'Yes';

    const valueChangeHandler = (event, value) => {
        const returnValue = value === true ? 'Yes' : 'No';
        props.valueChangeHandler(
            props.config.Id ?? props.config.id,
            returnValue,
            props.config.extraInfo
        );
    };

    const toggleId = props.config.Id ?? props.config.id;
    const toggleDisabled = props.config.Disabled === true || props.config.ReadOnly === true;
    let toggleDisplay = (
        <Toggle
            id={toggleId}
            onKeyDown={props.onKeyDown}
            label={props.config.Label ?? ''}
            inlineLabel
            checked={checkValue}
            onChange={valueChangeHandler}
            value={props.config.value}
            disabled={toggleDisabled}
            tabIndex={
                props.config.TabIndex != undefined ? props.config.TabIndex : 0
            }
        ></Toggle>
    );
    if (props.showText) {
        const onText = props.config.OnText
            ? props.config.OnText
            : TranslateTag('@GenYesA@', props.language);
        const offText = props.config.OffText
            ? props.config.OffText
            : TranslateTag('@GenNo@', props.language);
        toggleDisplay = (
            <Toggle
                id={toggleId}
                onKeyDown={props.onKeyDown}
                label={props.config.Label ?? ''}
                checked={checkValue}
                onText={onText}
                offText={offText}
                onChange={valueChangeHandler}
                value={props.config.value}
                disabled={toggleDisabled}
                tabIndex={
                    props.config.TabIndex != undefined
                        ? props.config.TabIndex
                        : 0
                }
            ></Toggle>
        );
    }

    return <div onKeyDown={props.onKeyDown}>{toggleDisplay}</div>;
};

const mapStateToProps = (state) => {
    return {
        language: state.config.language,
    };
};
export default connect(mapStateToProps)(ArcToggle);
