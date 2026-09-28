// import React, { useState, useEffect, useCallback } from 'react';
// import { connect } from 'react-redux';
// import Post from '../../../Data/Post';
// import RecordReportSection from '../../Reports/RecordReportSection/RecordReportSection';
// import './ViewRecordReport.css';


// const ViewRecordReport = (props) => {

//     const [reportData, setReportData] = useState(null);


//     const errorWhenRetrievingData = (response) => {
//     }

//     const reportDataRetrievedSuccessfully = useCallback((data, index) => {
//         setReportData(data);
//     }, []);

//     useEffect(() => {

//         var currentReport = props.recordreports.filter((report) => {
//             return report.Name === props.type;
//         })[0];

//         if (currentReport !== null && currentReport.QueryName !== null && currentReport.QueryName !== '') {
//             const criteria = { Name: currentReport.QueryName, Parameters: [{ Key: 'id', Value: props.itemId }]};
//             Post('query/filteredget', criteria, reportDataRetrievedSuccessfully, errorWhenRetrievingData, 0);
//         }
//     }, [props.recordreports, props.type, reportDataRetrievedSuccessfully, props.itemId]);

//     let content = null;
//     if (reportData != null) {
//         content = (
//             <div className="viewrecordreport-content">
//                 {reportData.Sections.map((section) => {
//                     return <RecordReportSection key={section.Id} config={section}></RecordReportSection>
//                 })}
//             </div>
//         );
//     }

//     return (
//         <React.Fragment>
//             {content}
//         </React.Fragment>
//     );
// };

// const mapStateToProps = state => {
//     return {
//         recordreports: state.config.recordreports,
//     };
// }

// export default connect(mapStateToProps)(ViewRecordReport);
