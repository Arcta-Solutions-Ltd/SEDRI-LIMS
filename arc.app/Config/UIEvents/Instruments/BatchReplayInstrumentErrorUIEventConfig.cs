using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// Provides the configuration for the batch replay instrument error event.
    /// </summary>
    internal class BatchReplayInstrumentErrorUIEventConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the batch replay instrument error UI event.
        /// </summary>
        /// <returns>A JSON string that represents the event configuration.</returns>
        public string Get()
        {
            var newEvent = @"{
                        name: 'batchreplayinstrumenterroruievent',
                        description: '@InsRepC@',
                        type: 'form',
                        action: 'batchreplayinstrumenterrorform'
                    }";

            return newEvent;
        }
    }
}
