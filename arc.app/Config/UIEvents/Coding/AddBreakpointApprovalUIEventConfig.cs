using arc.app.Common;

namespace arc.app.Config.UIEvents.Coding;

/// <summary>
/// Configuration for the "Add Breakpoint Approval" UI event.
/// Opens the form to add a new approval/rejection record for a breakpoint.
/// </summary>
internal class AddBreakpointApprovalUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Breakpoint Approval" UI event.
    /// </summary>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addbreakpointapprovaluievent',
                        description: 'Add Approval/Rejection',
                        type: 'form',
                        action: 'addbreakpointapprovalform'
                    }";

        return newEvent;
    }
}
