using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SpecimenCommentPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'specimencomment',
                            pageTitle: '@GenAddC@',
                            text: '@SpeAddH@.',
                            required: 'CannedComment,Comment',
                            requiredRule: 'or',
                            requiredError: '@ValOrA@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        { key: 'fg1',
                                            fields: [                                       
                                                { id: 'CommentType', type: 'combobox', label: '@GenComF@', required: true, multiselect: false, placeholder: '@GenComL@', optionsName: 'CommentType'}                                                
                                                ]                                                
                                        },
                                        { key: 'fg2',
                                            rules:[{ effect: 'visible', field: 'CommentType', rule: 'isnotempty'}, { effect: 'visible', field: 'Comment', rule: 'isempty'}],
                                            fields: [                                        
                                                    { id: 'CannedComment', type: 'combobox', label: '@GenComM@', placeholder: '@GenEntA@', optionsName: 'CannedComments', parentList: 'CommentType'},
                                            ]
                                        },
                                        { key: 'fg3',
                                            rules:[{ effect: 'visible', field: 'CommentType', rule: 'isnotempty'}, { effect: 'visible', field: 'CannedComment', rule: 'isempty'}],
                                            fields: [                                        
                                                { id: 'Comment', type: 'multiline', label: '@GenComT@', placeholder: '@GenEntA@'},
                                            ]
                                        },
                                        { key: 'fg4',                                     
                                            fields: [                                        
                                                { Id: 'PrintOnReport', Type: 'toggle', Width: 'small', defaultValue: 'Yes', label: '@GenDis@' }
                                            ]
                                        },
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}
