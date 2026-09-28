using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Defines the configuration for the "Delete Specimen Type Workflow" form.
/// </summary>
internal class DeleteSpecimenTypeWorkflowFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the form configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the form name, title, associated event, and pages.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'deletespecimentypeworkflowform',
                        title: 'Delete an existing specimen type workflow form.',
                        saveEvent: 'deletespecimentypeworkflowevent',
                        suppressRecordView: true,
                        initialQuery: 'editspecimentypeworkflowquery',
                        pages: ['deletespecimentypeworkflowpage']
                    }";

        return form;
    }
}
