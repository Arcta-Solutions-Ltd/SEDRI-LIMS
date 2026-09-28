using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Edit Workflow" page.
/// </summary>
internal class EditWorkflowPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Workflow" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the structure and layout of the "Edit Workflow" page.
    /// It includes metadata such as the page's name, title, and descriptive text.
    /// The page is divided into columns, with each column containing form groups that house specific fields.
    /// Fields include a text field for editing the workflow's name and a single-line field for editing its description.
    /// Both fields are marked as required to ensure proper input from users.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing its structure, 
    /// columns, form groups, and fields.
    /// </returns>
    public string Get()
    {
        var page = @"{  
                        name: 'editworkflowpage',
                        pageTitle: '@ConEdiAC@',
                        text: '@ConEdiAD@',
                        columns: [
                            { 
                                key: 'col1',
                                formGroups: [
                                    { 
                                        key: 'fg1',
                                        fields: [
                                            { id: 'Name', type: 'text', label: '@GenNam@', required: true },
                                            { id: 'Description', type: 'singleline', label: '@GenDes@', required: true }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
