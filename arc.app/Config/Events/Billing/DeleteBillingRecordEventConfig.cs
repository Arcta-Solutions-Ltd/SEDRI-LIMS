using arc.app.Common;

namespace arc.app.Config.Events.Billing;

internal class DeleteBillingRecordEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        EventName: 'deletebillingrecord',
                        Description: '@BilRecDel@',
                        EventType: 'deletedata',
                        Topic: 'Billing',
                        TableName: 'BillingRecord',
                        ValidationRules: [
                            { field: 'DirectTestName', rule: 'required', message: '@BilDir@' }
                        ]
                    }";
    }
}
