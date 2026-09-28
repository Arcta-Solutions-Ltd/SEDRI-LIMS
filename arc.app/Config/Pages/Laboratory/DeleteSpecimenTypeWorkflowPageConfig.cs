using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Defines the configuration for the "Delete Specimen Type Workflow" page.
/// </summary>
internal class DeleteSpecimenTypeWorkflowPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the page name, title, description, and form structure.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'deletespecimentypeworkflowpage',
                        pageTitle: '@LabDelM@',
                        text: '@LabDelN@',
                        columns: [
                            {
                                key: 'col1',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'GroupDescription', type: 'text', label: '@GenWorA@' }
                                        ]
                                    }
                                ]
                            }
                        ],
                        nextButton: { show: true, buttonText: '@GenDelC@' }
                    }";

        return page;
    }
}
