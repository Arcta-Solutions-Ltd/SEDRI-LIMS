import React from 'react';
import './FilterNumber.css'
import ArcNumber2 from '../../../Forms/ArcNumber/ArcNumber2';
import TranslateTag from '../../../../Utils/Local/TranslateTag';

const FilterNumber = (props) => {

    const fieldValue = props.value.length === 0 ? ["",""] : props.value;

    const firstChangeHandler = (key, value) => {

        fieldValue[0] = value.toString();
        props.changeHandler(props.config.Key, fieldValue);
    }

    const secondChangeHandler = (key, value) => {
        fieldValue[1] = value.toString();
        props.changeHandler(props.config.Key, fieldValue);
    }    

    const numberOneConfig = { 
        id: props.config.Key + "1", 
        type: 'number', 
        minWidth: 30, 
        maxWidth: 30, 
        value: fieldValue[0], 
        Placeholder: TranslateTag("@GenFro@", props.language), 
        styles: {field: {backgroundColor:'whitesmoke'},
                 root: {
                        selectors: {
                                    '& .ms-TextField-fieldGroup': {
                                        borderColor: 'lightgray',
                                        borderWidth: '1px'                 
                                    }
                                }
                        }
                }
        };

    const numberTwoConfig = { 
        id: props.config.Key + "2", 
        type: 'number', 
        minWidth: 30, 
        maxWidth: 30, 
        value: fieldValue[1], 
        Placeholder: TranslateTag("@GenTo@", props.language),
        styles: {field: {backgroundColor:'whitesmoke'},
                 root: {
                        selectors: {
                                    '& .ms-TextField-fieldGroup': {
                                        borderColor: 'lightgray',
                                        borderWidth: '1px'                 
                                    }
                                }
                        }
                }
    };

    return (
        <div className="filternumber-container">
            <div className="filterbydate-child">
                <div className="filterbydate-text">
                {props.config.Label}
                </div>
            </div>
            <div className="filterbydate-child">
                <ArcNumber2
                    key={props.config.Key + "1"} 
                    config={numberOneConfig} 
                    changeHandler={firstChangeHandler}                  
                />
            </div>
            <div className="filterbydate-child">
                <ArcNumber2
                    key={props.config.Key + "2"} 
                    config={numberTwoConfig} 
                    changeHandler={secondChangeHandler}                  
                />
            </div>
        </div>
    )
}

export default FilterNumber;