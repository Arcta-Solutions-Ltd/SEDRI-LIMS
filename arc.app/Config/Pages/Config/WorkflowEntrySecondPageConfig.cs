using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class WorkflowEntrySecondPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'workflowentrysecondpage',
                            pageTitle: '@ConAddT@',
                            text: '@ConAddU@.',
                            crafted: false,
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'StatePassthroughGrid', type: 'fieldgrid', label: '@ConSta@', gridfields: [
                                                        { id: 'ExitState', type: 'dropdown', optionsName: 'specimenworkflowitems', GridTitle: '@ConExi@' },
                                                        { id: 'EntryState', type: 'dropdown', optionsName: 'specimenworkflowitems', GridTitle: '@ConEntC@' }
                                                    ]
                                                },
                                                { id: 'Action', type: 'combobox', label: '@GenActA@', required: false, placeholder: '@ConSelE@', optionsName: 'actionlist' }
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
