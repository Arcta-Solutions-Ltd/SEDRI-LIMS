using arc.app.Common;
using arc.app.Config.Pages.Config;

namespace arc.app.Config.Pages;

/// <summary>
/// Factory class for creating page configuration instances based on the definition name.
/// </summary>
internal class ConfigPageFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of a page configuration based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the page configuration to create.</param>
    /// <returns>
    /// An instance of <see cref="IDefinition"/> corresponding to the specified definition name,
    /// or <c>null</c> if no matching definition is found.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addexistingfieldpage" => new AddExistingFieldPageConfig(),
            "addfieldpage" => new AddFieldPageConfig(),
            "addfieldgridcolumnpage" => new AddFieldGridColumnPageConfig(),
            "addformgrouppage" => new AddFormGroupPageConfig(),
            "addmappingpage" => new AddMappingPageConfig(),
            "addpagepage" => new AddPagePageConfig(),
            "addreportconfigpage" => new AddReportConfigPageConfig(),
            "addsectionpage" => new AddSectionPageConfig(),
            "addstatepage" => new AddStatePageConfig(),
            "addworkflowpage" => new AddWorkflowPageConfig(),
            "addworkflowentrypage" => new AddWorkflowEntryPageConfig(),
            "addtestpage" => new AddTestPageConfig(),
            "culturetypeculturetestpage" => new CultureTypeCultureTestPageConfig(),
            "organismscopeculturetestpage" => new OrganismScopeCultureTestPageConfig(),
            "deleteculturetypeculturetestpage" => new DeleteCultureTypeCultureTestPageConfig(),
            "deleteorganismscopeculturetestpage" => new DeleteOrganismScopeCultureTestPageConfig(),
            "deletefieldpage" => new DeleteFieldPageConfig(),
            "deleteformgrouppage" => new DeleteFormGroupPageConfig(),
            "deletemappingpage" => new DeleteMappingPageConfig(),
            "deletepagepage" => new DeletePagePageConfig(),
            "deletereportconfigpage" => new DeleteReportConfigPageConfig(),
            "deletesectionpage" => new DeleteSectionPageConfig(),
            "deletetestpage" => new DeleteTestPageConfig(),
            "deleteworkflowpage" => new DeleteWorkflowPageConfig(),
            "disabletestpage" => new DisableTestPageConfig(),
            "editculturetypeculturetestpage" => new EditCultureTypeCultureTestPageConfig(),
            "editorganismscopeculturetestpage" => new EditOrganismScopeCultureTestPageConfig(),
            "editfieldpage" => new EditFieldPageConfig(),
            "editfieldgridcolumnpage" => new EditFieldGridColumnPageConfig(),
            "editformgrouppage" => new EditFormGroupPageConfig(),
            "movefieldpage" => new MoveFieldPageConfig(),
            "moveformgrouppage" => new MoveFormGroupPageConfig(),
            "editmappingpage" => new EditMappingPageConfig(),
            "editpagepage" => new EditPagePageConfig(),
            "editpagespage" => new EditPagesPageConfig(),
            "editpagerulespage" => new EditPageRulesPageConfig(),
            "editreportconfigpage" => new EditReportConfigPageConfig(),
            "editsectionpage" => new EditSectionPageConfig(),
            "editformspecimentypepage" => new EditFormSpecimenTypePageConfig(),
            "editspecimentypeculturetypepage" => new EditSpecimenTypeCultureTypePageConfig(),
            "editspecimentypedirecttestpage" => new EditSpecimenTypeDirectTestPageConfig(),
            "editstatepage" => new EditStatePageConfig(),
            "editdirecttestpage" => new EditDirectTestPageConfig(),
            "edittestpage" => new EditTestPageConfig(),
            "editworkflowpage" => new EditWorkflowPageConfig(),
            "editworkflowentrypage" => new EditWorkflowEntryPageConfig(),
            "exportconfigurationpage" => new ExportConfigurationPageConfig(),
            "deleteformspecimentypepage" => new DeleteFormSpecimenTypePageConfig(),
            "deletespecimentypeculturetypepage" => new DeleteSpecimenTypeCultureTypePageConfig(),
            "deletespecimentypedirecttestpage" => new DeleteSpecimenTypeDirectTestPageConfig(),
            "deletestatepage" => new DeleteStatePageConfig(),
            "deleteworkflowentrypage" => new DeleteWorkflowEntryPageConfig(),
            "editreportsectionpage" => new EditReportSectionPageConfig(),
            "fieldselectorpage" => new FieldSelectorPageConfig(),
            "importconfigurationpage" => new ImportConfigurationPageConfig(),
            "removeculturetestpage" => new RemoveCultureTestPageConfig(),
            "removedirecttestpage" => new RemoveDirectTestPageConfig(),
            "reorderformgroupspage" => new ReorderFormGroupsPageConfig(),
            "formspecimentypepage" => new FormSpecimenTypePageConfig(),
            "specimentypeculturetypepage" => new SpecimenTypeCultureTypePageConfig(),
            "specimentypedirecttestpage" => new SpecimenTypeDirectTestPageConfig(),
            "workflowentrysecondpage" => new WorkflowEntrySecondPageConfig(),
            _ => null,
        };
    }
}