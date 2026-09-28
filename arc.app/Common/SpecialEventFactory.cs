using arc.app.Alert;
using arc.app.Admission;
using arc.app.Asset;
using arc.app.AST;
using arc.app.Barcodes;
using arc.app.Coding;
using arc.app.ExpertRule;
using arc.app.Config;
using arc.app.Config.Queries;
using arc.app.Configuration;
using arc.app.Configuration.Events;
using arc.app.Exports;
using arc.app.Import;
using arc.app.Instruments;
using arc.app.Laboratory;
using arc.app.Language;
using arc.app.List;
using arc.app.Location;
using arc.app.Patient;
using arc.app.Quality;
using arc.app.Reports;
using arc.app.Reports.Events;
using arc.app.Request;
using arc.app.Roles;
using arc.app.Security;
using arc.app.Settings;
using arc.app.Specimen;
using arc.app.Tests;
using arc.app.Users;
using arc.app.Images.Events;
using arc.app.Images;
using arc.common.Models;
using arc.common.Utils;
using System;


namespace arc.app.Common
{
    /// <summary>
    /// This factory class creates instances of special events.
    /// </summary>
    public class SpecialEventFactory : ISpecialEventFactory
    {

        private readonly IASTRepository _ASTRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITestRepository _testRepository;
        private readonly ISpecimenRepository _specimenRepository;
        private readonly IJsonUtils _jsonUtils;
        private readonly IConfigFactory _configFactory;
        private readonly ILanguageRepository _languageRepository;
        private readonly IQueryAdapter _queryAdapter;
        private readonly ICodingRepository _codingRepository;
        private readonly IListRepository _listRepository;
        private readonly IBarcodesHandler _barcodesHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOrganisationRepository _organisationRepository;
        private readonly ILogWriter _logWriter;
        private readonly IConfigExtractionUtils _configExtractionUtils;
        private readonly ICultureRepository _cultureRepository;
        private readonly IListViewConfigFactory _listViewConfigFactory;
        private readonly IInstrumentRepository _instrumentResultsRepository;
        private readonly IInstrumentManualRequestService _instrumentManualRequestService;
        private readonly IAccessionNumberCalculator _accessionNumberCalculator;
        private readonly ICommentRepository _commentRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ImageSpecialEventFactory _imageSpecialEventFactory;

