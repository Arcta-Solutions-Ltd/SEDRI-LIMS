using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Defines the configuration for the "Edit Specimen Type Workflow" form.
/// </summary>
internal class EditSpecimenTypeWorkflowFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the form configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the form name, title, associated event, and pages.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'editspecimentypeworkflowform',
                        title: 'Edit an existing specimen type workflow form.',
                        saveEvent: 'editspecimentypeworkflowevent',
                        suppressRecordView: true,
                        initialQuery: 'editspecimentypeworkflowquery',
                        pages: ['editspecimentypeworkflowpage']
                    }";

        return form;
    }
}
