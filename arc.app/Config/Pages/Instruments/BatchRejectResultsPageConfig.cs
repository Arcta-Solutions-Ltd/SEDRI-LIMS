using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Represents the configuration for the Batch Reject Results page.
/// </summary>
internal class BatchRejectResultsPageConfig : IDefinition
{
    /// <summary>
    /// Builds and returns a JSON string defining the Batch Reject Results page configuration.
    /// </summary>
    /// <returns>A JSON-formatted string describing the page configuration.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'batchrejectresultspage',
                        pageTitle: '@InsBatE@',       
                        text: '@InsBatI@.',         
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

