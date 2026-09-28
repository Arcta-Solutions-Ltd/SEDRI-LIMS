using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules;
internal class ExpertRulePageFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addexpertruleapprovalpage" => new AddExpertRuleApprovalPageConfig(),
            "batchapproveexpertrulepage" => new BatchApproveExpertRulePageConfig(),
            "batchrejectexpertrulepage" => new BatchRejectExpertRulePageConfig(),
            "addexpertruleactionpage" => new AddExpertRuleActionPageConfig(),
            "addexpertruleconditionpage" => new AddExpertRuleConditionPageConfig(),
            "addexpertruletestconditionpage" => new AddExpertRuleTestConditionPageConfig(),
            "deleteexpertrulepage" => new DeleteExpertRulePageConfig(),
            "deleteexpertruleconditionpage" => new DeleteExpertRuleConditionPageConfig(),
            "deleteexpertruleactionpage" => new DeleteExpertRuleActionPageConfig(),
            "deleteexpertruletestconditionpage" => new DeleteExpertRuleTestConditionPageConfig(),
            "editexpertruleactionpage" => new EditExpertRuleActionPageConfig(),
            "editexpertruleconditionpage" => new EditExpertRuleConditionPageConfig(),
            "editexpertruletestconditionpage" => new EditExpertRuleTestConditionPageConfig(),
            "editexpertruledetailspage" => new EditExpertRuleDetailsPageConfig(),
            "expertruledetailspage" => new ExpertRuleDetailsPageConfig(),
            "expertruledetailssecondpage" => new ExpertRuleDetailsSecondPageConfig(),
            _ => null,
        };
    }
}
