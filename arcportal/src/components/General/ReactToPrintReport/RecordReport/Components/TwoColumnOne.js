import React from 'react';
import TwoColumnRow from './TwoColumnRow';
import TwoColumnTable from './TwoColumnTable';

const TwoColumnOne = (props) => {

    let rowsToDisplay = [];
    const lastRow = props.config.Config[props.config.Config.length-1];

    for (let i = 0; i < lastRow.Key; i=i+2) {
        rowsToDisplay.push({col1: i+1, col2: i+2, list: "No"});
    }

    // Add the type to rowsToDisplay

    for (const row of rowsToDisplay) {
        const foundRow = props.config.Config.filter((f) => f.Key === row.col1);
        if (foundRow.length > 0) {
            row.list = foundRow[0].List === undefined ? "No" : foundRow[0].List === "Yes" ? foundRow[0].Value : "No";
        }
    }

    const showHeading = props.showHeading === undefined ? true : props.showHeading;
    const heading = showHeading ? props.config.Heading : null;

    return (
        <div className="com-leftmargin">
            <div className="com-section-heading">
                {heading}
            </div>
            <table>
                <tbody>
                    {rowsToDisplay.map((row) => {
                        if (row.list === "No") {
                            return <TwoColumnRow colOne={row.col1} colTwo={row.col2} config={props.config.Config} data={props.data}></TwoColumnRow>
                        } else {
                            const listData = props.data.Lists.filter((f) => f.Key.toLowerCase() === row.list.toLowerCase())[0];
                            return <TwoColumnTable data={listData} ></TwoColumnTable>
                        }
                    })}
                </tbody>
            </table>
        </div>
    )
}

export default TwoColumnOne