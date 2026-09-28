using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Delete Specimen Type Culture Type" page.
/// </summary>
internal class DeleteSpecimenTypeCultureTypePageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Specimen Type Culture Type" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the structure and layout of the page for deleting specimen type culture types.
    /// It includes metadata such as the page title, descriptive text, columns, form groups, fields, and the next button's visibility and text.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing elements like page metadata, column structure, and button properties.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'deletespecimentypeculturetypepage',
                            pageTitle: '@ConDelD@',
                            text: '@ConDelF@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'GroupDescription', type: 'text', label: '@SpeSpeB@' }
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
