using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Page definition for configuring page visibility and the workflow state a page sets.
    /// Renders the single PageRules editor; the selectable states, page fields and read only flags
    /// all come from pagerulesquery.
    /// </summary>
    internal class EditPageRulesPageConfig : IDefinition
    {
        /// <summary>
        /// Gets the JSON string that defines the edit page rules page.
        /// </summary>
        /// <returns>A JSON formatted page definition.</returns>
        public string Get()
        {
            var page = @"{
                            name: 'editpagerulespage',
                            pageTitle: '@ConPagVis@',
                            text: '@ConPagVisA@.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'PageRules', type: 'pagerules', label: '@ConPagVis@' }
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
