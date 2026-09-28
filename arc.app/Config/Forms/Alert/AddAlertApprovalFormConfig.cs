using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Add Alert Approval" form.
/// Allows adding a new approval or rejection record for an alert.
/// </summary>
internal class AddAlertApprovalFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Alert Approval" form.
    /// </summary>
    public string Get()
    {
        var form = @"{
                        name: 'addalertapprovalform',
                        viewTitle: 'Add Approval/Rejection',
                        saveEvent: 'addalertapproval',
                        suppressRecordView: true,
                        InitialQuery: 'AlertApprovalFormInitialQuery',
                        pages: [ 'addalertapprovalpage']
                    }";

        return form;
    }
}
