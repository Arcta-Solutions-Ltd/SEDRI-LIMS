using arc.app.Common;

namespace arc.app.Config.UIEvents.Billing;

internal class DeleteBillingRuleUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'deletebillingruleuievent',
                        description: 'Delete Billing Rule',
                        type: 'form',
                        action: 'deletebillingruleform'
                    }";
    }
}
