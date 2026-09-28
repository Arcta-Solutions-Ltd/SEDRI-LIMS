using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddExpertRuleEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addExpertRule', 
                        Description: '@RulAdd@',
                        EventType : 'specialadddata', 
                        Topic : 'ExpertRules', 
                        TableName: 'ExpertRule',
                        ValidationRules: [
                            { field: 'ExpertRuleName', rule: 'required', message: '@RulRulA@'},
                            { field: 'RuleText', rule: 'required', message: '@RulRulB@'}
                        ]
                    }";
        }
    }
}
