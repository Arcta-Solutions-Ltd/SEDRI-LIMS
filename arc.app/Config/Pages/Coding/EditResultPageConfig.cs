using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditResultPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'editresultpage',
                            pageTitle: '@BreEdiF@',
                            text: '@BreEdiG@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'resultid', type: 'dropdown', label: '@GenSus@', optionsName: 'testresult', dynamic: true},
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@GenLisA@', Max: 100 }
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
