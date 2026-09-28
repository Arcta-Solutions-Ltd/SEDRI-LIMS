import React, {useEffect, useState} from 'react';
import ArcPicker from '../ArcPicker/ArcPicker';
import ArcTextField from '../ArcTextField/ArcTextField';
import ArcCombo from '../ArcCombo/ArcCombo';

const ArcHierarchy = (props) => {

    const [optionArray, setOptionArray] = useState([]);
    const [displayedOptions, setDisplayedOptions] = useState([]);
    const [values, setValues] = useState({});

    let controlsToDisplay = null;
    let codeField = null;

    const displayLevels = props.config.Levels !== undefined && Array.isArray(props.config.Levels);

    const setupComboBoxOptions = () => {
        const options = props.config.Options;
        const newOptionArray = [];
        for (const level of props.config.Levels) {
            newOptionArray.push([]);
        }
        for (const option of options) {
            const optionSplit = option.text.split(":");
            const newEntry = {...option, text: optionSplit[optionSplit.length - 1], code: getCodeWithinBrackets(optionSplit[optionSplit.length - 1])}
            const level = optionSplit.length - 1;
            newOptionArray[level].push(newEntry);
            setOptionArray(newOptionArray);
            setDisplayedOptions([newOptionArray[0], [], []]);
        }        
    }

    useEffect(() => {
        if (displayLevels) {
            setupComboBoxOptions();
        }
    }, []);
    
    const codeChangeHandler = (id, value) => {
        let currentKey = value;
        let parentKey = 0;
        let found = false;
        const newDisplayedOptions = [...displayedOptions];
        if (optionArray.length > 0) {
            for (let i = props.config.Levels.length-1; i > -1; i--) {
                if (! found) {
                    let result = optionArray[i].filter(f => f.code === currentKey);
                    if (result.length > 0 && ! found) {
                        found = true;
                        currentKey = result[0].key;
                        updateValues("xx" + (i + 1), currentKey);
                        parentKey = result[0].ParentKey;
                    }                    
                } else {
                    let result = optionArray[i].filter(f => f.key === parentKey);
                    updateValues("xx" + (i + 1), parentKey);
                    parentKey = result[0].ParentKey;
                }
                if (parentKey !== undefined) {
                    newDisplayedOptions[i] = optionArray[i].filter(f => f.ParentKey === parentKey);
                }
            }
            setDisplayedOptions(newDisplayedOptions);
        }
    }

    if (props.config.Levels === undefined || props.config.Levels.length === 0) {
        controlsToDisplay = <ArcPicker config={props.config} valueChangeHandler={props.changeHandler}></ArcPicker>
    } 

    if (props.config.DisplayCode !== undefined && props.config.DisplayCode ) {
        const codeFieldConfig = {...props.config, Label: props.config.CodeLabel, PlaceHolder: props.config.CodePlaceholder }
        codeField = <ArcTextField key={props.key} config={codeFieldConfig} changeHandler={codeChangeHandler} ></ArcTextField>
    }
    let controlCount = 1;

    const calculateDisplayOptionsOnClick = (id,value) => {
        const numberFromId = parseInt(id.slice(-1));
        if (numberFromId < props.config.Levels.length) {
            const newDisplayedOptions = [...displayedOptions];
            newDisplayedOptions[numberFromId] = optionArray[numberFromId].filter(f => f.ParentKey === value);
            if (numberFromId+1 < props.config.Levels.length) {
                for (let i = numberFromId+1; i < props.config.Levels.length; i++) {
                    newDisplayedOptions[i] = [];
                }
            }
            setDisplayedOptions(newDisplayedOptions);
        }
    }

    const getCodeWithinBrackets = (text) => {
        let code = "";
        if (text.indexOf( '(' ) !== -1) {
            code =  text.substring( text.indexOf( '(' ) + 1, text.indexOf( ')' ) );
        }
        return code;
    }

    const updateValues = (id, value) => {
        const valuesToUpdate = {...values};
        valuesToUpdate[id] = value;
        setValues(valuesToUpdate);
    }

    const changeHandler = (id, value) => {
        updateValues(id, value);
        calculateDisplayOptionsOnClick(id, value);
        props.changeHandler(props.config.Id, value);
    }



    return (
        <React.Fragment>
            {controlsToDisplay}
            {displayLevels && props.config.Levels.map((level) => {
                    const levelOptions = displayedOptions.length >= controlCount ? displayedOptions[controlCount - 1] : undefined;
                    const newConfig = { Id: "xx" + controlCount, Label: level.Label, Placeholder: level.Placeholder, Options: levelOptions, value: values["xx" + controlCount]}
                    if (controlCount > 1) {
                        newConfig.ParentList = "xx" + (controlCount - 1);
                    }
                    controlCount++;
                    return (
                        <ArcCombo config={newConfig} changeHandler={changeHandler}></ArcCombo>
                    )
                }
            )}
            {codeField}
        </React.Fragment>
   )
}

export default ArcHierarchy;

