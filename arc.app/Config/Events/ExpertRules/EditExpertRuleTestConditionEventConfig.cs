using arc.app.Common;

namespace arc.app.Config.Events.ExpertRules;

/// <summary>
/// Configuration for the edit expert rule test condition event.
/// </summary>
internal class EditExpertRuleTestConditionEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON event configuration for editing an expert rule test condition.
    /// </summary>
    /// <returns>JSON string defining the edit event.</returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'editexpertruletestcondition', 
                    Description: '@GenRulU@',
                    EventType : 'special', 
                    Topic : 'ExpertRules',
                    TableName: 'ExpertRuleTestCondition'
                }";
    }
}
