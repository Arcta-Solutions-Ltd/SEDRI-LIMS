import React from 'react';
import './Component.css';
import SingleDataValue, {SingleLabelText} from '../../../../Reports/Functions/SingleDataValue';

const HeaderOne = (props) => {

        return (
            <h1 className="com-leftmargin">
                <table>
                    <tbody>
                        <tr>
                            <td className="com-large">{SingleDataValue(1,props.data, props.config)}</td>
                        </tr>
                    </tbody>
                </table>
                <table>
                    <tbody>
                        <tr>
                            <td className="com-title">{SingleDataValue(2,props.data, props.config)}</td>
                            <td>{SingleDataValue(3,props.data, props.config)}</td>
                        </tr>
                    </tbody>
                </table>
                <table>
                    <tbody>
                        <tr>
                            <td className="com-large com-bold">{SingleDataValue(4,props.data, props.config)}</td>
                        </tr>
                    </tbody>
                </table>
                <table>
                    <tbody>
                        <tr>
                            <td>{SingleLabelText(5,props.config)}</td>
                            <td className="com-field">{SingleDataValue(5, props.data, props.config)}</td>
                            <td>{SingleLabelText(6,props.config)}</td>
                            <td className="com-field">{SingleDataValue(6, props.data, props.config)}</td>
                        </tr>
                    </tbody>
                </table>
                <hr></hr>
            </h1>
        )
}

export default HeaderOne