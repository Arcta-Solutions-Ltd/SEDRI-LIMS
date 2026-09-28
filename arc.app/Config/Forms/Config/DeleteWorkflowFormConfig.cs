using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Delete Workflow" form.
/// </summary>
internal class DeleteWorkflowFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Workflow" form.
    /// </summary>
    /// <remarks>
    /// This configuration defines the structure and behavior of the "Delete Workflow" form.
    /// It includes metadata such as the form's name, view title, save event, and associated pages.
    /// Additionally, it suppresses the record view to streamline the workflow deletion process.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, detailing attributes such as 
    /// the save event and the page involved in the workflow deletion process.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'deleteworkflowform',
                        viewTitle: 'Delete workflow.',
                        saveEvent: 'deleteworkflowevent',
                        suppressRecordView: true,
                        pages: [ 'deleteworkflowpage']
                    }";

        return form;
    }
}

