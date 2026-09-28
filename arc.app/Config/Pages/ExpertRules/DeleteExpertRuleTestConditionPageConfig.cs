using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules;

/// <summary>
/// Page configuration for the delete expert rule test condition confirmation form.
/// </summary>
internal class DeleteExpertRuleTestConditionPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON page configuration for deleting an expert rule test condition.
    /// </summary>
    /// <returns>JSON string defining the page structure.</returns>
    public string Get()
    {
        var page = @"{
                            name: 'deleteexpertruletestconditionpage',
                            pageTitle: '@GenDelC@',
                            text: 'Delete an expert rule test condition.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'TestName', type: 'text', label: '@GenRulW@' },
                                                { id: 'FieldName', type: 'text', label: '@GenFieA@' },
                                                { id: 'Comparison', type: 'text', label: '@AleCom@' },
                                                { id: 'CompValue', type: 'text', label: '@GenVal@' }
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
