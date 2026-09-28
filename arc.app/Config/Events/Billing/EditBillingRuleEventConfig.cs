using arc.app.Common;

namespace arc.app.Config.Events.Billing;

internal class EditBillingRuleEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        EventName: 'editbillingrule',
                        Description: '@BilEdi@',
                        EventType: 'editdata',
                        Topic: 'Billing',
                        TableName: 'BillingRule',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@GenId@' },
                            { field: 'Name', rule: 'required', message: '@GenNam@' }
                        ]
                    }";
    }
}
