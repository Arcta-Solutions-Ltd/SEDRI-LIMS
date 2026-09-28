using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Configuration class for the results confirmation page.
    /// </summary>
    internal class ResultsConfirmationPageConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the results confirmation page.
        /// </summary>
        /// <returns>A JSON string that represents the page configuration.</returns>
        public string Get()
        {
            var page = @"{
                        name: 'resultsconfirmation',
                        pageTitle: '@InsAcc@',       
                        text: '@InsAccA@.',         
                        columns: [                  
                            {
                                key: 'col1',         
                                formGroups: [        
                                    {
                                        key: 'fg1',  
                                        fields: [    
                                            { id: 'AccessionNumber', type: 'text', label: '@SpeAcc@' }
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
