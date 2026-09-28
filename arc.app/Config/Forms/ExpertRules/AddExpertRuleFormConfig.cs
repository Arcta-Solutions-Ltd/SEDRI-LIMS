using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for adding a new expert rule.
/// </summary>
internal class AddExpertRuleFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration for the add expert rule form.
    /// </summary>
    /// <returns>JSON string defining the form structure, pages, and rules.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'addexpertruleform',
                        viewTitle: 'Add a new expert rule.',
                        saveEvent: 'addexpertrule',
                        suppressRecordView: true,
                        pages: [ 'expertruledetailspage', 'editorganismscopepage', 'selectorganismpage','organismlistpage','expertruledetailssecondpage'],
                        rules:
                        [
                            { Outcome: 'visible', page: 'selectorganismpage', state: 'organismsearch' },
                            { Outcome: 'visible', page: 'organismlistpage', state: 'organismsearch' }
                        ]
                    }";

        return form;
    }
}
