using arc.app.Common;

namespace arc.app.Config.Pages;
/// <summary>
/// Represents the configuration for the Batch Accept Results page.
/// </summary>
internal class BatchAcceptResultsPageConfig : IDefinition
{
    /// <summary>
    /// Builds and returns a JSON string defining the Batch Accept Results page configuration.
    /// </summary>
    /// <returns>A JSON-formatted string describing the page configuration.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'batchacceptresultspage',              
                        pageTitle: '@InsBatD@',                     
                        text: '@InsBatH@.',                         
                        columns: [                                  
                            {
                                key: 'col1',                        
                                formGroups: [                       
                                    {
                                        key: 'fg1',                 
                                        fields: [                   
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}

