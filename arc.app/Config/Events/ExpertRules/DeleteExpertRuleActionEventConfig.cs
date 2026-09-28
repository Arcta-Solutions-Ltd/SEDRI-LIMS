using arc.app.Common;

namespace arc.app.Config.Events.ExpertRules;

/// <summary>
/// Configuration for the delete expert rule action event.
/// </summary>
internal class DeleteExpertRuleActionEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON event configuration for deleting an expert rule action.
    /// </summary>
    /// <returns>JSON string defining the delete event.</returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'deleteexpertruleaction', 
                    Description: '@RulDel@',
                    EventType : 'special', 
                    Topic : 'ExpertRules',
                    TableName: 'ExpertRuleAction'
                }";
    }
}
