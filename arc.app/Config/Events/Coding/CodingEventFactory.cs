using arc.app.Common;
using arc.app.Config.Events.Coding;

namespace arc.app.Config.Events;

internal class CodingEventFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addantibiotic" => new AddAntibioticEventConfig(),
            "addantibioticentry" => new AddAntibioticEntryEventConfig(),
            "addantibioticgroup" => new AddAntibioticGroupEventConfig(),
            "addbreakpoint" => new AddBreakpointEventConfig(),
            "addbreakpointapproval" => new AddBreakpointApprovalEventConfig(),
            "batchapprovebreakpoint" => new BatchApproveBreakpointEventConfig(),
            "batchrejectbreakpoint" => new BatchRejectBreakpointEventConfig(),
            "addcodinglist" => new AddCodingListEventConfig(),
            "addcustomentry" => new AddCustomEntryEventConfig(),
            "addhost" => new AddHostEventConfig(),
            "addorganism" => new AddOrganismEventConfig(),
            "addresult" => new AddResultEventConfig(),
            "addrulecategory" => new AddRuleCategoryEventConfig(),
            "addsource" => new AddSourceEventConfig(),
            "addtestpattern" => new AddTestPatternEventConfig(),
            "addtestmethod" => new AddTestMethodEventConfig(),
            "deleteantibiotic" => new DeleteAntibioticEventConfig(),
            "deleteantibioticentry" => new DeleteAntibioticEntryEventConfig(),
            "deleteantibioticgroup" => new DeleteAntibioticGroupEventConfig(),
            "deletebreakpoint" => new DeleteBreakpointEventConfig(),
            "deletecodinglist" => new DeleteCodingListEventConfig(),
            "deletehost" => new DeleteHostEventConfig(),
            "deleteresult" => new DeleteResultEventConfig(),
            "deleterulecategory" => new DeleteRuleCategoryEventConfig(),
            "deletesource" => new DeleteSourceEventConfig(),
            "deletetestpattern" => new DeleteTestPatternEventConfig(),
            "deletetestmethod" => new DeleteTestMethodEventConfig(),
            "deleteorganism" => new DeleteOrganismEventConfig(),
            "editantibiotic" => new EditAntibioticEventConfig(),
            "editbreakpoint" => new EditBreakpointEventConfig(),
            "editcustomentry" => new EditCustomEntryEventConfig(),
            "edithost" => new EditHostEventConfig(),
            "editresult" => new EditResultEventConfig(),
            "editrulecategory" => new EditRuleCategoryEventConfig(),
            "edittestpattern" => new EditTestPatternEventConfig(),
            "edittestmethod" => new EditTestMethodEventConfig(),
            "synonym" => new SynonymEventConfig(),
            _ => null,
        };
    }
}
