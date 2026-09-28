using arc.app.Common;

namespace arc.app.Config.UIEvents.Billing;

internal class DeleteBillingRecordUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'deletebillingrecorduievent',
                        description: 'Delete Billing Record',
                        type: 'form',
                        action: 'deletebillingrecordform'
                    }";
    }
}
