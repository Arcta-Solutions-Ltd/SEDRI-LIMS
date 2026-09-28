using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddAlertCategoryPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addalertcategorypage',
                            pageTitle: '@AleAddD@',
                            text: '@AleAddE@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@GenLisA@', Max: 30 },
                                                { id: 'AlertCategoryId', type: 'combobox', label: '@GenLev@', required: true, optionsName: 'AlertType' },
                                                { id: 'PositionId', type: 'combobox', label: '@GenPos@', required: true, optionsName: 'AlertPosition' },
                                                { id: 'Colour', type: 'colourpicker', label: '@GenColA@', required: true },
                                                { id: 'ReportPositionId', type: 'combobox', label: '@AleDis@', required: true, optionsName: 'AlertPosition' }
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
