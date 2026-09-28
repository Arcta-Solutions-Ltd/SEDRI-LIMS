using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Form Specimen Type" page.
/// </summary>
internal class FormSpecimenTypePageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Form Specimen Type" page.
    /// </summary>
    /// <remarks>
    /// GroupId holds the request form name rather than a list item id, which is why its combobox is fed by
    /// requestformlist instead of a plain table maintenance list.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing the form group and its fields.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'formspecimentypepage',
                            pageTitle: '@ConFormSpeAdd@',
                            text: '@ConFormSpeA@.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'GroupId', type: 'combobox', label: '@ConFormNam@', required: true, multiselect: false, placeholder: '@GenSelQ@', optionsName: 'requestformlist', tab: true },
                                                { id: 'AssociatedListId', type: 'combobox', label: '@SpeSpeB@', required: true, multiselect: true, placeholder: '@SpeSelC@', optionsName: 'SpecimenType', tab: true }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}
