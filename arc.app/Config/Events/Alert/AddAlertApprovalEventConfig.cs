using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Add Alert Approval" event.
/// Stores a new approval/rejection record for an alert.
/// </summary>
internal class AddAlertApprovalEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Alert Approval" event.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'addalertapproval',
            Description: '@BreAddApp@',
            EventType: 'specialadddata',
            Topic: 'Alert',
            TableName: 'alertapproval',
            ValidationRules: [
                { field: 'AlertId', rule: 'required', message: '@GenReqB@' },
                { field: 'CodingStatusId', rule: 'required', message: '@GenReqB@' }
            ]
        }";
    }
}
