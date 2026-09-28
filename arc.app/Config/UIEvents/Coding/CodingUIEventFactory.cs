using arc.app.Common;
using arc.app.Config.UIEvents.Coding;

namespace arc.app.Config.UIEvents
{
    internal class CodingUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "actionselectoruievent" => new ActionSelectorUIEventConfig(),
                "addantibioticuievent" => new AddAntibioticUIEventConfig(),
                "addantibioticentryuievent" => new AddAntibioticEntryUIEventConfig(),
                "addantibioticgroupuievent" => new AddAntibioticGroupUIEventConfig(),
                "addbreakpointapprovaluievent" => new AddBreakpointApprovalUIEventConfig(),
                "batchapprovebreakpointuievent" => new BatchApproveBreakpointUIEventConfig(),
                "batchrejectbreakpointuievent" => new BatchRejectBreakpointUIEventConfig(),
                "addbreakpointuievent" => new AddBreakpointUIEventConfig(),
                "addcustomuievent" => new AddCustomUIEventConfig(),
                "addexpertruleuievent" => new AddExpertRuleUIEventConfig(),
                "addhostuievent" => new AddHostUIEventConfig(),
                "addlistuievent" => new AddListUIEventConfig(),
                "addorganismuievent" => new AddOrganismUIEventConfig(),
                "addresultuievent" => new AddResultUIEventConfig(),
                "addrulecategoryuievent" => new AddRuleCategoryUIEventConfig(),
                "addexpertruleactionuievent" => new AddExpertRuleActionUIEventConfig(),
                "addexpertruleconditionuievent" => new AddExpertRuleConditionUIEventConfig(),
                "addexpertruletestconditionuievent" => new AddExpertRuleTestConditionUIEventConfig(),
                "addsourceuievent" => new AddSourceUIEventConfig(),
                "addtestpatternuievent" => new AddTestPatternUIEventConfig(),
                "addtestmethoduievent" => new AddTestMethodUIEventConfig(),
                "conditionselectoruievent" => new ConditionSelectorUIEventConfig(),
                "deleteantibioticuievent" => new DeleteAntibioticUIEventConfig(),
                "deleteantibioticentryuievent" => new DeleteAntibioticEntryUIEventConfig(),
                "deleteantibioticgroupuievent" => new DeleteAntibioticGroupUIEventConfig(),
                "deletebreakpointuievent" => new DeleteBreakpointUIEventConfig(),
                "deleteexpertruleuievent" => new DeleteExpertRuleUIEventConfig(),
                "deleteexpertruleconditionuievent" => new DeleteExpertRuleConditionUIEventConfig(),
                "deleteexpertruleactionuievent" => new DeleteExpertRuleActionUIEventConfig(),
                "deleteexpertruletestconditionuievent" => new DeleteExpertRuleTestConditionUIEventConfig(),
                "deletehostuievent" => new DeleteHostUIEventConfig(),
                "deletelistuievent" => new DeleteListUIEventConfig(),
                "deletesourceuievent" => new DeleteSourceUIEventConfig(),
                "deletetestpatternuievent" => new DeleteTestPatternUIEventConfig(),
                "deleteorganismuievent" => new DeleteOrganismUIEventConfig(),
                "deleteresultuievent" => new DeleteResultUIEventConfig(),
                "deleterulecategoryuievent" => new DeleteRuleCategoryUIEventConfig(),
                "deletetestmethoduievent" => new DeleteTestMethodUIEventConfig(),
                "editantibioticuievent" => new EditAntibioticUIEventConfig(),
                "editbreakpointuievent" => new EditBreakpointUIEventConfig(),
                "editcustomuievent" => new EditCustomUIEventConfig(),
                "editexpertruleuievent" => new EditExpertRuleUIEventConfig(),
                "editexpertruleactionuievent" => new EditExpertRuleActionUIEventConfig(),
                "editexpertruleconditionuievent" => new EditExpertRuleConditionUIEventConfig(),
                "editexpertruletestconditionuievent" => new EditExpertRuleTestConditionUIEventConfig(),
                "edithostuievent" => new EditHostUIEventConfig(),
                "editorganismuievent" => new EditOrganismUIEventConfig(),
                "editresultuievent" => new EditResultUIEventConfig(),
                "editrulecategoryuievent" => new EditRuleCategoryUIEventConfig(),
                "edittestpatternuievent" => new EditTestPatternUIEventConfig(),
                "edittestmethoduievent" => new EditTestMethodUIEventConfig(),
                "viewbreakpointuievent" => new ViewBreakpointUIEventConfig(),
                "viewexpertruleuievent" => new ViewExpertRuleUIEventConfig(),
                "synonymuievent" => new SynonymUIEventConfig(),
                _ => null,
            };
        }
    }
}
