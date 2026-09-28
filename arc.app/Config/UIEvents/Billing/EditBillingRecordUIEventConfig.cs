using arc.app.Common;

namespace arc.app.Config.UIEvents.Billing;

internal class EditBillingRecordUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'editbillingrecorduievent',
                        description: 'Edit Billing Record',
                        type: 'form',
                        action: 'editbillingrecordform'
                    }";
    }
}
