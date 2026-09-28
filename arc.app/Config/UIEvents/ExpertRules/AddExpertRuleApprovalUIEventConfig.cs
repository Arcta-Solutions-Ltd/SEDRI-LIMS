using arc.app.Common;

namespace arc.app.Config.UIEvents.ExpertRules;

/// <summary>
/// Configuration for the "Add Expert Rule Approval" UI event.
/// Opens the form to add a new approval/rejection record for an expert rule.
/// </summary>
internal class AddExpertRuleApprovalUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Expert Rule Approval" UI event.
    /// </summary>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addexpertruleapprovaluievent',
                        description: 'Add Approval/Rejection',
                        type: 'form',
                        action: 'addexpertruleapprovalform'
                    }";

        return newEvent;
    }
}
