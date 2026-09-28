using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Defines the configuration for the "Add Specimen Type Workflow" form.
/// </summary>
internal class AddSpecimenTypeWorkflowFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the form configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the form name, title, associated event, and pages.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'addspecimentypeworkflowform',
                        title: 'Add new specimen type workflow form.',
                        saveEvent: 'addspecimentypeworkflowevent',
                        suppressRecordView: true,
                        pages: ['addspecimentypeworkflowpage']
                    }";

        return form;
    }
}
