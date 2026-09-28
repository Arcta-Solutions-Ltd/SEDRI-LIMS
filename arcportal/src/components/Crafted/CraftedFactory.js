import React from 'react';
import AST from './AST/AST';
import MenuPermissions from './Roles/MenuPermissions/MenuPermissions';
import EventPermissions from './Roles/EventPermissions/EventPermissions';
import JsonViewer from './Queue/JsonViewer/JsonViewer';
import FormattedJsonViewer from './Queue/FormattedJsonViewer/FormattedJsonViewer';
import PatientSearchResult from './Patient/PatientSearchResult/PatientSearchResult';
import DirectTests from './Specimen/DirectTests/DirectTests';
import TableEntry from './Tables/TableEntry/TableEntry';
import OrganismList from './Organism/OrganismList/OrganismList';
import OrganismSearch from './Organism/OrganismSearch/OrganismSearch';
import PrintPublish from './Reports/PrintPublish';
import OrganismScope from './Organism/OrganismScope/OrganismScope';
import CustomOrganism from './Organism/CustomOrganism/CustomOrganism';
import CultureOrganismSelection from './Organism/CultureOrganismSelection/CultureOrganismSelection';
import BatchPrint from './Reports/BatchPrint/BatchPrint';
import BatchPublish from './Reports/BatchPublish/BatchPublish';
import Exports from './Exports/Export';
import MappingEditor from './Exports/MappingEditor/MappingEditor';
import FieldSelector from './Reports/FieldSelector/FieldSelector';
import WorkflowRulePage from './Config/WorkflowRulePage/WorkflowRulePage';
import InputIqcResultsPage from './Quality/InputIqcResultsPage';
import SelectQcOrganismsPage from './Quality/SelectQcOrganismsPage';
import ChangeOrganismSelector from './Organism/ChangeOrganismSelector/ChangeOrganismSelector';
import ProcessImportFile from './Import/ProcessImportFile';
import ExportConfiguration from './Config/ExportConfiguration/ExportConfiguration';
import AdmissionSelection from './Admission/AdmissionSelection/AdmissionSelection';
import RequestSelection from './Admission/RequestSelection/RequestSelection';

