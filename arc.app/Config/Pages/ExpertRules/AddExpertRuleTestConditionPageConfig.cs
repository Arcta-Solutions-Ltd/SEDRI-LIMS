using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules;
internal class AddExpertRuleTestConditionPageConfig : IDefinition

{
    public string Get()
    {
        var page = @"{
                            name: 'addexpertruletestconditionpage',
                            pageTitle: '@GenRulS@',
                            text: '@GenRulT@.',
                            wider: true,
                            columns: [
                                {
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'TestGrid', type: 'crafted', label: '@RulTes@', gridfields: [
                                                        { id: 'Test', type: 'dropdown', optionsName: 'testconfiglist'  },
                                                        { id: 'Field', type: 'dropdown', optionsName: 'fieldlist', dynamic: true },
                                                        { id: 'Comparison', type: 'dropdown', optionsName: 'comparison', width: 'small'  },
                                                        { id: 'StringValue', type: 'singleline', width: 'medium', Max: 20 },
                                                        { id: 'NumberValue', type: 'number', width: 'medium' },
                                                        { id: 'ListValue', type: 'dropdown', width: 'medium' }
                                                    ], RemoveGridAddButton: true
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}