        /// <summary>
        /// Initializes a new instance of the SpecialEventFactory class.
        /// </summary>
        /// <param name="ASTRepository">The AST repository instance.</param>
        /// <param name="roleRepository">The role repository instance.</param>
        /// <param name="specimenRepository">The specimen repository instance.</param>
        /// <param name="jsonUtils">The JSON utilities instance.</param>
        /// <param name="testRepository">The test repository instance.</param>
        /// <param name="configFactory">The configuration factory instance.</param>
        /// <param name="languageRepository">The language repository instance.</param>
        /// <param name="queryAdapter">The query adapter instance.</param>
        /// <param name="codingRepository">The coding repository instance.</param>
        /// <param name="listRepository">The list repository instance.</param>
        /// <param name="barcodesHandler">The barcodes handler instance.</param>
        /// <param name="serviceProvider">The service provider instance.</param>
        /// <param name="organisationRepository">The organisation repository instance.</param>
        /// <param name="logWriter">The log writer instance.</param>
        /// <param name="configExtractionUtils">The configuration extraction utilities instance.</param>
        /// <param name="listViewConfigFactory">The list view configuration factory instance.</param>
        /// <param name="instrumentResultsRepository">The instrument results repository instance.</param>
        /// <param name="cultureRepository">The culture repository instance.</param>
        /// <param name="accessionNumberCalculator">The accession number calculator instance.</param>
        /// <param name="commentRepository">The comment repository instance.</param>
        public SpecialEventFactory(IASTRepository ASTRepository, IRoleRepository roleRepository, ISpecimenRepository specimenRepository, IJsonUtils jsonUtils,
            ITestRepository testRepository, IConfigFactory configFactory, ILanguageRepository languageRepository, IQueryAdapter queryAdapter, ICodingRepository codingRepository,
            IListRepository listRepository, IBarcodesHandler barcodesHandler, IServiceProvider serviceProvider,
            IOrganisationRepository organisationRepository, ILogWriter logWriter, IConfigExtractionUtils configExtractionUtils, IListViewConfigFactory listViewConfigFactory,
            IInstrumentRepository instrumentResultsRepository, IInstrumentManualRequestService instrumentManualRequestService, ICultureRepository cultureRepository, IAccessionNumberCalculator accessionNumberCalculator, ICommentRepository commentRepository, IImageRepository imageRepository)
        {
            _ASTRepository = ASTRepository;
            _roleRepository = roleRepository;
            _testRepository = testRepository;
            _specimenRepository = specimenRepository;
            _jsonUtils = jsonUtils;
            _configFactory = configFactory;
            _languageRepository = languageRepository;
            _queryAdapter = queryAdapter;
            _codingRepository = codingRepository;
            _listRepository = listRepository;
            _barcodesHandler = barcodesHandler;
            _serviceProvider = serviceProvider;
            _organisationRepository = organisationRepository;
            _logWriter = logWriter;
            _configExtractionUtils = configExtractionUtils;
            _instrumentResultsRepository = instrumentResultsRepository;
            _instrumentManualRequestService = instrumentManualRequestService;
            _cultureRepository = cultureRepository;
            _accessionNumberCalculator = accessionNumberCalculator;
            _listViewConfigFactory = listViewConfigFactory;
            _commentRepository = commentRepository;
            _imageRepository = imageRepository;
            _imageSpecialEventFactory = new ImageSpecialEventFactory(_imageRepository);
        }

