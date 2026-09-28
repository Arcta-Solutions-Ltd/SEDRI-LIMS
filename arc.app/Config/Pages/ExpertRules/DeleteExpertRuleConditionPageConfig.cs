using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules;

/// <summary>
/// Page configuration for the delete expert rule condition confirmation form.
/// </summary>
internal class DeleteExpertRuleConditionPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON page configuration for deleting an expert rule condition.
    /// </summary>
    /// <returns>JSON string defining the page structure.</returns>
    public string Get()
    {
        var page = @"{
                            name: 'deleteexpertruleconditionpage',
                            pageTitle: '@GenDelC@',
                            text: 'Delete an expert rule condition.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AntibioticName', type: 'text', label: '@GenAnt@' },
                                                { id: 'SusceptibilityName', type: 'text', label: '@GenSus@' }
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
