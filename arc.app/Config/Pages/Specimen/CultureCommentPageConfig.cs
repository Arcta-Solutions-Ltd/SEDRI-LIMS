using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the JSON configuration for the culture comment page, including page metadata,
/// validation rules, and dynamic visibility for comment fields based on user selection.
/// </summary>
internal class CultureCommentPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the culture comment page.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string containing page name, title, descriptive text, required field rules,
    /// and column/form-group/field structures with conditional visibility logic.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'culturecommentpage',
                            pageTitle: '@GenAddC@',
                            text: '@SpeAddL@.',
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
