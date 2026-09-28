using arc.app.Common;

namespace arc.app.Config.UIEvents.ExpertRules;

/// <summary>
/// UI event configuration for batch rejecting expert rules from the list view.
/// </summary>
internal class BatchRejectExpertRuleUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch reject expert rule UI event.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'batchrejectexpertruleuievent',
            description: 'Batch reject expert rules',
            type: 'form',
            action: 'batchrejectexpertruleform'
        }";
    }
}
