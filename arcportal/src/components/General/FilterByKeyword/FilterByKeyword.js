import React from 'react';
import { SearchBox } from '@fluentui/react';
import TranslateTag from '../../../Utils/Local/TranslateTag';

const FilterByKeyword = (props) => {
    return (
        <div>
            <SearchBox
                id="FilterByKeyword"
                placeholder={TranslateTag('@GenFilB@', props.language)}
                iconProps={{ iconName: 'Filter' }}
                onChange={props.searchChangeHandler}
                styles={{
                    root: {
                        backgroundColor: 'whitesmoke',
                        border: '1px lightgray solid',
                    },
                }}
                value={props.value}
            />
        </div>
    );
};

export default FilterByKeyword;
