using arc.app.Common;

namespace arc.app.Config.UIEvents.ExpertRules;

/// <summary>
/// UI event configuration for batch approving expert rules from the list view.
/// </summary>
internal class BatchApproveExpertRuleUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch approve expert rule UI event.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'batchapproveexpertruleuievent',
            description: 'Batch approve expert rules',
            type: 'form',
            action: 'batchapproveexpertruleform'
        }";
    }
}
