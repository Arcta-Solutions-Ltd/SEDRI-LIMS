using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration class for the delete instrument results event.
/// </summary>
internal class DeleteInstrumentResultsEventConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON string for the delete instrument results event.
    /// </summary>
    /// <returns>A JSON string that represents the event configuration.</returns>
    public string Get()
    {
        return @"{ 
                        EventName: 'deleteinstrumentresults',
                        Description: '@InsRejB@',
                        Topic : 'Instruments',
                        EventType : 'deletedata', 
                        TableName: 'instrumentresults',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@InsErrG@'}
                        ]
            }";
    }
}
