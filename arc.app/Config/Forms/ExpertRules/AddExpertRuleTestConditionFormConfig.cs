using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for adding a test condition to an expert rule.
/// </summary>
internal class AddExpertRuleTestConditionFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration for the add expert rule test condition form.
    /// </summary>
    /// <returns>JSON string defining the form structure and pages.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'addexpertruletestconditionform',
                        viewTitle: 'Add a test condition to an expert rule.',
                        saveEvent: 'addexpertruletestcondition',
                        saveOperation: 'updategrid',
                        suppressRecordView: true,
                        pages: ['addexpertruletestconditionpage']
                    }";

        return form;
    }
}
