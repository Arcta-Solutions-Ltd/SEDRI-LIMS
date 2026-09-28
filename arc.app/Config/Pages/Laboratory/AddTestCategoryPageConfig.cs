using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Add Test Category" page.
/// </summary>
internal class AddTestCategoryPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Test Category" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the layout and structure for the page where users can add
    /// test categories. It includes page metadata, column definitions, form groups, and fields 
    /// necessary for user interaction.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing its elements such as fields 
    /// for specifying test category and direct test configurations.
    /// </returns>
    public string Get()
    {
        var page = @"{
                        name: 'addtestcategorypage',
                        pageTitle: '@LabAddB@',
                        text: '@LabAddC@',
                        columns: [
                            {
                                key: 'col1',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'GroupId', type: 'combobox', label: '@GenCat@', required: true, multiselect: false, placeholder: '@GenCat@', optionsName: 'TestCategory', tab: true },
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

