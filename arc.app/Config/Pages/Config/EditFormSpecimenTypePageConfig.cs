using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Edit Form Specimen Type" page.
/// </summary>
internal class EditFormSpecimenTypePageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Form Specimen Type" page.
    /// </summary>
    /// <remarks>
    /// The form itself is shown read only because changing it would move the restriction to a different form
    /// rather than edit this one; only the specimen types can be changed.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing the form group and its fields.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'editformspecimentypepage',
                            pageTitle: '@ConFormSpeEdi@',
                            text: '@ConFormSpeA@.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'GroupDescription', type: 'text', label: '@ConFormNam@' },
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
