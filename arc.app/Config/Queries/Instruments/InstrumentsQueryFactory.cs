using arc.app.Common;
using arc.app.Config.Queries.Instruments;

namespace arc.app.Config.Queries;

/// <summary>
/// Factory class for creating instrument query definitions.
/// </summary>
internal class InstrumentsQueryFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of the appropriate query definition based on the definition name.
    /// </summary>
    /// <param name="definitionName">The name of the query definition to create.</param>
    /// <returns>An instance of IDefinition corresponding to the definition name, or null if no match is found.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "cultureinstrumentresults" => new CultureInstrumentResultsQuery(),
            "editinstrumentprofilequery" => new EditInstrumentProfileQuery(),
            "instrumenterrorjsoncontentsquery" => new InstrumentErrorJsonContentsQuery(),
            "instrumenterrorlistquery" => new InstrumentErrorListQuery(),
            "instrumenterrorreplayquery" => new InstrumentErrorReplayQuery(),
            "instrumentprofilelistquery" => new InstrumentProfileListQuery(),
            "instrumentrequestquery" => new InstrumentRequestQuery(),
            "instrumentresultbyidquery" => new InstrumentResultByIdQuery(),
            "instrumentresultslistquery" => new InstrumentResultsListQuery(),
            "requestinstrumenttestforminitialquery" => new RequestInstrumentTestFormInitialQueryConfig(),
            "singleinstrumentprofilelistquery" => new SingleInstrumentProfileListQuery(),
            "specimeninstrumentresultsquery" => new SpecimenInstrumentResultsQuery(),
            "testinstrumentresultsquery" => new TestInstrumentResultsQuery(),
            "instrumentresultrecordviewbyid" => new InstrumentResultRecordViewByIdQueryConfig(),
            "instrumentresultattachmentsforinstrumentresultview" => new InstrumentResultAttachmentsForInstrumentResultViewQuery(),
            _ => null,
        };
    }
}

