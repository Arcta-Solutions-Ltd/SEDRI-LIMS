using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Add Organism Scope Culture Test Option" form.
/// User selects organism scope on editorganismscopepage, then selects isolate tests on organismscopeculturetestpage.
/// </summary>
internal class AddOrganismScopeCultureTestOptionFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Organism Scope Culture Test Option" form.
    /// </summary>
    /// <remarks>
    /// This configuration defines the form's name, title, save event, and associated pages.
    /// Reuses editorganismscopepage from breakpoint forms for organism scope selection.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'addorganismscopeculturetestoptionform',
                        title: 'Add organism scope isolate test option.',
                        saveEvent: 'addorganismscopeculturetestoptionevent',
                        suppressRecordView: true,
                        configurable: 'Yes',
                        pages: ['editorganismscopepage', 'organismscopeculturetestpage']
                    }";

        return form;
    }
}
