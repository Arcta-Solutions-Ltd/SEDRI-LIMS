using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules
{
    internal class DeleteExpertRulePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deleteexpertrulepage',
                            pageTitle: '@RulDel@',
                            text: '@RulManB@.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'ExpertRuleName', type: 'text', label: '@GenRul@'},
                                                { id: 'RuleText', type: 'multitext', label: '@GenRulD@'}
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
}
