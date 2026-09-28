using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Provides the page configuration definition for the culture print selector page.
    /// This page contains field grids for culture tests, AST results, and culture comments,
    /// with add and delete buttons removed from the grids.
    /// </summary>
    internal class CulturePrintSelectorPageConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON string representation of the culture print selector page configuration.
        /// </summary>
        /// <returns>A JSON string containing the page configuration with field grids for culture tests, AST results, and culture comments.</returns>
        public string Get()
        {
            var page = @"{
                            name: 'cultureprintselectorpage',
                            pageTitle: '@RepCulC@',
                            text: '@RepCulD@.',
                            extrawide: false,
                            columns: [
                                {
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'CultureTestGrid', type: 'fieldgrid', label: '@RepCulB@', RemoveGridAddButton: true, RemoveGridDeleteButton: true, IncludeFirstLine: false, gridfields: [
                                                        { Id: 'Id', Type: 'hidden' },
                                                        { Id: 'CultureTest', Type: 'text', Width: 'extrawide' },
                                                        { Id: 'PrintOnReport', Type: 'toggle', Width: 'small' }
                                                    ]
                                                },
                                                { id: 'AstGrid', type: 'fieldgrid', label: '@RepAstA@', RemoveGridAddButton: true, RemoveGridDeleteButton: true, IncludeFirstLine: false, gridfields: [
                                                        { Id: 'Id', Type: 'hidden' },
                                                        { Id: 'SpecialConsiderationId', Type: 'hidden' },
                                                        { Id: 'AstTest', Type: 'text', Width: 'extrawide' },
                                                        { Id: 'PrintOnReport', Type: 'toggle', Width: 'small' }
                                                    ]
                                                },
                                                { id: 'CultureCommentGrid', type: 'fieldgrid', label: '@RepComA@', RemoveGridAddButton: true, RemoveGridDeleteButton: true, IncludeFirstLine: false, gridfields: [
                                                        { Id: 'Id', Type: 'hidden' },
                                                        { Id: 'Comment', Type: 'text', Width: 'extrawide' },
                                                        { Id: 'PrintOnReport', Type: 'toggle', Width: 'small' }
                                                    ]
                                                },
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
