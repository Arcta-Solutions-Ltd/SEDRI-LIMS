using arc.app.Common;
using arc.app.Config.Language;
using arc.app.Configuration.Queries;
using arc.app.Laboratory;
using arc.app.Language;
using arc.app.Settings;
using System;

namespace arc.app.Configuration;

/// <summary>
/// Factory class for creating configuration instances based on the provided configuration name.
/// </summary>
public class ConfigFactory : IConfigFactory
{
    private readonly IHandleQuery _queryHandler;
    private readonly ILanguageFactory _languageFactory;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigFactory"/> class.
    /// </summary>
    /// <param name="queryHandler">The query handler for executing queries.</param>
    /// <param name="languageFactory">The language factory for creating language-related configurations.</param>
    /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
    /// <param name="configExtractionUtils">Utilities for extracting configuration data.</param>
    /// <param name="listRepository">The list repository for managing list-based configurations.</param>
    public ConfigFactory(
        IHandleQuery queryHandler,
        ILanguageFactory languageFactory,
        IServiceProvider serviceProvider)
    {
        _queryHandler = queryHandler;
        _languageFactory = languageFactory;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Creates a configuration instance based on the specified configuration name.
    /// </summary>
    /// <param name="name">The name of the configuration to create.</param>
    /// <returns>
    /// An instance of <see cref="ISingleConfig"/> corresponding to the specified name,
    /// or <c>null</c> if no matching configuration is found.
    /// </returns>
    public ISingleConfig Create(string name)
    {
        return name.ToLower() switch
        {
            "accessionnumberquery" => new AccessionNumberQuery(_serviceProvider),
            "deletedirecttestdefaultquery" => new GetLaboratoryConfigByIdQuery(_serviceProvider),
            "culturetestconfiglistquery" => new GetFormConfigList(_serviceProvider, "culturetest"),
            "culturetypecategorisationlistquery" => new GetRuleListQuery("culturetypecategory", _serviceProvider),
            "culturetypeculturetestmappinglistquery" => new GetRuleListQuery("culturetypeculturetestdefault", _serviceProvider),
            "culturetypeculturetestoptionlistquery" => new GetRuleListQuery("culturetypeculturetestoption", _serviceProvider),
            "editorganismscopeculturetestoptionquery" => new GetOrganismScopeCultureTestOptionByIdQuery(_serviceProvider),
            "organismscopeculturetestoptionlistquery" => new GetRuleListQuery("organismscopeculturetestoption", _serviceProvider),
            //"deleteculturetypeculturetestdefaultquery" => new DeleteCultureTypeCultureTestDefaultQuery(_serviceProvider, _configExtractionUtils),
            "deletefieldquery" => new GetDeleteFieldQuery(_serviceProvider),
            "deletemappingquery" => new GetDeleteMappingQuery(_serviceProvider),
            "deletereportconfigquery" => new GetDeleteReportConfigQuery(_serviceProvider),
            "deletesectionquery" => new GetDeleteSectionQuery(_serviceProvider),
            "deletesettingquery" => new DeleteSettingQuery(_serviceProvider),
            "deleteworkflowentryquery" => new GetDeleteWorkflowEntryQuery(_serviceProvider),
            "directtestconfiglistquery" => new GetFormConfigList(_serviceProvider, "directtest"),
            "editaccessionnumberquery" => new EditAccessionNumberQuery(_serviceProvider),
            "editpatientreferencequery" => new EditPatientReferenceQuery(_serviceProvider),
            //"editculturetypedefaultquery" => new GetEditCultureTypeDefault(_configExtractionUtils, _listRepository),
            //"editculturetypeculturetestdefaultquery" => new GetEditCultureTypeCultureTestDefault(_configExtractionUtils, _listRepository),
            "editdirecttestdefaultquery" => new GetLaboratoryConfigByIdQuery(_serviceProvider),
            "editfieldquery" => new GetEditFieldQuery(_serviceProvider),
            "editmappingquery" => new GetEditMappingQuery(_serviceProvider),
            "editpagequery" => new GetEditPageQuery(_serviceProvider),
            "addpagequery" => new GetAddPageQuery(_serviceProvider),
            "editpagesquery" => new GetEditPagesQuery(_serviceProvider),
            "editexportprofilefieldsbyexportprofileid" => new GetEditPagesQuery(_serviceProvider),
            "editreportconfigquery" => new GetEditReportConfigQuery(_serviceProvider),
            "editreportsectionquery" => new GetEditReportSectionQuery(_serviceProvider),
            "editsectionquery" => new GetEditSectionQuery(_serviceProvider),
            "editsettingquery" => new EditSettingQuery(_serviceProvider),
            "editspecimentypeworkflowquery" => new EditSpecimenTypeWorkflowQuery(_serviceProvider),
            "edittestquery" => new GetEditTestQuery(_serviceProvider),
            "editworkflowentryquery" => new GetEditWorkflowEntryQuery(_serviceProvider),
            "existingfieldlistquery" => new GetExistingFieldListQuery(_serviceProvider),
            "fieldsforeventquery" => new GetFieldsForAnEventQuery(_serviceProvider),
            "fieldparentlinkquery" => new GetFieldParentLinkQuery(_serviceProvider),
            "fieldselectionlistquery" => new GetFieldSelectionListQuery(_serviceProvider),
            "formfieldoptionsquery" => new GetFormFieldOptionsQuery(_serviceProvider),
            "formgroupquery" => new GetFormGroupQuery(_serviceProvider),
            "movefieldquery" => new GetMoveFieldQuery(_serviceProvider),
            "moveformgroupquery" => new GetMoveFormGroupQuery(_serviceProvider),
            "formgroupsforpagequery" => new GetFormGroupsForPageQuery(_serviceProvider),
            "formconfiglistquery" => new GetFormConfigList(_serviceProvider, "form"),
            "generalsettingsquery" => new GeneralSettingsQuery(_serviceProvider),
            "languagelist" => new GetSingleLanguage(_queryHandler, _languageFactory),
            "menuitemconfiglistquery" => new GetMenuOptions(_serviceProvider),
            "ordertablequery" => new GetOrderTableQuery(_serviceProvider),
            "pagerulesquery" => new GetPageRulesQuery(_serviceProvider),
            "pagesinformquery" => new GetPagesInForm(_serviceProvider),
            "patientreferencequery" => new PatientReferenceQuery(_serviceProvider),
            "reportdefinitionquery" => new GetReportDefinition(_serviceProvider),
            "reportconfiglistquery" => new GetReportConfigList(_serviceProvider),
            "specimentypedirecttestmappinglistquery" => new GetRuleListQuery("specimentypedirecttestdefault", _serviceProvider),
            "specimentypedirecttestoptionlistquery" => new GetRuleListQuery("specimentypedirecttestoption", _serviceProvider),
            "specimentypeculturetypemappinglistquery" => new GetRuleListQuery("specimentypeculturetypedefault", _serviceProvider),
            "specimentypeculturetypeoptionlistquery" => new GetRuleListQuery("specimentypeculturetypeoption", _serviceProvider),
            "formspecimentypeoptionlistquery" => new GetRuleListQuery("formspecimentypeoption", _serviceProvider),
            "editformspecimentypeoptionquery" => new GetFormSpecimenTypeOptionByIdQuery(_serviceProvider),
            "specimentypeworkflowlistquery" => new GetRuleListQuery("specimentypeworkflow", _serviceProvider),
            "testcategorylistquery" => new GetRuleListQuery("testcategory", _serviceProvider),
            "workflowsteplistquery" => new GetWorkflowSteps(_serviceProvider),
            _ => null,
        };
    }
}
