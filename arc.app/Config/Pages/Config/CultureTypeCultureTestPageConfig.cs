using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Culture Type Culture Test" page.
/// </summary>
internal class CultureTypeCultureTestPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Culture Type Culture Test" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the layout and structure of the page for managing culture type culture test settings.
    /// It includes metadata such as the page title, descriptive text, columns, form groups, and fields. 
    /// Additional field attributes include placeholders, options for comboboxes, and validation requirements.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing elements such as 
    /// columns, form groups, fields, and their associated properties.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'culturetypeculturetestpage',
                            pageTitle: '@LabCulD@',
                            text: '@ConAddAA@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'GroupId', type: 'combobox', label: '@CulTyp@', required: true, multiselect: false, placeholder: '@CulTyp@', optionsName: 'culturetype', tab: true },
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
