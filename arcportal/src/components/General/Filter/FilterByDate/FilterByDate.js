import React from 'react';
import FilterDate from './FilterDate';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import './FilterByDate.css'

const FilterByDate = (props) => {

    const config1 = { Id: "StartDate", 
                      value: props.startDate,
                      Placeholder: TranslateTag("@GenFro@", props.language), 
                      styles: {
                               root: {
                                      selectors: {
                                                    '& .ms-TextField-fieldGroup': {
                                                        borderColor: 'lightgray',
                                                        borderWidth: '1px',
                                                        backgroundColor:'whitesmoke',                 
                                                    }
                                                }
                                    }
                                }
                     };

    const config2 = { Id: "EndDate", 
                      value: props.endDate,
                      Placeholder: TranslateTag("@GenTo@", props.language), 
                      styles: {
                                root: {
                                    selectors: {
                                                    '& .ms-TextField-fieldGroup': {
                                                        borderColor: 'lightgray',
                                                        borderWidth: '1px',
                                                        backgroundColor:'whitesmoke'                 
                                                    }
                                                }
                                    }
                                }
                    };
   
    return (
        <div>
            <div className="filterbydate-container">
            <div className="filterbydate-child">
            <div className="filterbydate-text">
                {(props.dateSearchLabel || TranslateTag("@RepPreA@", props.language)) + ":"}
                </div>
            </div>
            <div className="filterbydate-child">
                <FilterDate config={config1} changeHandler={props.dateChangeHandler}></FilterDate>
            </div>
            <div className="filterbydate-child">
                <FilterDate config={config2} changeHandler={props.dateChangeHandler}></FilterDate>
            </div>
        </div>
        </div>
    );
};
  
export default FilterByDate;