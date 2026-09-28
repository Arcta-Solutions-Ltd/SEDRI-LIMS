using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Configuration class for the batch replay instrument error event.
    /// </summary>
    internal class BatchReplayInstrumentErrorEventConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the batch replay instrument error event.
        /// </summary>
        /// <returns>A JSON string that represents the event configuration.</returns>
        public string Get()
        {
            return @"{ 
                        EventName: 'batchreplayinstrumenterror',
                        Description: '@InsBatA@',
                        EventType: 'batch',
                        Topic: 'Instruments',
                        TableName: 'InstrumentErrors',
                        BatchEvent: 'replayinstrumenterror'
                    }";
        }
    }

}
