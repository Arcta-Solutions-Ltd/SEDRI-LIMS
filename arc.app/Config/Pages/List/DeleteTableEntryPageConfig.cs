using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteTableEntryPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deletetableentrypage',
                            pageTitle: '@TabDel@',
                            text: '@TabDelA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Value', type: 'text', label: '@GenVal@' },
                                                { id: 'Parent', type: 'text', label: '@GenPar@'}
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
