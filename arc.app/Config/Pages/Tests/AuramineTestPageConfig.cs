using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AuramineTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'auraminetestpage',
                            pageTitle: '@TesAur@',
                            text: '@TesEntH@.',
                            configureActions: 'add,edit',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'auramineId', type: 'dropdown', label: '@TesTesA@', optionsName: 'seen', required: true},
                                                { id: 'printonreport', type: 'toggle', label: '@GenDis@', defaultValue: 'Yes', Configurable: 'No'}
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
