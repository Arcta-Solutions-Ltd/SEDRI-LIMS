using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Factory for ExpertRule form configurations (add/edit/delete expert rules and nested condition/action forms).
/// </summary>
/// <remarks>
/// Definition names are matched case-insensitively. Returns null for unknown names.
/// </remarks>
internal class ExpertRuleFormFactory : IDefinitionFactory
{
    /// <inheritdoc />
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addexpertruleapprovalform" => new AddExpertRuleApprovalFormConfig(),
            "batchapproveexpertruleform" => new BatchApproveExpertRuleFormConfig(),
            "batchrejectexpertruleform" => new BatchRejectExpertRuleFormConfig(),
            "addexpertruleform" => new AddExpertRuleFormConfig(),
            "addexpertruleactionform" => new AddExpertRuleActionFormConfig(),
            "addexpertruleconditionform" => new AddExpertRuleConditionFormConfig(),
            "addexpertruletestconditionform" => new AddExpertRuleTestConditionFormConfig(),
            "deleteexpertruleform" => new DeleteExpertRuleFormConfig(),
            "deleteexpertruleconditionform" => new DeleteExpertRuleConditionFormConfig(),
            "deleteexpertruleactionform" => new DeleteExpertRuleActionFormConfig(),
            "deleteexpertruletestconditionform" => new DeleteExpertRuleTestConditionFormConfig(),
            "editexpertruleform" => new EditExpertRuleFormConfig(),
            "editexpertruleactionform" => new EditExpertRuleActionFormConfig(),
            "editexpertruleconditionform" => new EditExpertRuleConditionFormConfig(),
            "editexpertruletestconditionform" => new EditExpertRuleTestConditionFormConfig(),
            _ => null,
        };
    }
}
