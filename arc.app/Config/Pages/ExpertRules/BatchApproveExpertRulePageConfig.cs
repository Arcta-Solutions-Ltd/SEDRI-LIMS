using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules;

/// <summary>
/// Page configuration for batch approving expert rules from the list view.
/// </summary>
internal class BatchApproveExpertRulePageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch approve expert rule page.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'batchapproveexpertrulepage',
            pageTitle: '@RulBatC@',
            text: '@RulBatD@',
            columns: [
                {
                    key: 'col1',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                                { id: 'ExpertRuleId', type: 'hidden' },
                                { id: 'CodingStatusId', type: 'hidden', defaultValue: '145' }
                            ]
                        }
                    ]
                }
            ],
            nextButton: { show: true, buttonText: '@GenApp@' }
        }";
    }
}
