using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddWorkflowEntryPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addworkflowentrypage',
                            pageTitle: '@ConAddT@',
                            text: '@ConAddU@.',
                            crafted: true,
                            required: 'EntryStates, EventField',
                            requiredRule: 'and',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'EventField', type: 'combobox', label: '@GenEve@', required: true, placeholder: '@GenSelO@', optionsName: 'specimenevent', dynamic: true, translateoptions: true },
                                                { id: 'EntryStates', type: 'combobox', label: '@ConEnt@', required: true, placeholder: '@ConSelE@', optionsName: 'specimenworkflowitems', dynamic: true },
                                                { id: 'DefaultExitStates', type: 'combobox', label: '@ConDefC@', required: false, placeholder: '@ConSelE@', optionsName: 'specimenworkflowitems', dynamic: true }
                                            ]
                                        },
                                        { 
                                            key: 'fg2',
                                            rules:[{ effect: 'visible', field: 'Event', rule: 'isnotempty'}],
                                            fields: [
                                                { id: 'WorkflowRuleGrid', type: 'crafted', label: '@ConCon@', gridfields: [
                                                        { id: 'ExitState', type: 'dropdown', optionsName: 'specimenworkflowitems', GridTitle: '@ConExi@' },
                                                        { id: 'Field', type: 'dropdown', optionsName: 'fieldlist', dynamic: true, GridTitle: '@GenFieA@' },
                                                        { id: 'StringValue', type: 'singleline', width: 'medium' },
                                                        { id: 'NumberValue', type: 'number', width: 'medium' },
                                                        { id: 'ListValue', type: 'dropdown', width: 'medium' }
                                                    ]
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
}
