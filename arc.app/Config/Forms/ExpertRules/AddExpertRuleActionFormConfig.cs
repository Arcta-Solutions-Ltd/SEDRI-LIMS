using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for adding an action to an expert rule.
/// </summary>
internal class AddExpertRuleActionFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration for the add expert rule action form.
    /// </summary>
    /// <returns>JSON string defining the form structure and pages.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'addexpertruleactionform',
                        viewTitle: 'Add an action to an expert rule.',
                        saveEvent: 'addexpertruleaction',
                        saveOperation: 'updategrid',
                        suppressRecordView: true,
                        pages: ['addexpertruleactionpage']                    
                    }";

        return form;
    }
}
