using arc.app.Common;

namespace arc.app.Config.Events.Billing;

internal class DeleteBillingRuleEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        EventName: 'deletebillingrule',
                        Description: '@BilDel@',
                        EventType: 'deletedata',
                        Topic: 'Billing',
                        TableName: 'BillingRule',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@GenNam@' }
                        ]
                    }";
    }
}
