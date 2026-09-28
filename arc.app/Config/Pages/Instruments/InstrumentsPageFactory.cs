using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Factory class to create page configuration instances based on the page name.
    /// </summary>
    internal class InstrumentsPageFactory : IDefinitionFactory
    {
        /// <summary>
        /// Creates an instance of the page configuration based on the provided page name.
        /// </summary>
        /// <param name="definitionName">The name of the page configuration to create.</param>
        /// <returns>An instance of the corresponding page configuration, or null if the name is not recognized.</returns>
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addinstrumentprofilepage" => new AddInstrumentProfilePageConfig(),
                "batchacceptresultspage" => new BatchAcceptResultsPageConfig(),
                "batchrejectresultspage" => new BatchRejectResultsPageConfig(),
                "batchreplayinstrumenterrorpage" => new BatchReplayInstrumentErrorPageConfig(),
                "deleteinstrumentprofilepage" => new DeleteInstrumentProfilePageConfig(),
                "editinstrumentprofilepage" => new EditInstrumentProfilePageConfig(),
                "instrumentconfigdetailsonepage" => new InstrumentConfigDetailsOnePageConfig(),
                "replayinstrumenterrorpage" => new ReplayInstrumentErrorPageConfig(),
                "requestinstrumenttestpage" => new RequestInstrumentTestPageConfig(),
                "resultsconfirmation" => new ResultsConfirmationPageConfig(),
                "resultsrejection" => new ResultsRejectionPageConfig(),
                _ => null,
            };
        }
    }
}
