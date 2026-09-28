using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Delete Culture Type Culture Test" page.
/// </summary>
internal class DeleteCultureTypeCultureTestPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Culture Type Culture Test" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the layout and structure of the page for deleting culture type culture test settings.
    /// It includes metadata such as the page title, descriptive text, columns, and form groups. 
    /// Additionally, it configures the visibility and text for the "next" button.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing elements such as 
    /// columns, form groups, fields, and their associated properties.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'deleteculturetypeculturetestpage',
                            pageTitle: '@ConDelAA@',
                            text: '@ConDelAB@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'GroupDescription', type: 'text', label: '@CulTyp@' }
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
