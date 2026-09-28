using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditCustomPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'editcustompage',
                            pageTitle: '@CodEdi@',
                            text: '@CodEdiA@.',
                            crafted: false,
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Description', type: 'text', label: '@GenEntB@', required: true, placeholder: '@CodNew@', Max: 50 },
                                                { id: 'Code', type: 'singleline', label: '@GenCodA@', required: true, placeholder: '@GenCodA@', Max: 10 }
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
