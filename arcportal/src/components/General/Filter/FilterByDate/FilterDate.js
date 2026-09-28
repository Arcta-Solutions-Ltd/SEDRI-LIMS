import React from 'react';
import ParseDate from '../../../Forms/ArcDate/ParseDate';
import { DatePicker } from '@fluentui/react';

const FilterDate = (props) => {
    const valueChangeHandler = (value) => {
        props.changeHandler(props.config.Id, value);
    };

    const onParseDateFromString = React.useCallback((newValue) => {
        return ParseDate(newValue);
    });

    return (
        <DatePicker
            id={props.config.Id}
            isRequired={props.config.Required}
            placeholder={props.config.Placeholder}
            allowTextInput={true}
            onSelectDate={valueChangeHandler}
            value={props.config.value}
            parseDateFromString={onParseDateFromString}
            label={props.config.Label}
            styles={props.config.styles !== undefined && props.config.styles !== null ? props.config.styles : {}}
        />
    );
};

export default FilterDate;
