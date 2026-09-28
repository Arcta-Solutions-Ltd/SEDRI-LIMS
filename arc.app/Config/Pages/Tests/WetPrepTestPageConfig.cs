using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class WetPrepTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'wetpreptestpage',
                            pageTitle: '@TesWet@',
                            text: '@TesEntE@.',
                            extrawide: false,
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
                                                { id: 'wbcwetprep', type: 'combobox', label: '@TesWbcB@', optionsName: 'pluses'},
                                                { id: 'rbcwetprep', type: 'combobox', label: '@TesRbcB@', optionsName: 'pluses'},
                                                { id: 'parasitegrid', type: 'fieldgrid', label: '@TesPar@', gridfields: [
                                                        { id: 'parasite', type: 'dropdown', optionsName: 'parasite'  },
                                                        { id: 'parasitetype', type: 'dropdown', optionsName: 'parasitetype', width: 'medium' },
                                                        { id: 'foundparasite', type: 'dropdown', optionsName: 'found', width: 'small' }
                                                    ]
                                                },
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
