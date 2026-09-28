using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddSourcePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addsourcepage',
                            pageTitle: '@CodAddG@',
                            text: '@CodAddH@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@CodSou@', Max: 100 }
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