        /// <summary>
        /// Gets the event instance based on the event name.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="token">The token information model.</param>
        /// <returns>An instance of the event.</returns>
        public IRun GetEvent(string eventName, TokenInfoModel token)
        {
            _logWriter.LogInfo($"Calling the special event handler for : {eventName.ToLower()}", "SpecialEventFactory", "GetEvent");

            // Delegate to feature-specific factory first to avoid duplicating mappings here
            var imageEvent = _imageSpecialEventFactory.GetEvent(eventName, token);
            if (imageEvent != null)
            {
                return imageEvent;
            }

            return eventName.ToLower() switch
            {
                "acceptresultsevent" => new AcceptResultsEvent(_serviceProvider, token),
                "ackreceipt" => new ACKReceiptEvent(_specimenRepository, _listViewConfigFactory),
                "addantibiotic" => new AddAntibioticEvent(_serviceProvider),
                "addculture" => new AddCultureEvent(_serviceProvider, token),
                "addculturetest" => new AddCultureTestEvent(_serviceProvider),
                "addculturetypecategoryevent" => new AddLaboratoryConfigEvent("culturetypecategory", _serviceProvider),
                "addculturetypeculturetest" => new AddLaboratoryConfigEvent("culturetypeculturetestdefault", _serviceProvider),
                "addculturetypeculturetestoptionevent" => new AddLaboratoryConfigEvent("culturetypeculturetestoption", _serviceProvider),
                "addorganismscopeculturetestoptionevent" => new AddLaboratoryConfigEvent("organismscopeculturetestoption", _serviceProvider),
                "adddirecttest" => new AddDirectTestEvent(_serviceProvider),
                "addexpertrule" => new AddExpertRuleEvent(_serviceProvider),
                "addiqctest" => new AddIqcTestEvent(_serviceProvider),
                "addiqctestprofile" => new AddIqcTestProfileEvent(_serviceProvider),
                "addisolateevent" => new AddIsolateEvent(_serviceProvider, token),
                "addspecimentag" => new AddSpecimenTagEvent(_serviceProvider),
                "managespecimenattachments" => new ManageSpecimenAttachmentsEvent(_serviceProvider),
                "managecultureattachments" => new ManageCultureAttachmentsEvent(_serviceProvider),
                "managepatientattachments" => new ManagePatientAttachmentsEvent(_serviceProvider),
                "manageadmissionattachments" => new ManageAdmissionAttachmentsEvent(_serviceProvider),
                "managerequestattachments" => new ManageRequestAttachmentsEvent(_serviceProvider),
                "addspecimentypeculturetype" => new AddLaboratoryConfigEvent("specimentypeculturetypedefault", _serviceProvider),
                "addspecimentypedirecttest" => new AddLaboratoryConfigEvent("specimentypedirecttestdefault",_serviceProvider),
                "addformspecimentypeoption" => new AddLaboratoryConfigEvent("formspecimentypeoption", _serviceProvider),
                "addspecimentypeculturetypeoptionevent" => new AddLaboratoryConfigEvent("specimentypeculturetypeoption", _serviceProvider),
                "addspecimentypedirecttestoptionevent" => new AddLaboratoryConfigEvent("specimentypedirecttestoption", _serviceProvider),
                "addspecimentypeworkflowevent" => new AddLaboratoryConfigEvent("specimentypeworkflow", _serviceProvider),
                "addtestcategoryevent" => new AddLaboratoryConfigEvent("testcategory", _serviceProvider),
                "addaccessionnumbertext" => new AddAccessionNumberTextEvent(_serviceProvider),
                "addpatientreferencetext" => new AddPatientReferenceTextEvent(_serviceProvider),
                "addalert" => new AddAlertEvent(_serviceProvider),
                "addalertapproval" => new AddAlertApprovalEvent(_serviceProvider, token),
                "addbreakpoint" => new AddBreakpointEvent(_serviceProvider),
                "addbreakpointapproval" => new AddBreakpointApprovalEvent(_serviceProvider, token, _logWriter),
                "addexpertruleapproval" => new AddExpertRuleApprovalEvent(_serviceProvider, token, _logWriter),
                "addcustomentry" => new AddCustomEntryEvent(_codingRepository),
                "addfield" => new AddFieldEvent(_serviceProvider),
                "addexistingfield" => new AddExistingFieldEvent(_serviceProvider),
                "addexportprofile" => new AddExportProfileEvent(_serviceProvider),
                "addexportprofilefield" => new AddExportProfileFieldEvent(_serviceProvider),
                "addexportschedule" => new AddExportScheduleEvent(_serviceProvider),
                //"addimportprofile" => new AddImportProfileEvent(_serviceProvider),
                "addinstrumentprofile" => new AddInstrumentProfileEvent(_serviceProvider, token),
                "addlanguage" => new AddLanguageEvent(_configFactory, _queryAdapter, _languageRepository),
                "addlocation" => new AddLocationEvent(_serviceProvider),
                "addmappingevent" => new AddMappingEvent(_serviceProvider),
                "addorganisation" => new AddOrganisationEvent(_organisationRepository),
                "addorganismalert" => new AddAlertEvent(_serviceProvider),
                "addpatienttag" => new AddPatientTagEvent(_serviceProvider),
                "addpage" => new AddPageEvent(_serviceProvider),
                "addreportconfig" => new AddReportConfigEvent(_serviceProvider),
                "addsection" => new AddSectionEvent(_serviceProvider),
                "addstorageevent" => new AddStorageEvent(_serviceProvider, token),
                "addtag" => new AddTagEvent(_listRepository, _logWriter),
                "addtable" => new AddTableEvent(_listRepository),
                "addtableentry" => new AddTableEntryEvent(_listRepository, _logWriter),
                "addtestpattern" => new AddTestPatternEvent(_serviceProvider),
                "addworkflowentry" => new AddWorkflowEntryEvent(_serviceProvider, "add"),
                "approvereportevent" => new ApproveReportEvent(_serviceProvider, token),
                "clonerole" => new CloneRoleEvent(_jsonUtils, _roleRepository),
                "cultureprintselector" => new CulturePrintSelectorEvent(_serviceProvider),
                "culturetestselection" => new CultureTestSelectionEvent(_testRepository),
                "day1benchread" => new Day1BenchReadEvent(_listViewConfigFactory, _cultureRepository, token, _serviceProvider),
                "deletealert" => new DeleteAlertEvent(_serviceProvider),
                "deleteantibioticgroup" => new DeleteAntibioticGroupEvent(_serviceProvider),
                "deletebreakpoint" => new DeleteBreakpointEvent(_serviceProvider),
                "deletecodinglist" => new DeleteCodingListEvent(_codingRepository),
                "deletecomment" => new DeleteCommentEvent(_commentRepository),
                "deleteculture" => new DeleteCultureEvent(_serviceProvider),
                "deleteculturetestconfig" => new DeleteDirectTestEvent(_serviceProvider),
                "deletedirecttest" => new DeleteDirectTestEvent(_serviceProvider),
                "deleteexpertrule" => new DeleteExpertRuleEvent(_serviceProvider),
                "deleteexpertrulecondition" => new DeleteExpertRuleConditionEvent(_serviceProvider),
                "deleteexpertruleaction" => new DeleteExpertRuleActionEvent(_serviceProvider),
                "deleteexpertruletestcondition" => new DeleteExpertRuleTestConditionEvent(_serviceProvider),
                "addexpertruletestcondition" => new AddExpertRuleTestConditionEvent(_serviceProvider),
                "editexpertruletestcondition" => new EditExpertRuleTestConditionEvent(_serviceProvider),
                "addexpertrulecondition" => new AddExpertRuleConditionEvent(_serviceProvider),
                "editexpertrulecondition" => new EditExpertRuleConditionEvent(_serviceProvider),
                "addexpertruleaction" => new AddExpertRuleActionEvent(_serviceProvider),
                "editexpertruleaction" => new EditExpertRuleActionEvent(_serviceProvider),
                "deleteexportprofile" => new DeleteExportProfileEvent(_serviceProvider),
                "deleteexportprofilefield" => new DeleteExportProfileFieldEvent(_serviceProvider),
                "deleteexportschedule" => new DeleteExportScheduleEvent(_serviceProvider),
                "deletefield" => new DeleteFieldEvent(_serviceProvider),
                "deleteinstrumentprofile" => new DeleteInstrumentProfileEvent(_serviceProvider),
                "deleteiqctestprofile" => new DeleteIqcTestProfileEvent(_serviceProvider),
                "deleteisolateevent" => new DeleteIsolateEvent(_serviceProvider),
                "deletelaboratory" => new DeleteLaboratoryEvent(_serviceProvider),
                "deletelanguage" => new DeleteLanguageEvent(_languageRepository),
                "deletemappingevent" => new DeleteMappingEvent(_serviceProvider),
                "deleteorganism" => new DeleteOrganismEvent(_codingRepository),
                "deletepage" => new DeletePageEvent(_serviceProvider),
                "deletereportconfig" => new DeleteReportConfigEvent(_serviceProvider),
                "deleterole" => new DeleteRoleEvent(_roleRepository),
                "deletesection" => new DeleteSectionEvent(_serviceProvider),
                "deletesetting" => new DeleteSettingEvent(_serviceProvider),
                "deletetable" => new DeleteTableEvent(_listRepository),
                "deletetableentry" => new DeleteTableEntryEvent(_listRepository),
                "deletetestpattern" => new DeleteTestPatternEvent(_serviceProvider),
                "deleteuser" => new DeleteUserEvent(_serviceProvider),
                "deleteworkflowentry" => new DeleteWorkflowEntryEvent(_serviceProvider),
                "disableculturetest" => new DisableCultureTestEvent(_serviceProvider),
                "disabledirecttest" => new DisableDirectTestEvent(_serviceProvider),
                "editalert" => new EditAlertEvent(_serviceProvider),
                "editaccessionnumber" => new EditAccessionNumberEvent(_serviceProvider),
                "editpatientreference" => new EditPatientReferenceEvent(_serviceProvider),
                "editbarcodeprintconfig" => new EditBarcodePrintConfigEvent(_serviceProvider),
                "editbreakpoint" => new EditBreakpointEvent(_serviceProvider),
                "editculture" => new EditCultureEvent(_serviceProvider, token),
                "editculturetest" => new EditDirectTestEvent(_serviceProvider),
                "editculturetypecategoryevent" => new UpdateLaboratoryConfigEvent(_serviceProvider),
                "editculturetypeculturetest" => new UpdateLaboratoryConfigEvent(_serviceProvider),
                "editculturetypeculturetestoptionevent" => new UpdateLaboratoryConfigEvent(_serviceProvider),
                "editorganismscopeculturetestoptionevent" => new UpdateLaboratoryConfigEvent(_serviceProvider),
                "editcustomentry" => new EditCustomEntryEvent(_codingRepository),
                "editdirecttest" => new EditDirectTestEvent(_serviceProvider),
                "editexpertrule" => new EditExpertRuleEvent(_serviceProvider),
                "editexportprofile" => new EditExportProfileEvent(_serviceProvider),
                "editexportschedule" => new EditExportScheduleEvent(_serviceProvider),
                "editexportprofilefields" => new EditExportProfileFieldsEvent(_serviceProvider),
                "editfield" => new EditFieldEvent(_serviceProvider),
                "editinstrumentprofile" => new EditInstrumentProfileEvent(_serviceProvider, token),
                "editiqcresult" => new EditIqcResultEvent(_serviceProvider),
                "editiqctestqcorganisms" => new EdiIiqcTestQcOrganismsEvent(_serviceProvider),
                "editisolateevent" => new EditCultureEvent(_serviceProvider, token),
                "editlocation" => new EditLocationEvent(_serviceProvider),
                "editmappingevent" => new EditMappingEvent(_serviceProvider),
                "editorganisation" => new EditOrganisationEvent(_organisationRepository),
                "editpage" => new EditPageEvent(_serviceProvider),
                "editpages" => new EditPagesEvent(_serviceProvider),
                "editpagerules" => new EditPageRulesEvent(_serviceProvider),
                "addformgroup" => new AddFormGroupEvent(_serviceProvider),
                "edittag" => new EditTagEvent(_listRepository, _logWriter),
                "editformgroup" => new EditFormGroupEvent(_serviceProvider),
                "movefield" => new MoveFieldEvent(_serviceProvider),
                "moveformgroup" => new MoveFormGroupEvent(_serviceProvider),
                "deleteformgroup" => new DeleteFormGroupEvent(_serviceProvider),
                "editiqctestprofileqcorganism" => new EditIqcTestProfileEvent(_serviceProvider),
                "editreportconfig" => new EditReportConfigEvent(_serviceProvider),
                "editreportsection" => new EditReportSectionEvent(_serviceProvider),
                "editsection" => new EditSectionEvent(_serviceProvider),
                "editsetting" => new EditSettingEvent(_serviceProvider),
                "editspecimen" => new EditSpecimenEvent(_specimenRepository, _serviceProvider, _logWriter),
                "editspecification" => new EditSpecificationEvent(_serviceProvider),
                "editspecimentypeculturetype" => new UpdateLaboratoryConfigEvent(_serviceProvider),
                "editformspecimentypeoption" => new UpdateLaboratoryConfigEvent(_serviceProvider),
                "editspecimentypeculturetypeoptionevent" => new UpdateLaboratoryConfigEvent(_serviceProvider),
                "editspecimentypedirecttest" => new UpdateLaboratoryConfigEvent(_serviceProvider),
                "editspecimentypedirecttestoptionevent" => new UpdateLaboratoryConfigEvent(_serviceProvider),
                "editspecimentypeworkflowevent" => new UpdateLaboratoryConfigEvent(_serviceProvider),
                "editstorageevent" => new EditStorageEvent(_serviceProvider),
                "edittableentry" => new EditTableEntryEvent(_listRepository, _logWriter),
                "edittestcategoryevent" => new UpdateLaboratoryConfigEvent(_serviceProvider),
                "updateturnaroundtimeconfig" => new UpdateTurnAroundTimeConfigEvent(_serviceProvider),
                "edittestpattern" => new EditTestPatternEvent(_serviceProvider),
                "editword" => new EditWordEvent(_serviceProvider),
                "editworkflowentry" => new AddWorkflowEntryEvent(_serviceProvider, "edit"),
                "ordertable" => new OrderTableEvent(_listRepository),
                "importconfiguration" => new ImportConfigurationEvent(_serviceProvider),
                "instrumentculture" => new InstrumentCultureEvent(_serviceProvider),
                "instrumentresults" => new InstrumentResultsEvent(_serviceProvider, token),
                "mergepatient" => new MergePatientEvent(_serviceProvider),
                "movepatient" => new MovePatientEvent(_serviceProvider),
                "preference" => new EditUserPreferenceEvent(_serviceProvider, token),
                "savefilterpresetsevent" => new SaveFilterPresetsEvent(_serviceProvider, token),
                "savecolumnlayoutsevent" => new SaveColumnLayoutsEvent(_serviceProvider, token, _logWriter),
                "savehomedashboardevent" => new SaveHomeDashboardEvent(_serviceProvider, token, _logWriter),
                "rejectresultsevent" => new RejectResultsEvent(_serviceProvider),
                "reorderformgroups" => new ReorderFormGroupsEvent(_serviceProvider),
                "remotespecimen" => new RemoteSpecimenRequestEvent(_specimenRepository, _barcodesHandler, token, _logWriter, _accessionNumberCalculator, _listViewConfigFactory, _serviceProvider),
                "replayinstrumenterror" => new ReplayInstrumentErrorEvent(_serviceProvider, token),
                "requestinstrumenttest" => new RequestInstrumentTestEvent(_instrumentManualRequestService, token),
                "runexportprofile" => new RunExportEvent(_serviceProvider),
                "saveexportprofilemapping" => new SaveExportProfileMappingEvent(_serviceProvider),
                "runiqctest" => new RunIqcTestEvent(_serviceProvider),
                "markiqctestcomplete" => new MarkIqcTestCompleteEvent(_serviceProvider),
                "neoshieldspecimen" => new NeoshieldSpecimenRequestEvent(_specimenRepository, _barcodesHandler, token, _logWriter, _accessionNumberCalculator, _listViewConfigFactory, _serviceProvider),
                "newreceivedspecimen" => new ReceivedSpecimenRequestEvent(_specimenRepository, _barcodesHandler, token, _logWriter, _accessionNumberCalculator, _listViewConfigFactory, _serviceProvider),
                "reportsectionmove" => new ReportSectionMoveEvent(_serviceProvider),
                "savereportdesignerconfig" => new SaveReportDesignerConfigEvent(_serviceProvider),
                "saveworkflowdesignerconfig" => new SaveWorkflowDesignerConfigEvent(_serviceProvider),
                "specimenapprovalone" => new SpecimenApprovalEvent(_serviceProvider),
                "specimenapprovaltwo" => new SpecimenApprovalEvent(_serviceProvider),
                "specimenreport" => new SpecimenReportEvent(_serviceProvider),
                "synonym" => new SynonymEvent(_serviceProvider),
                "testselection" => new TestSelectionEvent(_testRepository, _logWriter),
                "unapprovereportevent" => new UnapproveReportEvent(_serviceProvider, token),
                "updateast" => new ASTUpdateEvent(_ASTRepository, token),
                _ => throw new NotSupportedException($"Event '{eventName}' is configured for deferSave and has no backend handler."),
            };
        }
    }
}
