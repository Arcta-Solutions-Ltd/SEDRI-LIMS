using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Add Alert Approval" UI event.
/// Opens the form to add a new approval/rejection record for an alert.
/// </summary>
internal class AddAlertApprovalUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Alert Approval" UI event.
    /// </summary>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addalertapprovaluievent',
                        description: 'Add Approval/Rejection',
                        type: 'form',
                        action: 'addalertapprovalform'
                    }";

        return newEvent;
    }
}
