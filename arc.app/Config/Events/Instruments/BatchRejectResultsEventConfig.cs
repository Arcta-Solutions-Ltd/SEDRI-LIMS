using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Represents the event configuration for batch rejection of results
/// from an external instrument.
/// </summary>
internal class BatchRejectResultsEventConfig : IDefinition
{
    /// <summary>
    /// Builds and returns a JSON string defining the Batch Reject Results event configuration.
    /// </summary>
    /// <returns>A JSON-formatted string describing the event configuration.</returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'batchrejectresultsevent',
                    Description: '@InsBatG@',
                    EventType: 'special',
                    Topic: 'Instruments',
                    TableName: 'InstrumentResults',
                }";
    }
}

