using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Specimen Type Culture Type" page.
/// </summary>
internal class SpecimenTypeCultureTypePageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Specimen Type Culture Type" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the structure and layout of the page for managing specimen type culture types.
    /// It includes metadata such as the page title, descriptive text, columns, form groups, fields, and additional attributes like placeholders and options for comboboxes.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing elements such as 
    /// form groups, fields, and their associated properties.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'specimentypeculturetypepage',
                            pageTitle: '@ConCulA@',
                            text: '@ConAddF@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'GroupId', type: 'combobox', label: '@SpeSpeB@', required: true, multiselect: false, placeholder: '@SpeSelC@', optionsName: 'SpecimenType', tab: true },
                                                { id: 'AssociatedListId', type: 'combobox', label: '@CulTyp@', required: true, multiselect: true, placeholder: '@CulSel@', optionsName: 'CultureType', tab: true },
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}
