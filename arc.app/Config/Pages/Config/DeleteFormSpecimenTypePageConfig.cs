using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Delete Form Specimen Type" page.
/// </summary>
internal class DeleteFormSpecimenTypePageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Form Specimen Type" page.
    /// </summary>
    /// <returns>
    /// A string representation of the page configuration, detailing the confirmation field and delete button.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'deleteformspecimentypepage',
                            pageTitle: '@ConFormSpeDel@',
                            text: '@ConFormSpeDelA@.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'GroupDescription', type: 'text', label: '@ConFormNam@' }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";

        return page;
    }
}
