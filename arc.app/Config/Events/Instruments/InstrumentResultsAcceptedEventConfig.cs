using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration class for the accept instrument results event.
/// </summary>
internal class AcceptInstrumentResultsEventConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON string for the accept instrument results event.
    /// </summary>
    /// <returns>A JSON string that represents the event configuration.</returns>
    public string Get()
    {
        return @"{ 
                        EventName: 'acceptinstrumentresults',
                        Description: '@InsAccB@',
                        EventType : 'specialadddata',
                        Topic : 'Instruments',
                        TableName: 'instrumentresults',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@InsErrG@'}
                        ]
                    }";
    }
}
