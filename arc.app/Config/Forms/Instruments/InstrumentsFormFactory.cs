using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Factory class to create form configuration instances based on the form name.
/// </summary>
internal class InstrumentsFormFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of the form configuration based on the provided form name.
    /// </summary>
    /// <param name="definitionName">The name of the form configuration to create.</param>
    /// <returns>An instance of the corresponding form configuration, or null if the name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addinstrumentprofileform" => new AddInstrumentProfileFormConfig(),
            "batchacceptresultsform" => new BatchAcceptResultsFormConfig(),
            "batchrejectresultsform" => new BatchRejectResultsFormConfig(),
            "batchreplayinstrumenterrorform" => new BatchReplayInstrumentErrorFormConfig(),
            "deleteinstrumentprofileform" => new DeleteInstrumentProfileFormConfig(),
            "editinstrumentprofileform" => new EditInstrumentProfileFormConfig(),
            "replayinstrumenterrorform" => new ReplayInstrumentErrorFormConfig(),
            "requestinstrumenttestform" => new RequestInstrumentTestFormConfig(),
            "resultsconfirmationform" => new ResultsConfirmationFormConfig(),
            "resultsrejectionform" => new ResultsRejectionFormConfig(),
            "viewinstrumenterrordetailsform" => new ViewInstrumentErrorDetailsFormConfig(),
            _ => null,
        };
    }
}
