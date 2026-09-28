using arc.app.Common;

namespace arc.app.Config.Forms.Billing;

internal class AddBillingRuleFormConfig : IDefinition
{
    public string Get()
    {
        var form = @"{
                        name: 'addbillingruleform',
                        viewTitle: '@BilAdd@',
                        saveEvent: 'addbillingrule',
                        suppressRecordView: true,
                        pages: [ 'addbillingrulepage' ]
                    }";

        return form;
    }
}
