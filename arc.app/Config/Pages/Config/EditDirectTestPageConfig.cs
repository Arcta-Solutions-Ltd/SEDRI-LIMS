using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Page configuration for editing a direct test definition title from Configuration &gt; Views.
    /// </summary>
    internal class EditDirectTestPageConfig : IDefinition
    {
        /// <summary>
        /// Returns the page configuration for editing a direct test definition title.
        /// </summary>
        /// <returns>A JSON string containing the edit direct test definition page layout.</returns>
        public string Get()
        {
            var page = @"{  
                            name: 'editdirecttestpage',
                            pageTitle: '@ConEdiDT@',
                            text: '@ConEdiDTA@',
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
