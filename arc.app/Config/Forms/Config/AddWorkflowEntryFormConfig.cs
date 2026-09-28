using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Add Workflow Entry" form.
/// </summary>
internal class AddWorkflowEntryFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Workflow Entry" form.
    /// </summary>
    /// <remarks>
    /// This configuration defines the structure and behavior of the "Add Workflow Entry" form.
    /// It includes metadata such as the form's name, view title, save event, and associated pages.
    /// Additionally, it suppresses the record view to focus on the form pages.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, including details such as 
    /// the save event and the list of pages included in the workflow entry process.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'addworkflowentryform',
                        viewTitle: 'Add workflow entry.',
                        saveEvent: 'addworkflowentry',
                        suppressRecordView: true,
                        pages: [ 'addworkflowentrypage', 'workflowentrysecondpage']
                    }";

        return form;
    }
}
