using arc.app.Common;

namespace arc.app.Config.Events.ExpertRules;

/// <summary>
/// Configuration for the delete expert rule condition event.
/// </summary>
internal class DeleteExpertRuleConditionEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON event configuration for deleting an expert rule condition.
    /// </summary>
    /// <returns>JSON string defining the delete event.</returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'deleteexpertrulecondition', 
                    Description: '@RulDel@',
                    EventType : 'special', 
                    Topic : 'ExpertRules',
                    TableName: 'ExpertRuleCondition'
                }";
    }
}
