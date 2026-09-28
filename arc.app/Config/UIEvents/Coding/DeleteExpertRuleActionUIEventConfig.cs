using arc.app.Common;

namespace arc.app.Config.UIEvents.Coding;

/// <summary>
/// UI event configuration for deleting an expert rule action from the record view.
/// </summary>
internal class DeleteExpertRuleActionUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON UI event configuration.
    /// </summary>
    /// <returns>JSON string defining the delete action UI event.</returns>
    public string Get()
    {
        return @"{
                        name: 'deleteexpertruleactionuievent',
                        description: 'Delete expert rule action',
                        type: 'form',
                        action: 'deleteexpertruleactionform'
                    }";
    }
}
