using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules;

/// <summary>
/// Page configuration for the delete expert rule action confirmation form.
/// </summary>
internal class DeleteExpertRuleActionPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON page configuration for deleting an expert rule action.
    /// </summary>
    /// <returns>JSON string defining the page structure.</returns>
    public string Get()
    {
        var page = @"{
                            name: 'deleteexpertruleactionpage',
                            pageTitle: '@GenDelC@',
                            text: 'Delete an expert rule action.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AntibioticName', type: 'text', label: '@GenAnt@' },
                                                { id: 'SusceptibilityName', type: 'text', label: '@GenSus@' },
                                                { id: 'DisplayOnReport', type: 'text', label: '@GenInc@' }
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
