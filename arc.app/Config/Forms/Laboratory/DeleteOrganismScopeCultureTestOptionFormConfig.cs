using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Delete Organism Scope Culture Test Option" form.
/// </summary>
internal class DeleteOrganismScopeCultureTestOptionFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Organism Scope Culture Test Option" form.
    /// </summary>
    /// <returns>
    /// A string representation of the form configuration.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'deleteorganismscopeculturetestoptionform',
                        viewTitle: 'Delete organism scope culture test option.',
                        saveEvent: 'deleteorganismscopeculturetestoptionevent',
                        suppressRecordView: true,
                        initialQuery: 'editorganismscopeculturetestoptionquery',
                        pages: ['deleteorganismscopeculturetestpage']
                    }";

        return form;
    }
}
