using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteTablePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deletetablepage',
                            pageTitle: '@TabDelD@',
                            text: '@TabDelE@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'ListId', type: 'dropdown', label: '@GenNam@', required: true, placeholder: '@TabTabA@', optionsName: 'customtablelist', dynamic: true, removeFixed: true }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";

            return page;
        }
    }
}
