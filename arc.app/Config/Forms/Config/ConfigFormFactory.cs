using arc.app.Common;
using arc.app.Config.Forms.Config;

namespace arc.app.Config.Forms;

/// <summary>
/// Factory class for creating configuration form instances based on the definition name.
/// </summary>
internal class ConfigFormFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of a configuration form based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the configuration form to create.</param>
    /// <returns>
    /// An instance of <see cref="IDefinition"/> corresponding to the specified definition name, 
    /// or <c>null</c> if no matching definition is found.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addculturetestform" => new AddCultureTestFormConfig(),
            "addculturetypeculturetestform" => new AddCultureTypeCultureTestFormConfig(),
            "adddirecttestform" => new AddDirectTestFormConfig(),
            "addexistingfieldform" => new AddExistingFieldFormConfig(),
            "addfieldform" => new AddFieldFormConfig(),
            "addfieldgridcolumnform" => new AddFieldGridColumnFormConfig(),
            "addformgroupform" => new AddFormGroupFormConfig(),
            "addformform" => new AddFormFormConfig(),
            "addmappingform" => new AddMappingFormConfig(),
            "addmenuoptionform" => new AddMenuOptionFormConfig(),
            "addreportconfigform" => new AddReportConfigFormConfig(),
            "addpageform" => new AddPageFormConfig(),
            "addsectionform" => new AddSectionFormConfig(),
            "addspecimentypeculturetypeform" => new AddSpecimenTypeCultureTypeFormConfig(),
            "addspecimentypedirecttestform" => new AddSpecimenTypeDirectTestFormConfig(),
            "addstateform" => new AddStateFormConfig(),
            "addworkflowform" => new AddWorkflowFormConfig(),
            "addworkflowentryform" => new AddWorkflowEntryFormConfig(),
            "deleteculturetestform" => new DeleteCultureTestFormConfig(),
            "deleteculturetypeculturetestform" => new DeleteCultureTypeCultureTestFormConfig(),
            "deletedirecttestform" => new DeleteDirectTestFormConfig(),
            "deletefieldform" => new DeleteFieldFormConfig(),
            "deleteformgroupform" => new DeleteFormGroupFormConfig(),
            "deleteformform" => new DeleteFormFormConfig(),
            "deletemappingform" => new DeleteMappingFormConfig(),
            "deletemenuoption" => new DeleteMenuOptionFormConfig(),
            "deletepageform" => new DeletePageFormConfig(),
            "deletereportconfigform" => new DeleteReportConfigFormConfig(),
            "deletesectionform" => new DeleteSectionFormConfig(),
            "deletespecimentypeculturetypeform" => new DeleteSpecimenTypeCultureTypeFormConfig(),
            "deletespecimentypedirecttestform" => new DeleteSpecimenTypeDirectTestFormConfig(),
            "deletestateform" => new DeleteStateFormConfig(),
            "deleteworkflowform" => new DeleteWorkflowFormConfig(),
            "deleteworkflowentryform" => new DeleteWorkflowEntryFormConfig(),
            "disableculturetestform" => new DisableCultureTestFormConfig(),
            "disabledirecttestform" => new DisableDirectTestFormConfig(),
            "editculturetestform" => new EditCultureTestFormConfig(),
            "editculturetypeculturetestform" => new EditCultureTypeCultureTestFormConfig(),
            "editdirecttestform" => new EditDirectTestFormConfig(),
            "editfieldform" => new EditFieldFormConfig(),
            "editfieldgridcolumnform" => new EditFieldGridColumnFormConfig(),
            "editformgroupform" => new EditFormGroupFormConfig(),
            "movefieldform" => new MoveFieldFormConfig(),
            "moveformgroupform" => new MoveFormGroupFormConfig(),
            "editformform" => new EditFormFormConfig(),
            "editmappingform" => new EditMappingFormConfig(),
            "editmenuoptionform" => new EditMenuOptionFormConfig(),
            "editpageform" => new EditPageFormConfig(),
            "editpagesform" => new EditPagesFormConfig(),
            "editpagerulesform" => new EditPageRulesFormConfig(),
            "editreportconfigform" => new EditReportConfigFormConfig(),
            "editreportsectionform" => new EditReportSectionFormConfig(),
            "editsectionform" => new EditSectionFormConfig(),
            "editspecimentypeculturetypeform" => new EditSpecimenTypeCultureTypeFormConfig(),
            "editspecimentypedirecttestform" => new EditSpecimenTypeDirectTestFormConfig(),
            "editstateform" => new EditStateFormConfig(),
            "editworkflowform" => new EditWorkflowFormConfig(),
            "editworkflowentryform" => new EditWorkflowEntryFormConfig(),
            "exportconfigurationform" => new ExportConfigurationFormConfig(),
            "reorderformgroupsform" => new ReorderFormGroupsFormConfig(),
            "importconfigurationform" => new ImportConfigurationFormConfig(),
            _ => null,
        };
    }
}
