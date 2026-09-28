using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules;

/// <summary>
/// Page configuration for batch rejecting expert rules from the list view.
/// </summary>
internal class BatchRejectExpertRulePageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch reject expert rule page.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'batchrejectexpertrulepage',
            pageTitle: '@RulBatE@',
            text: '@RulBatF@',
            columns: [
                {
                    key: 'col1',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                                { id: 'ExpertRuleId', type: 'hidden' },
                                { id: 'CodingStatusId', type: 'hidden', defaultValue: '146' }
                            ]
                        }
                    ]
                }
            ],
            nextButton: { show: true, buttonText: '@GenRej@' }
        }";
    }
}
