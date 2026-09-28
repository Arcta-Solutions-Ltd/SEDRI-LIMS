using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Delete Workflow" page.
/// </summary>
internal class DeleteWorkflowPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Workflow" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the layout and structure of the "Delete Workflow" page. 
    /// It includes metadata such as the page's name, title, and descriptive text. The page 
    /// is organized into columns, each containing form groups with fields. Fields include:
    /// - A text field for displaying or inputting the workflow's name.
    /// - A single-line field for providing the workflow's description.
    /// Both fields are marked as required to ensure proper input is provided when deleting a workflow.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing its structure, columns, 
    /// form groups, and fields.
    /// </returns>
    public string Get()
    {
        var page = @"{  
                        name: 'deleteworkflowpage',
                        pageTitle: '@ConDelAD@',
                        text: '@ConDelAE@',
                        columns: [
                            { 
                                key: 'col1',
                                formGroups: [
                                    { 
                                        key: 'fg1',
                                        fields: [
                                            { id: 'Name', type: 'text', label: '@GenNam@' },
                                            { id: 'Description', type: 'text', label: '@GenDes@' }
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
