using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules;

/// <summary>
/// Configuration for the "Add Expert Rule Approval" page.
/// Contains the CodingStatus dropdown (defaulting to Approved) and hidden ExpertRuleId field.
/// </summary>
internal class AddExpertRuleApprovalPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Expert Rule Approval" page.
    /// </summary>
    public string Get()
    {
        var page = @"{
            name: 'addexpertruleapprovalpage',
            pageTitle: '@BreAddApp@',
            text: '@BreAddAppB@',
            columns: [
                {
                    key: 'col1',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                                { id: 'ExpertRuleId', type: 'hidden' },
                                { id: 'CodingStatusId', type: 'dropdown', label: '@GenStaA@', required: true, optionsName: 'CodingApprovalStatus', defaultValue: '145' }
                            ]
                        }
                    ]
                }
            ]
        }";

        return page;
    }
}
