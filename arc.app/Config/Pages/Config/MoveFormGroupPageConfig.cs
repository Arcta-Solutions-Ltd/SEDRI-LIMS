using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Page definition for the Move Subsection form.
    /// </summary>
    internal class MoveFormGroupPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'moveformgrouppage',
                            pageTitle: '@ConMovFGGrp@',
                            text: '@ConMovFGGrpText@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'SubsectionLabel', type: 'text', label: '@ConFG@' },
                                                { id: 'TargetPageId', type: 'dropdown', label: '@ConMovFGPag@', required: true, placeholder: '@GenSelP@', optionsName: 'pageOptions' }
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
