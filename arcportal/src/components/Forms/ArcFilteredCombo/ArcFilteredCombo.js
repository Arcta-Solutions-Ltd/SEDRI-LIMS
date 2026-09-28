import React, { useState, useEffect } from 'react';
import { ComboBox } from '@fluentui/react';

const ArcFilteredCombo = (props) => {
    const [options, setOptions] = useState();
    const [selectedKey, setSelectedKey] = useState();
    const [displayText, setDisplayText] = useState();

    useEffect(() => {
        if (
            props.config.value !== undefined &&
            props.config.Options !== undefined
        ) {
            const newOptions = props.config.Options.filter((f) => {
                return f.key === props.config.value.toString();
            });
            setOptions(newOptions);
        }
        if (props.MinFilterLength !== undefined) {
            setOptions(props.config.Options);
        }
        //setDisplayText("");
    }, [props.config.Options]);

    useEffect(() => {
        if (props.config.value !== undefined) {
            setSelectedKey(props.config.value.toString());
        } else {
            setSelectedKey(undefined);
        }
    }, [props.config.value]);

    //  let selectedValue = "";
    //  if (props.config.value !== undefined) {
    //     selectedValue = props.config.value.toString();
    //  }

    const onChange = React.useCallback((value) => {
        const newValue = value.toLowerCase();

        let newOptions = [];
        if (newValue.length > 0) {
            newOptions = props.config.Options.filter((f) => {
                return f.text.toLowerCase().lastIndexOf(newValue, 0) === 0;
            });
        }
        setOptions(newOptions);

        setDisplayText(value);
        setSelectedKey(undefined);
        props.changeHandler(undefined, { key: undefined });
        props.changeHandler(
            undefined,
            { key: value },
            props.config.Id + 'Text'
        );
    });

    const onItemClick = (event, option) => {
        setSelectedKey(option.key);
        setDisplayText(undefined);
    };

    const autofill = {
        onInputValueChange: onChange,
    };

    const localChangeHandler = (event, option, index, value) => {
        if (event.nativeEvent.key === 'Enter') {
            setSelectedKey(option.key);
            setDisplayText(undefined);
        }
        props.changeHandler(event, option, index, value);
    };

    const label = props.config.Label;

    return (
        <ComboBox
            id={props.config.Id}
            required={props.config.Required}
            placeholder={props.config.Placeholder}
            label={label}
            allowFreeform
            text={displayText}
            autoComplete="off"
            options={options}
            selectedKey={selectedKey}
            onChange={localChangeHandler}
            onItemClick={onItemClick}
            autofill={autofill}
        />
    );
};

export default ArcFilteredCombo;
