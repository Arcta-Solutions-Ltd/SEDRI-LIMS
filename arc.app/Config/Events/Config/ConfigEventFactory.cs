using arc.app.Common;
using arc.app.Config.Events.Config;

namespace arc.app.Config.Events;

/// <summary>
/// Factory class for creating event configuration instances based on the definition name.
/// </summary>
internal class ConfigEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of an event configuration based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the event configuration to create.</param>
    /// <returns>
    /// An instance of <see cref="IDefinition"/> corresponding to the specified definition name,
    /// or <c>null</c> if no matching definition is found.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addculturetest" => new AddCultureTestEventConfig(),
            "addculturetypeculturetest" => new AddCultureTypeCultureTestEventConfig(),
            "adddirecttest" => new AddDirectTestEventConfig(),
            "addexistingfield" => new AddExistingFieldEventConfig(),
            "addfield" => new AddFieldEventConfig(),
            "addfieldgridcolumn" => new AddFieldGridColumnEventConfig(),
            "addform" => new AddFormEventConfig(),
            "addformgroup" => new AddFormGroupEventConfig(),
            "addmappingevent" => new AddMappingEventConfig(),
            "addmenuoption" => new AddMenuOptionEventConfig(),
            "addpage" => new AddPageEventConfig(),
            "addreportconfig" => new AddReportConfigEventConfig(),
            "addsection" => new AddSectionEventConfig(),
            "addspecimentypeculturetype" => new AddSpecimenTypeCultureTypeEventConfig(),
            "addspecimentypedirecttest" => new AddSpecimenTypeDirectTestEventConfig(),
            "addstate" => new AddStateEventConfig(),
            "addworkflowevent" => new AddWorkflowEventConfig(),
            "addworkflowentry" => new AddWorkflowEntryEventConfig(),
            "deleteculturetestconfig" => new DeleteCultureTestConfigEventConfig(),
            "deleteculturetypeculturetest" => new DeleteCultureTypeCultureTestEventConfig(),
            "deletedirecttest" => new DeleteDirectTestEventConfig(),
            "deletefield" => new DeleteFieldEventConfig(),
            "deleteform" => new DeleteFormEventConfig(),
            "deleteformgroup" => new DeleteFormGroupEventConfig(),
            "deletemappingevent" => new DeleteMappingEventConfig(),
            "deletemenuoption" => new DeleteMenuOptionEventConfig(),
            "deletepage" => new DeletePageEventConfig(),
            "deletereportconfig" => new DeleteReportConfigEventConfig(),
            "deletesection" => new DeleteSectionEventConfig(),
            "deletestate" => new DeleteStateEventConfig(),
            "deletespecimentypeculturetype" => new DeleteSpecimenTypeCultureTypeEventConfig(),
            "deletespecimentypedirecttest" => new DeleteSpecimenTypeDirectTestEventConfig(),
            "deleteworkflowevent" => new DeleteWorkflowEventConfig(),
            "deleteworkflowentry" => new DeleteWorkflowEntryEventConfig(),
            "disableculturetest" => new DisableCultureTestEventConfig(),
            "disabledirecttest" => new DisableDirectTestEventConfig(),
            "editculturetest" => new EditCultureTestEventConfig(),
            "editculturetypeculturetest" => new EditCultureTypeCultureTestEventConfig(),
            "editdirecttest" => new EditDirectTestEventConfig(),
            "editfield" => new EditFieldEventConfig(),
            "editfieldgridcolumn" => new EditFieldGridColumnEventConfig(),
            "editform" => new EditFormEventConfig(),
            "editformgroup" => new EditFormGroupEventConfig(),
            "movefield" => new MoveFieldEventConfig(),
            "moveformgroup" => new MoveFormGroupEventConfig(),
            "editmappingevent" => new EditMappingEventConfig(),
            "editmenuoption" => new EditMenuOptionEventConfig(),
            "editpage" => new EditPageEventConfig(),
            "editpages" => new EditPagesEventConfig(),
            "editpagerules" => new EditPageRulesEventConfig(),
            "editreportconfig" => new EditReportConfigEventConfig(),
            "editreportsection" => new EditReportSectionEventConfig(),
            "editsection" => new EditSectionEventConfig(),
            "editspecimentypeculturetype" => new EditSpecimenTypeCultureTypeEventConfig(),
            "editspecimentypedirecttest" => new EditSpecimenTypeDirectTestEventConfig(),
            "editstate" => new EditStateEventConfig(),
            "editworkflowevent" => new EditWorkflowEventConfig(),
            "editworkflowentry" => new EditWorkflowEntryEventConfig(),
            "exportconfiguration" => new ExportConfigurationEventConfig(),
            "importconfiguration" => new ImportConfigurationEventConfig(),
            "reportsectionmove" => new ReportSectionMoveEventConfig(),
            "reorderformgroups" => new ReorderFormGroupsEventConfig(),
            _ => null,
        };
    }
}
