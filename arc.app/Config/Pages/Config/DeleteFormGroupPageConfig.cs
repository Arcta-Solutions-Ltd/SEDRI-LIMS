using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Page definition for deleting a form group.
    /// Shows form group key and confirmation.
    /// </summary>
    internal class DeleteFormGroupPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'deleteformgrouppage',
                            pageTitle: '@ConDelFG@',
                            text: '@ConDelFGText@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Key', type: 'text', label: '@GenKey@' }
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
