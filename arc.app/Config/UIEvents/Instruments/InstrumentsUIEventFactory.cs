using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// Factory class to create UI event configuration instances based on the event name.
    /// </summary>
    internal class InstrumentsUIEventFactory : IDefinitionFactory
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
                "acceptresultsuievent" => new AcceptResultsUIEventConfig(),
                "addinstrumentprofileuievent" => new AddInstrumentProfileUIEventConfig(),
                "batchacceptresultsuievent" => new BatchAcceptResultsUIEventConfig(),
                "batchrejectresultsuievent" => new BatchRejectResultsUIEventConfig(),
                "batchreplayinstrumenterroruievent" => new BatchReplayInstrumentErrorUIEventConfig(),
                "deleteinstrumentprofileuievent" => new DeleteInstrumentProfileUIEventConfig(),
                "editinstrumentprofileuievent" => new EditInstrumentProfileUIEventConfig(),
                "rejectresultsuievent" => new RejectResultsUIEventConfig(),
                "replayinstrumenterroruievent" => new ReplayInstrumentErrorUIEventConfig(),
                "requestinstrumenttestuievent" => new RequestInstrumentTestUIEventConfig(),
                "viewinstrumenterrordetailsuievent" => new ViewInstrumentErrorDetailsUIEventConfig(),
                "viewinstrumentresultrecord" => new ViewInstrumentResultRecordUIEventConfig(),
                _ => null,
            };
        }
    }
}
