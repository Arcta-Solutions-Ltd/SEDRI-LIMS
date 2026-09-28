using arc.app.Common;

namespace arc.app.Config.Events.ExpertRules;

/// <summary>
/// Configuration for the batch reject expert rules event.
/// </summary>
internal class BatchRejectExpertRuleEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the batch reject expert rules event configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'batchrejectexpertrule',
            Description: '@RulBatE@',
            EventType: 'batch',
            Topic: 'ExpertRules',
            TableName: 'expertrule',
            BatchEvent: 'addexpertruleapproval',
            BatchParentIdField: 'ExpertRuleId'
        }";
    }
}
