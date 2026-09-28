import React from 'react';

const TableWithHeadings = (props) => {

    let headingComponent = (null);
    
    if (props.config.Heading !== undefined) {
        const headings = props.config.Heading.split("|");

        headingComponent = <tr><th className="com-tr">{headings[0]}</th></tr>
        if (headings.length === 2) {
            headingComponent = <tr><th className="com-tr">{headings[0]}</th><th className="com-tr">{headings[1]}</th></tr>
        }
        if (headings.length === 3) {
            headingComponent = <tr><th className="com-tr">{headings[0]}</th><th className="com-tr">{headings[1]}</th><th className="com-tr">{headings[2]}</th></tr>
        }
        if (headings.length === 4) {
            headingComponent = <tr><th className="com-tr">{headings[0]}</th><th className="com-tr">{headings[1]}</th><th className="com-tr">{headings[2]}</th><th className="com-tr">{headings[3]}</th></tr>
        }
    }

    const listData = props.data.Lists.filter((f) => f.Key.toLowerCase() === props.config.List.toLowerCase())[0];

    return (
        <div className="com-mediumindent">
            <table className="printtable">
                <thead>
                    {headingComponent}
                </thead>
                <tbody>
                    {listData.Rows.map((row) => {
                        const cells = row.split("|");
                        if (cells.length === 1) {
                            return <tr><td className="com-extralargecol">{cells[0]}</td></tr>
                        }
                        if (cells.length === 2) {
                            return <tr><td className="com-largecol">{cells[0]}</td><td className="com-largecol">{cells[1]}</td></tr>
                        }
                        if (cells.length === 3) {
                            //return <tr><td className="com-field com-mediumcol">{cells[0]}</td><td className="com-field com-mediumcol">{cells[1]}</td><td className="com-field com-mediumcol">{cells[2]}</td></tr>
                            return <tr><td className="com-mediumcol">{cells[0]}</td><td className="com-mediumcol">{cells[1]}</td><td className="com-mediumcol">{cells[2]}</td></tr>
                        }
                        if (cells.length === 4) {
                            //return <tr><td className="com-field com-smallcol">{cells[0]}</td><td className="com-field com-smallcol">{cells[1]}</td><td className="com-field com-smallcol">{cells[2]}</td><td className="com-field com-smallcol">{cells[3]}</td></tr>
                            return <tr><td className="com-smallcol">{cells[0]}</td><td className="com-smallcol">{cells[1]}</td><td className="com-smallcol">{cells[2]}</td><td className="com-smallcol">{cells[3]}</td></tr>
                        }
                    })}
                </tbody>
            </table>
            <br></br>
        </div>
    )
}

export default TableWithHeadings