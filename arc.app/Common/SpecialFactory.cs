using arc.app.Admission;
using arc.app.Alert;
using arc.app.AST;
using arc.app.Barcodes;
using arc.app.Request;
using arc.app.Coding;
using arc.app.Configuration;
using arc.app.ExpertRule;
using arc.app.Exports;
using arc.app.Instruments;
using arc.app.Language;
using arc.app.Monitoring;
using arc.app.Patient;
using arc.app.Quality;
using arc.app.Roles;
using arc.app.Security;
using arc.app.Specification;
using arc.app.Specimen;
using arc.app.SystemConfig;
using arc.app.Tests;
using arc.app.Utils;
using arc.common.Models;
using arc.common.Models.Export;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public class SpecialFactory : ISpecialFactory
    {
        private static readonly JsonSerializerSettings ListViewJsonSettings = new()
        {
            ContractResolver = new LowercaseContractResolver()
        };

        private readonly IAlertHandler _alertHandler;
        private readonly IASTRepository _ASTRepository;
        private readonly IASTHandler _ASTHandler;
        private readonly IUserRepository _userRepository;
        private readonly IMenuPermissionHandler _menuPermissionHandler;
        private readonly IEventPermissionHandler _eventPermissionHandler;
        private readonly IAddRoleDataHandler _addRoleDataHandler;
        private readonly ICurrentRoleDetailsHandler _currentRoleDetailsHandler;
        private readonly IWorkflowHandler _workflowHandler;
        private readonly IFormHandler _formHandler;
        private readonly IViewHandler _viewHandler;
        private readonly ITestSelectionHandler _testSelectionHandler;
        private readonly IFormattedJsonQueryHandler _formattedJsonQueryHandler;
        private readonly IExpertRuleHandler _expertRuleHandler;
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IOrganismRepository _organismRepository;
        private readonly ICodingRepository _codingRepository;
        private readonly ISpecificationRepository _specificationRepository;
        private readonly ITableEntryHandler _tableEntryHandler;
        private readonly IListRepository _listRepository;
        private readonly IBreakpointRepository _breakpointRepository;
        private readonly ITestPatternRepository _testPatternRepository;
        private readonly IBreakpointHandler _breakpointHandler;
        private readonly ITestPatternHandler _testPatternHandler;
        private readonly ICultureRepository _cultureRepository;
        private readonly ILaboratoryRepository _laboratoryRepository;
        private readonly ISpecimenRepository _specimenRepository;
        private readonly ICultureList _cultureList;
        private readonly ICultureHandler _cultureHandler;
        private readonly IAlertRepository _alertRepository;
        private readonly ISpecimenAlertRepository _specimenAlertRepository;
        private readonly IPreferenceHandler _preferenceHandler;
        private readonly IBarcodesHandler _barcodeHandler;
        private readonly ILanguageListHandler _languageListHandler;
        private readonly ILogWriter _logWriter;
        private readonly IPatientRepository _patientrepository;
        private readonly IQualityRepository _qualityRepository;
        private readonly IIqcTestProfileQueryHandler _iqcTestProfiles;
        private readonly IACKReceiptLoader _aCKReceiptLoader;
        private readonly IRunIqcTestHandler _runIqcTestHandler;
        private readonly IEditIqcResultHandler _editIqcResultHandler;
        private readonly IExportProfileRepository _exportProfileRepository;
        private readonly IExportProfileQueryHandler _exportProfileQueryHandler;
        private readonly IExportProfileMappingHandler _exportProfileMappingHandler;
        private readonly IExportRunHistoryRepository _exportRunHistoryRepository;
        private readonly IExportScheduleRepository _exportScheduleRepository;
        private readonly IInstrumentRepository _instrumentResultsRepository;
        private readonly ISpecialQueryFactory _specialQueryFactory;
        private readonly IAntibioticRepository _antibioticRepository;
        private readonly ITestRepository _testRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IConfigRepository _configRepository;
        private readonly IQueueListQueryHandler _queueListQueryHandler;
        private readonly IExpertRuleRepository _expertRuleRepository;
        private readonly IExpertRuleTestConditionListDisplayEnricher _expertRuleTestConditionListDisplayEnricher;
        private readonly IAdmissionRepository _admissionRepository;
        private readonly IRequestRepository _requestRepository;

        public SpecialFactory(IASTRepository ASTRepository, IUserRepository userRepository, IMenuPermissionHandler menuPermissionHandler, IEventPermissionHandler eventPermissionHandler,
            IAddRoleDataHandler addRoleDataHandler, ICurrentRoleDetailsHandler currentRoleDetailsHandler, IWorkflowHandler workflowHandler, IFormHandler formHandler,
            IViewHandler viewHandler, ITestSelectionHandler testSelectionHandler, IFormattedJsonQueryHandler formattedJsonQueryHandler, IExpertRuleHandler expertRuleHandler, ILanguageListHandler languageListHandler, IOrganisationRepository organisationRepository,
            IOrganismRepository organismRepository, ICodingRepository codingRepository, ISpecificationRepository specificationRepository, ITableEntryHandler tableEntryHandler, IListRepository listRepository, IBreakpointRepository breakpointRepository,
            ITestPatternRepository testPatternRepository, IBreakpointHandler breakpointHandler, ITestPatternHandler testPatternHandler, IASTHandler ASTHandler, ICultureRepository cultureRepository,
            ILaboratoryRepository laboratoryRepository, ISpecimenRepository specimenRepository, ICultureList cultureList, ICultureHandler cultureHandler, IAlertRepository alertRepository,
            ISpecimenAlertRepository specimenAlertRepository, IPreferenceHandler preferenceHandler, IBarcodesHandler barcodeHandler, ILogWriter logWriter, IPatientRepository patientRepository,
            IQualityRepository qualityRepository, IIqcTestProfileQueryHandler qualityAssuranceProfile, IACKReceiptLoader aCKReceiptLoader,
            IInstrumentRepository instrumentResultsRepository, ISpecialQueryFactory specialQueryFactory, IAlertHandler alertHandler,
        IRunIqcTestHandler runIqcTestHandler, IEditIqcResultHandler editIqcResultHandler, IExportProfileRepository exportProfileRepository, IExportProfileQueryHandler exportProfileQueryHandler,
            IExportProfileMappingHandler exportProfileMappingHandler,
            IExportRunHistoryRepository exportRunHistoryRepository, IExportScheduleRepository exportScheduleRepository,
            IAntibioticRepository antibioticRepository, ITestRepository testRepository, ICommentRepository commentRepository, IConfigRepository configRepository,
            IQueueListQueryHandler queueListQueryHandler, IExpertRuleRepository expertRuleRepository,
            IExpertRuleTestConditionListDisplayEnricher expertRuleTestConditionListDisplayEnricher,
            IAdmissionRepository admissionRepository, IRequestRepository requestRepository)
        {
            _admissionRepository = admissionRepository;
            _requestRepository = requestRepository;
            _ASTRepository = ASTRepository;
            _ASTHandler = ASTHandler;
            _userRepository = userRepository;
            _menuPermissionHandler = menuPermissionHandler;
            _eventPermissionHandler = eventPermissionHandler;
            _addRoleDataHandler = addRoleDataHandler;
            _currentRoleDetailsHandler = currentRoleDetailsHandler;
            _workflowHandler = workflowHandler;
            _formHandler = formHandler;
            _viewHandler = viewHandler;
            _testSelectionHandler = testSelectionHandler;
            _formattedJsonQueryHandler = formattedJsonQueryHandler;
            _expertRuleHandler = expertRuleHandler;
            _languageListHandler = languageListHandler;
            _organisationRepository = organisationRepository;
            _organismRepository = organismRepository;
            _codingRepository = codingRepository;
            _specificationRepository = specificationRepository;
            _tableEntryHandler = tableEntryHandler;
            _listRepository = listRepository;
            _breakpointRepository = breakpointRepository;
            _testPatternRepository = testPatternRepository;
            _breakpointHandler = breakpointHandler;
            _testPatternHandler = testPatternHandler;
            _cultureRepository = cultureRepository;
            _laboratoryRepository = laboratoryRepository;
            _specimenRepository = specimenRepository;
            _cultureList = cultureList;
            _cultureHandler = cultureHandler;
            _alertRepository = alertRepository;
            _specimenAlertRepository = specimenAlertRepository;
            _preferenceHandler = preferenceHandler;
            _barcodeHandler = barcodeHandler;
            _logWriter = logWriter;
            _patientrepository = patientRepository;
            _qualityRepository = qualityRepository;
            _iqcTestProfiles = qualityAssuranceProfile;
            _aCKReceiptLoader = aCKReceiptLoader;
            _runIqcTestHandler = runIqcTestHandler;
            _editIqcResultHandler = editIqcResultHandler;
            _exportProfileRepository = exportProfileRepository;
            _exportProfileQueryHandler = exportProfileQueryHandler;
            _exportProfileMappingHandler = exportProfileMappingHandler;
            _exportRunHistoryRepository = exportRunHistoryRepository;
            _exportScheduleRepository = exportScheduleRepository;
            _instrumentResultsRepository = instrumentResultsRepository;
            _specialQueryFactory = specialQueryFactory;
            _antibioticRepository = antibioticRepository;
            _testRepository = testRepository;
            _alertHandler = alertHandler;
            _commentRepository = commentRepository;
            _configRepository = configRepository;
            _queueListQueryHandler = queueListQueryHandler;
            _expertRuleRepository = expertRuleRepository;
            _expertRuleTestConditionListDisplayEnricher = expertRuleTestConditionListDisplayEnricher;
        }

        public async Task<string> RunQueryAsync(string queryName, QueryFilterConfig parameters = null, TokenInfoModel token = null)
        {
            _logWriter.LogInfo($"Calling the special query handler for : {queryName}", "SpecialFactory", "RunQuery");

            string result;
            switch (queryName.ToLower())
            {
                case "addiqctestquery":
                    result = await _iqcTestProfiles.GetQcOrganismsProfileAsync(parameters);
                    break;
                case "admissionsforpatient":
                    var admissionsForPatient = await _admissionRepository.AdmissionsForPatientAsync(parameters);
                    result = JsonConvert.SerializeObject(admissionsForPatient, ListViewJsonSettings);
                    break;
                case "editadmissionquery":
                    var admissionForEdit = await _admissionRepository.AdmissionByIdAsync(parameters);
                    var admissionJson = JsonConvert.SerializeObject(admissionForEdit);
                    _logWriter.LogInfo($"Loaded admission {admissionForEdit?.Id} for edit query", "SpecialFactory", "RunQuery");
                    result = MoreDataUtils.MergeMoreDataFields(admissionJson);
                    break;
                case "editrequestquery":
                    var requestForEdit = await _requestRepository.RequestByIdAsync(parameters);
                    var requestJson = JsonConvert.SerializeObject(requestForEdit);
                    _logWriter.LogInfo($"Loaded request {requestForEdit?.Id} for edit query", "SpecialFactory", "RunQuery");
                    result = MoreDataUtils.MergeMoreDataFields(requestJson);
                    break;
                case "alertlist":
                    var alertList = await _alertRepository.AlertListQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(alertList);
                    break;
                case "alertsusceptibilitycriterialistbyalertid":
                    var susceptCriteria = await _alertRepository.AlertSusceptibilityCriteriaListByAlertIdQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(susceptCriteria);
                    break;
                case "alerttestcriterialistbyalertid":
                    var testCriteria = await _alertRepository.AlertTestCriteriaListByAlertIdQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(testCriteria);
                    break;
                case "astaddquery":
                    result = await _ASTHandler.GetASTDataForCultureAsync(parameters);
                    break;
                case "astlistbycultureid":
                    var astList = await _ASTRepository.GetAstListByCultureIdAsync(parameters);
                    result = JsonConvert.SerializeObject(astList);
                    break;
                case "blankalltestselection":
                    result = await _testSelectionHandler.GetAllTestsAsync("tests", token);
                    break;
                case "blankalltestselectionwithpatientref":
                    result = await _testSelectionHandler.GetAllTestsWithPatientRefAsync(parameters, "tests", token);
                    break;
                case "blanktestselection":
                    result = await _testSelectionHandler.GetTestsAsync(parameters, "tests", token);
                    break;
                case "blankculturetestselection":
                    result = await _testSelectionHandler.GetTestsAsync(parameters, "culturetests", token);
                    break;
                case "blankculturetypeandtestselection":
                    result = await _testSelectionHandler.GetTestsAndCultureTypesAsync("tests", token);
                    break;
                case "blankculturetypeandtestselectionwithpatientref":
                    result = await _testSelectionHandler.GetTestsAndCultureTypesWithPatientRefAsync(parameters, "tests", token);
                    break;
                //case "breakpointsforastrow":
                //    result = await _ASTHandler.GetBreakpointsForASTRowAsync(parameters);
                //    break;
                case "breakpointlinelistbybreakpointid":
                    var breakpointLineList = await _breakpointRepository.BreakpointLineListByBreakpointIdQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(breakpointLineList);
                    break;
                case "breakpointlist":
                    var breakpointList = await _breakpointRepository.BreakpointListQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(breakpointList);
                    break;
                case "checkwhethersourcecontainsbreakpoints":
                    var breakpointCountBySource = await _codingRepository.GetBreakpointCountBySourceGuidelinesIdAsync(parameters);
                    result = breakpointCountBySource.ToString();
                    break;
                case "checkwhethersourcecontainsexpertrules":
                    var expertRuleCountBySource = await _codingRepository.GetExpertRuleCountBySourceGuidelinesIdAsync(parameters);
                    result = expertRuleCountBySource.ToString();
                    break;
                case "checkwhetherdocumentinspecification":
                    var specCountByDocument = await _specificationRepository.GetSpecificationCountByDocumentIdAsync(parameters);
                    result = specCountByDocument.ToString();
                    break;
                case "checkwhetherversioninspecification":
                    var specCountByVersion = await _specificationRepository.GetSpecificationCountByVersionNumberIdAsync(parameters);
                    result = specCountByVersion.ToString();
                    break;
                case "checkwhetheryearinspecification":
                    var specCountByYear = await _specificationRepository.GetSpecificationCountByPublicationYearIdAsync(parameters);
                    result = specCountByYear.ToString();
                    break;
                case "checkduplicatecustomentry":
                    var record = await _codingRepository.GetCustomerEntryAsync(parameters);
                    result = record.ToString();
                    break;
                case "checkduplicatecustomentrycode":
                    var record1 = await _codingRepository.CheckCustomEntryCodeAsync(parameters);
                    result = record1.ToString();
                    break;
                //case "commentlistbyspecimenid":
                //    result = await _commentRepository.GetSpecimenListViewCommentsAsync(parameters);
                //    break;
                case "culturecommentlistquery":
                    var commentList = await _commentRepository.GetCultureCommentListByIdAsync(parameters);
                    result = JsonConvert.SerializeObject(commentList);
                    break;
                case "cultureviewdetailsquery":
                case "cultureforcultureview":
                case "isolateforcultureviewquery":
                    var cultureView = await _cultureRepository.GetCultureViewByIdAsync(parameters);
                    var cultureViewJson = JsonConvert.SerializeObject(cultureView);
                    result = MoreDataUtils.MergeMoreDataFields(cultureViewJson);
                    break;
                case "culturebyid":
                    result = await _cultureHandler.GetCultureByIdAsync(parameters);
                    break;
                case "culturelistbyspecimenid":
                    var cultureList = await _cultureList.GetAsync(parameters, token);
                    result = JsonConvert.SerializeObject(cultureList);
                    break;
                case "culturetestusagecountquery":
                    var cultureTestCount = await _testRepository.CultureTestUsageCountAsync(parameters);
                    result = JsonConvert.SerializeObject(cultureTestCount);
                    break;
                case "customentrybyorganismid":
                    var customResult = await _codingRepository.GetCustomerEntryByOrganismIdAsync(parameters);
                    result = JsonConvert.SerializeObject(customResult);
                    break;
                case "deletespecificationquery":
                    var deleteSpecificationData = await _specificationRepository.DeleteSpecificationAsync(parameters);
                    result = JsonConvert.SerializeObject(deleteSpecificationData);
                    break;
                case "directtestusagecountquery":
                    var directTestCount = await _testRepository.DirectTestUsageCountAsync(parameters);
                    result = JsonConvert.SerializeObject(directTestCount);
                    break;
                case "editalert":
                    result = await _alertHandler.GetAlertAsync(parameters);
                    break;
                case "editpatientbarcode":
                    result = await _barcodeHandler.GetBarcodeToEditAsync("Patient", parameters);
                    break;
                case "editiqctestprofileqcorganismquery":
                    result = await _iqcTestProfiles.GetProfileAsync(parameters);
                    break;
                case "editspecimenbarcode":
                    result = await _barcodeHandler.GetBarcodeToEditAsync("Specimen", parameters);
                    break;
                case "editbreakpoint":
                    result = await _breakpointHandler.GetBreakpoint(parameters);
                    break;
                case "editiqcresult":
                    var editIqcTestData = await _editIqcResultHandler.GetInitialDataAsync(parameters);
                    result = JsonConvert.SerializeObject(editIqcTestData);
                    break;
                case "editexpertrulequery":
                    result = await _expertRuleHandler.GetExpertRuleAsync(parameters);
                    break;
                case "editexportprofile":
                    var exportProfile = await _exportProfileRepository.EditExportProfileQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(exportProfile);
                    break;
                case "editiqctestqcorganismsquery":
                    result = await _iqcTestProfiles.GetQcOrganismsForEditIqcTestQcOrganismsPageAsync(parameters);
                    break;
                case "editspecificationquery":
                    var editSpecificationData = await _specificationRepository.EditSpecificationAsync(parameters);
                    result = JsonConvert.SerializeObject(editSpecificationData);
                    break;
                case "edittestpattern":
                    result = await _testPatternHandler.GetTestPatternAsync(parameters);
                    break;
                case "exportprofilelist":
                    var exportProfiles = await _exportProfileRepository.GetExportProfileListAsync(parameters);
                    result = JsonConvert.SerializeObject(exportProfiles);
                    break;
                case "exporthistorylist":
                    var exportHistoryList = await _exportRunHistoryRepository.GetExportHistoryListAsync(parameters);
                    result = JsonConvert.SerializeObject(exportHistoryList);
                    break;
                case "exporthistoryrecordview":
                    var exportHistoryRecord = await _exportRunHistoryRepository.GetExportHistoryByIdAsync(parameters.GetIntegerValue("id"));
                    result = exportHistoryRecord != null ? BuildEnrichedExportHistoryRecord(exportHistoryRecord) : "{}";
                    break;
                case "editexportprofilefieldsbyexportprofileid":
                    result = await _exportProfileQueryHandler.GetProfileFieldsForEditAsync(parameters);
                    break;
                case "editexportschedule":
                    var scheduleForEdit = await _exportScheduleRepository.GetByIdAsync(parameters);
                    result = scheduleForEdit != null ? BuildExportScheduleForEdit(scheduleForEdit) : "{}";
                    break;
                case "expertruleconditionlistbyexpertruleid":
                    var conditionList = await _expertRuleRepository.ExpertRuleConditionListByExpertRuleIdQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(conditionList);
                    break;
                case "expertruletestconditionlistbyexpertruleid":
                    var testConditionList = await _expertRuleRepository.ExpertRuleTestConditionListByExpertRuleIdQueryAsync(parameters);
                    var enrichedTestConditions = await _expertRuleTestConditionListDisplayEnricher.EnrichAsync(testConditionList, token);
                    result = JsonConvert.SerializeObject(enrichedTestConditions);
                    break;
                case "expertruleactionlistbyexpertruleid":
                    var actionList = await _expertRuleRepository.ExpertRuleActionListByExpertRuleIdQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(actionList);
                    break;
                case "expertrulelist":
                    var ruleList = await _expertRuleRepository.ExpertRuleListQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(ruleList);
                    break;
                case "exportprofilerecordview":
                    var exportProfileFields = await _exportProfileRepository.ExportProfileRecordViewQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(exportProfileFields);
                    break;
                case "exportprofilemappingforprofile":
                    var mappingViewModel = await _exportProfileMappingHandler.LoadAsync(parameters, token);
                    var mappingAvPairList = new List<JsonFieldModel>
                    {
                        new JsonFieldModel { Key = "mapping", Value = JsonConvert.SerializeObject(mappingViewModel) }
                    };
                    var mappingCraftedList = new List<CraftedModel>
                    {
                        new CraftedModel { Name = "manageexportprofilemappingpage", Contents = JsonConvert.SerializeObject(mappingAvPairList) }
                    };
                    var mappingCraftedWrapper = new CraftedWithIdForEventModel<CraftedModel> { Crafted = mappingCraftedList };
                    result = JsonConvert.SerializeObject(mappingCraftedWrapper);
                    break;
                case "exportschedulelistbyprofileid":
                    var exportScheduleList = await _exportScheduleRepository.GetSchedulesByProfileIdAsync(parameters);
                    result = JsonConvert.SerializeObject(exportScheduleList);
                    break;
                case "familylist":
                    var familyList = await _organismRepository.GetFamilyListAsync(parameters);
                    result = JsonConvert.SerializeObject(familyList);
                    break;
                case "formlist":
                    result = await _formHandler.GetFormListAsync();
                    break;
                case "genuslist":
                    var genusList = await _organismRepository.GetGenusListAsync(parameters);
                    result = JsonConvert.SerializeObject(genusList);
                    break;
                case "iqcresultsgridquery":
                    var iqcTests = await _qualityRepository.GetIqcTestResultsAsync(parameters);
                    result = JsonConvert.SerializeObject(iqcTests);
                    break;
                case "duplicateiqctestprofilenamequery":
                    var existingIqcTestProfile = await _qualityRepository.GetIqcTestProfileByNameAsync(parameters);
                    result = JsonConvert.SerializeObject(existingIqcTestProfile == null || existingIqcTestProfile.Id == 0 ? 0 : 1);
                    break;
                case "iqctestslist":
                    var iqcTestList = await _qualityRepository.GetIqcTestsListQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(iqcTestList, new JsonSerializerSettings { ContractResolver = new LowercaseContractResolver() });
                    break;
                case "iqctestprofileqcantibioticsdisk":
                    var antibioticsDisk = await _qualityRepository.GetDiskAntibioticsDetailsForIqcTestProfileQcOrganismAsync(parameters);
                    result = JsonConvert.SerializeObject(antibioticsDisk);
                    break;
                case "iqctestprofileqcantibioticsmic":
                    var antibioticsMic = await _qualityRepository.GetMicAntibioticsDetailsForIqcTestProfileQcOrganismAsync(parameters);
                    result = JsonConvert.SerializeObject(antibioticsMic);
                    break;
                case "instrumentresultslist":
                    var instrumentResultsList = await _instrumentResultsRepository.GetInstrumentResultsListAsync(parameters);
                    result = JsonConvert.SerializeObject(instrumentResultsList);
                    break;
                case "itemscontentsquery":
                    result = await _formattedJsonQueryHandler.GetDataAsync(parameters);
                    break;
                case "laboratorylist":
                    var laboratoryList = await _laboratoryRepository.GetLaboratoryListAsync(parameters);
                    result = JsonConvert.SerializeObject(laboratoryList);
                    break;
                case "languagelist":
                    var languageResult = await _languageListHandler.GetListAsync(parameters);
                    result = JsonConvert.SerializeObject(languageResult);
                    break;
                case "listbyid":
                    result = await _tableEntryHandler.AddTableEntryAsync(parameters);
                    break;
                case "listcontents":
                    var listResult = await _listRepository.GetListContentsAsync(parameters);
                    result = JsonConvert.SerializeObject(listResult);
                    break;
                case "listentrybyidforedit":
                    result = await _tableEntryHandler.EditTableEntryAsync(parameters);
                    break;
                case "locationpatientcount":
                    var locationCount = await _patientrepository.LocationPatientCountAsync(parameters);
                    result = JsonConvert.SerializeObject(locationCount);
                    break;
                case "mappinglistquery":
                    var mappings = await _configRepository.GetMappingListAsync(parameters);
                    result = JsonConvert.SerializeObject(mappings);
                    break;
                case "menuitemconfiglistquery":
                    result = "{}";
                    break;
                case "orderandfamilyfromgenusid":
                    var orderAndFamily = await _organismRepository.GetOrderAndFamilyFromGenusIdAsync(parameters);
                    result = JsonConvert.SerializeObject(orderAndFamily);
                    break;
                case "orderlist":
                    var orderList = await _organismRepository.GetOrderListAsync();
                    result = JsonConvert.SerializeObject(orderList);
                    break;
                case "organisationforvalidation":
                    var organisationResult = await _organisationRepository.GetOrganisationForValidationByIdAsync(parameters);
                    result = JsonConvert.SerializeObject(organisationResult);
                    break;
                case "organismculturecount":
                    var organismCountResult = await _organismRepository.GetOrganismCultureCountAsync(parameters);
                    result = JsonConvert.SerializeObject(organismCountResult);
                    break;
                case "organismlist":
                    var organismResult = await _organismRepository.GetOrganismListAsync(parameters);
                    result = JsonConvert.SerializeObject(organismResult);
                    break;
                case "organismlistentrybyid":
                    var organismEntryResult = await _organismRepository.GetOrganismListEntrByIdAsync(parameters);
                    result = JsonConvert.SerializeObject(organismEntryResult);
                    break;
                case "organismsearch":
                    var organismSearchResult = await _organismRepository.OrganismSearchAsync(parameters);
                    result = JsonConvert.SerializeObject(organismSearchResult);
                    break;
                case "iqctestprofilelistquery":
                    var iqcTestProfiles = await _qualityRepository.GetIqcTestProfileListQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(iqcTestProfiles);
                    break;
                case "iqctestprofilesinglequery":
                    var qcOrganism = await _qualityRepository.GetIqcTestProfileSingleQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(qcOrganism);
                    break;
                case "qcorganismforiqctestprofileview":
                    var qcOrganismresult = await _qualityRepository.GetIqcTestProfileSingleQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(qcOrganismresult);
                    break;
                case "patientbarcodelist":
                    result = await _barcodeHandler.GetBarcodeListAsync("3");
                    break;
                case "preference":
                    result = await _preferenceHandler.GetAsync(parameters, token);
                    break;
                case "reportcomments":
                    result = await _commentRepository.GetReportCommentsAsync(parameters);
                    break;
                case "requestsforadmission":
                    var requestsForAdmission = await _requestRepository.RequestsForAdmissionAsync(parameters);
                    result = JsonConvert.SerializeObject(requestsForAdmission, ListViewJsonSettings);
                    break;
                case "requestsforpatient":
                    var requestsForPatient = await _requestRepository.RequestsForPatientAsync(parameters);
                    result = JsonConvert.SerializeObject(requestsForPatient, ListViewJsonSettings);
                    break;
                case "resistantantibioticquery":
                    var resistantAntibiotics = await _antibioticRepository.GetResistantAntibioticQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(resistantAntibiotics);
                    break;
                case "resistantorganismquery":
                    var resistantOrganisms = await _organismRepository.GetResistantOrganismQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(resistantOrganisms);
                    break;
                case "roleaddquery":
                    result = await _addRoleDataHandler.GetInitialDataAsync();
                    break;
                case "rolebyidforrecordviewquery":
                    result = await _currentRoleDetailsHandler.HandleAsync(parameters);
                    break;
                case "roleeventpermissions":
                    result = await _eventPermissionHandler.GetEventPermissionsForRoleAsync(parameters);
                    break;
                case "rolemenupermissions":
                    result = await _menuPermissionHandler.GetMenuPermissionsForRoleAsync(parameters);
                    break;
                case "runiqctestquery":
                    var iqcresult = await _runIqcTestHandler.GetInitialDataAsync(parameters);
                    result = JsonConvert.SerializeObject(iqcresult);
                    break;
                case "serotypelist":
                    var serotypeList = await _organismRepository.GetSerotypeListAsync(parameters);
                    result = JsonConvert.SerializeObject(serotypeList);
                    break;
                case "singlealertforalertlist":
                    var singleAlert = await _alertRepository.SingleAlertForAlertListQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(singleAlert);
                    break;
                case "singleexpertruleforexpertrulelist":
                    var singleRule = await _expertRuleRepository.SingleExpertRuleForExpertRuleListQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(singleRule);
                    break;
                case "singlelaboratoryforlaboratorylist":
                    var laboratory = await _laboratoryRepository.LaboratoryByIdAsync(parameters);
                    result = JsonConvert.SerializeObject(laboratory);
                    break;
                case "singleorganismfororganismlist":
                    var organismEntry = await _organismRepository.GetOrganismListEntrByIdAsync(parameters);
                    result = JsonConvert.SerializeObject(organismEntry);
                    break;
                case "singlespecificationforspecificationlist":
                    var singleSpecification = await _specificationRepository.GetSingleSpecificationForSpecificationListAsync(parameters);
                    result = JsonConvert.SerializeObject(singleSpecification);
                    break;
                case "specimenbyidforack":
                    result = await _aCKReceiptLoader.LoadDataAsync(parameters, token);
                    break;
                case "patientlabelavailablefields":
                case "specimenlabelavailablefields":
                    result = "{}";
                    break;
                case "singletableentryforlist":
                    var listItem = await _listRepository.GetListContentsByIdAsync(parameters);
                    result = JsonConvert.SerializeObject(listItem);
                    break;
                case "specieslist":
                    var speciesList = await _organismRepository.GetSpeciesListAsync(parameters);
                    result = JsonConvert.SerializeObject(speciesList);
                    break;
                case "specificationexists":
                    var specificationExistsCount = await _specificationRepository.GetSpecificationCountAsync(parameters);
                    result = JsonConvert.SerializeObject(specificationExistsCount);
                    break;
                case "specificationinuse":
                    var specificationInUseCount = await _specificationRepository.GetSpecificationUseCountAsync(parameters);
                    result = JsonConvert.SerializeObject(specificationInUseCount);
                    break;
                case "specificationlist":
                    var specificationList = await _specificationRepository.GetSpecificationListAsync(parameters);
                    result = JsonConvert.SerializeObject(specificationList);
                    break;
                case "specimenalertlist":
                    var specimenAlertList = await _specimenAlertRepository.SpecimenAlertListQueryAsync(parameters);
                    result = JsonConvert.SerializeObject(specimenAlertList);
                    break;
                case "specimenbarcodelist":
                    result = await _barcodeHandler.GetBarcodeListAsync("4");
                    break;
                case "specimenbatchlist":
                    var specimenBatchList = await _specimenRepository.SpecimenBatchListAsync(parameters, token);
                    result = JsonConvert.SerializeObject(specimenBatchList);
                    break;
                case "subspecieslist":
                    var subspeciesList = await _organismRepository.GetSubSpeciesListAsync(parameters);
                    result = JsonConvert.SerializeObject(subspeciesList);
                    break;
                case "specimenorganism":
                    var organismList = await _organismRepository.GetOrganismDropdownAsync(token);
                    result = JsonConvert.SerializeObject(organismList);
                    break;
                case "specimenorganismcode":
                    var organismCodeList = await _organismRepository.GetOrganismCodeDropdownAsync(token);
                    result = JsonConvert.SerializeObject(organismCodeList);
                    break;
                case "specimentypecount":
                    var specimenCount = await _specimenRepository.SpecimenTypeCountAsync(parameters);
                    result = JsonConvert.SerializeObject(specimenCount);
                    break;
                case "specimenstatecount":
                    var specimenState = await _specimenRepository.SpecimenStateCountAsync(parameters);
                    result = JsonConvert.SerializeObject(specimenState);
                    break;
                case "specimentagcount":
                    var specimenTag = await _specimenRepository.SpecimenTagCountAsync(parameters);
                    result = JsonConvert.SerializeObject(specimenTag);
                    break;
                case "homedashboardrecentlyused":
                    AppendHomeDashboardTokenParameters(parameters, token);
                    var homeRecent = await _specimenRepository.HomeDashboardRecentlyUsedAsync(parameters);
                    _logWriter.LogInfo(
                        $"homedashboardrecentlyused returned {homeRecent?.Count ?? 0} rows",
                        nameof(SpecialFactory),
                        nameof(RunQueryAsync));
                    result = JsonConvert.SerializeObject(homeRecent);
                    break;
                case "homedashboardtatcompliance":
                    AppendHomeDashboardTokenParameters(parameters, token);
                    var tatKpi = await _specimenRepository.HomeDashboardTatComplianceAsync(parameters);
                    _logWriter.LogInfo(
                        $"homedashboardtatcompliance total={tatKpi?.TotalCount ?? 0} rag={tatKpi?.Rag}",
                        nameof(SpecialFactory),
                        nameof(RunQueryAsync));
                    result = JsonConvert.SerializeObject(tatKpi);
                    break;
                case "testlist":
                    result = await _formHandler.GetFormListAsync(true);
                    break;
                case "testpatternlist":
                    var patternList = await _testPatternRepository.GetTestPatternListAsync(parameters);
                    result = JsonConvert.SerializeObject(patternList);
                    break;
                case "testpatternwithbreakpoints":
                    parameters.AddString("id",parameters.GetStringValue("cultureid"));
                    result = await _ASTHandler.GetASTDataForCultureAsync(parameters);
                    break;
                case "testselection":
                    result = await _testSelectionHandler.GetTestsForSpecimenAsync(parameters);
                    break;
                case "userbyid":
                    result = await _userRepository.GetUserByIdAsync(parameters);
                    break;
                case "singleuserforuserlist":
                    result = await _userRepository.GetSingleUserForUserListAsync(parameters);
                    break;
                case "userlist":
                    result = await _userRepository.GetListAsync(parameters);
                    break;
                case "viewlist":
                    result = await _viewHandler.GetListView();
                    break;
                case "queuelist":
                    result = await _queueListQueryHandler.GetQueueListAsync(parameters, token);
                    break;
                default:
                    var queryDef = _specialQueryFactory.GetQuery(queryName);
                    result = await queryDef.RunAsync(parameters, token);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Builds an enriched export history record with filter criteria expanded into individual fields
        /// for display in the record view.
        /// </summary>
        /// <param name="record">The export history record from the repository.</param>
        /// <returns>JSON string of the enriched record with startdate, enddate, specimentypeids, etc.</returns>
        private static string BuildEnrichedExportHistoryRecord(ExportHistoryModel record)
        {
            var obj = new JObject
            {
                ["id"] = record.Id,
                ["exportprofileid"] = record.ExportProfileId,
                ["exportprofilename"] = record.ExportProfileName ?? "",
                ["runat"] = record.RunAt,
                ["fileattachmentid"] = record.FileAttachmentId,
                ["schedulename"] = record.ScheduleName ?? ""
            };

            static string JoinIds(System.Collections.Generic.IEnumerable<string> ids)
            {
                if (ids == null) return "";
                var list = ids.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                return list.Count > 0 ? string.Join(",", list) : "";
            }

            static string EmptyOrBlank(string value)
            {
                return string.IsNullOrWhiteSpace(value) ? "" : value;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(record.Filter))
                {
                    var filter = JsonConvert.DeserializeObject<ExportRunRequestModel>(record.Filter);
                    if (filter != null)
                    {
                        obj["startdate"] = filter.StartDate != default ? filter.StartDate.ToString("yyyy-MM-dd") : "";
                        obj["enddate"] = filter.EndDate != default ? filter.EndDate.ToString("yyyy-MM-dd") : "";
                        obj["specimentypeids"] = JoinIds(filter.SpecimenTypeIds);
                        obj["specimenstateids"] = JoinIds(filter.SpecimenStateIds);
                        obj["tagids"] = JoinIds(filter.TagIds);
                        obj["organisationids"] = JoinIds(filter.OrganisationIds);
                        obj["locationids"] = JoinIds(filter.LocationIds);
                        obj["testids"] = JoinIds(filter.TestIds);
                        obj["organismids"] = JoinIds(filter.OrganismIds);
                        obj["astexclusive"] = EmptyOrBlank(filter.ASTExclusive);
                        return obj.ToString();
                    }
                }
            }
            catch
            {
                // Fall through to add default empty values
            }

            obj["startdate"] = "";
            obj["enddate"] = "";
            obj["specimentypeids"] = "";
            obj["specimenstateids"] = "";
            obj["tagids"] = "";
            obj["organisationids"] = "";
            obj["locationids"] = "";
            obj["testids"] = "";
            obj["organismids"] = "";
            obj["astexclusive"] = "";

            return obj.ToString();
        }

        /// <summary>
        /// Builds an export schedule for the edit form with filter criteria expanded into individual fields.
        /// </summary>
        /// <param name="schedule">The export schedule from the repository.</param>
        /// <returns>JSON string with Id, ExportProfileId, Name, filter fields (OrganismId, SpecimenTypeId, etc.), Frequency, TimeOfDay, DayOfMonth, ChangesToInclude, OutputDirectory, Enabled.</returns>
        private static string BuildExportScheduleForEdit(ExportScheduleModel schedule)
        {
            var obj = new JObject
            {
                ["id"] = schedule.Id,
                ["exportprofileid"] = schedule.ExportProfileId,
                ["name"] = schedule.Name ?? "",
                ["frequency"] = schedule.Frequency ?? "",
                ["timeofday"] = schedule.TimeOfDay?.ToString(@"hh\:mm") ?? "",
                ["dayofmonth"] = schedule.DayOfMonth,
                ["incrementalonly"] = schedule.IncrementalOnly ? "Yes" : "No",
                ["changestoinclude"] = MapChangesToIncludeForForm(schedule.ChangesToInclude),
                ["ChangesToInclude"] = MapChangesToIncludeForForm(schedule.ChangesToInclude),
                ["outputdirectory"] = schedule.OutputDirectory ?? "",
                ["OutputDirectory"] = schedule.OutputDirectory ?? "",
                ["enabled"] = schedule.Enabled ? "Yes" : "No"
            };

            try
            {
                if (!string.IsNullOrWhiteSpace(schedule.Filter))
                {
                    var filter = JObject.Parse(schedule.Filter);
                    foreach (var prop in filter.Properties())
                    {
                        obj[prop.Name.ToLowerInvariant()] = prop.Value;
                    }
                }
            }
            catch
            {
                // Filter parse failed; form fields will use defaults
            }

            return obj.ToString();
        }

        private static string MapChangesToIncludeForForm(string? stored)
        {
            return stored?.Trim() switch
            {
                "newonly" => "1529",
                "newandmodified" => "1530",
                _ => ""
            };
        }

        /// <summary>
        /// Adds username and organisation/laboratory scope parameters for the home dashboard Recently Used query (server-side only).
        /// </summary>
        private static void AppendHomeDashboardTokenParameters(QueryFilterConfig parameters, TokenInfoModel token)
        {
            if (parameters == null)
            {
                return;
            }

            parameters.Parameters ??= new List<QueryValuesConfig>();
            if (token == null)
            {
                return;
            }

            if (!parameters.Parameters.Any(p => string.Equals(p.Key, "dashboardusername", StringComparison.OrdinalIgnoreCase)))
            {
                parameters.Parameters.Add(new QueryValuesConfig { Key = "dashboardusername", Value = token.Username ?? "" });
            }

            if (!parameters.Parameters.Any(p => string.Equals(p.Key, "allowedlaboratories", StringComparison.OrdinalIgnoreCase)))
            {
                var labs = !string.IsNullOrEmpty(token.AllowedLaboratories) ? token.AllowedLaboratories : token.LaboratoryId;
                if (!string.IsNullOrEmpty(labs))
                {
                    parameters.Parameters.Add(new QueryValuesConfig { Key = "allowedlaboratories", Value = labs });
                }
            }

            if (!parameters.Parameters.Any(p => string.Equals(p.Key, "allowedorganisations", StringComparison.OrdinalIgnoreCase)))
            {
                var orgs = !string.IsNullOrEmpty(token.AllowedOrganisations) ? token.AllowedOrganisations : token.OrganisationId;
                if (!string.IsNullOrEmpty(orgs))
                {
                    parameters.Parameters.Add(new QueryValuesConfig { Key = "allowedorganisations", Value = orgs });
                }
            }
        }
    }
}
