using arc.app.Common;

namespace arc.app.Config.Events.Billing;

internal class BillingEventFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addbillingrule" => new AddBillingRuleEventConfig(),
            "editbillingrule" => new EditBillingRuleEventConfig(),
            "deletebillingrule" => new DeleteBillingRuleEventConfig(),
            "editbillingrecord" => new EditBillingRecordEventConfig(),
            "deletebillingrecord" => new DeleteBillingRecordEventConfig(),
            _ => null,
        };
    }
}
