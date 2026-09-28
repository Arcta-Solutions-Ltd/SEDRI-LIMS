using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Page definition for the Move to Form Group form.
    /// Displays the field being moved (read-only) and a dropdown to select the target form group.
    /// Target form group may be on a different page.
    /// </summary>
    internal class MoveFieldPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'movefieldpage',
                            pageTitle: '@ConMovFG@',
                            text: '@ConMovFGText@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'FieldLabel', type: 'text', label: '@GenFie@' },
                                                { id: 'TargetPageId', type: 'dropdown', label: '@ConMovFGPag@', required: true, placeholder: '@GenSelP@', optionsName: 'pageOptions' },
                                                { id: 'TargetFormGroupId', type: 'dropdown', label: '@ConMovFG@', required: true, placeholder: '@GenSelP@', optionsName: 'formGroupOptions', parentList: 'TargetPageId' }
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
