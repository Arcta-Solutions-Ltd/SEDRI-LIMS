using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class WorkflowEntryPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'workflowentrypage',
                            pageTitle: 'Workflow Entry Page',
                            text: 'Add or edit the details for a workflow',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'name', type: 'singleline', label: 'Name'},
                                                { id: 'description', type: 'singleline', label: 'Description'},
                                                { id: 'view', type: 'dropdown', label: 'View', optionsName: 'views', dataload: true},
                                                { id: 'entryconditions', type: 'fieldgrid', label: 'Entry Conditions', gridfields: [
                                                        { id: 'field', type: 'dropdown', optionsName: 'viewFieldList', dataload: true },
                                                        { id: 'dropdownvalue', type: 'dropdown', optionsName: 'dropdownvalue' },
                                                        { id: 'textvalue', type: 'singleline'}
                                                    ]
                                                },
                                                { id: 'states', type: 'fieldgrid', label: '@ConSta@', gridfields: [
                                                        { id: 'textvalue', type: 'singleline'}
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
