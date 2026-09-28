import React from 'react';
import FooterOne from './Components/FooterOne';
import HeaderOne from './Components/HeaderOne';
import HeadingOne from './Components/HeadingOne';
import TwoColumnOne from './Components/TwoColumnOne';
import TwoColumnTable from './Components/TwoColumnTable';
import TwoColumnWithName from './Components/TwoColumnWithName';
import TableWithHeadings from './Components/TableWithHeadings';

const RecordReportComponentFactory = (props) => {

    let componentToDisplay = (null);
    switch(props.config.Component.toLowerCase()) {
        case "footerone":
            componentToDisplay = <FooterOne data={props.data} config={props.config.Config} blanklines={props.config.BlankLines}></FooterOne>
            break;
        case "headerone":
            componentToDisplay = <HeaderOne data={props.data} config={props.config.Config}></HeaderOne>
            break;
        case "headingone":
            componentToDisplay = <HeadingOne data={props.data} config={props.config.Config}></HeadingOne>
            break;
        case "tablewithheadings":
            componentToDisplay = <TableWithHeadings data={props.data} config={props.config}></TableWithHeadings>
            break;  
        case "twocolumnone":
            componentToDisplay = <TwoColumnOne data={props.data} config={props.config}></TwoColumnOne>
            break;  
        case "twocolumntable":
            componentToDisplay = <TwoColumnTable data={props.data} config={props.config.Config}></TwoColumnTable>
            break;  
        case "twocolumnwithname":
            componentToDisplay = <TwoColumnWithName data={props.data} config={props.config}></TwoColumnWithName>
            break;             
        default:
            componentToDisplay = <div>No component found</div>
            break;
    }

    return (
        <React.Fragment>
            {componentToDisplay}
        </React.Fragment>
    );
}

export default RecordReportComponentFactory