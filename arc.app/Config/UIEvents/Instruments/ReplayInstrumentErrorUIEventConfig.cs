using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// Provides the configuration for the replay instrument error event.
    /// </summary>
    internal class ReplayInstrumentErrorUIEventConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the replay instrument error UI event.
        /// </summary>
        /// <returns>A JSON string that represents the event configuration.</returns>
        public string Get()
        {
            var newEvent = @"{
                        name: 'replayinstrumenterroruievent',
                        description: '@InsRepC@',
                        type: 'form',
                        action: 'replayinstrumenterrorform'
                    }";

            return newEvent;
        }
    }
}
