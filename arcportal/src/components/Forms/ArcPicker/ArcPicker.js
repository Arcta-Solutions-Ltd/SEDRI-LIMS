import React from 'react';
import { TagPicker, Label } from '@fluentui/react';

const ArcPicker = (props) => {

    const pickerSuggestionsProps = {
        noResultsFoundText: 'No entries found'
    };

    const items = props.config.Options === undefined ? [] : props.config.Options.map(item => ({ key: item.key, name: item.text }));

    let selectedItem = [];
    if (props.config.value !== undefined) {
        const itemList = props.config.value.toString().split(",");
        for (const item of itemList) { 
            const itemToFind = item.trim().toString();
            const itemFound = items.filter(item => item.key === itemToFind);
            if (itemFound.length > 0) {
                selectedItem.push(itemFound[0]);
            }
        }
    }

    const getTextFromItem = (item) => item.name;

    const filterSuggestedTags = (filterText) => {
        if (filterText !== undefined && filterText !== null && filterText !== "") {
            let results = items.filter(tag => tag.name.toLowerCase().indexOf(filterText.toLowerCase()) > -1);
            const tags = results.slice(0, 300);
            return tags;
        } else {
            return [];
        }
    };    

    const onChange = (items) => {
        if (items.length === 0) {
            props.valueChangeHandler(props.config.Id, undefined);
        } else {
            let itemList = "";
            for (const item of items) {
                itemList += itemList === "" ? item.key : "," + item.key;
            }
            props.valueChangeHandler(props.config.Id, itemList);
        }
    }

    const itemLimit = props.config.MultiSelect === undefined || ! props.config.MultiSelect ? 1 : 10
    if (props.config.Options !== undefined && props.config.Options.length > 0) {
        return (
            <div id={props.config.Id} onKeyDown={props.onKeyDown}>
                <Label>{props.config.Label + (props.config.Required ? ' *' : '')}</Label>
                <TagPicker
                    required={props.config.Required}
                    onResolveSuggestions={filterSuggestedTags}
                    getTextFromItem={getTextFromItem}
                    pickerSuggestionsProps={pickerSuggestionsProps}
                    itemLimit={itemLimit}
                    onChange={onChange}
                    selectedItems={selectedItem}
                    inputProps={{placeholder: props.config.Placeholder, id: props.config.Id + '-Input'}}
                />
            </div>
        )
     } else {
        return (null);
    }
}

export default ArcPicker;