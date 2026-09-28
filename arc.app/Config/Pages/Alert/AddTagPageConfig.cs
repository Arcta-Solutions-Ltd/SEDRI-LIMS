using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddTagPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addtagpage',
                            pageTitle: '@AleAddB@',
                            text: '@AleB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@GenTagC@', Max: 200 },
                                                { id: 'ParentTagId', type: 'hierarchicalpicker', label: '@GenTagH@', placeholder: '@GenTagI@', optionsName: 'Tag', dynamic: true }
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
