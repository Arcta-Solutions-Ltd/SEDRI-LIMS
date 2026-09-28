using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Config.Pages.Specimen
{
    internal class EditCommentForSelectorPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'editcommentforselectorpage',
                            pageTitle: 'Edit Comment',
                            text: 'Edit a comment.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        { key: 'fg1',
                                            rules:[{ effect: 'visible', field: 'CannedComment', rule: 'isnotempty'}],
                                            fields: [                                                
                                                { id: 'CannedComment', type: 'combobox', label: '@GenComM@', required: false, multiselect: false, placeholder: '@GenEntA@', optionsName: 'CannedComments', parentList: 'CommentType'}
                                            ]
                                        },
                                        { key: 'fg2',
                                            rules:[{ effect: 'visible', field: 'Comment', rule: 'isnotempty'}],
                                            fields: [
                                                { id: 'Comment', type: 'multiline', label: '@GenComT@'}
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
