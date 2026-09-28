using arc.app.Common;

namespace arc.app.Config.UIEvents.Billing;

internal class BillingUIEventFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addbillingruleuievent" => new AddBillingRuleUIEventConfig(),
            "editbillingruleuievent" => new EditBillingRuleUIEventConfig(),
            "deletebillingruleuievent" => new DeleteBillingRuleUIEventConfig(),
            "editbillingrecorduievent" => new EditBillingRecordUIEventConfig(),
            "deletebillingrecorduievent" => new DeleteBillingRecordUIEventConfig(),
            _ => null,
        };
    }
}
