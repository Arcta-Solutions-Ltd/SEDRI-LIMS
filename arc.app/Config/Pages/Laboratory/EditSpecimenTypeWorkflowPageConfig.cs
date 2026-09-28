using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Defines the configuration for the "Edit Specimen Type Workflow" page.
/// </summary>
internal class EditSpecimenTypeWorkflowPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the page name, title, description, and form structure.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'editspecimentypeworkflowpage',
                        pageTitle: '@LabEdiM@',
                        text: '@LabEdiN@',
                        columns: [
                            {
                                key: 'col1',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'GroupDescription', type: 'text', label: '@GenWorA@', optionsName: 'WorkflowList', tab: true },
                                            { id: 'AssociatedListId', type: 'combobox', label: '@SpeTyp@', required: true, multiselect: true, placeholder: '@SpeSelC@', optionsName: 'specimentype', tab: true }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
