using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditAlertCategoryPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'editalertcategorypage',
                            pageTitle: '@AleEdiC@',
                            text: '@AleEdiD@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@GenLisA@', Max: 100 },
                                                { id: 'Alertcategoryid', type: 'combobox', label: '@GenLev@', required: true, optionsName: 'AlertType' },
                                                { id: 'Positionid', type: 'combobox', label: '@GenPos@', required: true, optionsName: 'AlertPosition' },
                                                { id: 'Colour', type: 'colourpicker', label: '@GenColA@', required: true },
                                                { id: 'Reportpositionid', type: 'combobox', label: '@AleDis@', required: true, optionsName: 'AlertPosition' }
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
