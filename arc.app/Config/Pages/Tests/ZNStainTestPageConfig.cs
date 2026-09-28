using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class ZNStainTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'znstaintestpage',
                            pageTitle: '@TesZns@',
                            text: '@TesEntF@.',
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
                                                { id: 'AFBQuantity', type: 'dropdown', label: '@TesAfb@', optionsName: 'afbquantity', required: true},
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
