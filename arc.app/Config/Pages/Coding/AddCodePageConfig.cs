using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddCodePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addcodepage',
                            pageTitle: '@CodAss@',
                            text: '@CodAssA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Code', type: 'singleline', label: '@GenCodA@', required: true, Max: 10}
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


