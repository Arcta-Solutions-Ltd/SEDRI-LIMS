using arc.app.Common;

namespace arc.app.Config.Events;

internal class EditExpertRuleEventConfig : IDefinition
{
    public string Get()
    {
        return @"{ 
                    EventName: 'editExpertRule', 
                    Description: '@RulEdi@',
                    EventType : 'special', 
                    Topic : 'ExpertRules', 
                    TableName: 'ExpertRule',
                    ValidationRules: [
                    { field: 'ExpertRuleName', rule: 'required', message: '@RulRulA@'}
                    ]
                }";
    }
}
