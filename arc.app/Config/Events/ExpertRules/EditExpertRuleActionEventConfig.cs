using arc.app.Common;

namespace arc.app.Config.Events;
internal class EditExpertRuleActionEventConfig : IDefinition
{
    public string Get()
    {
        return @"{ 
                        EventName: 'editexpertruleaction', 
                        Description: '@RulEdiB@',
                        EventType : 'special', 
                        Topic : 'ExpertRules',
                        TableName: 'ExpertRuleAction'
                    }";
    }
}
