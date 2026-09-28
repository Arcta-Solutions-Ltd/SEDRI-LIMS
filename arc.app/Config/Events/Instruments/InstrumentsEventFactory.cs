using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Factory class to create event configuration instances based on the event name.
    /// </summary>
    internal class InstrumentsEventFactory : IDefinitionFactory
    {
        /// <summary>
        /// Creates an instance of the event configuration based on the provided event name.
        /// </summary>
        /// <param name="definitionName">The name of the event configuration to create.</param>
        /// <returns>An instance of the corresponding event configuration, or null if the name is not recognized.</returns>
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "acceptinstrumentresults" => new AcceptInstrumentResultsEventConfig(),
                "addinstrumentprofile" => new AddInstrumentProfileEventConfig(),
                "batchacceptresultsevent" => new BatchAcceptResultsEventConfig(),
                "batchrejectresultsevent" => new BatchRejectResultsEventConfig(),
                "batchreplayinstrumenterror" => new BatchReplayInstrumentErrorEventConfig(),
                "deleteinstrumentprofile" => new DeleteInstrumentProfileEventConfig(),
                "deleteinstrumentresults" => new DeleteInstrumentResultsEventConfig(),
                "editinstrumentprofile" => new EditInstrumentProfileEventConfig(),
                "instrumentculture" => new InstrumentCultureEventConfig(),
                "instrumentresults" => new InstrumentResultsEventConfig(),
                "replayinstrumenterror" => new ReplayInstrumentErrorEventConfig(),
                "requestinstrumenttest" => new RequestInstrumentTestEventConfig(),
                "viewinstrumenterrordetails" => new ViewInstrumentErrorDetailsEventConfig(),
                _ => null,
            };
        }
    }
}
