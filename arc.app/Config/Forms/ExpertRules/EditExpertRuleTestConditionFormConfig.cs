using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for editing an expert rule test condition.
/// When opened from the record view embedded list, <c>initialQuery</c> loads the row by id and save persists immediately.
/// </summary>
internal class EditExpertRuleTestConditionFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration for the edit expert rule test condition form.
    /// </summary>
    /// <returns>JSON string defining the form structure and pages.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'editexpertruletestconditionform',
                        viewTitle: '@GenRulU@',
                        saveEvent: 'editexpertruletestcondition',
                        initialQuery: 'editexpertruletestconditionquery',
                        pages: ['editexpertruletestconditionpage']
                    }";

        return form;
    }
}
