using arc.app.Common;

namespace arc.app.Config.Pages;
/// <summary>
/// Defines the configuration for the "Add Culture Type Category" page.
/// </summary>
internal class AddCultureTypeCategoryPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the page structure, fields, and form groups.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'addculturetypecategorypage',
                        pageTitle: '@LabAddE@',
                        text: '@LabAddF@',
                        columns: [
                            {
                                key: 'col1',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'GroupId', type: 'combobox', label: '@GenCat@', required: true, multiselect: false, placeholder: '@GenCat@', optionsName: 'CultureTypeCategory', tab: true },
                                            { id: 'AssociatedListId', type: 'combobox', label: '@LabCulC@', required: true, multiselect: true, placeholder: '@CulSel@', optionsName: 'CultureType', tab: true }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
