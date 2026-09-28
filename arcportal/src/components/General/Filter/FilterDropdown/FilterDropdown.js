import React from 'react';
import { Dropdown } from '@fluentui/react';

const FilterDropdown = (props) => {

    let content = "";

    const dropDownWidth = props.filter.DropDownWidth === undefined || props.filter.DropDownWidth === 0 ? "auto" : props.filter.DropDownWidth;
    const displayValues = props.filter.values === undefined ? [] : props.filter.values;

    if (props.filter.MultiSelect) {
        content = <Dropdown
            id={props.filter.FieldName}
            selectedKeys={displayValues}
            onChange={(event, option) => {props.click(event, option, props.filter.Key)}}
            placeholder={props.filter.PlaceHolder}
            multiSelect={props.filter.MultiSelect}
            options={props.filter.Options}
            dropdownWidth={dropDownWidth}
            styles={{
                title: { backgroundColor: 'whitesmoke', border: '1px lightgray solid' },
                dropdown: { minWidth: props.filter.Width }
            }}
        />
    } else {
        content = <Dropdown
            id={props.filter.FieldName}
            selectedKey={displayValues}
            onChange={(event, option) => {props.click(event, option, props.filter.Key)}}
            placeholder={props.filter.PlaceHolder}
            multiSelect={props.filter.MultiSelect}
            options={props.filter.Options}
            dropdownWidth={dropDownWidth}
            styles={{
                title: { backgroundColor: 'whitesmoke', border: '1px lightgray solid' },
                dropdown: { minWidth: props.filter.Width }
            }}
        />
    }

    return (
        <React.Fragment>
            {content}
        </React.Fragment>
    );
};
  
export default FilterDropdown;