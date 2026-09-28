import React from 'react';
import SingleDataValue, {SingleLabelText} from '../../../../Reports/Functions/SingleDataValue';

const TwoColumnRow = (props) => {

    const firstLabel = SingleLabelText(props.colOne,props.config);
    const secondLabel = SingleLabelText(props.colTwo,props.config);
    const firstValue = SingleDataValue(props.colOne,props.data, props.config);
    const secondValue = SingleDataValue(props.colTwo,props.data, props.config);

    return (
        <tr>
            <td className="com-small com-rightjustify">{firstLabel}</td>
            <td className="com-field com-medium">{firstValue}</td>
            {secondLabel !== "" && secondValue != "" && <td className="com-small com-rightjustify">{secondLabel}</td>}
            {secondValue !== "" && <td className="com-field com-medium">{secondValue}</td>}
        </tr>
    )
}

export default TwoColumnRow