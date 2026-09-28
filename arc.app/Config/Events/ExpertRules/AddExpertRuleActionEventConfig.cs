using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddExpertRuleActionEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addexpertruleaction', 
                        Description: '@RulAddB@',
                        EventType : 'specialadddata', 
                        Topic : 'ExpertRules',
                        TableName: 'ExpertRuleAction'
                    }";
        }
    }
}
