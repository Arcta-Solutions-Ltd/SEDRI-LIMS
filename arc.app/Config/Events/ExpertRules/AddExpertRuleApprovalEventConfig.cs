using arc.app.Common;

namespace arc.app.Config.Events.ExpertRules;

/// <summary>
/// Configuration for the "Add Expert Rule Approval" event.
/// Stores a new approval/rejection record for an expert rule.
/// </summary>
internal class AddExpertRuleApprovalEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Expert Rule Approval" event.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'addexpertruleapproval',
            Description: '@BreAddApp@',
            EventType: 'specialadddata',
            Topic: 'ExpertRules',
            TableName: 'expertruleapproval',
            ValidationRules: [
                { field: 'ExpertRuleId', rule: 'required', message: '@GenReqB@' },
                { field: 'CodingStatusId', rule: 'required', message: '@GenReqB@' }
            ]
        }";
    }
}
