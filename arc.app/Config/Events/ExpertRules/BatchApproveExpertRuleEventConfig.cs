using arc.app.Common;

namespace arc.app.Config.Events.ExpertRules;

/// <summary>
/// Configuration for the batch approve expert rules event.
/// </summary>
internal class BatchApproveExpertRuleEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the batch approve expert rules event configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'batchapproveexpertrule',
            Description: '@RulBatC@',
            EventType: 'batch',
            Topic: 'ExpertRules',
            TableName: 'expertrule',
            BatchEvent: 'addexpertruleapproval',
            BatchParentIdField: 'ExpertRuleId'
        }";
    }
}
