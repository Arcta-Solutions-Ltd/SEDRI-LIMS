using arc.app.Common;

namespace arc.app.Config.Pages;
/// <summary>
/// Configuration for the "Delete Test Category" page.
/// </summary>
internal class DeleteTestCategoryPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Test Category" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the structure and layout of the page used for deleting test categories.
    /// It includes page metadata, column definitions, form groups, fields, and a next button for proceeding with the action.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, including metadata such as 
    /// page title, columns, fields, and the next button properties.
    /// </returns>
    public string Get()
    {
        var page = @"{
                        name: 'deletetestcategorypage',
                        pageTitle: '@LabDelD@',
                        text: '@LabDelE@',
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
