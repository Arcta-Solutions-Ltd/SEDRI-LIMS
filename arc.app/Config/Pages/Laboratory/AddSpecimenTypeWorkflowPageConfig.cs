using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Defines the configuration for the "Add Specimen Type Workflow" page.
/// </summary>
internal class AddSpecimenTypeWorkflowPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the page name, title, description, and form structure.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'addspecimentypeworkflowpage',
                        pageTitle: '@LabAddK@',
                        text: '@LabAddL@',
                        columns: [
                            {
                                key: 'col1',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'GroupId', type: 'combobox', label: '@GenWorA@', required: true, multiselect: false, placeholder: '@ConSelF@', optionsName: 'WorkflowList', tab: true },
                                            { id: 'AssociatedListId', type: 'combobox', label: '@SpeTyp@', required: true, multiselect: true, placeholder: '@SpeSelC@', optionsName: 'SpecimenType', tab: true }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
