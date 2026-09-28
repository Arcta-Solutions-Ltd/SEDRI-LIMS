using arc.app.Config.Views.ListViews;
using arc.app.Configuration;
using arc.app.SystemConfig;
using arc.data.model.Configuration;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace arc.app.Config;

/// <summary>
/// Factory for creating list view configurations.
/// </summary>

public class ListViewConfigFactory : IListViewConfigFactory
{

    /// <summary>
    /// Cache for retrieving configuration records.
    /// </summary>

    private readonly IConfigRepository _configRepository;

    private readonly IConfigCache _configCache;


    /// <summary>
    /// Initializes a new instance of the <see cref="ListViewConfigFactory"/> class.
    /// </summary>
    /// <param name="configRepository">The configuration repository used for retrieving configuration data.</param>
    /// <param name="configCache">The configuration cache for storing and retrieving cached data.</param>
    public ListViewConfigFactory(IConfigRepository configRepository, IConfigCache configCache)

    {

        _configRepository = configRepository;

        _configCache = configCache;
    }


    /// <summary>
    /// Retrieves the view configuration for a specified view name.
    /// </summary>
    /// <param name="viewName">The name of the view to retrieve the configuration for.</param>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object representing the view configuration.
    /// If the view configuration is not available, an internal configuration is returned.
    /// </returns>

    public async Task<ListViewConfig> GetViewAsync(string viewName)

    {
        var configRecord = new ConfigsDataModel();
        if (_configCache.isLoaded())
        {
            configRecord = _configCache.GetConfig(viewName);
        }
        else
        {
            var parameters = new QueryFilterConfig
            {
                Parameters = new List<QueryValuesConfig>
                {
                    new QueryValuesConfig { Key = "ConfigName", Value = viewName }
                }
            };
            configRecord = await _configRepository.SingleConfigByNameAsync(parameters);
        }


        if (configRecord == null || configRecord.Contents == null || configRecord.Contents == "{}")
        {
            return GetInternalView(viewName);
        }

        var view = JsonConvert.DeserializeObject<ListViewConfig>(configRecord.Contents);
        view.Id = configRecord.Id;

        var internalView = GetInternalView(viewName);
        if (internalView?.ReportCategories != null
            && internalView.ReportCategories.Count > 0
            && (view.ReportCategories == null || view.ReportCategories.Count == 0))
        {
            view.ReportCategories = internalView.ReportCategories;
        }

        if (internalView != null)
        {
            if (string.IsNullOrWhiteSpace(view.AllowedHeaders) && !string.IsNullOrWhiteSpace(internalView.AllowedHeaders))
            {
                view.AllowedHeaders = internalView.AllowedHeaders;
            }

            if (string.IsNullOrWhiteSpace(view.AllowedFooters) && !string.IsNullOrWhiteSpace(internalView.AllowedFooters))
            {
                view.AllowedFooters = internalView.AllowedFooters;
            }
        }

        return view;
    }

    /// <summary>
    /// Retrieves an internal configuration based on the specified view name.
    /// </summary>
    /// <param name="viewName">The name of the view to retrieve the internal configuration for.</param>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object representing the internal view configuration.
    /// If the view name is not recognized, <c>null</c> is returned.
    /// </returns>

