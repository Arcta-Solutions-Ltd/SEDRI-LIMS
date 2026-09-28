using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddCustomPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addcustompage',
                            pageTitle: '@CodAddA@',
                            text: '@CodAddB@.',
                            crafted: true,
                            columns: [
                                {
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Description', type: 'singleline', label: '@GenEntB@', required: true, placeholder: '@CodNew@', Max: 50 },
                                                { id: 'Code', type: 'singleline', label: '@GenCodA@', required: false, placeholder: '@GenCodA@', Max: 10 }
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
