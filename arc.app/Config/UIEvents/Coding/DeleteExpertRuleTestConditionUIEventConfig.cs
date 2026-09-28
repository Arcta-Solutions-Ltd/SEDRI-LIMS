using arc.app.Common;

namespace arc.app.Config.UIEvents.Coding;

/// <summary>
/// UI event configuration for deleting an expert rule test condition from the record view.
/// </summary>
internal class DeleteExpertRuleTestConditionUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON UI event configuration.
    /// </summary>
    /// <returns>JSON string defining the delete test condition UI event.</returns>
    public string Get()
    {
        return @"{
                        name: 'deleteexpertruletestconditionuievent',
                        description: 'Delete expert rule test condition',
                        type: 'form',
                        action: 'deleteexpertruletestconditionform'
                    }";
    }
}
