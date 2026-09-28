using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Edit Workflow" form.
/// </summary>
internal class EditWorkflowFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Workflow" form.
    /// </summary>
    /// <remarks>
    /// This configuration defines the structure and behavior of the "Edit Workflow" form.
    /// It includes metadata such as the form's name, view title, save event, and associated pages.
    /// Additionally, it suppresses the record view to focus on the workflow editing process.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, detailing attributes such as 
    /// the save event and the pages involved in the workflow editing process.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'editworkflowform',
                        viewTitle: 'Edit workflow.',
                        saveEvent: 'editworkflowevent',
                        suppressRecordView: true,
                        pages: [ 'editworkflowpage']
                    }";

        return form;
    }
}
