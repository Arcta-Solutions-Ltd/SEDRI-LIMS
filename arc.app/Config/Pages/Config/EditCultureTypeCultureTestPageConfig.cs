using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Edit Culture Type Culture Test" page.
/// </summary>
internal class EditCultureTypeCultureTestPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Culture Type Culture Test" page.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the layout and structure of the page used for editing culture type culture test settings.
    /// It includes metadata such as the page title, descriptive text, columns, form groups, and fields.
    /// Field attributes include validation requirements, placeholders, and dropdown options.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing elements such as 
    /// columns, form groups, fields, and their associated properties.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'editculturetypeculturetestpage',
                            pageTitle: '@LabCulD@',
                            text: '@LabAD@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'GroupDescription', type: 'text', label: '@CulTyp@', required: false, multiselect: false, placeholder: '@CulTyp@', optionsName: 'culturetype', tab: true },
                                                { id: 'AssociatedListId', type: 'combobox', label: '@SpeCulC@', required: true, multiselect: true, placeholder: '@SpeCulC@', optionsName: 'culturetestconfiglist', tab: true },
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}
