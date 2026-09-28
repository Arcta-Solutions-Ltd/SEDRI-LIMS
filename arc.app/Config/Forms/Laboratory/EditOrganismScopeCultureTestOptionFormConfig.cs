using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Edit Organism Scope Culture Test Option" form.
/// </summary>
internal class EditOrganismScopeCultureTestOptionFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Organism Scope Culture Test Option" form.
    /// </summary>
    /// <remarks>
    /// Uses editorganismscopeculturetestdefaultquery to load the config by ID.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'editorganismscopeculturetestoptionform',
                        viewTitle: 'Edit organism scope culture test option.',
                        saveEvent: 'editorganismscopeculturetestoptionevent',
                        suppressRecordView: true,
                        initialQuery: 'editorganismscopeculturetestoptionquery',
                        pages: ['editorganismscopeculturetestpage']
                    }";

        return form;
    }
}
