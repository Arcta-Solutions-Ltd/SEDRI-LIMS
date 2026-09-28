using arc.app.Common;

namespace arc.app.Config.Forms.Billing;

internal class BillingFormFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addbillingruleform" => new AddBillingRuleFormConfig(),
            "editbillingruleform" => new EditBillingRuleFormConfig(),
            "deletebillingruleform" => new DeleteBillingRuleFormConfig(),
            "editbillingrecordform" => new EditBillingRecordFormConfig(),
            "deletebillingrecordform" => new DeleteBillingRecordFormConfig(),
            _ => null,
        };
    }
}
