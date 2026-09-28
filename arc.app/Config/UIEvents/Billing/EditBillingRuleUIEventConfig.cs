using arc.app.Common;

namespace arc.app.Config.UIEvents.Billing;

internal class EditBillingRuleUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'editbillingruleuievent',
                        description: 'Edit Billing Rule',
                        type: 'form',
                        action: 'editbillingruleform'
                    }";
    }
}
