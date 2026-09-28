using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Add Workflow" form.
/// </summary>
internal class AddWorkflowFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Workflow" form.
    /// </summary>
    /// <remarks>
    /// This configuration defines the structure and behavior of the "Add Workflow" form.
    /// It includes metadata such as the form's name, view title, save event, and associated pages.
    /// Additionally, it suppresses the record view to focus on the workflow creation process.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, detailing attributes such as 
    /// the save event and the pages involved in the workflow creation.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'addworkflowform',
                        viewTitle: 'Add workflow.',
                        saveEvent: 'addworkflowevent',
                        suppressRecordView: true,
                        pages: [ 'addworkflowpage']
                    }";

        return form;
    }
}

