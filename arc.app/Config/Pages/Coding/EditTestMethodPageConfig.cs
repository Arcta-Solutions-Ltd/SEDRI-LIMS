using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditTestMethodPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'edittestmethodpage',
                            pageTitle: '@BreEdiB@',
                            text: '@BreEdiD@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'testmethodid', type: 'dropdown', label: '@BreTes@', optionsName: 'testmethod', dynamic: true},
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
