using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddLanguagePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addlanguagepage',
                            pageTitle: '@LanAdd@',
                            text: '@LanCreA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'SourceId', type: 'dropdown', label: '@LanTraB@', required: true, placeholder: '@LanSel@', optionsName: 'Translation', dynamic: true },
                                                { id: 'Name', type: 'singleline', label: '@LanTraA@', required: true, placeholder: '@LanEnt@' }
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
