using arc.app.Common;

namespace arc.app.Config.UIEvents.Coding;

/// <summary>
/// UI event configuration for deleting an expert rule condition from the record view.
/// </summary>
internal class DeleteExpertRuleConditionUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON UI event configuration.
    /// </summary>
    /// <returns>JSON string defining the delete condition UI event.</returns>
    public string Get()
    {
        return @"{
                        name: 'deleteexpertruleconditionuievent',
                        description: 'Delete expert rule condition',
                        type: 'form',
                        action: 'deleteexpertruleconditionform'
                    }";
    }
}
