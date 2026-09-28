using arc.app.Common;
using arc.app.Config.UIEvents.Config;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Factory class for creating UI event configurations based on definition names.
/// </summary>
internal class ConfigUIEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of a UI event configuration based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the definition for which a configuration is required.</param>
    /// <returns>
    /// An instance of <see cref="IDefinition"/> representing the UI event configuration, 
    /// or null if the definition name does not match any predefined configurations.
    /// </returns>
    /// <remarks>
    /// This factory uses a switch expression to map definition names to their respective 
    /// configuration classes. The names are matched in a case-insensitive manner.
    /// Example:
    /// - "addworkflowuievent" maps to <see cref="AddWorkflowUIEventConfig"/>.
    /// - "editformuievent" maps to <see cref="EditFormUIEventConfig"/>.
    /// </remarks>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addculturetestuievent" => new AddCultureTestUIEventConfig(),
            "addculturetypeculturetestuievent" => new AddCultureTypeCultureTestUIEventConfig(),
            "adddirecttestuievent" => new AddDirectTestUIEventConfig(),
            "addexistingfielduievent" => new AddExistingFieldUIEventConfig(),
            "addfielduievent" => new AddFieldUIEventConfig(),
            "addfieldgridcolumnuievent" => new AddFieldGridColumnUIEventConfig(),
            "addformgroupuievent" => new AddFormGroupUIEventConfig(),
            "addformuievent" => new AddFormUIEventConfig(),
            "addmappinguievent" => new AddMappingUIEventConfig(),
            "addmenuoptionuievent" => new AddMenuOptionUIEventConfig(),
            "addpageuievent" => new AddPageUIEventConfig(),
            "addreportconfiguievent" => new AddReportConfigUIEventConfig(),
            "addsectionuievent" => new AddSectionUIEventConfig(),
            "addspecimentypeculturetypeuievent" => new AddSpecimenTypeCultureTypeUIEventConfig(),
            "addspecimentypedirecttestuievent" => new AddSpecimenTypeDirectTestUIEventConfig(),
            "addstateuievent" => new AddStateUIEventConfig(),
            "addworkflowuievent" => new AddWorkflowUIEventConfig(),
            "addworkflowentryuievent" => new AddWorkflowEntryUIEventConfig(),
            "deleteculturetestuievent" => new DeleteCultureTestUIEventConfig(),
            "deleteculturetypeculturetestuievent" => new DeleteCultureTypeCultureTestUIEventConfig(),
            "deletedirecttestuievent" => new DeleteDirectTestUIEventConfig(),
            "deletefielduievent" => new DeleteFieldUIEventConfig(),
            "deleteformgroupuievent" => new DeleteFormGroupUIEventConfig(),
            "deleteformuievent" => new DeleteFormUIEventConfig(),
            "deletemappinguievent" => new DeleteMappingUIEventConfig(),
            "deletemenuoptionuievent" => new DeleteMenuOptionUIEventConfig(),
            "deletepageuievent" => new DeletePageUIEventConfig(),
            "deletereportconfiguievent" => new DeleteReportConfigUIEventConfig(),
            "deletesectionuievent" => new DeleteSectionUIEventConfig(),
            "deletespecimentypeculturetypeuievent" => new DeleteSpecimenTypeCultureTypeUIEventConfig(),
            "deletespecimentypedirecttestuievent" => new DeleteSpecimenTypeDirectTestUIEventConfig(),
            "deletestateuievent" => new DeleteStateUIEventConfig(),
            "deleteworkflowuievent" => new DeleteWorkflowUIEventConfig(),
            "deleteworkflowentryuievent" => new DeleteWorkflowEntryUIEventConfig(),
            "disableculturetestuievent" => new DisableCultureTestUIEventConfig(),
            "disabledirecttestuievent" => new DisableDirectTestUIEventConfig(),
            "editculturetestuievent" => new EditCultureTestUIEventConfig(),
            "editculturetypeculturetestuievent" => new EditCultureTypeCultureTestUIEventConfig(),
            "editdirecttestuievent" => new EditDirectTestUIEventConfig(),
            "editfielduievent" => new EditFieldUIEventConfig(),
            "editfieldgridcolumnuievent" => new EditFieldGridColumnUIEventConfig(),
            "editformgroupuievent" => new EditFormGroupUIEventConfig(),
            "movefielduievent" => new MoveFieldUIEventConfig(),
            "moveformgroupuievent" => new MoveFormGroupUIEventConfig(),
            "editformuievent" => new EditFormUIEventConfig(),
            "editmappinguievent" => new EditMappingUIEventConfig(),
            "editmenuoptionuievent" => new EditMenuOptionUIEventConfig(),
            "editpageuievent" => new EditPagesUIEventConfig(),
            "editpagesuievent" => new EditPageUIEventConfig(),
            "editpagerulesuievent" => new EditPageRulesUIEventConfig(),
            "editreportconfiguievent" => new EditReportConfigUIEventConfig(),
            "editreportsectionuievent" => new EditReportSectionUIEventConfig(),
            "editsectionuievent" => new EditSectionUIEventConfig(),
            "editspecimentypeculturetypeuievent" => new EditSpecimenTypeCultureTypeUIEventConfig(),
            "editspecimentypedirecttestuievent" => new EditSpecimenTypeDirectTestUIEventConfig(),
            "editstateuievent" => new EditStateUIEventConfig(),
            "editworkflowuievent" => new EditWorkflowUIEventConfig(),
            "editworkflowentryuievent" => new EditWorkflowEntryUIEventConfig(),
            "exportconfigurationuievent" => new ExportConfigurationUIEventConfig(),
            "importconfigurationuievent" => new ImportConfigurationUIEventConfig(),
            "pageconfiguievent" => new PageConfigUIEventConfig(),
            "reorderformgroupsuievent" => new ReorderFormGroupsUIEventConfig(),
            "reportconfiguievent" => new ReportConfigUIEventConfig(),
            "viewconfiguievent" => new ViewConfigUIEventConfig(),
            "viewworkflowuievent" => new ViewWorkflowUIEventConfig(),
            _ => null,
        };
    }
}
