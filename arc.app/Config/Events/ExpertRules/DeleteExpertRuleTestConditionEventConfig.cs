using arc.app.Common;

namespace arc.app.Config.Events.ExpertRules;

/// <summary>
/// Configuration for the delete expert rule test condition event.
/// </summary>
internal class DeleteExpertRuleTestConditionEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON event configuration for deleting an expert rule test condition.
    /// </summary>
    /// <returns>JSON string defining the delete event.</returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'deleteexpertruletestcondition', 
                    Description: '@RulDel@',
                    EventType : 'special', 
                    Topic : 'ExpertRules',
                    TableName: 'ExpertRuleTestCondition'
                }";
    }
}
