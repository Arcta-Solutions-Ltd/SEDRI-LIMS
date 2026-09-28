using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Edit Test Category" page.
/// </summary>
internal class EditTestCategoryPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Test Category" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the structure and layout of the page for editing test categories.
    /// It includes page metadata, column definitions, form groups, and fields for user interaction.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing its elements such as 
    /// form fields and associated metadata.
    /// </returns>
    public string Get()
    {
        var page = @"{
                        name: 'edittestcategorypage',
                        pageTitle: '@LabEdiD@',
                        text: '@LabEdiE@',
                        columns: [
                            {
                                key: 'col1',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'GroupDescription', type: 'text', label: '@GenCat@', placeholder: '@GenCat@', optionsName: 'TestCategory', tab: true },
                                            { id: 'AssociatedListId', type: 'combobox', label: '@ConDirA@', required: true, multiselect: true, placeholder: '@CulSelA@', optionsName: 'directtestconfiglist', tab: true }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
