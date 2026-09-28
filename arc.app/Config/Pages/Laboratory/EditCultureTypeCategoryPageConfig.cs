using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Defines the configuration for the "Edit Culture Type Category" page.
/// </summary>
internal class EditCultureTypeCategoryPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the page structure, fields, and form groups.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'editculturetypecategorypage',
                        pageTitle: '@LabEdiG@',
                        text: '@LabEdiH@',
                        columns: [
                            {
                                key: 'col1',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'GroupDescription', type: 'text', label: '@GenCat@', placeholder: '@GenCat@', optionsName: 'CultureTypeCategory', tab: true },
                                            { id: 'AssociatedListId', type: 'combobox', label: '@LabCulC@', required: true, multiselect: true, placeholder: '@CulSelA@', optionsName: 'culturetype', tab: true }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
