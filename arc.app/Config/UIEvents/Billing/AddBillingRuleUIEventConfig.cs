using arc.app.Common;

namespace arc.app.Config.UIEvents.Billing;

internal class AddBillingRuleUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'addbillingruleuievent',
                        description: 'Add Billing Rule',
                        type: 'form',
                        action: 'addbillingruleform'
                    }";
    }
}
