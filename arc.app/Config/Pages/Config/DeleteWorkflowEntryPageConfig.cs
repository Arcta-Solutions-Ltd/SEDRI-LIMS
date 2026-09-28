using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteWorkflowEntryPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deleteworkflowentrypage',
                            pageTitle: '@ConEdiV@',
                            text: '@ConEdiV@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'description', type: 'text', label: '@GenEve@' }
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
