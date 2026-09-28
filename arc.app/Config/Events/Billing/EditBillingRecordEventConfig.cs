using arc.app.Common;

namespace arc.app.Config.Events.Billing;

internal class EditBillingRecordEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        EventName: 'editbillingrecord',
                        Description: '@BilRecEdi@',
                        EventType: 'editdata',
                        Topic: 'Billing',
                        TableName: 'BillingRecord',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@GenId@' },
                            { field: 'TotalAmount', rule: 'required', message: '@BilAmt@' }
                        ]
                    }";
    }
}
