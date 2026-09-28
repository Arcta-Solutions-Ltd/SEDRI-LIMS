import React from 'react';
import './Component.css';
import SingleDataValue from '../../../../Reports/Functions/SingleDataValue';

const FooterOne = (props) => {

    const lines = props.blanklines === undefined ? 0 : props.blanklines;
    const lineArray = new Array();
    for (let i = 0; i < lines; i++) {
        lineArray.push("");
    }

    return (
        <footer className="com-fullwidth">
            {lineArray.map((row) => {
                return <br></br>
            })}
            <hr></hr>
            <table>
                <tbody>
                    <tr>
                        <td>{SingleDataValue(1,props.data, props.config)}</td>
                    </tr>
                    <tr>
                        <td>{SingleDataValue(2,props.data, props.config)}</td>
                        <td>{SingleDataValue(3,props.data, props.config)}</td>
                    </tr>
                </tbody>
            </table>
        </footer>
    )
}

export default FooterOne