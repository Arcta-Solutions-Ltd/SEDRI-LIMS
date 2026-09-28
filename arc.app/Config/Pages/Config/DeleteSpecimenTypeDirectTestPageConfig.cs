using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Delete Specimen Type Direct Test" page.
/// </summary>
internal class DeleteSpecimenTypeDirectTestPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Specimen Type Direct Test" page.
    /// </summary>
    /// <remarks>
    /// This configuration includes the page structure for deleting a specimen type direct test. 
    /// It defines the name, page title, columns, form groups, fields, and a next button for user actions.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing layout, fields, and navigation options.
    /// </returns>
    public string Get()
    {
        var page = @"{
                        name: 'deletespecimentypedirecttestpage',
                        pageTitle: '@ConDelE@',
                        text: '@ConDelG@',
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
