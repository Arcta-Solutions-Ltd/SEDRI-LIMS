using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Add Breakpoint Approval" event.
/// Stores a new approval/rejection record for a breakpoint.
/// </summary>
internal class AddBreakpointApprovalEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Breakpoint Approval" event.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'addbreakpointapproval',
            Description: '@BreAddApp@',
            EventType: 'specialadddata',
            Topic: 'Breakpoints',
            TableName: 'breakpointapproval',
            ValidationRules: [
                { field: 'BreakpointId', rule: 'required', message: '@GenReqB@' },
                { field: 'CodingStatusId', rule: 'required', message: '@GenReqB@' }
            ]
        }";
    }
}
