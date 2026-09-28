using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddExpertRuleConditionEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addexpertrulecondition', 
                        Description: '@RulAddCond@',
                        EventType : 'specialadddata', 
                        Topic : 'ExpertRules',
                        TableName: 'ExpertRuleCondition'
                    }";
        }
    }
}
