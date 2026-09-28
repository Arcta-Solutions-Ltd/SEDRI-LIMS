using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Configuration class for the replay instrument error form.
    /// </summary>
    internal class ReplayInstrumentErrorFormConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the replay instrument error form.
        /// </summary>
        /// <returns>A JSON string that represents the form configuration.</returns>
        public string Get()
        {
            var form = @"{
                        name: 'replayinstrumenterrorform',
                        viewTitle: '@InsRep@',
                        initialQuery: 'InstrumentErrorReplayQuery',
                        saveEvent: 'replayinstrumenterror',
                        suppressRecordView: true,
                        pages: ['replayinstrumenterrorpage']
                    }";

            return form;
        }
    }

}
