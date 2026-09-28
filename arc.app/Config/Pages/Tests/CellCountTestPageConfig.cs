using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class CellCountTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'cellcounttestpage',
                            pageTitle: '@TesCel@',
                            text: '@TesEnt@.',
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
                                                { id: 'ccwbc', type: 'number', label: '@TesWbc@', Min: '0', Max: '1000', MaxDPs: '2', Suffix: 'x10^6/L' },
                                                { id: 'ccrbc', type: 'number', label: '@TesRbc@', Min: '0', Max: '1000', MaxDPs: '2', Suffix: 'x10^6/L' },
                                                { id: 'wbcqualitative', type: 'combobox', label: '@TesWbcA@', optionsName: 'qualitative', tab: true },
                                                { id: 'rbcqualitative', type: 'combobox', label: '@TesRbcA@', optionsName: 'qualitative', tab: true },
                                                { id: 'polymorphonuclear', type: 'number', label: '@TesPol@', Min: '0', Max: '100', MaxDPs: '0', Suffix: '%' },
                                                { id: 'mononuclear', type: 'number', label: '@TesMon@', Min: '0', Max: '100', MaxDPs: '0', Suffix: '%' },
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
