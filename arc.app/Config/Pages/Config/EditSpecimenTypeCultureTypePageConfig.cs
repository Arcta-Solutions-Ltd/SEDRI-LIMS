using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Edit Specimen Type Culture Type" page.
/// </summary>
internal class EditSpecimenTypeCultureTypePageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Specimen Type Culture Type" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the structure and layout of the page used for editing specimen type culture types.
    /// It includes metadata such as the page title, descriptive text, columns, and form groups. 
    /// Additionally, the fields specify attributes like placeholders, options, multi-select, and required validation.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing elements such as 
    /// form groups, fields, and their associated properties.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'editspecimentypeculturetypepage',
                            pageTitle: '@ConCulA@',
                            text: '@ConAddF@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'GroupDescription', type: 'text', label: '@SpeSpeB@', multiselect: false, placeholder: '@SpeSelC@', optionsName: 'SpecimenType', tab: true },
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