const CraftedFactory = (props) => {


    let pageToDisplay = (null);
    switch(props.config.Name.toLowerCase()) {
        case "editiqctestqcorganismspage":        
            pageToDisplay = <MenuPermissions config={props.config} data={props.data} changeHandler={props.changeHandler} language={props.language}></MenuPermissions>
            break;
        case "selectqcorganismspage":
            pageToDisplay = <SelectQcOrganismsPage config={props.config} data={props.data} inputData={props.inputData} changeHandler={props.changeHandler} language={props.language}></SelectQcOrganismsPage>
            break;
        case "admissionselectionpage":
            pageToDisplay = <AdmissionSelection config={props.config} data={props.inputData} changeHandler={props.changeHandler} rightButtonClick={props.rightButtonClick} language={props.language}></AdmissionSelection>
            break;
        case "requestselectionpage":
        case "requestselectionforadmissionpage":
            pageToDisplay = <RequestSelection config={props.config} data={props.inputData} changeHandler={props.changeHandler} rightButtonClick={props.rightButtonClick} language={props.language}></RequestSelection>
            break;
        case "addworkflowentrypage":
            pageToDisplay = <WorkflowRulePage config={props.config} data={props.data} changeHandler={props.changeHandler} language={props.language}></WorkflowRulePage>
            break;
        case "ast":
            pageToDisplay = <AST config={props.config} data={props.data} changeHandler={props.changeHandler} language={props.language} startConfig={props.startConfig}></AST>
            break;
        case "batchprintpage":
            const viewHistoryReport = props.startConfig.view === "approvedreportview";
            pageToDisplay = <BatchPrint config={props.config} records={props.startConfig.allSelectedRecords} language={props.language} refresh={props.refresh} history={viewHistoryReport}></BatchPrint>
            break;
        case "batchpublishpage":
            pageToDisplay = <BatchPublish config={props.config} records={props.startConfig.allSelectedRecords} language={props.language} refresh={props.refresh}></BatchPublish>
            break;
        case "changeorganismselectorpage":
            pageToDisplay = <ChangeOrganismSelector config={props.config} data={props.data} changeHandler={props.changeHandler} fullScreen={props.fullScreen} language={props.language} rightButtonClick={props.rightButtonClick}></ChangeOrganismSelector>
            break;
        case "cultureorganismpage":
            pageToDisplay = <CultureOrganismSelection config={props.config} data={props.data} changeHandler={props.changeHandler} language={props.language} leftButtonClick={props.leftButtonClick} rightButtonClick={props.rightButtonClick} close={props.close} onKeyDown={props.onKeyDown}></CultureOrganismSelection>
            break;
        case "culturetypeselectionpage":
            const cultureTypeCategoryId = props?.inputData?.find((t) => t.key === "CultureTypeCategoryId")?.value;
            pageToDisplay = <MenuPermissions config={props.config} data={props.data} changeHandler={props.changeHandler} language={props.language} onKeyDown={props.onKeyDown} categorySelection={cultureTypeCategoryId} categoryName={"culturetypecategoryid"}></MenuPermissions>
            break;
        case "culturetestpage":
            pageToDisplay = <DirectTests type="culture" config={props.config} data={props.data} changeHandler={props.changeHandler} id={props.id} refresh={props.refresh} language={props.language} startConfig={props.startConfig} fullScreen={props.fullScreen}></DirectTests>
            break;
        case "addcustompage":
        case "editcustompage":
            pageToDisplay = <CustomOrganism config={props.config} data={props.data} changeHandler={props.changeHandler} fullScreen={props.fullScreen} language={props.language}></CustomOrganism>
            break;
        case "exportconfigurationpage":
            pageToDisplay = <ExportConfiguration config={props.config} language={props.language}></ExportConfiguration>
            break;
        case "directtestpage":
            pageToDisplay = <DirectTests type="specimen" config={props.config} data={props.data} changeHandler={props.changeHandler} id={props.id} refresh={props.refresh} language={props.language} startConfig={props.startConfig} fullScreen={props.fullScreen}></DirectTests>
            break;
        case "editiqctestprofileantibioticspage":
            pageToDisplay = <MenuPermissions config={props.config} data={props.data} changeHandler={props.changeHandler} language={props.language}></MenuPermissions>
            break;
        case "editworkflowentrypage":
            pageToDisplay = <WorkflowRulePage config={props.config} data={props.data} changeHandler={props.changeHandler} language={props.language}></WorkflowRulePage>
            break;
        case "eventpermission":
            pageToDisplay = <EventPermissions config={props.config} data={props.data} changeHandler={props.changeHandler} language={props.language}></EventPermissions>
            break;
        case "fieldselectorpage":
            pageToDisplay = <FieldSelector config={props.config} data={props.inputData} changeHandler={props.changeHandler} language={props.language}></FieldSelector>
            break;
        case "formattedjsonviewer":
            pageToDisplay = <FormattedJsonViewer config={props.config} data={props.data} language={props.language}></FormattedJsonViewer>
            break;
        case "jsonviewer":
            pageToDisplay = <JsonViewer config={props.config} data={props.data} language={props.language}></JsonViewer>
            break;
        case "loadimportfilepage":
            pageToDisplay = <ProcessImportFile config={props.config} language={props.language} changeHandler={props.changeHandler}></ProcessImportFile>
            break;
        case "menupermission":
            pageToDisplay = <MenuPermissions config={props.config} data={props.data} changeHandler={props.changeHandler} language={props.language}></MenuPermissions>
            break;  
        case "organismlistpage":
            pageToDisplay = <OrganismList config={props.config} data={props.inputData} changeHandler={props.changeHandler} language={props.language} rightButtonClick={props.rightButtonClick} leftButtonClick={props.leftButtonClick}></OrganismList>
            break;                
        case "patientsearchresultspage":
            pageToDisplay = <PatientSearchResult config={props.config} data={props.inputData} changeHandler={props.changeHandler} rightButtonClick={props.rightButtonClick} language={props.language} allowNewPatient={true}></PatientSearchResult>
            break;
        case "patientsearchresultswithnoaddpage":
            pageToDisplay = <PatientSearchResult config={props.config} data={props.inputData} changeHandler={props.changeHandler} rightButtonClick={props.rightButtonClick} language={props.language} allowNewPatient={false}></PatientSearchResult>
            break;
        case "runiqctestpage":
        case "editiqcresultpage":
            pageToDisplay = <InputIqcResultsPage config={props.config} data={props.data} changeHandler={props.changeHandler} language={props.language}></InputIqcResultsPage>
            break;
        case "runexportpage":
            pageToDisplay = <Exports config={props.config} changeHandler={props.changeHandler} save={props.save} close={props.close} language={props.language} id={props.id}></Exports>
            break;
        case "manageexportprofilemappingpage":
            pageToDisplay = <MappingEditor config={props.config} data={props.data} changeHandler={props.changeHandler} save={props.save} language={props.language} id={props.id}></MappingEditor>
            break;
        case "selectorganismpage":
            pageToDisplay = <OrganismSearch config={props.config} data={props.data} changeHandler={props.changeHandler} fullScreen={props.fullScreen} rightButtonClick={props.rightButtonClick} language={props.language}></OrganismSearch>
            break;   
        case "specimenreportpage": {
            const selectedRecord = props.startConfig?.selectedRecord;
            const currentRecord = props.startConfig?.currentRecord;
            const laboratoryId =
                selectedRecord?.laboratoryid ??
                selectedRecord?.LaboratoryId ??
                currentRecord?.laboratoryid ??
                currentRecord?.LaboratoryId;
            pageToDisplay = (
                <PrintPublish
                    config={props.config}
                    data={props.data}
                    id={props.id}
                    refresh={props.refresh}
                    language={props.language}
                    laboratoryid={laboratoryId}
                />
            );
            break;
        }
        case "selectorganismscopepage":
            pageToDisplay = <OrganismScope config={props.config} data={props.data} changeHandler={props.changeHandler} fullScreen={props.fullScreen} language={props.language} allLevels={true}></OrganismScope>
            break;
        case "editorganismscopepage":
            pageToDisplay = <OrganismScope config={props.config} data={props.data} changeHandler={props.changeHandler} fullScreen={props.fullScreen} language={props.language} allLevels={true} rightButtonClick={props.rightButtonClick}></OrganismScope>
            break;
        case "edittableentrypage":
        case "tableentrypage":
            pageToDisplay = <TableEntry config={props.config} fullScreen={props.fullScreen} data={props.data} changeHandler={props.changeHandler} rightButtonClick={props.rightButtonClick} language={props.language}></TableEntry>
            break;
        case "testcultureselectionpage":
            //const data = props.data.map(obj => ({...obj, Display: "Yes" }));
            pageToDisplay = <MenuPermissions config={props.config} data={props.data} changeHandler={props.changeHandler} language={props.language} onKeyDown={props.onKeyDown} ></MenuPermissions>
            break;
        case "testselectionpage":
            let testCategoryId = props?.inputData?.find((t) => t.key === "TestCategoryId")?.value;
            if (testCategoryId === undefined) {
                testCategoryId = props.startConfig?.selectedRecord?.testcategoryid;
            }
            pageToDisplay = <MenuPermissions config={props.config} data={props.data} changeHandler={props.changeHandler} language={props.language} onKeyDown={props.onKeyDown} categorySelection={testCategoryId} categoryName={"testcategoryid"}></MenuPermissions>
            break;
        case "whonetexportpage":
            pageToDisplay = <Exports config={props.config} changeHandler={props.changeHandler} save={props.save} language={props.language}></Exports>
            break;
        default:
            pageToDisplay = <div>No crafted form found</div>
            break;
    }

    return (
        <div>
            {pageToDisplay}
        </div>
    );
};
  
export default CraftedFactory;