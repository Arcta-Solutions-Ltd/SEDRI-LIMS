using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for deleting an existing expert rule.
/// </summary>
internal class DeleteExpertRuleFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration for the delete expert rule form.
    /// </summary>
    /// <returns>JSON string defining the form structure, pages, and initial query.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'deleteexpertruleform',
                        viewTitle: 'Delete an existing expert rule.',
                        saveEvent: 'deleteexpertrule',
                        initialQuery: 'deleteexpertrule',
                        pages: ['deleteexpertrulepage']
                    }";

        return form;
    }
}
