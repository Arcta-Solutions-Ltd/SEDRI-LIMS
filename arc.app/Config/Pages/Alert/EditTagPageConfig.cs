using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditTagPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'edittagpage',
                            pageTitle: '@AleEdiA@',
                            text: '@AleEdiB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@GenTagD@' },
                                                { id: 'ParentTagId', type: 'hierarchicalpicker', label: '@GenTagH@', placeholder: '@GenTagI@', optionsName: 'Tag', dynamic: true },
                                                { id: 'Enabled', type: 'toggle', label: '@GenEna@', required: true, value: 'Yes', defaultValue: 'Yes' }
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
