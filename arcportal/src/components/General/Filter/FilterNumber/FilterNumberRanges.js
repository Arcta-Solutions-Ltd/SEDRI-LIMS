import React from 'react';
import FilterNumber from './FilterNumber';

const FilterNumberRanges = (props) => {

    return (
        <React.Fragment>
            { props.numberRanges.map((config) => {
                const valueRecord = props.filters.filter((f) => f.Key === config.Key);
                let currentValue = ["", ""];
                if (valueRecord.length > 0) {
                    currentValue = valueRecord[0].values;
                }
                return (
                    <FilterNumber config={config} language={props.language} changeHandler={props.changeHandler} value={currentValue}></FilterNumber>
                )})
            }
        </React.Fragment>
    )
}

export default FilterNumberRanges;
