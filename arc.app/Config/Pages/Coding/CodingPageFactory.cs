using arc.app.Common;
using arc.app.Config.Pages.Coding;

namespace arc.app.Config.Pages;

internal class CodingPageFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addantibioticpage" => new AddAntibioticPageConfig(),
            "addantibioticentrypage" => new AddAntibioticEntryPageConfig(),
            "addantibioticgrouppage" => new AddAntibioticGroupPageConfig(),
            "addbreakpointapprovalpage" => new AddBreakpointApprovalPageConfig(),
            "batchapprovebreakpointpage" => new BatchApproveBreakpointPageConfig(),
            "batchrejectbreakpointpage" => new BatchRejectBreakpointPageConfig(),
            "addbreakpointpage" => new AddBreakpointPageConfig(),
            "addbreakpointcriteriapage" => new AddBreakpointCriteriaPageConfig(),
            "addcodepage" => new AddCodePageConfig(),
            "addcustompage" => new AddCustomPageConfig(),
            "addhostpage" => new AddHostPageConfig(),
            "addlistpage" => new AddListPageConfig(),
            "addresultpage" => new AddResultPageConfig(),
            "addrulecategorypage" => new AddRuleCategoryPageConfig(),
            "addsourcepage" => new AddSourcePageConfig(),
            "addtestpatterndetailspage" => new AddTestPatternDetailsPageConfig(),
            "addtestpatterngeneralpage" => new AddTestPatternGeneralPageConfig(),
            "addtestmethodpage" => new AddTestMethodPageConfig(),
            "changeorganismselectorpage" => new ChangeOrganismSelectorPageConfig(),
            "deleteantibioticpage" => new DeleteAntibioticPageConfig(),
            "deleteantibioticentrypage" => new DeleteAntibioticEntryPageConfig(),
            "deleteantibioticgrouppage" => new DeleteAntibioticGroupPageConfig(),
            "deletebreakpointpage" => new DeleteBreakpointPageConfig(),
            "deletehostpage" => new DeleteHostPageConfig(),
            "deletelistpage" => new DeleteListPageConfig(),
            "deleteorganismpage" => new DeleteOrganismPageConfig(),
            "deleteresultpage" => new DeleteResultPageConfig(),
            "deleterulecategorypage" => new DeleteRuleCategoryPageConfig(),
            "deletesourcepage" => new DeleteSourcePageConfig(),
            "deletetestpatternpage" => new DeleteTestPatternPageConfig(),
            "deletetestmethodpage" => new DeleteTestMethodPageConfig(),
            "editantibioticpage" => new EditAntibioticPageConfig(),
            "editbreakpointpage" => new EditBreakpointPageConfig(),
            "editbreakpointcriteriapage" => new EditBreakpointCriteriaPageConfig(),
            "editcustompage" => new EditCustomPageConfig(),
            "edithostpage" => new EditHostPageConfig(),
            "editorganismscopepage" => new EditOrganismScopePageConfig(),
            "editresultpage" => new EditResultPageConfig(),
            "editrulecategorypage" => new EditRuleCategoryPageConfig(),
            "edittestpatterndetailspage" => new EditTestPatternDetailsPageConfig(),
            "edittestpatterngeneralpage" => new EditTestPatternGeneralPageConfig(),
            "edittestmethodpage" => new EditTestMethodPageConfig(),
            "organismlistpage" => new OrganismListPageConfig(),
            "selectorganismpage" => new SelectOrganismPageConfig(),
            "selectorganismscopepage" => new SelectOrganismScopePageConfig(),
            "synonympage" => new SynonymPageConfig(),
            _ => null,
        };
    }
}
