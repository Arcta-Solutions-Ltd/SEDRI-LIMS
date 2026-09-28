using arc.app.Common;
using arc.app.Config.Events.ExpertRules;

namespace arc.app.Config.Events;
internal class ExpertRuleEventFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addexpertrule" => new AddExpertRuleEventConfig(),
            "addexpertruleapproval" => new AddExpertRuleApprovalEventConfig(),
            "batchapproveexpertrule" => new BatchApproveExpertRuleEventConfig(),
            "batchrejectexpertrule" => new BatchRejectExpertRuleEventConfig(),
            "addexpertruleaction" => new AddExpertRuleActionEventConfig(),
            "addexpertrulecondition" => new AddExpertRuleConditionEventConfig(),
            "addexpertruletestcondition" => new AddExpertRuleTestConditionEventConfig(),
            "deleteexpertrule" => new DeleteExpertRuleEventConfig(),
            "deleteexpertrulecondition" => new DeleteExpertRuleConditionEventConfig(),
            "deleteexpertruleaction" => new DeleteExpertRuleActionEventConfig(),
            "deleteexpertruletestcondition" => new DeleteExpertRuleTestConditionEventConfig(),
            "editexpertrule" => new EditExpertRuleEventConfig(),
            "editexpertruleaction" => new EditExpertRuleActionEventConfig(),
            "editexpertrulecondition" => new EditExpertRuleConditionEventConfig(),
            "editexpertruletestcondition" => new EditExpertRuleTestConditionEventConfig(),
            _ => null,
        };
    }
}

