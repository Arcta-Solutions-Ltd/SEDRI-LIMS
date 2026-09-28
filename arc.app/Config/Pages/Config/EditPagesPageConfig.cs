using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Page definition for the Page Order editor. Renders the <c>pageorder</c> field; group
    /// membership and locked anchors come from editpagesquery.
    /// </summary>
    internal class EditPagesPageConfig : IDefinition
    {
        /// <summary>
        /// Gets the JSON string that defines the edit pages page.
        /// </summary>
        /// <returns>A JSON formatted page definition.</returns>
        public string Get()
        {
            var page = @"{  
                            name: 'editpagespage',
                            pageTitle: '@ConPagA@',
                            text: '@ConCha@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'PageOrder', type: 'pageorder', label: '@GenPag@' }
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
