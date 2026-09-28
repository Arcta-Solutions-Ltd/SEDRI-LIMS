using arc.app.Common;

namespace arc.app.Config.Forms.Coding;

/// <summary>
/// Configuration for the "Add Breakpoint Approval" form.
/// Allows adding a new approval or rejection record for a breakpoint.
/// </summary>
internal class AddBreakpointApprovalFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Breakpoint Approval" form.
    /// </summary>
    public string Get()
    {
        var form = @"{
                        name: 'addbreakpointapprovalform',
                        viewTitle: 'Add Approval/Rejection',
                        saveEvent: 'addbreakpointapproval',
                        suppressRecordView: true,
                        InitialQuery: 'BreakpointApprovalFormInitialQuery',
                        pages: [ 'addbreakpointapprovalpage']
                    }";

        return form;
    }
}
