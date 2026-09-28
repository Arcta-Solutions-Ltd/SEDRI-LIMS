using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Page configuration for deleting a culture.
/// Displays read-only culture details and a confirm delete action.
/// </summary>
internal class DeleteCulturePageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the Delete Culture page.
    /// Includes a summary of culture fields and a visible Next (confirm) button.
    /// </summary>
    public string Get()
    {
        var page = @"{
                            name: 'deleteculturepage',
                            pageTitle: '@SpeDelB@',
                            text: '@SpeFulB@.',
                            columns: [
                                {
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Type', type: 'text', label: '@CulTyp@' },
                                                { id: 'Growth', type: 'text', label: '@SpeGroC@' }
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
