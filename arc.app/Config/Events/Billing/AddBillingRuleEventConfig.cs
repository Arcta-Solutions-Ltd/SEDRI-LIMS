using arc.app.Common;

namespace arc.app.Config.Events.Billing;

internal class AddBillingRuleEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        EventName: 'addbillingrule',
                        Description: '@BilAdd@',
                        EventType: 'adddata',
                        Topic: 'Billing',
                        TableName: 'BillingRule',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@GenNam@' }
                        ]
                    }";
    }
}
