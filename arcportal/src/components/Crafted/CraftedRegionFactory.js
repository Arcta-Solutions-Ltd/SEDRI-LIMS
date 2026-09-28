import React from 'react';
import DirectTestsGrid from './Specimen/DirectTestsGrid/DirectTestsGrid';
import ReportHistoryGrid from './Reports/ReportHistoryGrid/ReportHistoryGrid';
import FormDefinition from './Config/FormDefinition/FormDefinition';
import ReportDefinition from './Config/ReportDefinition/ReportDefinition';
import IqcResultsGrid from './Quality/IqcResultsGrid';
import TestRecordViewSection from './Specimen/TestRecordViewSection/TestRecordViewSection';
// import CultureList from './Culture/CultureList/CultureList';

const CraftedRegionFactory = (props) => {

    let pageToDisplay = (null);
    switch(props.config.Name.toLowerCase()) {
        case "testrecordviewsection":
            pageToDisplay = <TestRecordViewSection
                                config={props.config}
                                region={props.region}
                                data={props.data}
                                id={props.id}
                                language={props.language}
                                lists={props.lists}
                                forms={props.forms}
                                pages={props.pages}
                                refresh={props.refresh}
                                onRefreshDone={props.onRefreshDone}
                            />;
            break;
        case "directtestsgrid":
            pageToDisplay = <DirectTestsGrid
                                config={props.config}
                                region={props.region}
                                containerVisibility={props.containerVisibility}
                                refresh={props.refresh}
                                passive={props.passive}
                                suppressRowMenus={props.suppressRowMenus}
                                language={props.language}
                                onRefresh={props.onRefresh}
                                id={props.id}
                                stateid={props.stateid}
                                parentSpecimenStateId={props.parentSpecimenStateId}
                                selectedRecord={props.selectedRecord}
                                specimenContextStateIdRef={props.specimenContextStateIdRef}
                                onNavigateToRecordView={props.onNavigateToRecordView}
                                testListSync={props.testListSync}
                            >
                            </DirectTestsGrid>
            break;
        case "reporthistorygrid":
            pageToDisplay = <ReportHistoryGrid
                                config={props.config}
                                id={props.id}
                                onRefresh={props.onRefresh}
                                refresh={props.refresh}
                                language={props.language}
                                passive={props.passive}>
                            </ReportHistoryGrid>
            break;
        case "iqcresultsgrid":
            pageToDisplay = <IqcResultsGrid
                                config={props.config} 
                                data={props.data}
                                id={props.id}
                                refresh={props.refresh}
                                onRefresh={props.onRefresh}
                                containerVisibility={props.containerVisibility}
                                language={props.language}
                                onButtonClick={props.onButtonClick}
                                stateid={props.stateid}>
                            </IqcResultsGrid>
            break;
        case "pagesgrid":
            pageToDisplay = <FormDefinition 
                                config={props.config} 
                                data={props.data}
                                refresh={props.refresh}
                                onRefresh={props.onRefresh}
                                language={props.language}>
                            </FormDefinition>
            break;
        case "reportdefinition":
            pageToDisplay = <ReportDefinition 
                                config={props.config} 
                                data={props.data}
                                refresh={props.refresh}
                                onRefresh={props.onRefresh}
                                language={props.language}>
                            </ReportDefinition>
            break;
        // case "isolatelist":
        //     pageToDisplay = <CultureList
        //                         config={props.config} 
        //                         region={props.region}
        //                         containerVisibility={props.containerVisibility}
        //                         data={props.data}
        //                         passive={props.passive}
        //                         refresh={props.refresh}
        //                         onRefresh={props.onRefresh}
        //                         onRefreshDone={props.onRefreshDone}
        //                         id={props.id}
        //                         stateid={props.stateid}
        //                         onButtonClick={props.onButtonClick}
        //                         language={props.language}
        //                         selectedRecord={props.selectedRecord}>
        //                     </CultureList>
        //     break;

        default:
            pageToDisplay = <div>No crafted region found</div>
            break;
    }

    return (
        <React.Fragment>
            {pageToDisplay}
        </React.Fragment>
    );
};
  
export default CraftedRegionFactory;


