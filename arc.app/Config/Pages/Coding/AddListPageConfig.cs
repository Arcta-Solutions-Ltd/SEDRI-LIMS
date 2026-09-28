using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddListPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addlistpage',
                            pageTitle: '@CodAddE@',
                            text: '@CodAddF@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@GenGroA@', Max: 30 }
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
