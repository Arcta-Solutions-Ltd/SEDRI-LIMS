using arc.app.Common;

namespace arc.app.Config.Pages.Specimen;

/// <summary>
/// Page configuration for deleting an isolate.
/// Shows read-only isolate details and provides a confirm delete action.
/// </summary>
internal class DeleteIsolatePageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the Delete Isolate page.
    /// Includes a summary of isolate fields and a visible Next (confirm) button.
    /// </summary>
    public string Get()
    {
        var page = @"{
                            name: 'deleteisolatepage',
                            pageTitle: '@SpeDelA@',
                            text: '@SpeFulA@.',
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
                                                { id: 'Growth', type: 'text', label: '@SpeGroC@' },
                                                { id: 'SpecimenQuantity', type: 'text', label: '@GenQua@' },
                                                { id: 'SpecimenOrganism', type: 'text', label: '@GenOrgA@' },
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
