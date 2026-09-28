using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Edit Specimen Type Direct Test" page.
/// </summary>
internal class EditSpecimenTypeDirectTestPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Specimen Type Direct Test" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the layout and structure for editing specimen type direct tests.
    /// It includes details such as page name, title, fields, and columns for user interaction.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, including metadata and layout elements.
    /// </returns>
    public string Get()
    {
        var page = @"{
                        name: 'editspecimentypedirecttestpage',
                        pageTitle: '@ConDirB@',
                        text: '@ConAddV@',
                        columns: [
                            {
                                key: 'col1',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'GroupDescription', type: 'text', label: '@SpeSpeB@', multiselect: false, placeholder: '@SpeSelC@', optionsName: 'SpecimenType', tab: true },
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