    private ListViewConfig GetInternalView(string viewName)
    {
        return viewName.ToLower() switch
        {
            "accessionnumberlistview" => AccessionNumberListViewConfig.GetView(),
            "admissionrequestslist" => AdmissionRequestListViewConfig.GetView(),
            "admissionspecimenlist" => AdmissionSpecimenListViewConfig.GetView(),
            "alertapprovals" => AlertApprovalListViewConfig.GetView(),
            "alertsconfig" => new AlertListViewConfig().GetView(),
            "alertsusceptibilitycriteria" => AlertSusceptibilityCriteriaListViewConfig.GetView(),
            "alerttestcriteria" => AlertTestCriteriaListViewConfig.GetView(),
            "alerttype" => new AlertTypeViewConfig().GetView(),
            "antibiotics" => new AntibioticListViewConfig().GetView(),
            "approvedreportview" => ApprovedReportListViewConfig.GetView(),
            "archive" => new ArchiveListViewConfig().GetView(),
            "ast" => ASTListViewConfig.GetView(),
            //            "batch" => BatchViewConfig.GetView(),
            "billingrecord" => BillingRecordViewConfig.GetView(),
            "billingrule" => BillingRuleViewConfig.GetView(),
            "breakpoints" => BreakpointListViewConfig.GetView(),
            "breakpointapprovals" => BreakpointApprovalListViewConfig.GetView(),
            "breakpointlines" => BreakpointLineListViewConfig.GetView(),
            "confighistory" => ConfigHistoryListViewConfig.GetView(),
            "cultures" => CultureListViewConfig.GetView(),
            "culturecomments" => CultureCommentListViewConfig.GetView(),
            "cultureinstresults" => CultureInstResultsListViewConfig.GetView(),
            "culturetestconfig" => CultureTestConfigViewConfig.GetView(),
            "culturetests" => CultureTestViewConfig.GetView(),
            "culturetypecategorisationview" => new CultureTypeCategorisationViewConfig().GetView(),
            "culturetypeculturetest" => CultureTypeCultureTestMappingViewConfig.GetView(),
            "culturetypeculturetestoptionview" => CultureTypeCultureTestOptionViewConfig.GetView(),
            "organismscopeculturetestoptionview" => OrganismScopeCultureTestOptionViewConfig.GetView(),
            "directtestconfig" => DirectTestConfigViewConfig.GetView(),
            "expertrules" => ExpertRuleListViewConfig.GetView(),
            "expertruleconditions" => ExpertRuleConditionListViewConfig.GetView(),
            "expertruletestconditions" => ExpertRuleTestConditionListViewConfig.GetView(),
            "expertruleactions" => ExpertRuleActionListViewConfig.GetView(),
            "expertruleapprovals" => ExpertRuleApprovalListViewConfig.GetView(),
            "exportprofile" => ExportProfileListViewConfig.GetView(),
            "exportprofilefield" => ExportProfileFieldViewConfig.GetView(),
            "exportschedules" => ExportScheduleListViewConfig.GetView(),
            "exporthistory" => ExportHistoryListViewConfig.GetView(),
            "fileimport" => new FileImportViewConfig().GetView(),
            "formconfig" => new FormConfigViewConfig().GetView(),
            "generalsettingslistview" => new GeneralSettingsListViewConfig().GetView(),
            "graphs" => new GraphViewConfig().GetView(),
            "iqctestprofile" => new IqcTestProfileViewConfig().GetView(),
            "iqctests" => new IqcTestsViewConfig().GetView(),
            "instrumenterror" => InstrumentErrorViewConfig.GetView(),
            "instrumentresults" => new InstrumentResultsListViewConfig().GetView(),
            "images" => new ImageListViewConfig().GetView(),
            "instrumentconfig" => new InstrumentConfigListViewConfig().GetView(),
            "laboratories" => new LaboratoryListViewConfig().GetView(),
            "language" => new LanguageListViewConfig().GetView(),
            "locations" => new LocationListViewConfig().GetView(),
            "mappingconfigview" => new MappingListViewConfig().GetView(),
            "menuitemconfig" => new MenuItemViewConfig().GetView(),
            "monitoring" => new MonitoringListViewConfig().GetView(),
            "organisations" => new OrganisationListViewConfig().GetView(),
            "organism" => new OrganismListViewConfig().GetView(),
            "patientbarcodes" => BarcodePatientListViewConfig.GetView(),
            "patientadmissionslist" => PatientAdmissionListViewConfig.GetView(),
            "patientcomments" => new PatientCommentListViewConfig().GetView(),
            "patientrequestslist" => PatientRequestListViewConfig.GetView(),
            "requestspecimenlist" => RequestSpecimenListViewConfig.GetView(),
            "patientreferencelistview" => PatientReferenceListViewConfig.GetView(),
            "patients" => new PatientListViewConfig().GetView(),
            "patientspecimenlist" => new PatientSpecimenListViewConfig().GetView(),
            "qcorganismantimicrobialsdisklist" => new QcOrganismAntimicrobialsDiskListViewConfig().GetView(),
            "qcorganismantimicrobialsmiclist" => new QcOrganismAntimicrobialsMicListViewConfig().GetView(),
            "reportlistconfig" => new ReportListViewConfig().GetView(),
            "roles" => new RoleListViewConfig().GetView(),
            "specifications" => SpecificationListViewConfig.GetView(),
            "specimenbarcodes" => BarcodeSpecimenListViewConfig.GetView(),
            //"specimenbatch" => new SpecimenBatchProcessingListViewConfig().GetView(),
            "specimencomments" => new SpecimenCommentListViewConfig().GetView(),
            "specimeninstresults" => new SpecimenInstResultsListViewConfig().GetView(),
            "testinstresults" => TestInstResultsListViewConfig.GetView(),
            "specimens" => new SpecimenListViewConfig().GetView(),
            "specimentypeculturetype" => new SpecimenTypeCultureTypeMappingViewConfig().GetView(),
            "specimentypeculturetypeoptionview" => new SpecimenTypeCultureTypeOptionViewConfig().GetView(),
            "formspecimentypeoptionview" => new FormSpecimenTypeOptionViewConfig().GetView(),
            "specimentypedirecttest" => new SpecimenTypeDirectTestMappingViewConfig().GetView(),
            "storage" => new StorageListViewConfig().GetView(),
            "suppliers" => new SupplierListViewConfig().GetView(),
            "specimentypedirecttestoptionview" => new SpecimenTypeDirectTestOptionListViewConfig().GetView(),
            "specimentypeworkflowmappingview" => new SpecimenTypeWorkflowMappingViewConfig().GetView(),
            "tagsconfig" => new TagListViewConfig().GetView(),
            "tables" => new TableListViewConfig().GetView(),
            "testcategorisationview" => new TestCategorisationViewConfig().GetView(),
            "testpatterns" => new TestPatternListViewConfig().GetView(),
            "tests" => new TestsViewConfig().GetView(),
            "unapprovedreportview" => UnapprovedReportListViewConfig.GetView(),
            "users" => new UserListViewConfig().GetView(),
            "views" => new ViewListViewConfig().GetView(),
            "workflows" => new WorkflowListViewConfig().GetView(),
            "workflowstep" => new WorkflowStepViewConfig().GetView(),
            _ => null
        };
    }
}
