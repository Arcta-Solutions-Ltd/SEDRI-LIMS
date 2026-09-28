using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditCommentPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'editcommentpage',
                            pageTitle: '@GenComP@',
                            text: '@GenComQ@.',
                            required: 'CannedComment,Comment',
                            requiredRule: 'or',
                            requiredError: '@ValOrA@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        { key: 'fg1',
                                            rules:[{ effect: 'visible', field: 'CannedCommentId', rule: 'isnotempty'}],
                                            fields: [                                               
                                                { id: 'CannedComment', type: 'combobox', label: '@GenComM@', required: false, multiselect: false, placeholder: '@GenEntA@', optionsName: 'CannedComments', parentList: 'CommentType'}
                                            ]
                                        },
                                        { key: 'fg2',
                                            rules:[{ effect: 'visible', field: 'CannedCommentId', rule: 'isempty'}],
                                            fields: [
                                                { id: 'Comment', type: 'multiline', label: '@GenComT@'}
                                            ]
                                        },
                                        { key: 'fg3',
                                            fields: [
                                                { Id: 'PrintOnReport', Type: 'toggle', Width: 'small', label: '@GenDis@' }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenSav@' }
                        }";

            return page;
        }
    }
}
