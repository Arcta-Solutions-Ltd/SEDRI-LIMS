using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for editing an existing expert rule.
/// </summary>
internal class EditExpertRuleFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration for the edit expert rule form.
    /// </summary>
    /// <returns>JSON string defining the form structure, pages, and visibility rules.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'editexpertruleform',
                        viewTitle: 'Edit an existing expert rule.',
                        saveEvent: 'editexpertrule',
                        initialQuery: 'editexpertrulequery',
                        startstate: 'noorganism',
                        suppressRecordView: true,
                        pages: ['editexpertruledetailspage', 'changeorganismselectorpage', 'editorganismscopepage', 'selectorganismpage','organismlistpage', 'expertruledetailssecondpage'],
                        rules:
                        [
                            { Outcome: 'visible', page: 'editorganismscopepage', state: 'neworganism' },
                            { Outcome: 'visible', page: 'selectorganismpage', state: 'organismsearch' },
                            { Outcome: 'visible', page: 'organismlistpage', state: 'organismsearch' }
                        ]
                    }";

        return form;
    }
}
