import React from 'react';
import TwoColumnOne from './TwoColumnOne';

const TwoColumnWithName = (props) => {

    const fieldList = props.config.map
    return (
        <div className="com-leftmargin">
            <table>
                <tbody>
                    <tr>
                        <td className="com-leftname">{props.config.Heading}</td>
                        <td><TwoColumnOne data={props.data} config={props.config} showHeading={false}></TwoColumnOne></td>
                    </tr>
                </tbody>
            </table>
        </div>
    )
}

export default TwoColumnWithName