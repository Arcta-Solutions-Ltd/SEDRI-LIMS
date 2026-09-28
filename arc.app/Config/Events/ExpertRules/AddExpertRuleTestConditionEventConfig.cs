using arc.app.Common;



namespace arc.app.Config.Events.ExpertRules;



/// <summary>

/// Configuration for the add expert rule test condition event.

/// </summary>

internal class AddExpertRuleTestConditionEventConfig : IDefinition

{

    /// <summary>

    /// Returns the JSON event configuration for adding an expert rule test condition.

    /// </summary>

    /// <returns>JSON string defining the add event.</returns>

    public string Get()

    {

        return @"{ 

                    EventName: 'addexpertruletestcondition', 

                    Description: '@RulAddC@',

                    EventType : 'specialadddata', 

                    Topic : 'ExpertRules',

                    TableName: 'ExpertRuleTestCondition'

                }";

    }

}

