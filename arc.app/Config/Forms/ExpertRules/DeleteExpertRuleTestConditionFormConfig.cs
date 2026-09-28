using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for deleting an expert rule test condition from the record view.
/// </summary>
internal class DeleteExpertRuleTestConditionFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration for the delete expert rule test condition form.
    /// </summary>
    /// <returns>JSON string defining the form structure and pages.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'deleteexpertruletestconditionform',
                        viewTitle: 'Delete an expert rule test condition.',
                        saveEvent: 'deleteexpertruletestcondition',
                        suppressRecordView: true,
                        initialQuery: 'deleteexpertruletestconditionquery',
                        pages: ['deleteexpertruletestconditionpage']
                    }";

        return form;
    }
}
