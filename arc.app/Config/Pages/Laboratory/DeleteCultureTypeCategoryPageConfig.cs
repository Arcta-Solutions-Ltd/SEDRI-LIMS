using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Defines the configuration for the "Delete Culture Type Category" page.
/// </summary>
internal class DeleteCultureTypeCategoryPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the page structure, fields, and navigation settings.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'deleteculturetypecategorypage',
                        pageTitle: '@LabDelG@',
                        text: '@LabDelH@',
                        columns: [
                            {
                                key: 'col1',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'GroupDescription', type: 'text', label: '@GenCat@' }
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

