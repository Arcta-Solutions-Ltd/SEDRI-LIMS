using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for editing an expert rule condition.
/// When opened from the expert rule fieldgrid, the portal passes row data from the parent form (no InitialQuery);
/// save uses deferred merge into the grid (<c>saveOperation: updategrid</c>) until the parent expert rule is saved.
/// When opened from the record view embedded list, <c>initialQuery</c> loads the row by id and save persists immediately.
/// </summary>
internal class EditExpertRuleConditionFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration for the edit expert rule condition form.
    /// </summary>
    /// <returns>JSON string defining the form structure and pages.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'editexpertruleconditionform',
                        viewTitle: 'Edit an expert rule condition.',
                        saveEvent: 'editexpertrulecondition',
                        saveOperation: 'updategrid',
                        suppressRecordView: true,
                        initialQuery: 'editexpertruleconditionquery',
                        pages: ['editexpertruleconditionpage']
                    }";

        return form;
    }
}
