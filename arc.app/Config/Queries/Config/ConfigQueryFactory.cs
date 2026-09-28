using arc.app.Common;
using arc.app.Config.Queries.Config;

namespace arc.app.Config.Queries;

/// <summary>
/// Factory class responsible for creating definition query instances based on a definition name.
/// </summary>
internal class ConfigQueryFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IDefinition"/> based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the definition to create.</param>
    /// <returns>
    /// An instance of <see cref="IDefinition"/> corresponding to the specified definition name, 
    /// or <c>null</c> if no matching definition is found.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "confighistorylistquery" => new ConfigHistoryListQuery(),
            "configlist" => new ConfigListQuery(),
            "culturetestconfiglistquery" => new CultureTestConfigListQuery(),
            "culturetypeculturetestmappinglistquery" => new CultureTypeCultureTestMappingListQuery(),
            "directtestconfiglistquery" => new DirectTestConfigListQuery(),
            "deleteculturetypedefaultquery" => new DeleteCultureTypeDefaultQuery(),
            "deleteculturetypeculturetestdefaultquery" => new DeleteCultureTypeCultureTestDefaultQuery(),
            "deletedirecttestdefaultquery" => new DeleteDirectTestDefaultQuery(),
            "deletefieldquery" => new DeleteFieldQuery(),
            "deletemappingquery" => new DeleteMappingQuery(),
            "deletemappingvalidationquery" => new DeleteMappingValidationQuery(),
            "deletereportconfigquery" => new DeleteReportConfigQuery(),
            "deletesectionquery" => new DeleteSectionQuery(),
            "deleteworkflowentryquery" => new DeleteWorkflowEntryQuery(),
            "editculturetypedefaultquery" => new EditCultureTypeDefaultQuery(),
            "editculturetypeculturetestdefaultquery" => new EditCultureTypeCultureTestDefaultQuery(),
            "editdirecttestdefaultquery" => new EditDirectTestDefaultQuery(),
            "editfieldquery" => new EditFieldQuery(),
            "editmappingquery" => new EditMappingQuery(),
            "editpagequery" => new EditPageQuery(),
            "addpagequery" => new AddPageQuery(),
            "editpagesquery" => new EditPagesQuery(),
            "editreportconfigquery" => new EditReportConfigQuery(),
            "editreportsectionquery" => new EditReportConfigQuery(),
            "editsectionquery" => new EditSectionQuery(),
            "edittestquery" => new EditTestQuery(),
            "existingfieldlistquery" => new ExistingFieldListQuery(),
            "editpatientbarcode" => new BarcodeEditQuery(),
            "editspecimenbarcode" => new BarcodeEditQuery(),
            "editworkflowentryquery" => new EditWorkflowEntryQuery(),
            "fieldsforeventquery" => new FieldsForEventQuery(),
            "fieldparentlinkquery" => new FieldParentLinkQuery(),
            "fieldselectionlistquery" => new FieldSelectionListQuery(),
            "formfieldoptionsquery" => new FormFieldOptionsQuery(),
            "formgroupquery" => new FormGroupQuery(),
            "formgroupsforpagequery" => new FormGroupsForPageQuery(),
            "movefieldquery" => new MoveFieldQuery(),
            "moveformgroupquery" => new MoveFormGroupQuery(),
            "formconfiglistquery" => new FormConfigListQuery(),
            "formlist" => new FormListQuery(),
            "mappinglistquery" => new MappingListQuery(),
            "menuitemconfiglistquery" => new MenuItemConfigListQuery(),
            "pagerulesquery" => new PageRulesQuery(),
            "pagesinformquery" => new PagesInFormQuery(),
            "patientbarcodelist" => new PatientBarcodeListQuery(),
            "reportconfiglistquery" => new ReportConfigListQuery(),
            "reportdefinitionquery" => new ReportDefinitionQuery(),
            "singlespecimentypeculturetypemappinglistquery" => new SingleSpecimenTypeCultureTypeMappingListQuery(),
            "specimenbarcodelist" => new SpecimenBarcodeListQuery(),
            "specimentypeculturetypemappinglistquery" => new SpecimenTypeCultureTypeMappingListQuery(),
            "specimentypedirecttestmappinglistquery" => new SpecimenTypeDirectTestMappingListQuery(),
            "stateexistsquery" => new StateExistsQuery(),
            "stateexistsinqueuequery" => new StateExistsInQueueQuery(),
            "testcategorylistquery" => new TestCategoryListQuery(),
            "testlist" => new TestListQuery(),
            "viewlist" => new ViewListQuery(),
            "workflowlistquery" => new WorkflowListQuery(),
            "workflowsteplistquery" => new WorkflowStepListQuery(),
            _ => null,
        };
    }
}
