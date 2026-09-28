using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules
{
    internal class ExpertRuleDetailsSecondPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'expertruledetailssecondpage',
                            pageTitle: '@GenRulJ@',
                            text: '@GenRulK@.',
                            wider: true,
                            columns: [
                            {
                                key: 'col1',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { Id: 'RuleConditionGrid', Type: 'fieldgrid', Label: '@GenRulI@', Icon: 'Edit', IconText: '@GenComG@', Embedded: true, GridFields: [
                                            { Id: 'Id', Type: 'hidden'},
                                            { Id: 'ConditionId', Type: 'text', Width: 'extrawide', FieldFormat: '@AntibioticId@|@antibioticgroupid@|@SusceptibilityId@|@TestMethodId@' }
                                            ], RemoveGridAddButton: false, RemoveGridDeleteButton: false, IncludeGridFormButton: true, AddFormUIEvent: 'addexpertruleconditionuievent', FormUIEvent: 'editexpertruleconditionuievent', onFinish: 'updategrid'},
                                            { Id: 'RuleTestConditionGrid', type: 'crafted', label: '@RulTes@', gridfields: [
                                                        { id: 'Test', type: 'dropdown', optionsName: 'culturetestconfiglist'  },
                                                        { id: 'Field', type: 'dropdown', optionsName: 'fieldlist', dynamic: true },
                                                        { id: 'Comparison', type: 'dropdown', optionsName: 'comparison', width: 'small'  },
                                                        { id: 'StringValue', type: 'singleline', width: 'medium', Max: 20 },
                                                        { id: 'NumberValue', type: 'number', width: 'medium' },
                                                        { id: 'ListValue', type: 'dropdown', width: 'medium' }
                                                    ]
                                            },                                         
                                            { Id: 'CombinationRule', type: 'dropdown', optionsName: 'andor', label: '@GenRulP@', width: 'narrow'  },
                                            { Id: 'RuleActionGrid', Type: 'fieldgrid', Label: '@GenRulF@', Icon: 'Edit', IconText: '@GenComG@', Embedded: true, GridFields: [
                                            { Id: 'Id', Type: 'hidden'},
                                            { Id: 'ActionId', Type: 'text', Width: 'extrawide', FieldFormat: '@antibioticid@|@antibioticgroupid@|@SusceptibilityId@|@DisplayOnReport@' }
                                            ], RemoveGridAddButton: false, RemoveGridDeleteButton: false, IncludeGridFormButton: true, AddFormUIEvent: 'addexpertruleactionuievent', FormUIEvent: 'editexpertruleactionuievent', onFinish: 'updategrid'},
                                            { Id: 'TagId', type: 'combobox', label: '@GenTag@', optionsName: 'tag', required: false, multiselect: true, dynamic: true }
                                        ]
                                    }
                                ]
                            }
                          ]
                        }";
            return page;
        }
    }
}
