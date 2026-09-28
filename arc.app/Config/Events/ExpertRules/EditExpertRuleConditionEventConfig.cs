using arc.app.Common;

namespace arc.app.Config.Events;

internal class EditExpertRuleConditionEventConfig : IDefinition
{
    public string Get()
    {
        return @"{ 
                    EventName: 'editexpertrulecondition', 
                    Description: '@RulEdiCond@',
                    EventType : 'special', 
                    Topic : 'ExpertRules',
                    TableName: 'ExpertRuleCondition'
                }";
    }
}
