using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteLanguagePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deletelanguagepage',
                            pageTitle: '@LanDel@',
                            text: '@LanDelA@.',
                            required: 'Name',
                            requiredRule: 'and',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'TranslationId', type: 'dropdown', label: '@LanTraB@', required: true, placeholder: '@LanSel@', optionsName: 'Translation', dynamic: true, removeFixed: true }
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
