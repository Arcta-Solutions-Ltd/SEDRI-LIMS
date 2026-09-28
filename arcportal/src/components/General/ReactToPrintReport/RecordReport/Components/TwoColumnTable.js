import React from 'react';

const TwoColumnTable = (props) => {

        return (
            <div>
            <table>
                <tbody>
                    {props.data.Rows.map((row) => {
                        const cells = row.split("|");
                        if (cells.length === 1) {
                            return <tr><td className="com-field">{cells[0]}</td></tr>
                        }
                        if (cells.length === 2) {
                            return <tr><td className="com-field">{cells[0]}</td><td className="com-field">{cells[1]}</td></tr>
                        }
                        if (cells.length === 3) {
                            return <tr><td className="com-field">{cells[0]}</td><td className="com-field">{cells[1]}</td><td className="com-field">{cells[2]}</td></tr>
                        }
                    })}
                </tbody>
            </table>
            </div>
        )
}

export default TwoColumnTable