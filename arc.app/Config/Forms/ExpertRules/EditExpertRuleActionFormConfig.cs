using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for editing an expert rule action.
/// When opened from the expert rule fieldgrid, the portal passes row data from the parent form (no InitialQuery);
/// save uses deferred merge into the grid (<c>saveOperation: updategrid</c>) until the parent expert rule is saved.
/// When opened from the record view embedded list, <c>initialQuery</c> loads the row by id and save persists immediately.
/// </summary>
internal class EditExpertRuleActionFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration for the edit expert rule action form.
    /// </summary>
    /// <returns>JSON string defining the form structure and pages.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'editexpertruleactionform',
                        viewTitle: 'Edit an expert rule action.',
                        saveEvent: 'editexpertruleaction',
                        saveOperation: 'updategrid',
                        suppressRecordView: true,
                        initialQuery: 'editexpertruleactionquery',
                        pages: ['editexpertruleactionpage']
                    }";

        return form;
    }
}
