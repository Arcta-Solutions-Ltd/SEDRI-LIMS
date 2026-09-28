using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for adding a condition to an expert rule.
/// </summary>
internal class AddExpertRuleConditionFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration for the add expert rule condition form.
    /// </summary>
    /// <returns>JSON string defining the form structure and pages.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'addexpertruleconditionform',
                        viewTitle: 'Add a condition to an expert rule.',
                        saveEvent: 'addexpertrulecondition',
                        saveOperation: 'updategrid',
                        suppressRecordView: true,
                        pages: ['addexpertruleconditionpage']
                    }";

        return form;
    }
}
