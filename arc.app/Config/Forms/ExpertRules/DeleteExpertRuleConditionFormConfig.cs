using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for deleting an expert rule condition from the record view.
/// </summary>
internal class DeleteExpertRuleConditionFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration for the delete expert rule condition form.
    /// </summary>
    /// <returns>JSON string defining the form structure and pages.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'deleteexpertruleconditionform',
                        viewTitle: 'Delete an expert rule condition.',
                        saveEvent: 'deleteexpertrulecondition',
                        suppressRecordView: true,
                        initialQuery: 'deleteexpertruleconditionquery',
                        pages: ['deleteexpertruleconditionpage']
                    }";

        return form;
    }
}
