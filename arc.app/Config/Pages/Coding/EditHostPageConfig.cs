using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditHostPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'edithostpage',
                            pageTitle: '@BreEdiC@',
                            text: '@BreEdiE@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'hostid', type: 'dropdown', label: '@GenHos@', optionsName: 'host', dynamic: true},
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
