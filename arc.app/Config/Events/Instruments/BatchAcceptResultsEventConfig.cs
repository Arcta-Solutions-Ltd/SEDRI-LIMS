using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Represents the event configuration for batch acceptance of results
/// from an external instrument.
/// </summary>
internal class BatchAcceptResultsEventConfig : IDefinition
{
    /// <summary>
    /// Builds and returns a JSON string defining the Batch Accept Results event configuration.
    /// </summary>
    /// <returns>A JSON-formatted string describing the event configuration.</returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'batchacceptresultsevent',
                    Description: '@InsBatF@',
                    EventType: 'special',
                    Topic: 'Instruments',
                    TableName: 'InstrumentResults',
                }";
    }
}

