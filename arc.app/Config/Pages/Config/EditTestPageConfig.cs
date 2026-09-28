using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Page configuration for editing an isolate/culture test definition title from Configuration &gt; Views.
    /// Used by <c>editculturetestform</c>.
    /// </summary>
    internal class EditTestPageConfig : IDefinition
    {
        /// <summary>
        /// Returns the page configuration for editing an isolate test definition title.
        /// </summary>
        /// <returns>A JSON string containing the edit isolate test definition page layout.</returns>
        public string Get()
        {
            var page = @"{  
                            name: 'edittestpage',
                            pageTitle: '@ConEdiF@',
                            text: '@ConEdiG@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Title', type: 'singleline', label: '@GenTit@' }
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
