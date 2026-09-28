using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Specimen Type Direct Test" page.
/// </summary>
internal class SpecimenTypeDirectTestPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Specimen Type Direct Test" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the layout and structure for managing specimen type 
    /// and direct test mappings. It includes page metadata, column definitions, form groups, 
    /// and fields for user interaction.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing its elements and structure.
    /// </returns>
    public string Get()
    {
        var page = @"{
                        name: 'specimentypedirecttestpage',
                        pageTitle: '@ConDirB@',
                        text: '@ConAddV@',
                        columns: [
                            {
                                key: 'col1',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'GroupId', type: 'combobox', label: '@SpeSpeB@', required: true, multiselect: false, placeholder: '@SpeSelC@', optionsName: 'SpecimenType', tab: true },
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