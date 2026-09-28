using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteCommentPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'deletecommentpage',
                            pageTitle: '@GenComN@',
                            text: '@GenComO@.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        { key: 'fg1',
                                           rules:[{ effect: 'visible', field: 'Comment', rule: 'isnotempty'}],
                                            fields: [
                                                { id: 'Comment', type: 'multitext', label: '@GenComB@'},
                                            ]
                                        },
                                        { key: 'fg2',
                                           rules:[{ effect: 'visible', field: 'CannedComment', rule: 'isnotempty'}],
                                            fields: [
                                                { id: 'CannedComment', type: 'multitext', label: '@GenComB@'},
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
