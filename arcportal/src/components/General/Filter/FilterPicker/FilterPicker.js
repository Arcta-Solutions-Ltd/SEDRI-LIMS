import React from 'react';
import { TagPicker } from '@fluentui/react/lib/Pickers';
import TranslateTag from '../../../../Utils/Local/TranslateTag';

const FilterPicker = (props) => {

    let itemsToDisplay = [];

    const pickerSuggestionsProps = {
        noResultsFoundText: TranslateTag('@GenNoe@', props.language)
    };

    const items = props.filter.Options === undefined ? [] : props.filter.Options.map(item => ({ key: item.key, name: item.text }));

    if (props.filter !== undefined && props.filter.values !== undefined && Array.isArray(props.filter.values)) {
        for (const item of props.filter.values) {
            const name = props.filter.Options.filter(f => f.key === item);
            if (name.length > 0) {
                itemsToDisplay.push({key: item, name: name[0].text});
            }
        }
    }

    const getTextFromItem = (item) => item.name;

    const filterSuggestedTags = (filterText) => {
        return filterText
          ? items.filter(
              tag => tag.name.toLowerCase().indexOf(filterText.toLowerCase()) > -1
            )
          : [];
    };    

    const onChange = (items) => {

        //Check if item does not exist in list.

        let affectedItem = []
        let selected = true;

        if (props.filter.values !== undefined && Array.isArray(props.filter.values)) {
            affectedItem = items.filter((f) => ! props.filter.values.includes(f.key));

            if (affectedItem.length === 0) {
                affectedItem = props.filter.values.filter((f) => ! items.map(i => i.key).includes(f));
                selected = false;
            }
        } else {
            affectedItem = items;
        }

        if (affectedItem.length > 0) {
            const key = affectedItem[0].key === undefined ? affectedItem[0] : affectedItem[0].key;
            props.click(null, {key: key, selected: selected}, props.filter.Key);
        }
    }

    const itemLimit = props.filter.MultiSelect ? 10 : 1;

    return (
        <div id={props.filter.Id}>
            <TagPicker
                onResolveSuggestions={filterSuggestedTags}
                getTextFromItem={getTextFromItem}
                pickerSuggestionsProps={pickerSuggestionsProps}
                itemLimit={itemLimit}
                onChange={onChange}
                selectedItems={itemsToDisplay}
                // inputProps={{placeholder: props.filter.PlaceHolder}}
                inputProps={{placeholder: itemsToDisplay.length === 0 ? props.filter.PlaceHolder : "", id: props.filter.FieldName + '-Input'}}
            />
        </div>
    )
}

export default FilterPicker;