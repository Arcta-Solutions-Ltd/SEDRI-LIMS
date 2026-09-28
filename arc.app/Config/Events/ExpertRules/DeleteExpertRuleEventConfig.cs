using arc.app.Common;

namespace arc.app.Config.Events;

internal class DeleteExpertRuleEventConfig : IDefinition
{
    public string Get()
    {
        return @"{ 
                    EventName: 'deleteExpertRule', 
                    Description: '@RulDel@',
                    EventType : 'special', 
                    Topic : 'ExpertRules', 
                    TableName: 'ExpertRule'
                }";
    }
}
