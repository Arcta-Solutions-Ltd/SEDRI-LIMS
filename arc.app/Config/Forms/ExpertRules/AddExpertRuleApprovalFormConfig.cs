using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Configuration for the "Add Expert Rule Approval" form.
/// Allows adding a new approval or rejection record for an expert rule.
/// </summary>
internal class AddExpertRuleApprovalFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Expert Rule Approval" form.
    /// </summary>
    public string Get()
    {
        var form = @"{
                        name: 'addexpertruleapprovalform',
                        viewTitle: 'Add Approval/Rejection',
                        saveEvent: 'addexpertruleapproval',
                        suppressRecordView: true,
                        InitialQuery: 'ExpertRuleApprovalFormInitialQuery',
                        pages: [ 'addexpertruleapprovalpage']
                    }";

        return form;
    }
}
