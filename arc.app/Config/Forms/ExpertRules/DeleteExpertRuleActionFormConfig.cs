using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for deleting an expert rule action from the record view.
/// </summary>
internal class DeleteExpertRuleActionFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration for the delete expert rule action form.
    /// </summary>
    /// <returns>JSON string defining the form structure and pages.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'deleteexpertruleactionform',
                        viewTitle: 'Delete an expert rule action.',
                        saveEvent: 'deleteexpertruleaction',
                        suppressRecordView: true,
                        initialQuery: 'deleteexpertruleactionquery',
                        pages: ['deleteexpertruleactionpage']
                    }";

        return form;
    }
}
