using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Configuration class for the replay instrument error event.
    /// </summary>
    internal class ReplayInstrumentErrorEventConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the replay instrument error event.
        /// </summary>
        /// <returns>A JSON string that represents the event configuration.</returns>
        public string Get()
        {
            return @"{ 
                        EventName: 'replayinstrumenterror',
                        Description: '@InsRepA@',
                        EventType: 'special',
                        Topic: 'Instruments',
                        TableName: 'InstrumentErrors'
                    }";
        }
    }
}
