using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Configuration class for the batch replay instrument error form.
    /// </summary>
    internal class BatchReplayInstrumentErrorFormConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the batch replay instrument error form.
        /// </summary>
        /// <returns>A JSON string that represents the form configuration.</returns>
        public string Get()
        {
            var form = @"{
                        name: 'batchreplayinstrumenterrorform',
                        viewTitle: '@InsRep@',
                        saveEvent: 'batchreplayinstrumenterror',
                        suppressRecordView: true,
                        pages: ['batchreplayinstrumenterrorpage']
                    }";

            return form;
        }
    }
}
